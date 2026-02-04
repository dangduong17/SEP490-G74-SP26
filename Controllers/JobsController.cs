using Microsoft.AspNetCore.Mvc;

namespace SEP490_G74_RJMS.Controllers
{
    public class JobsController : Controller
    {
        // GET: Jobs
        public IActionResult Index()
        {
            return View();
        }

        // GET: Jobs/Details/5
        public IActionResult Details(int? id)
        {
            return View();
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        [HttpPost]
        public IActionResult CreatePost( /* job model */
        )
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: Jobs/Edit/5
        public IActionResult Edit(int? id)
        {
            return View();
        }

        // POST: Jobs/Edit/5
        [HttpPost]
        public IActionResult Edit(
            int id /* job model */
        )
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: Jobs/Delete/5
        public IActionResult Delete(int? id)
        {
            return View();
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: Jobs/Apply/5
        public IActionResult Apply(int? id)
        {
            return View();
        }

        // POST: Jobs/Apply/5
        [HttpPost]
        public IActionResult Apply( /* application model */
        )
        {
            return RedirectToAction(
                "Details",
                new
                { /* id */
                }
            );
        }
    }
}
