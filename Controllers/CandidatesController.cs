using Microsoft.AspNetCore.Mvc;

namespace SEP490_G74_RJMS.Controllers
{
    public class CandidatesController : Controller
    {
        // GET: Candidates
        public IActionResult Index()
        {
            return View();
        }

        // GET: Candidates/Details/5
        public IActionResult Details(int? id)
        {
            return View();
        }

        // GET: Candidates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Candidates/Create
        [HttpPost]
        public IActionResult CreatePost( /* candidate model */
        )
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: Candidates/Edit/5
        public IActionResult Edit(int? id)
        {
            return View();
        }

        // POST: Candidates/Edit/5
        [HttpPost]
        public IActionResult Edit(
            int id /* candidate model */
        )
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: Candidates/Delete/5
        public IActionResult Delete(int? id)
        {
            return View();
        }

        // POST: Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
