using AutoMapper;
using Microsoft.AspNetCore.Identity;
using vn.edu.fpt.dto;
using vn.edu.fpt.entity;
using vn.edu.fpt.repository;
using vn.edu.fpt.service.Interfaces;

namespace vn.edu.fpt.service.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AuthService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IEmailService emailService,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return null;
            var checkPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!checkPassword) return null;
            return "dummy-token";
        }

        public async Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterDto registerDto, string confirmationLink)
        {
            var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null)
                return (false, "Email này đã được đăng ký. Vui lòng sử dụng email khác hoặc đăng nhập.");

            var user = new User
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return (false, "Đăng ký thất bại. Vui lòng thử lại.");

            if (!await _roleManager.RoleExistsAsync(registerDto.Role))
                await _roleManager.CreateAsync(new IdentityRole(registerDto.Role));
            await _userManager.AddToRoleAsync(user, registerDto.Role);

            switch (registerDto.Role.ToLower())
            {
                case "candidate":
                    await _unitOfWork.Candidates.AddAsync(new Candidate { UserId = user.Id });
                    break;
                case "recruiter":
                    await _unitOfWork.Recruiters.AddAsync(new Recruiter { UserId = user.Id });
                    break;
                case "admin":
                    await _unitOfWork.Admins.AddAsync(new Admin { UserId = user.Id });
                    break;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var fullLink = $"{confirmationLink}&token={Uri.EscapeDataString(token)}";
            var fullName = $"{registerDto.LastName} {registerDto.FirstName}".Trim();
            await _emailService.SendEmailAsync(user.Email!, "Kích hoạt tài khoản - Finding Jobs", BuildConfirmEmail(fullName, fullLink));

            await _unitOfWork.CompleteAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> RegisterRecruiterAsync(RecruiterRegisterViewModel model, string confirmationLink)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return (false, "Email này đã được đăng ký. Vui lòng sử dụng email khác hoặc đăng nhập.");

            if (!string.IsNullOrWhiteSpace(model.CompanyTaxCode))
            {
                var taxExists = (await _unitOfWork.Companies.FindAsync(c => c.TaxCode == model.CompanyTaxCode)).Any();
                if (taxExists)
                    return (false, "Mã số thuế đã tồn tại trong hệ thống. Vui lòng kiểm tra lại.");
            }

            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return (false, string.Join("; ", result.Errors.Select(e => e.Description)));

            if (!await _roleManager.RoleExistsAsync("Recruiter"))
                await _roleManager.CreateAsync(new IdentityRole("Recruiter"));
            await _userManager.AddToRoleAsync(user, "Recruiter");

            // Company via mapper
            var company = _mapper.Map<Company>(model);
            company.IsVerified = false;
            company.CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            company.UpdatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.CompleteAsync();

            // Recruiter profile via mapper
            var recruiter = _mapper.Map<Recruiter>(model);
            recruiter.UserId = user.Id;
            recruiter.CompanyId = company.Id;
            recruiter.FullName = $"{model.FirstName} {model.LastName}".Trim();
            recruiter.IsVerified = false;
            recruiter.CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            recruiter.UpdatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            await _unitOfWork.Recruiters.AddAsync(recruiter);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var fullLink = $"{confirmationLink}&token={Uri.EscapeDataString(token)}";
            var fullName = $"{model.LastName} {model.FirstName}".Trim();
            await _emailService.SendEmailAsync(user.Email!,
                "Kích hoạt tài khoản Nhà tuyển dụng - Finding Jobs",
                BuildConfirmEmail(fullName, fullLink, isRecruiter: true, companyName: model.CompanyName));

            await _unitOfWork.CompleteAsync();
            return (true, null);
        }

        public async Task SendRawEmailAsync(string email, string subject, string message)
        {
            await _emailService.SendEmailAsync(email, subject, message);
        }

        // ─── Email Templates ─────────────────────────────────────────────────────

        public static string BuildConfirmEmail(string fullName, string confirmUrl,
            bool isRecruiter = false, string? companyName = null)
        {
            var roleNote = isRecruiter
                ? $"<p style='color:#374151;margin:0 0 12px;'>Công ty đã đăng ký: <strong>{companyName}</strong></p>"
                : "";

            return $@"<!DOCTYPE html>
<html lang='vi'><head><meta charset='UTF-8'/><meta name='viewport' content='width=device-width,initial-scale=1'/></head>
<body style='margin:0;padding:0;background:#f3f4f6;font-family:Inter,Segoe UI,Arial,sans-serif;'>
  <table width='100%' cellpadding='0' cellspacing='0' style='padding:40px 16px;'>
    <tr><td align='center'>
      <table width='600' cellpadding='0' cellspacing='0' style='max-width:600px;width:100%;'>
        <tr><td style='background:#00b14f;padding:28px 36px;border-radius:14px 14px 0 0;text-align:center;'>
          <h1 style='margin:0;color:#fff;font-size:26px;font-weight:800;'>Finding Jobs</h1>
          <p style='margin:6px 0 0;color:rgba(255,255,255,.85);font-size:14px;'>Nền tảng tuyển dụng hàng đầu Việt Nam</p>
        </td></tr>
        <tr><td style='background:#fff;padding:36px;border-radius:0 0 14px 14px;box-shadow:0 4px 24px rgba(0,0,0,.07);'>
          <h2 style='margin:0 0 16px;color:#0f172a;font-size:20px;font-weight:700;'>Xin chào, {fullName}!</h2>
          <p style='color:#374151;line-height:1.7;margin:0 0 12px;'>Cảm ơn bạn đã đăng ký tài khoản tại <strong style='color:#00b14f;'>Finding Jobs</strong>. Chúc mừng bạn trở thành thành viên của chúng tôi!</p>
          {roleNote}
          <p style='color:#374151;line-height:1.7;margin:0 0 24px;'>Để hoàn tất đăng ký, vui lòng xác nhận địa chỉ email bằng cách nhấn vào nút bên dưới:</p>
          <div style='text-align:center;margin:28px 0;'>
            <a href='{confirmUrl}' style='display:inline-block;background:#00b14f;color:#fff;text-decoration:none;padding:15px 40px;border-radius:8px;font-size:16px;font-weight:700;'>Xác nhận tài khoản</a>
          </div>
          <div style='background:#fffbeb;border-left:4px solid #f59e0b;padding:14px 16px;border-radius:6px;margin:0 0 24px;'>
            <p style='margin:0;color:#92400e;font-size:13px;'><strong>Lưu ý:</strong> Liên kết xác nhận sẽ hết hiệu lực sau <strong>5 phút</strong>. Nếu hết hạn, đăng nhập và yêu cầu gửi lại email xác nhận.</p>
          </div>
          <p style='color:#9ca3af;font-size:12px;line-height:1.6;margin:0;'>Nếu bạn không thực hiện đăng ký này, hãy bỏ qua email. Tài khoản sẽ không được kích hoạt.</p>
        </td></tr>
        <tr><td style='text-align:center;padding:20px 0;'>
          <p style='margin:0;color:#9ca3af;font-size:12px;'>© 2025 Finding Jobs • <a href='#' style='color:#9ca3af;text-decoration:none;'>Điều khoản</a> • <a href='#' style='color:#9ca3af;text-decoration:none;'>Bảo mật</a></p>
        </td></tr>
      </table>
    </td></tr>
  </table>
</body></html>";
        }

        public static string BuildResetPasswordEmail(string fullName, string newPassword)
        {
            return $@"<!DOCTYPE html>
<html lang='vi'><head><meta charset='UTF-8'/><meta name='viewport' content='width=device-width,initial-scale=1'/></head>
<body style='margin:0;padding:0;background:#f3f4f6;font-family:Inter,Segoe UI,Arial,sans-serif;'>
  <table width='100%' cellpadding='0' cellspacing='0' style='padding:40px 16px;'>
    <tr><td align='center'>
      <table width='600' cellpadding='0' cellspacing='0' style='max-width:600px;width:100%;'>
        <tr><td style='background:#00b14f;padding:28px 36px;border-radius:14px 14px 0 0;text-align:center;'>
          <h1 style='margin:0;color:#fff;font-size:26px;font-weight:800;'>Finding Jobs</h1>
          <p style='margin:6px 0 0;color:rgba(255,255,255,.85);font-size:14px;'>Đặt lại mật khẩu</p>
        </td></tr>
        <tr><td style='background:#fff;padding:36px;border-radius:0 0 14px 14px;box-shadow:0 4px 24px rgba(0,0,0,.07);'>
          <h2 style='margin:0 0 16px;color:#0f172a;font-size:20px;font-weight:700;'>Xin chào, {fullName}!</h2>
          <p style='color:#374151;line-height:1.7;margin:0 0 20px;'>Yêu cầu đặt lại mật khẩu của bạn đã được xử lý thành công. Dưới đây là mật khẩu mới:</p>
          <div style='background:#f0fdf4;border:2px dashed #00b14f;padding:22px;border-radius:10px;text-align:center;margin:0 0 24px;'>
            <p style='margin:0 0 6px;color:#6b7280;font-size:13px;'>Mật khẩu mới của bạn</p>
            <p style='margin:0;color:#0f172a;font-size:24px;font-weight:800;letter-spacing:3px;font-family:monospace;'>{newPassword}</p>
          </div>
          <div style='background:#fef2f2;border-left:4px solid #ef4444;padding:14px 16px;border-radius:6px;margin:0 0 24px;'>
            <p style='margin:0;color:#991b1b;font-size:13px;'><strong>Bảo mật:</strong> Hãy đăng nhập ngay và đổi sang mật khẩu cá nhân để đảm bảo an toàn tài khoản.</p>
          </div>
          <p style='color:#9ca3af;font-size:12px;line-height:1.6;margin:0;'>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng liên hệ chúng tôi ngay lập tức.</p>
        </td></tr>
        <tr><td style='text-align:center;padding:20px 0;'>
          <p style='margin:0;color:#9ca3af;font-size:12px;'>© 2025 Finding Jobs</p>
        </td></tr>
      </table>
    </td></tr>
  </table>
</body></html>";
        }

        public static string BuildResendConfirmEmail(string fullName, string confirmUrl)
        {
            return $@"<!DOCTYPE html>
<html lang='vi'><head><meta charset='UTF-8'/><meta name='viewport' content='width=device-width,initial-scale=1'/></head>
<body style='margin:0;padding:0;background:#f3f4f6;font-family:Inter,Segoe UI,Arial,sans-serif;'>
  <table width='100%' cellpadding='0' cellspacing='0' style='padding:40px 16px;'>
    <tr><td align='center'>
      <table width='600' cellpadding='0' cellspacing='0' style='max-width:600px;width:100%;'>
        <tr><td style='background:#00b14f;padding:28px 36px;border-radius:14px 14px 0 0;text-align:center;'>
          <h1 style='margin:0;color:#fff;font-size:26px;font-weight:800;'>Finding Jobs</h1>
          <p style='margin:6px 0 0;color:rgba(255,255,255,.85);font-size:14px;'>Nền tảng tuyển dụng hàng đầu Việt Nam</p>
        </td></tr>
        <tr><td style='background:#fff;padding:36px;border-radius:0 0 14px 14px;box-shadow:0 4px 24px rgba(0,0,0,.07);'>
          <h2 style='margin:0 0 16px;color:#0f172a;font-size:20px;font-weight:700;'>Chào bạn, {fullName}!</h2>
          <p style='color:#374151;line-height:1.7;margin:0 0 12px;'>Bạn đã yêu cầu gửi lại liên kết kích hoạt tài khoản tại <strong>Finding Jobs</strong>.</p>
          <p style='color:#374151;line-height:1.7;margin:0 0 24px;'>Vui lòng nhấn vào nút bên dưới để xác nhận và hoàn tất đăng ký:</p>
          <div style='text-align:center;margin:28px 0;'>
            <a href='{confirmUrl}' style='display:inline-block;background:#00b14f;color:#fff;text-decoration:none;padding:15px 40px;border-radius:8px;font-size:16px;font-weight:700;'>Xác nhận tài khoản</a>
          </div>
          <div style='background:#fffbeb;border-left:4px solid #f59e0b;padding:14px 16px;border-radius:6px;margin:0 0 24px;'>
            <p style='margin:0;color:#92400e;font-size:13px;'><strong>Lưu ý:</strong> Liên kết xác nhận sẽ hết hiệu lực sau <strong>5 phút</strong>. Nếu hết hạn, vui lòng yêu cầu gửi lại email mới.</p>
          </div>
          <p style='color:#9ca3af;font-size:12px;line-height:1.6;margin:0;'>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>
        </td></tr>
        <tr><td style='text-align:center;padding:20px 0;'>
          <p style='margin:0;color:#9ca3af;font-size:12px;'>© 2025 Finding Jobs</p>
        </td></tr>
      </table>
    </td></tr>
  </table>
</body></html>";
        }
    }
}
