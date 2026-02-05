using Microsoft.AspNetCore.Mvc;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.controller
{
    public class CVsController : Controller
    {
        // CVList - Display list of candidate CVs
        public IActionResult CVList()
        {
            // TODO: Implement CV list logic - select (120 LOC)
            return View();
        }

        // CreateCV - Allow candidate to create new CV
        public IActionResult CreateCV()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCV(CV cv)
        {
            // TODO: Implement create CV logic - insert (160 LOC)
            return RedirectToAction("CVList");
        }

        // CVDetail - Display CV detail information
        public IActionResult CVDetail(int id)
        {
            // TODO: Implement CV detail logic - select by id (110 LOC)
            return View();
        }

        // EditCV - Allow candidate to edit CV content
        public IActionResult EditCV(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditCV(CV cv)
        {
            // TODO: Implement edit CV logic - update (150 LOC)
            return RedirectToAction("CVDetail", new { id = cv.Id });
        }

        // DownloadCV - Download CV in PDF format
        public IActionResult DownloadCV(int id)
        {
            // TODO: Implement download CV logic - export pdf (100 LOC)
            return File(new byte[0], "application/pdf", "CV.pdf");
        }
    }
}
