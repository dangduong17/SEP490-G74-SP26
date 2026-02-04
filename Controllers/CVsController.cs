using Microsoft.AspNetCore.Mvc;

namespace SEP490_G74_RJMS.Controllers
{
    public class CVsController : Controller
    {
        // GET: CVs
        public IActionResult Index()
        {
            return View();
        }

        // GET: CVs/Details/5
        public IActionResult Details(int? id)
        {
            return View();
        }

        // GET: CVs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CVs/Create
        [HttpPost]
        public IActionResult CreatePost( /* cv model */
        )
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: CVs/Edit/5
        public IActionResult Edit(int? id)
        {
            return View();
        }

        // POST: CVs/Edit/5
        [HttpPost]
        public IActionResult Edit(
            int id /* cv model */
        )
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: CVs/Delete/5
        public IActionResult Delete(int? id)
        {
            return View();
        }

        // POST: CVs/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: CVs/SetDefault/5
        public IActionResult SetDefault(int? id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
