using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PeachDigital.Administration.Models;
using PeachDigital.Administration.Common.Helper;

namespace PeachDigital.Administration.Controllers
{
    public class ModulesController : Controller
    {
        private PeachAdministrationEntities db = new PeachAdministrationEntities();

        // GET: Modules
        [AuthorizeUser("Modules", "View")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Modules/Create
        [AuthorizeUser("Modules", "Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AuthorizeUser("Modules", "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Module module)
        {
            if (string.IsNullOrEmpty(module.Name))
            {
                ModelState.AddModelError("Name", "Module name is required");
            }

            if (ModelState.IsValid)
            {
                //Duplication Check
                var moduleExist = db.Modules.Any(u => u.Name == module.Name);
                if (moduleExist)
                {
                    ViewBag.ErrorMessage = "Module already Exist in the system";
                    return View(module);
                }
                module.isActive = true;
                db.Modules.Add(module);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(module);
        }

        // GET: Modules/Edit/5
        [AuthorizeUser("Modules", "Update")]
        public ActionResult Edit(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int moduleId = Convert.ToInt32(CryptoProvider.Decrypt(EncId));
            Module module = db.Modules.Find(moduleId);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AuthorizeUser("Modules", "Update")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Module module)
        {
            if (string.IsNullOrEmpty(module.Name))
            {
                ModelState.AddModelError("Name", "Module name is required");
            }

            if (ModelState.IsValid)
            {
                //Duplication Check
                var moduleExist = db.Modules.Any(u => u.Name == module.Name && u.Id != module.Id);
                if (moduleExist)
                {
                    ViewBag.ErrorMessage = "Module already Exist in the system";
                    return View(module);
                }
                module.isActive = true;
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(module);
        }

        // GET: Modules/Delete/5
        [AuthorizeUser("Modules", "Delete")]
        public ActionResult Delete(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int moduleId = Convert.ToInt32(CryptoProvider.Decrypt(EncId));
            Module module = db.Modules.Find(moduleId);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [AuthorizeUser("Modules", "Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string EncId)
        {
            int moduleId = Convert.ToInt32(CryptoProvider.Decrypt(EncId));
            Module module = db.Modules.Find(moduleId);
            module.isActive = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetModulesByPaging()
        {
            int start = Convert.ToInt32(Request.QueryString["iDisplayStart"]);
            int length = Convert.ToInt32(Request.QueryString["iDisplayLength"]);

            int totalResultsCount;
            var result = GetAllModuleData(length, start, out totalResultsCount);

            if (result != null && result.Any())
            {
                var res = result.Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name,
                    Actions = "<a class='edit-icon' href=\"/Modules/Edit?EncId=" + CryptoProvider.Encrypt(m.Id) + " \"><span data-toggle='tooltip' data-placement='left' title='Edit' class='fa fa-pencil'></span> </a> <a class='delete-icon' href=\"/Modules/Delete?EncId=" + CryptoProvider.Encrypt(m.Id) + " \"> <span data-toggle='tooltip' data-placement='right' title='Delete' class='fa fa-trash-o'></span> </a>"
                }).ToList();
                return Json(new { recordsFiltered = totalResultsCount, data = res.ToList(), recordsTotal = totalResultsCount }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { recordsFiltered = totalResultsCount, data = "", recordsTotal = totalResultsCount }, JsonRequestBehavior.AllowGet);
        }

        public List<Module> GetAllModuleData(int take, int skip, out int totalResultsCount)
        {
            using (PeachAdministrationEntities con = new PeachAdministrationEntities())
            {
                var result = con.Modules.Where(c => c.isActive).Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList().OrderByDescending(o => o.Id).Select(m => new Module()
                {
                    Id = m.Id,
                    Name = m.Name
                });
                if (result != null && result.Any())
                {
                    totalResultsCount = result.Count();
                    var res = result.Skip(skip).Take(take).ToList();
                    return res;
                }
                
                totalResultsCount = 0;
                return null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
