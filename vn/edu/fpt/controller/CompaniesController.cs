using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using vn.edu.fpt.repository;
using vn.edu.fpt.entity;
using vn.edu.fpt.dto;

namespace vn.edu.fpt.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompaniesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAll()
        {
            var companies = await _unitOfWork.Companies.GetAllAsync();
            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companyDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetById(int id)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(id);
            if (company == null)
                return NotFound();

            var companyDto = _mapper.Map<CompanyDto>(company);
            return Ok(companyDto);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyDto>> Create(Company company)
        {
            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.CompleteAsync();

            var companyDto = _mapper.Map<CompanyDto>(company);
            return CreatedAtAction(nameof(GetById), new { id = company.Id }, companyDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Company company)
        {
            if (id != company.Id)
                return BadRequest();

            _unitOfWork.Companies.Update(company);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(id);
            if (company == null)
                return NotFound();

            _unitOfWork.Companies.Delete(company);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
