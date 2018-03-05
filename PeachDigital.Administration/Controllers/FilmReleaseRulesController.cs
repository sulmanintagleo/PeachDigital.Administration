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
    public class FilmReleaseRulesController : Controller
    {
        private PeachAdministrationEntities db = new PeachAdministrationEntities();

        // GET: FilmReleaseRules
        public ActionResult Index()
        {
            return View();
        }

        // GET: FilmReleaseRules/Create
        public ActionResult Create()
        {
            ViewBag.CircuitId = new SelectList(db.Circuits, "Id", "Name");
            ViewBag.IsScheduled = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text");
            ViewBag.ComingSoon = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text");
            ViewBag.ComingSoonByCinema = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text");
            return View();
        }

        // POST: FilmReleaseRules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CircuitId,FilmReleaseTypeId,OpeningDate,DateOfFirstSession,DateOfLastSession,IsScheduled,ComingSoon,ComingSoonAttribute,ComingSoonByCinema")] FilmReleaseRule filmReleaseRule)
        {
            if (filmReleaseRule.CircuitId <= 0)
            {
                ModelState.AddModelError("CircuitId", "Circuit Id is required");
            }

            if (filmReleaseRule.FilmReleaseTypeId <= 0)
            {
                ModelState.AddModelError("FilmReleaseTypeId", "Film release type id is required");
            }

            if (ModelState.IsValid)
            {
                var ruleExist = db.FilmReleaseRules.Where(f => f.CircuitId == filmReleaseRule.CircuitId && f.FilmReleaseTypeId == filmReleaseRule.FilmReleaseTypeId).Any();
                if (ruleExist)
                {
                    ViewBag.ErrorMessage = "The record already exists. Please change circuit or release rule type.";
                }
                else
                {
                    filmReleaseRule.IsActive = true;
                    db.FilmReleaseRules.Add(filmReleaseRule);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CircuitId = new SelectList(db.Circuits.Where(c => c.isActive).ToList(), "Id", "Name", filmReleaseRule.CircuitId);
            ViewBag.IsScheduled = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text",filmReleaseRule.IsScheduled);
            ViewBag.ComingSoon = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text",filmReleaseRule.ComingSoon);
            ViewBag.ComingSoonByCinema = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text",filmReleaseRule.ComingSoonByCinema);
            return View(filmReleaseRule);
        }

        // GET: FilmReleaseRules/Edit/5
        public ActionResult Edit(string circuit, string filmReleaseType)
        {
            if (string.IsNullOrEmpty(circuit) && string.IsNullOrEmpty(filmReleaseType))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Int64 circuitId = Convert.ToInt64(CryptoProvider.Decrypt(circuit));
            Int32 filmReleaseTypeId = Convert.ToInt32(CryptoProvider.Decrypt(filmReleaseType));
            FilmReleaseRule filmReleaseRule = db.FilmReleaseRules.Where(f => f.CircuitId == circuitId && f.FilmReleaseTypeId == filmReleaseTypeId).FirstOrDefault();
            if (filmReleaseRule == null)
            {
                return HttpNotFound();
            }
            ViewBag.CircuitList = new SelectList(db.Circuits.Where(c=>c.isActive), "Id", "Name", filmReleaseRule.CircuitId);
            ViewBag.IsScheduled = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text", filmReleaseRule.IsScheduled);
            ViewBag.ComingSoon = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text", filmReleaseRule.ComingSoon);
            ViewBag.ComingSoonByCinema = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text", filmReleaseRule.ComingSoonByCinema);
            return View(filmReleaseRule);
        }

        // POST: FilmReleaseRules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CircuitId,FilmReleaseTypeId,OpeningDate,DateOfFirstSession,DateOfLastSession,IsScheduled,ComingSoon,ComingSoonAttribute,ComingSoonByCinema")] FilmReleaseRule filmReleaseRule)
        {
            if (filmReleaseRule.CircuitId <= 0)
            {
                ModelState.AddModelError("CircuitId", "Circuit Id is required");
            }

            if (filmReleaseRule.FilmReleaseTypeId <= 0)
            {
                ModelState.AddModelError("FilmReleaseTypeId", "Film release type id is required");
            }

            if (ModelState.IsValid)
            {
                filmReleaseRule.IsActive = true;
                db.Entry(filmReleaseRule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CircuitId = new SelectList(db.Circuits.Where(c=>c.isActive), "Id", "Name", filmReleaseRule.CircuitId);
            ViewBag.IsScheduled = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text", filmReleaseRule.IsScheduled);
            ViewBag.ComingSoon = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text", filmReleaseRule.ComingSoon);
            ViewBag.ComingSoonByCinema = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text", filmReleaseRule.ComingSoonByCinema);
            return View(filmReleaseRule);
        }

        // GET: FilmReleaseRules/Delete/5
        public ActionResult Delete(string circuit, string filmReleaseType)
        {
            if (string.IsNullOrEmpty(circuit) && string.IsNullOrEmpty(filmReleaseType))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Int64 circuitId = Convert.ToInt64(CryptoProvider.Decrypt(circuit));
            Int32 filmReleaseTypeId = Convert.ToInt32(CryptoProvider.Decrypt(filmReleaseType));
            FilmReleaseRule filmReleaseRule = db.FilmReleaseRules.Where(f => f.CircuitId == circuitId && f.FilmReleaseTypeId == filmReleaseTypeId).FirstOrDefault();

            if (filmReleaseRule == null)
            {
                return HttpNotFound();
            }
            return View(filmReleaseRule);
        }

        // POST: FilmReleaseRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string circuit, string filmReleaseType)
        {
            Int64 circuitId = Convert.ToInt64(CryptoProvider.Decrypt(circuit));
            Int32 filmReleaseTypeId = Convert.ToInt32(CryptoProvider.Decrypt(filmReleaseType));
            FilmReleaseRule filmReleaseRule = db.FilmReleaseRules.Where(f => f.CircuitId == circuitId && f.FilmReleaseTypeId == filmReleaseTypeId).FirstOrDefault();
            filmReleaseRule.IsActive = false;
            //db.FilmReleaseRules.Remove(filmReleaseRule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetFilmReleaseRulesByPaging()
        {
            int start = Convert.ToInt32(Request.QueryString["iDisplayStart"]);
            int length = Convert.ToInt32(Request.QueryString["iDisplayLength"]);

            int totalResultsCount;
            var result = GetAllFilmReleaseRulesData(length, start, out totalResultsCount);

            if (result != null && result.Any())
            {
                var res = result.Select(m => new
                {
                    //CircuitId = m.CircuitId,
                    //FilmReleaseTypeId = m.FilmReleaseTypeId,
                    OpeningDate = m.OpeningDate,
                    DateOfFirstSession = m.DateOfFirstSession,
                    DateOfLastSession = m.DateOfLastSession,
                    IsScheduled = m.IsScheduled,
                    ComingSoon = m.ComingSoon,
                    ComingSoonAttribute = m.ComingSoonAttribute,
                    ComingSoonByCinema = m.ComingSoonByCinema,
                    CircuitName = m.Circuit.Name,
                    Actions = "<a class='edit-icon' href=\"/FilmReleaseRules/Edit?circuit=" + CryptoProvider.Encrypt(m.CircuitId) + "&filmReleaseType=" + CryptoProvider.Encrypt(m.FilmReleaseTypeId) + "\"> <span data-toggle='tooltip' data-placement='left' title='Edit' class='fa fa-pencil'></span> </a> <a class='delete-icon' href=\"/FilmReleaseRules/Delete?circuit=" + CryptoProvider.Encrypt(m.CircuitId) + "&filmReleaseType=" + CryptoProvider.Encrypt(m.FilmReleaseTypeId) + " \"> <span data-toggle='tooltip' data-placement='right' title='Delete' class='fa fa-trash-o'></span> </a>"
                }).ToList();
                return Json(new { recordsFiltered = totalResultsCount, data = res.ToList(), recordsTotal = totalResultsCount }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { recordsFiltered = totalResultsCount, data = "", recordsTotal = totalResultsCount }, JsonRequestBehavior.AllowGet);
        }

        public List<FilmReleaseRule> GetAllFilmReleaseRulesData(int take, int skip, out int totalResultsCount)
        {
            using (PeachAdministrationEntities con = new PeachAdministrationEntities())
            {
                var result = con.FilmReleaseRules.Where(c => c.IsActive).Select(m => new
                {
                    CircuitId = m.CircuitId,
                    FilmReleaseTypeId = m.FilmReleaseTypeId,
                    OpeningDate = m.OpeningDate,
                    DateOfFirstSession = m.DateOfFirstSession,
                    DateOfLastSession = m.DateOfLastSession,
                    IsScheduled = m.IsScheduled,
                    ComingSoon = m.ComingSoon,
                    ComingSoonAttribute = m.ComingSoonAttribute,
                    ComingSoonByCinema = m.ComingSoonByCinema,
                    Circuit = m.Circuit
                }).ToList().OrderByDescending(o => o.CircuitId).Select(m => new FilmReleaseRule()
                {
                    CircuitId = m.CircuitId,
                    FilmReleaseTypeId = m.FilmReleaseTypeId,
                    OpeningDate = m.OpeningDate,
                    DateOfFirstSession = m.DateOfFirstSession,
                    DateOfLastSession = m.DateOfLastSession,
                    IsScheduled = m.IsScheduled,
                    ComingSoon = m.ComingSoon,
                    ComingSoonAttribute = m.ComingSoonAttribute,
                    ComingSoonByCinema = m.ComingSoonByCinema,
                    Circuit = m.Circuit
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