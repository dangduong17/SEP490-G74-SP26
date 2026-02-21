using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using vn.edu.fpt.dto;
using vn.edu.fpt.entity;
using vn.edu.fpt.repository;
using vn.edu.fpt.service.Interfaces;

namespace vn.edu.fpt.service.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AdminDashboardViewModel> GetDashboardAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var adminIds = (await _userManager.GetUsersInRoleAsync("Admin")).Select(u => u.Id).ToHashSet();
            var candidateIds = (await _userManager.GetUsersInRoleAsync("Candidate")).Select(u => u.Id).ToHashSet();
            var recruiterIds = (await _userManager.GetUsersInRoleAsync("Recruiter")).Select(u => u.Id).ToHashSet();

            return new AdminDashboardViewModel
            {
                TotalUsers = users.Count,
                ActiveUsers = users.Count(u => u.IsActive),
                InactiveUsers = users.Count(u => !u.IsActive),
                TotalAdmins = adminIds.Count,
                TotalCandidates = candidateIds.Count,
                TotalRecruiters = recruiterIds.Count
            };
        }

        public async Task<AdminUserListViewModel> GetUserListAsync(string? keyword, string? role, string? status, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = (pageSize != 10 && pageSize != 20 && pageSize != 50) ? 10 : pageSize;

            var users = await _userManager.Users.ToListAsync();
            var candidates = (await _unitOfWork.Candidates.GetAllAsync()).ToList();
            var recruiters = (await _unitOfWork.Recruiters.GetAllAsync()).ToList();
            var admins = (await _unitOfWork.Admins.GetAllAsync()).ToList();

            var candidateMap = candidates.ToDictionary(x => x.UserId, x => x);
            var recruiterMap = recruiters.ToDictionary(x => x.UserId, x => x);
            var adminMap = admins.ToDictionary(x => x.UserId, x => x);

            var adminIds = (await _userManager.GetUsersInRoleAsync("Admin")).Select(u => u.Id).ToHashSet();
            var candidateIds = (await _userManager.GetUsersInRoleAsync("Candidate")).Select(u => u.Id).ToHashSet();
            var recruiterIds = (await _userManager.GetUsersInRoleAsync("Recruiter")).Select(u => u.Id).ToHashSet();

            string ResolveRole(string userId)
            {
                if (adminIds.Contains(userId)) return "Admin";
                if (candidateIds.Contains(userId)) return "Candidate";
                if (recruiterIds.Contains(userId)) return "Recruiter";
                return "N/A";
            }

            string? ResolvePhone(string userId, string? identityPhone)
            {
                if (adminMap.TryGetValue(userId, out var admin)) return admin.Phone;
                if (candidateMap.TryGetValue(userId, out var candidate)) return candidate.Phone;
                if (recruiterMap.TryGetValue(userId, out var recruiter)) return recruiter.Phone;
                return identityPhone;
            }

            IEnumerable<User> query = users;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var key = keyword.Trim().ToLower();
                query = query.Where(u =>
                {
                    var fullName = $"{u.FirstName} {u.LastName}".Trim().ToLower();
                    var phone = ResolvePhone(u.Id, u.PhoneNumber) ?? string.Empty;
                    return (u.Email ?? string.Empty).ToLower().Contains(key) ||
                           fullName.Contains(key) ||
                           phone.Contains(key);
                });
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status.Equals("active", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(u => u.IsActive);
                }
                else if (status.Equals("inactive", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(u => !u.IsActive);
                }
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                query = query.Where(u => ResolveRole(u.Id) == role);
            }

            var filteredUsers = query.OrderByDescending(u => u.CreatedAt).ToList();
            var totalItems = filteredUsers.Count;
            var pagedUsers = filteredUsers.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new AdminUserListViewModel
            {
                Keyword = keyword,
                Role = role,
                Status = status,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Users = pagedUsers.Select(u => new AdminUserListItemViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = $"{u.FirstName} {u.LastName}".Trim(),
                    PhoneNumber = ResolvePhone(u.Id, u.PhoneNumber),
                    Role = ResolveRole(u.Id),
                    CreatedAt = u.CreatedAt,
                    IsActive = u.IsActive
                }).ToList()
            };
        }

        public async Task<AdminUpdateUserViewModel?> GetUpdateUserAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            var currentRole = roles.FirstOrDefault() ?? "Candidate";
            var admin = (await _unitOfWork.Admins.FindAsync(x => x.UserId == user.Id)).FirstOrDefault();
            var candidate = (await _unitOfWork.Candidates.FindAsync(x => x.UserId == user.Id)).FirstOrDefault();
            var recruiter = (await _unitOfWork.Recruiters.FindAsync(x => x.UserId == user.Id)).FirstOrDefault();

            return new AdminUpdateUserViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                PhoneNumber = admin?.Phone ?? candidate?.Phone ?? recruiter?.Phone ?? user.PhoneNumber,
                Role = currentRole,
                IsActive = user.IsActive,
                AdminDepartment = admin?.Department,
                CandidateTitle = candidate?.Title,
                CandidateCity = candidate?.City,
                CandidateDateOfBirth = candidate?.DateOfBirth,
                CandidateGender = candidate?.Gender,
                RecruiterPosition = recruiter?.Position,
                RecruiterDepartment = recruiter?.Department
            };
        }

        public async Task<ServiceResult> CreateAdminAsync(AdminCreateAdminViewModel model)
        {
            var baseUserResult = await CreateBaseUser(model.Email, model.Password, model.FirstName, model.LastName);
            if (!baseUserResult.Succeeded || baseUserResult.User == null) return baseUserResult.Result;

            await EnsureRoleAndAssign(baseUserResult.User, "Admin");

            var profile = _mapper.Map<Admin>(model);
            profile.UserId = baseUserResult.User.Id;
            profile.FullName = $"{model.FirstName} {model.LastName}".Trim();
            profile.CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            profile.UpdatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            await _unitOfWork.Admins.AddAsync(profile);
            await _unitOfWork.CompleteAsync();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> CreateCandidateAsync(AdminCreateCandidateViewModel model)
        {
            var validateResult = ValidateCandidateRequired(model.PhoneNumber, model.Gender, model.DateOfBirth);
            if (!validateResult.Succeeded) return validateResult;

            var baseUserResult = await CreateBaseUser(model.Email, model.Password, model.FirstName, model.LastName);
            if (!baseUserResult.Succeeded || baseUserResult.User == null) return baseUserResult.Result;

            await EnsureRoleAndAssign(baseUserResult.User, "Candidate");

            var profile = _mapper.Map<Candidate>(model);
            profile.UserId = baseUserResult.User.Id;
            profile.FullName = $"{model.FirstName} {model.LastName}".Trim();
            profile.CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            profile.UpdatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            await _unitOfWork.Candidates.AddAsync(profile);
            await _unitOfWork.CompleteAsync();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> CreateRecruiterAsync(AdminCreateRecruiterViewModel model)
        {
            var validateResult = ValidateRecruiterRequired(model.PhoneNumber, model.Position);
            if (!validateResult.Succeeded) return validateResult;
            if (!model.ProvinceCode.HasValue || !model.WardCode.HasValue ||
                string.IsNullOrWhiteSpace(model.ProvinceName) ||
                string.IsNullOrWhiteSpace(model.WardName) ||
                string.IsNullOrWhiteSpace(model.WorkAddress))
            {
                return ServiceResult.Failed(
                    new ServiceError { Key = nameof(model.WorkAddress), Message = "Địa chỉ làm việc là bắt buộc." },
                    new ServiceError { Key = nameof(model.ProvinceCode), Message = "Tỉnh/Thành phố là bắt buộc." },
                    new ServiceError { Key = nameof(model.WardCode), Message = "Phường/Xã là bắt buộc." });
            }

            if (!string.IsNullOrWhiteSpace(model.CompanyTaxCode))
            {
                var taxCodeExists = (await _unitOfWork.Companies.FindAsync(c => c.TaxCode == model.CompanyTaxCode)).Any();
                if (taxCodeExists)
                {
                    return ServiceResult.Failed(new ServiceError { Key = nameof(model.CompanyTaxCode), Message = "Mã số thuế đã tồn tại." });
                }
            }

            var baseUserResult = await CreateBaseUser(model.Email, model.Password, model.FirstName, model.LastName);
            if (!baseUserResult.Succeeded || baseUserResult.User == null) return baseUserResult.Result;

            await EnsureRoleAndAssign(baseUserResult.User, "Recruiter");

            var company = _mapper.Map<Company>(model);
            company.CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            company.UpdatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            company.IsVerified = true;
            company.VerifiedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.CompleteAsync();

            var profile = _mapper.Map<Recruiter>(model);
            profile.UserId = baseUserResult.User.Id;
            profile.FullName = $"{model.FirstName} {model.LastName}".Trim();
            profile.CompanyId = company.Id;
            profile.CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            profile.UpdatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            profile.IsVerified = true;
            profile.VerifiedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            await _unitOfWork.Recruiters.AddAsync(profile);

            var companyAddress = new CompanyAddress
            {
                CompanyId = company.Id,
                Address = model.WorkAddress,
                City = model.ProvinceName,
                Ward = model.WardName,
                AddressType = "Workplace",
                IsHeadquarter = true,
                Phone = model.CompanyPhone ?? model.PhoneNumber,
                CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam
            };
            await _unitOfWork.CompanyAddresses.AddAsync(companyAddress);
            await _unitOfWork.CompleteAsync();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> UpdateUserAsync(AdminUpdateUserViewModel model)
        {
            if (!IsSupportedRole(model.Role))
            {
                return ServiceResult.Failed(new ServiceError { Key = nameof(model.Role), Message = "Vai trò không hợp lệ." });
            }

            if (model.Role == "Candidate")
            {
                var validateCandidate = ValidateCandidateRequired(
                    model.PhoneNumber,
                    model.CandidateGender,
                    model.CandidateDateOfBirth,
                    nameof(AdminUpdateUserViewModel.PhoneNumber),
                    nameof(AdminUpdateUserViewModel.CandidateGender),
                    nameof(AdminUpdateUserViewModel.CandidateDateOfBirth));
                if (!validateCandidate.Succeeded) return validateCandidate;
            }

            if (model.Role == "Recruiter")
            {
                var validateRecruiter = ValidateRecruiterRequired(
                    model.PhoneNumber,
                    model.RecruiterPosition,
                    nameof(AdminUpdateUserViewModel.PhoneNumber),
                    nameof(AdminUpdateUserViewModel.RecruiterPosition));
                if (!validateRecruiter.Succeeded) return validateRecruiter;
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return ServiceResult.NotFoundResult();

            if (!string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailOwner = await _userManager.FindByEmailAsync(model.Email);
                if (emailOwner != null && emailOwner.Id != user.Id)
                {
                    return ServiceResult.Failed(new ServiceError { Key = nameof(model.Email), Message = "Email đã được sử dụng." });
                }
            }

            _mapper.Map(model, user);
            user.UpdatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;

            if (!string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase))
            {
                var userNameResult = await _userManager.SetUserNameAsync(user, model.Email);
                if (!userNameResult.Succeeded) return ToServiceResult(userNameResult);

                var emailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!emailResult.Succeeded) return ToServiceResult(emailResult);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(model.Role))
            {
                if (currentRoles.Any())
                {
                    var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeRolesResult.Succeeded) return ToServiceResult(removeRolesResult);
                }

                await EnsureRoleAndAssign(user, model.Role);
            }

            await UpdateRoleProfileAsync(user.Id, model);

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded) return ToServiceResult(updateResult);

            await _unitOfWork.CompleteAsync();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> SoftDeleteUserAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ServiceResult.Failed(new ServiceError { Message = "Id không hợp lệ." });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return ServiceResult.NotFoundResult();

            user.IsActive = false;
            user.UpdatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return ToServiceResult(result);

            return ServiceResult.Success();
        }

        private async Task<(bool Succeeded, User? User, ServiceResult Result)> CreateBaseUser(string email, string password, string firstName, string lastName)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return (false, null, ServiceResult.Failed(new ServiceError { Key = "Email", Message = "Email đã tồn tại." }));
            }

            var user = new User
            {
                Email = email,
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                EmailConfirmed = true,
                CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var createResult = await _userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                return (false, null, ToServiceResult(createResult));
            }

            return (true, user, ServiceResult.Success());
        }

        private async Task EnsureRoleAndAssign(User user, string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, userRoles);
            }

            await _userManager.AddToRoleAsync(user, roleName);
        }

        private async Task UpdateRoleProfileAsync(string userId, AdminUpdateUserViewModel model)
        {
            var fullName = $"{model.FirstName} {model.LastName}".Trim();
            if (model.Role == "Admin")
            {
                var profile = (await _unitOfWork.Admins.FindAsync(x => x.UserId == userId)).FirstOrDefault();
                var isNew = false;
                if (profile == null)
                {
                    profile = new Admin { UserId = userId, CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam };
                    await _unitOfWork.Admins.AddAsync(profile);
                    isNew = true;
                }

                _mapper.Map(model, profile);
                profile.FullName = fullName;
                profile.UpdatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
                if (!isNew) _unitOfWork.Admins.Update(profile);
            }
            else if (model.Role == "Candidate")
            {
                var profile = (await _unitOfWork.Candidates.FindAsync(x => x.UserId == userId)).FirstOrDefault();
                var isNew = false;
                if (profile == null)
                {
                    profile = new Candidate { UserId = userId, CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam };
                    await _unitOfWork.Candidates.AddAsync(profile);
                    isNew = true;
                }

                _mapper.Map(model, profile);
                profile.FullName = fullName;
                profile.UpdatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
                if (!isNew) _unitOfWork.Candidates.Update(profile);
            }
            else if (model.Role == "Recruiter")
            {
                var profile = (await _unitOfWork.Recruiters.FindAsync(x => x.UserId == userId)).FirstOrDefault();
                var isNew = false;
                if (profile == null)
                {
                    profile = new Recruiter { UserId = userId, CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam };
                    await _unitOfWork.Recruiters.AddAsync(profile);
                    isNew = true;
                }

                _mapper.Map(model, profile);
                profile.FullName = fullName;
                profile.UpdatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
                if (!isNew) _unitOfWork.Recruiters.Update(profile);
            }
        }

        private static bool IsSupportedRole(string role)
        {
            return role == "Admin" || role == "Candidate" || role == "Recruiter";
        }

        private static ServiceResult ToServiceResult(IdentityResult identityResult)
        {
            return ServiceResult.Failed(identityResult.Errors.Select(x => new ServiceError
            {
                Key = string.Empty,
                Message = x.Description
            }).ToArray());
        }

        private static ServiceResult ValidateCandidateRequired(
            string? phone,
            string? gender,
            DateTime? dateOfBirth,
            string phoneKey = nameof(AdminCreateCandidateViewModel.PhoneNumber),
            string genderKey = nameof(AdminCreateCandidateViewModel.Gender),
            string dateOfBirthKey = nameof(AdminCreateCandidateViewModel.DateOfBirth))
        {
            var errors = new List<ServiceError>();
            if (string.IsNullOrWhiteSpace(phone))
            {
                errors.Add(new ServiceError { Key = phoneKey, Message = "Số điện thoại là bắt buộc." });
            }

            if (string.IsNullOrWhiteSpace(gender))
            {
                errors.Add(new ServiceError { Key = genderKey, Message = "Giới tính là bắt buộc." });
            }

            if (!dateOfBirth.HasValue)
            {
                errors.Add(new ServiceError { Key = dateOfBirthKey, Message = "Ngày sinh là bắt buộc." });
            }

            return errors.Count == 0 ? ServiceResult.Success() : ServiceResult.Failed(errors.ToArray());
        }

        private static ServiceResult ValidateRecruiterRequired(
            string? phone,
            string? position,
            string phoneKey = nameof(AdminCreateRecruiterViewModel.PhoneNumber),
            string positionKey = nameof(AdminCreateRecruiterViewModel.Position))
        {
            var errors = new List<ServiceError>();
            if (string.IsNullOrWhiteSpace(phone))
            {
                errors.Add(new ServiceError { Key = phoneKey, Message = "Số điện thoại là bắt buộc." });
            }

            if (string.IsNullOrWhiteSpace(position))
            {
                errors.Add(new ServiceError { Key = positionKey, Message = "Vị trí công việc là bắt buộc." });
            }

            return errors.Count == 0 ? ServiceResult.Success() : ServiceResult.Failed(errors.ToArray());
        }
    }
}

