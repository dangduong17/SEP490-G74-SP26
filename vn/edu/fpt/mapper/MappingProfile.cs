using AutoMapper;
using vn.edu.fpt.entity;
using vn.edu.fpt.dto;

namespace vn.edu.fpt.mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : null));

            CreateMap<Job, JobDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.RecruiterName, opt => opt.MapFrom(src => src.Recruiter != null ? $"{src.Recruiter.FirstName} {src.Recruiter.LastName}" : null))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : null));

            CreateMap<CV, CVDto>()
                .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(src => src.Candidate != null ? $"{src.Candidate.FirstName} {src.Candidate.LastName}" : null));

            CreateMap<Application, ApplicationDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job != null ? src.Job.Title : null))
                .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(src => src.Candidate != null ? $"{src.Candidate.FirstName} {src.Candidate.LastName}" : null))
                .ForMember(dest => dest.CVTitle, opt => opt.MapFrom(src => src.CV != null ? src.CV.Title : null));

            CreateMap<Company, CompanyDto>();
        }
    }
}
