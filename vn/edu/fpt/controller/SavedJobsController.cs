using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using vn.edu.fpt.repository;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SavedJobsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SavedJobsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SavedJob>>> GetAll()
        {
            var savedJobs = await _unitOfWork.SavedJobs.GetAllAsync();
            return Ok(savedJobs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SavedJob>> GetById(int id)
        {
            var savedJob = await _unitOfWork.SavedJobs.GetByIdAsync(id);
            if (savedJob == null)
                return NotFound();

            return Ok(savedJob);
        }

        [HttpPost]
        public async Task<ActionResult<SavedJob>> Create(SavedJob savedJob)
        {
            await _unitOfWork.SavedJobs.AddAsync(savedJob);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetById), new { id = savedJob.Id }, savedJob);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var savedJob = await _unitOfWork.SavedJobs.GetByIdAsync(id);
            if (savedJob == null)
                return NotFound();

            _unitOfWork.SavedJobs.Delete(savedJob);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
