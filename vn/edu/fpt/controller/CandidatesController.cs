using Microsoft.AspNetCore.Mvc;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.controller
{
    public class CandidatesController : Controller
    {
        // JobList - Display list of available job postings
        public IActionResult JobList()
        {
            // TODO: Implement job list logic - select + filter (150 LOC)
            return View();
        }

        // JobDetail - Display job detail information
        public IActionResult JobDetail(int id)
        {
            // TODO: Implement job detail logic - select by id (100 LOC)
            return View();
        }

        // ApplyJob - Allow candidate to apply for a job
        public IActionResult ApplyJob(int jobId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult ApplyJob(Application application)
        {
            // TODO: Implement apply job logic - insert application (140 LOC)
            return RedirectToAction("ApplicationList");
        }

        // ApplicationList - Display applied jobs list
        public IActionResult ApplicationList()
        {
            // TODO: Implement application list logic - select (120 LOC)
            return View();
        }

        // ApplicationDetail - Display application detail
        public IActionResult ApplicationDetail(int id)
        {
            // TODO: Implement application detail logic - select by id (90 LOC)
            return View();
        }

        // CandidateDashboard - Overview applied jobs and status
        public IActionResult CandidateDashboard()
        {
            // TODO: Implement dashboard logic - summary select (130 LOC)
            return View();
        }
    }
}
