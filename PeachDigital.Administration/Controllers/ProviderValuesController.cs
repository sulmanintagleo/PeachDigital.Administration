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
    public class ProviderValuesController : Controller
    {
        private PeachAdministrationEntities db = new PeachAdministrationEntities();

        // GET: ProviderValues
        public ActionResult Index()
        {
            var providerValues = db.ProviderValues.Include(p => p.Provider);
            return View(providerValues.ToList());
        }

        // GET: ProviderValues/Create
        public ActionResult Create()
        {
            ViewBag.ProviderId = new SelectList(db.Providers, "Id", "Name");

            ViewBag.ModeId = new SelectList(db.Modes, "Id", "ModeName");
            ViewBag.CircuitId = new SelectList(db.Circuits.Where(c => c.isActive), "Id", "Name");
            ViewBag.CinemaId = new SelectList(db.Cinemas.Where(c => c.isActive), "Id", "Name");

            return View();
        }

        // POST: ProviderValues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProviderId,ModeId,CircuitId,CinemaId,ProviderSetUpName,ServiceName,ProviderUrl,UserName,Password,ClientID,WorkStationCode,ClientClass,PerformPayment,TemplatePrintStream,ClubId")] ProviderValue providerValue)
        {
            if (ModelState.IsValid)
            {
                db.ProviderValues.Add(providerValue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProviderId = new SelectList(db.Providers, "Id", "Name", providerValue.ProviderId);
            return View(providerValue);
        }

        // GET: ProviderValues/Edit/5
        public ActionResult Edit(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Int64 providerId = Convert.ToInt64(CryptoProvider.Decrypt(EncId));
            ProviderValue providerValue = db.ProviderValues.Find(providerId);
            
            if (providerValue == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProviderId = new SelectList(db.Providers, "Id", "Name", providerValue.ProviderId);

            ViewBag.ModeId = new SelectList(db.Modes, "Id", "ModeName", providerValue.ModeId);
            ViewBag.CircuitId = new SelectList(db.Circuits.Where(c => c.isActive), "Id", "Name", providerValue.CircuitId);
            ViewBag.CinemaId = new SelectList(db.Cinemas.Where(c => c.isActive), "Id", "Name", providerValue.CinemaId);
            return View(providerValue);
        }

        // POST: ProviderValues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProviderId,ModeId,CircuitId,CinemaId,ProviderSetUpName,ServiceName,ProviderUrl,UserName,Password,ClientID,WorkStationCode,ClientClass,PerformPayment,TemplatePrintStream,ClubId")] ProviderValue providerValue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(providerValue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProviderId = new SelectList(db.Providers, "Id", "Name", providerValue.ProviderId);

            ViewBag.ModeId = new SelectList(db.Modes, "Id", "ModeName", providerValue.ModeId);
            ViewBag.CircuitId = new SelectList(db.Circuits.Where(c => c.isActive), "Id", "Name", providerValue.CircuitId);
            ViewBag.CinemaId = new SelectList(db.Cinemas.Where(c => c.isActive), "Id", "Name", providerValue.CinemaId);
            return View(providerValue);
        }

        // GET: ProviderValues/Delete/5
        public ActionResult Delete(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Int64 providerId = Convert.ToInt64(CryptoProvider.Decrypt(EncId));
            ProviderValue providerValue = db.ProviderValues.Find(providerId);

            if (providerValue == null)
            {
                return HttpNotFound();
            }
            return View(providerValue);
        }

        // POST: ProviderValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string EncId)
        {
            Int64 providerId = Convert.ToInt64(CryptoProvider.Decrypt(EncId));
            ProviderValue providerValue = db.ProviderValues.Find(providerId);
            
            db.ProviderValues.Remove(providerValue);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetProvidersByPaging()
        {
            using (PeachAdministrationEntities con = new PeachAdministrationEntities())
            {
                int start = Convert.ToInt32(Request.QueryString["iDisplayStart"]);
                int length = Convert.ToInt32(Request.QueryString["iDisplayLength"]);

                int totalResultsCount;
                var result = GetAllProviderData(length, start, out totalResultsCount);

                if (result != null && result.Any())
                {
                    var res = result.Select(m => new
                    {
                        Id = m.Id,
                        ProviderName = m.Provider.Name,
                        Mode = m.Mode.ModeName,
                        Circuit = m.CircuitId != null ? con.Circuits.FirstOrDefault(c => c.isActive && c.Id == m.CircuitId) != null ? con.Circuits.First(c => c.isActive && c.Id == m.CircuitId).Name : "N/A" : "N/A",
                        Cinema = m.CinemaId != null ? con.Cinemas.FirstOrDefault(c => c.isActive && c.Id == m.CinemaId) != null ? con.Cinemas.First(c => c.isActive && c.Id == m.CinemaId).Name : "N/A" : "N/A",
                        ProviderSetUpName = m.ProviderSetUpName,
                        ServiceName = m.ServiceName,
                        ProviderUrl = m.ProviderUrl,
                        Username = m.UserName,
                        Password = m.Password,
                        ClientId = m.ClientID,
                        WorkStationCode = m.WorkStationCode,
                        ClientClass = m.ClientClass,
                        PerformPayment = m.PerformPayment,
                        TemplatePrintStream = m.TemplatePrintStream,
                        ClubId = m.ClubId,
                        Actions = "<a class='edit-icon' href=\"/ProviderValues/Edit?EncId=" + CryptoProvider.Encrypt(m.Id) + " \"><span data-toggle='tooltip' data-placement='left' title='Edit' class='fa fa-pencil'></span> </a><a class='delete-icon' href=\"/ProviderValues/Delete?EncId=" + CryptoProvider.Encrypt(m.Id) + " \"> <span data-toggle='tooltip' data-placement='right' title='Delete' class='fa fa-trash-o'></span> </a>"
                    }).ToList();
                    return Json(new { recordsFiltered = totalResultsCount, data = res.ToList(), recordsTotal = totalResultsCount }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { recordsFiltered = totalResultsCount, data = "", recordsTotal = totalResultsCount }, JsonRequestBehavior.AllowGet);
            }
        }

        public List<ProviderValue> GetAllProviderData(int take, int skip, out int totalResultsCount)
        {
            using (PeachAdministrationEntities con = new PeachAdministrationEntities())
            {
                var result = con.ProviderValues/*.Where(c => c.isActive)*/.Select(m => new
                {
                    Id = m.Id,
                    Provider = m.Provider,
                    Mode = m.Mode,
                    CircuitId = m.CircuitId,
                    CinemaId = m.CinemaId,
                    ProviderSetUpName = m.ProviderSetUpName,
                    ServiceName = m.ServiceName,
                    ProviderUrl = m.ProviderUrl,
                    UserName = m.UserName,
                    Password = m.Password,
                    ClientID = m.ClientID,
                    WorkStationCode= m.WorkStationCode,
                    ClientClass = m.ClientClass,
                    PerformPayment = m.PerformPayment,
                    TemplatePrintStream = m.TemplatePrintStream,
                    ClubId = m.ClubId
                }).ToList().OrderByDescending(o => o.Id).Select(m => new ProviderValue
                {
                    Id = m.Id,
                    Provider = m.Provider,
                    Mode = m.Mode,
                    CircuitId = m.CircuitId,
                    CinemaId = m.CinemaId,
                    ProviderSetUpName = m.ProviderSetUpName,
                    ServiceName = m.ServiceName,
                    ProviderUrl = m.ProviderUrl,
                    UserName = m.UserName,
                    Password = m.Password,
                    ClientID = m.ClientID,
                    WorkStationCode = m.WorkStationCode,
                    ClientClass = m.ClientClass,
                    PerformPayment = m.PerformPayment,
                    TemplatePrintStream = m.TemplatePrintStream,
                    ClubId = m.ClubId
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