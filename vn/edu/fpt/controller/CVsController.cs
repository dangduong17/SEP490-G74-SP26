using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using vn.edu.fpt.repository;
using vn.edu.fpt.entity;
using vn.edu.fpt.dto;

namespace vn.edu.fpt.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CVsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CVsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CVDto>>> GetAll()
        {
            var cvs = await _unitOfWork.CVs.GetAllAsync();
            var cvDtos = _mapper.Map<IEnumerable<CVDto>>(cvs);
            return Ok(cvDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CVDto>> GetById(int id)
        {
            var cv = await _unitOfWork.CVs.GetByIdAsync(id);
            if (cv == null)
                return NotFound();

            var cvDto = _mapper.Map<CVDto>(cv);
            return Ok(cvDto);
        }

        [HttpPost]
        public async Task<ActionResult<CVDto>> Create(CV cv)
        {
            await _unitOfWork.CVs.AddAsync(cv);
            await _unitOfWork.CompleteAsync();

            var cvDto = _mapper.Map<CVDto>(cv);
            return CreatedAtAction(nameof(GetById), new { id = cv.Id }, cvDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CV cv)
        {
            if (id != cv.Id)
                return BadRequest();

            _unitOfWork.CVs.Update(cv);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cv = await _unitOfWork.CVs.GetByIdAsync(id);
            if (cv == null)
                return NotFound();

            _unitOfWork.CVs.Delete(cv);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
