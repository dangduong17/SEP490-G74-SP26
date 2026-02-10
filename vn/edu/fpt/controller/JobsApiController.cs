using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using vn.edu.fpt.repository;
using vn.edu.fpt.entity;
using vn.edu.fpt.dto;

namespace vn.edu.fpt.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JobsApiController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobDto>>> GetAll()
        {
            var jobs = await _unitOfWork.Jobs.GetAllAsync();
            var jobDtos = _mapper.Map<IEnumerable<JobDto>>(jobs);
            return Ok(jobDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobDto>> GetById(int id)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(id);
            if (job == null)
                return NotFound();

            var jobDto = _mapper.Map<JobDto>(job);
            return Ok(jobDto);
        }

        [HttpPost]
        public async Task<ActionResult<JobDto>> Create(Job job)
        {
            await _unitOfWork.Jobs.AddAsync(job);
            await _unitOfWork.CompleteAsync();

            var jobDto = _mapper.Map<JobDto>(job);
            return CreatedAtAction(nameof(GetById), new { id = job.Id }, jobDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Job job)
        {
            if (id != job.Id)
                return BadRequest();

            _unitOfWork.Jobs.Update(job);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(id);
            if (job == null)
                return NotFound();

            _unitOfWork.Jobs.Delete(job);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
