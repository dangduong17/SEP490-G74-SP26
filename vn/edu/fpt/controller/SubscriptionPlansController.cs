using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using vn.edu.fpt.repository;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionPlansController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubscriptionPlansController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionPlan>>> GetAll()
        {
            var plans = await _unitOfWork.SubscriptionPlans.GetAllAsync();
            return Ok(plans);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionPlan>> GetById(int id)
        {
            var plan = await _unitOfWork.SubscriptionPlans.GetByIdAsync(id);
            if (plan == null)
                return NotFound();

            return Ok(plan);
        }

        [HttpPost]
        public async Task<ActionResult<SubscriptionPlan>> Create(SubscriptionPlan plan)
        {
            await _unitOfWork.SubscriptionPlans.AddAsync(plan);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetById), new { id = plan.Id }, plan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SubscriptionPlan plan)
        {
            if (id != plan.Id)
                return BadRequest();

            _unitOfWork.SubscriptionPlans.Update(plan);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var plan = await _unitOfWork.SubscriptionPlans.GetByIdAsync(id);
            if (plan == null)
                return NotFound();

            _unitOfWork.SubscriptionPlans.Delete(plan);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
