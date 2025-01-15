using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace K21CNT2_BuiTienAnh_2110900003Controllers
{
    public class FAQsController : Controller
    {
        // GET: FAQsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: FAQsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FAQsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FAQsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FAQsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FAQsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FAQsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FAQsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
