using AutoMapper;
using vn.edu.fpt.dto;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.mapper
{
    public class AdminMappingProfile : Profile
    {
        public AdminMappingProfile()
        {
            CreateMap<AdminCreateAdminViewModel, Admin>()
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<AdminCreateCandidateViewModel, Candidate>()
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CVs, opt => opt.Ignore())
                .ForMember(dest => dest.Applications, opt => opt.Ignore())
                .ForMember(dest => dest.SavedJobs, opt => opt.Ignore())
                .ForMember(dest => dest.FollowedCompanies, opt => opt.Ignore())
                .ForMember(dest => dest.Skills, opt => opt.Ignore())
                .ForMember(dest => dest.Educations, opt => opt.Ignore())
                .ForMember(dest => dest.WorkExperiences, opt => opt.Ignore());

            CreateMap<AdminCreateRecruiterViewModel, Recruiter>()
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.Company, opt => opt.Ignore())
                .ForMember(dest => dest.Jobs, opt => opt.Ignore());

            CreateMap<AdminCreateRecruiterViewModel, Company>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.TaxCode, opt => opt.MapFrom(src => src.CompanyTaxCode))
                .ForMember(dest => dest.CompanySize, opt => opt.MapFrom(src => src.CompanySize))
                .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => src.CompanyIndustry))
                .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.CompanyWebsite))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.CompanyEmail))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.CompanyPhone))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.CompanyDescription))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsVerified, opt => opt.Ignore())
                .ForMember(dest => dest.VerifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Logo, opt => opt.Ignore())
                .ForMember(dest => dest.CoverImage, opt => opt.Ignore())
                .ForMember(dest => dest.Benefits, opt => opt.Ignore())
                .ForMember(dest => dest.Addresses, opt => opt.Ignore())
                .ForMember(dest => dest.Jobs, opt => opt.Ignore())
                .ForMember(dest => dest.Recruiters, opt => opt.Ignore())
                .ForMember(dest => dest.Followers, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<AdminUpdateUserViewModel, User>()
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Candidate, opt => opt.Ignore())
                .ForMember(dest => dest.Recruiter, opt => opt.Ignore())
                .ForMember(dest => dest.Admin, opt => opt.Ignore());

            CreateMap<AdminUpdateUserViewModel, Admin>()
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.AdminDepartment))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<AdminUpdateUserViewModel, Candidate>()
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.CandidateTitle))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.CandidateCity))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.CandidateDateOfBirth))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.CandidateGender))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CVs, opt => opt.Ignore())
                .ForMember(dest => dest.Applications, opt => opt.Ignore())
                .ForMember(dest => dest.SavedJobs, opt => opt.Ignore())
                .ForMember(dest => dest.FollowedCompanies, opt => opt.Ignore())
                .ForMember(dest => dest.Skills, opt => opt.Ignore())
                .ForMember(dest => dest.Educations, opt => opt.Ignore())
                .ForMember(dest => dest.WorkExperiences, opt => opt.Ignore());

            CreateMap<AdminUpdateUserViewModel, Recruiter>()
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.RecruiterPosition))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.RecruiterDepartment))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.Company, opt => opt.Ignore())
                .ForMember(dest => dest.Jobs, opt => opt.Ignore());
        }
    }
}
