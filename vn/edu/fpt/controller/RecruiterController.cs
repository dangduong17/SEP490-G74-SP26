using Microsoft.AspNetCore.Mvc;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.controller
{
    public class RecruiterController : Controller
    {
        // RecruiterDashboard - Overview job postings and applications
        public IActionResult RecruiterDashboard()
        {
            // TODO: Implement recruiter dashboard logic - summary select (140 LOC)
            return View();
        }

        // JobPostingList - Display recruiter job postings
        public IActionResult JobPostingList()
        {
            // TODO: Implement job posting list logic - select (120 LOC)
            return View();
        }

        // CreateJob - Allow recruiter to create job posting
        public IActionResult CreateJob()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateJob(Job job)
        {
            // TODO: Implement create job logic - insert (160 LOC)
            return RedirectToAction("JobPostingList");
        }

        // EditJob - Allow recruiter to edit job posting
        public IActionResult EditJob(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditJob(Job job)
        {
            // TODO: Implement edit job logic - update (150 LOC)
            return RedirectToAction("JobPostingList");
        }

        // ApplicationList - View applications by job
        public IActionResult ApplicationList(int? jobId)
        {
            // TODO: Implement application list logic - select join (140 LOC)
            return View();
        }

        // UpdateApplicationStatus - Update application status
        [HttpPost]
        public IActionResult UpdateApplicationStatus(int applicationId, ApplicationStatus status)
        {
            // TODO: Implement update application status logic - update (120 LOC)
            return RedirectToAction("ApplicationList");
        }
    }
}
