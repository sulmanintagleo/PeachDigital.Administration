using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PeachDigital.Administration.Models;

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

        // GET: ProviderValues/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderValue providerValue = db.ProviderValues.Find(id);
            if (providerValue == null)
            {
                return HttpNotFound();
            }
            return View(providerValue);
        }

        // GET: ProviderValues/Create
        public ActionResult Create()
        {
            ViewBag.ProviderId = new SelectList(db.Providers, "Id", "Name");
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
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderValue providerValue = db.ProviderValues.Find(id);
            if (providerValue == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProviderId = new SelectList(db.Providers, "Id", "Name", providerValue.ProviderId);
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
            return View(providerValue);
        }

        // GET: ProviderValues/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderValue providerValue = db.ProviderValues.Find(id);
            if (providerValue == null)
            {
                return HttpNotFound();
            }
            return View(providerValue);
        }

        // POST: ProviderValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ProviderValue providerValue = db.ProviderValues.Find(id);
            db.ProviderValues.Remove(providerValue);
            db.SaveChanges();
            return RedirectToAction("Index");
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
