using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using vn.edu.fpt.repository;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SkillsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skill>>> GetAll()
        {
            var skills = await _unitOfWork.Skills.GetAllAsync();
            return Ok(skills);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Skill>> GetById(int id)
        {
            var skill = await _unitOfWork.Skills.GetByIdAsync(id);
            if (skill == null)
                return NotFound();

            return Ok(skill);
        }

        [HttpPost]
        public async Task<ActionResult<Skill>> Create(Skill skill)
        {
            await _unitOfWork.Skills.AddAsync(skill);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetById), new { id = skill.Id }, skill);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Skill skill)
        {
            if (id != skill.Id)
                return BadRequest();

            _unitOfWork.Skills.Update(skill);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var skill = await _unitOfWork.Skills.GetByIdAsync(id);
            if (skill == null)
                return NotFound();

            _unitOfWork.Skills.Delete(skill);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
