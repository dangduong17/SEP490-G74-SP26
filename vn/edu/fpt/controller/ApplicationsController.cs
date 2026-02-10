using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using vn.edu.fpt.repository;
using vn.edu.fpt.entity;
using vn.edu.fpt.dto;

namespace vn.edu.fpt.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApplicationsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetAll()
        {
            var applications = await _unitOfWork.Applications.GetAllAsync();
            var applicationDtos = _mapper.Map<IEnumerable<ApplicationDto>>(applications);
            return Ok(applicationDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationDto>> GetById(int id)
        {
            var application = await _unitOfWork.Applications.GetByIdAsync(id);
            if (application == null)
                return NotFound();

            var applicationDto = _mapper.Map<ApplicationDto>(application);
            return Ok(applicationDto);
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationDto>> Create(Application application)
        {
            await _unitOfWork.Applications.AddAsync(application);
            await _unitOfWork.CompleteAsync();

            var applicationDto = _mapper.Map<ApplicationDto>(application);
            return CreatedAtAction(nameof(GetById), new { id = application.Id }, applicationDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Application application)
        {
            if (id != application.Id)
                return BadRequest();

            _unitOfWork.Applications.Update(application);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var application = await _unitOfWork.Applications.GetByIdAsync(id);
            if (application == null)
                return NotFound();

            _unitOfWork.Applications.Delete(application);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
