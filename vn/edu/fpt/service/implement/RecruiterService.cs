using Microsoft.AspNetCore.Identity;
using vn.edu.fpt.dto;
using vn.edu.fpt.entity;
using vn.edu.fpt.repository;
using vn.edu.fpt.service.Interfaces;

namespace vn.edu.fpt.service.Implementations
{
    public class RecruiterService : IRecruiterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public RecruiterService(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<RecruiterDashboardViewModel?> GetDashboardAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var recruiter = (await _unitOfWork.Recruiters.FindAsync(r => r.UserId == userId)).FirstOrDefault();
            if (recruiter == null) return null;

            Company? company = null;
            if (recruiter.CompanyId.HasValue)
            {
                company = await _unitOfWork.Companies.GetByIdAsync(recruiter.CompanyId.Value);
            }

            return new RecruiterDashboardViewModel
            {
                RecruiterName = $"{user.FirstName} {user.LastName}".Trim(),
                RecruiterEmail = user.Email,
                RecruiterPhone = recruiter.Phone,
                Position = recruiter.Position,
                CompanyName = company?.Name,
                CompanyIndustry = company?.Industry,
                IsVerified = recruiter.IsVerified
            };
        }

        public async Task<RecruiterProfileUpdateViewModel?> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var recruiter = (await _unitOfWork.Recruiters.FindAsync(r => r.UserId == userId)).FirstOrDefault();
            if (recruiter == null) return null;

            Company? company = null;
            if (recruiter.CompanyId.HasValue)
            {
                company = await _unitOfWork.Companies.GetByIdAsync(recruiter.CompanyId.Value);
            }
            CompanyAddress? companyAddress = null;
            if (recruiter.CompanyId.HasValue)
            {
                companyAddress = (await _unitOfWork.CompanyAddresses.FindAsync(x => x.CompanyId == recruiter.CompanyId.Value))
                    .OrderByDescending(x => x.IsHeadquarter)
                    .FirstOrDefault();
            }

            return new RecruiterProfileUpdateViewModel
            {
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                Phone = recruiter.Phone ?? string.Empty,
                Position = recruiter.Position ?? string.Empty,
                Department = recruiter.Department,
                CompanyName = company?.Name ?? string.Empty,
                CompanyTaxCode = company?.TaxCode,
                CompanySize = company?.CompanySize,
                CompanyIndustry = company?.Industry,
                CompanyWebsite = company?.Website,
                CompanyEmail = company?.Email,
                CompanyPhone = company?.Phone,
                CompanyDescription = company?.Description,
                WorkAddress = companyAddress?.Address ?? string.Empty,
                ProvinceName = companyAddress?.City ?? string.Empty,
                WardName = companyAddress?.Ward ?? string.Empty
            };
        }

        public async Task<ServiceResult> UpdateProfileAsync(string userId, RecruiterProfileUpdateViewModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return ServiceResult.NotFoundResult("Khong tim thay tai khoan.");

            var recruiter = (await _unitOfWork.Recruiters.FindAsync(r => r.UserId == userId)).FirstOrDefault();
            if (recruiter == null) return ServiceResult.NotFoundResult("Khong tim thay thong tin nha tuyen dung.");

            if (string.IsNullOrWhiteSpace(model.Phone))
            {
                return ServiceResult.Failed(new ServiceError { Key = nameof(model.Phone), Message = "So dien thoai la bat buoc." });
            }

            if (string.IsNullOrWhiteSpace(model.Position))
            {
                return ServiceResult.Failed(new ServiceError { Key = nameof(model.Position), Message = "Vi tri cong viec la bat buoc." });
            }
            if (!model.ProvinceCode.HasValue || string.IsNullOrWhiteSpace(model.ProvinceName))
            {
                return ServiceResult.Failed(new ServiceError { Key = nameof(model.ProvinceCode), Message = "Tỉnh/Thành phố là bắt buộc." });
            }
            if (!model.WardCode.HasValue || string.IsNullOrWhiteSpace(model.WardName))
            {
                return ServiceResult.Failed(new ServiceError { Key = nameof(model.WardCode), Message = "Phường/Xã là bắt buộc." });
            }
            if (string.IsNullOrWhiteSpace(model.WorkAddress))
            {
                return ServiceResult.Failed(new ServiceError { Key = nameof(model.WorkAddress), Message = "Địa chỉ làm việc là bắt buộc." });
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UpdatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;

            var userUpdate = await _userManager.UpdateAsync(user);
            if (!userUpdate.Succeeded)
            {
                return ServiceResult.Failed(userUpdate.Errors.Select(e => new ServiceError { Message = e.Description }).ToArray());
            }

            recruiter.FullName = $"{model.FirstName} {model.LastName}".Trim();
            recruiter.Phone = model.Phone;
            recruiter.Position = model.Position;
            recruiter.Department = model.Department;
            recruiter.UpdatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            _unitOfWork.Recruiters.Update(recruiter);

            Company company;
            if (recruiter.CompanyId.HasValue)
            {
                var existingCompany = await _unitOfWork.Companies.GetByIdAsync(recruiter.CompanyId.Value);
                if (existingCompany != null)
                {
                    company = existingCompany;
                }
                else
                {
                    company = new Company
                    {
                        CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam
                    };
                    await _unitOfWork.Companies.AddAsync(company);
                    await _unitOfWork.CompleteAsync();
                    recruiter.CompanyId = company.Id;
                }
            }
            else
            {
                company = new Company
                {
                    CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam
                };
                await _unitOfWork.Companies.AddAsync(company);
                await _unitOfWork.CompleteAsync();
                recruiter.CompanyId = company.Id;
            }

            if (!string.IsNullOrWhiteSpace(model.CompanyTaxCode))
            {
                var duplicatedTaxCode = (await _unitOfWork.Companies.FindAsync(c =>
                    c.TaxCode == model.CompanyTaxCode && c.Id != company.Id)).Any();
                if (duplicatedTaxCode)
                {
                    return ServiceResult.Failed(new ServiceError
                    {
                        Key = nameof(model.CompanyTaxCode),
                        Message = "Ma so thue da ton tai."
                    });
                }
            }

            company.Name = model.CompanyName;
            company.TaxCode = model.CompanyTaxCode;
            company.CompanySize = model.CompanySize;
            company.Industry = model.CompanyIndustry;
            company.Website = model.CompanyWebsite;
            company.Email = model.CompanyEmail;
            company.Phone = model.CompanyPhone;
            company.Description = model.CompanyDescription;
            company.UpdatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
            _unitOfWork.Companies.Update(company);

            var companyAddress = (await _unitOfWork.CompanyAddresses.FindAsync(x => x.CompanyId == company.Id))
                .OrderByDescending(x => x.IsHeadquarter)
                .FirstOrDefault();
            var isNewAddress = false;

            if (companyAddress == null)
            {
                companyAddress = new CompanyAddress
                {
                    CompanyId = company.Id,
                    CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam,
                    IsHeadquarter = true,
                    AddressType = "Workplace"
                };
                await _unitOfWork.CompanyAddresses.AddAsync(companyAddress);
                isNewAddress = true;
            }

            companyAddress.Address = model.WorkAddress;
            companyAddress.City = model.ProvinceName;
            companyAddress.Ward = model.WardName;
            companyAddress.Phone = model.CompanyPhone ?? model.Phone;
            if (!isNewAddress)
            {
                _unitOfWork.CompanyAddresses.Update(companyAddress);
            }

            _unitOfWork.Recruiters.Update(recruiter);
            await _unitOfWork.CompleteAsync();

            return ServiceResult.Success();
        }
    }
}
