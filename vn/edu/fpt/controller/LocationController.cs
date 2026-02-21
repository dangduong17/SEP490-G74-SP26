using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vn.edu.fpt.service.Interfaces;

namespace vn.edu.fpt.controller
{
    [Authorize(Roles = "Admin,Recruiter")]
    public class LocationController : Controller
    {
        private readonly ILocationLookupService _locationLookupService;

        public LocationController(ILocationLookupService locationLookupService)
        {
            _locationLookupService = locationLookupService;
        }

        [HttpGet]
        public async Task<IActionResult> Provinces()
        {
            var provinces = await _locationLookupService.GetProvincesAsync();
            return Json(provinces);
        }

        [HttpGet]
        public async Task<IActionResult> Wards(int provinceCode)
        {
            if (provinceCode <= 0) return Json(new List<object>());
            var wards = await _locationLookupService.GetWardsByProvinceCodeAsync(provinceCode);
            return Json(wards);
        }
    }
}
