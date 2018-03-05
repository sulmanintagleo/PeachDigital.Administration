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
    public class CircuitsController : Controller
    {
        private PeachAdministrationEntities db = new PeachAdministrationEntities();

        // GET: Circuits
        public ActionResult Index()
        {
            return View();
        }

        // GET: Circuits/Create
        public ActionResult Create()
        {
            ViewBag.ContactID = new SelectList(db.Contacts, "Id", "BookingLine");
            ViewBag.CountryID = new SelectList(db.Countries, "Id", "Name");
            ViewBag.HeadOfficeID = new SelectList(db.HeadOffices, "Id", "Address");
            ViewBag.MapPointID = new SelectList(db.MapPoints, "Id", "Lat");
            ViewBag.UseLoyalty = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text");
            return View();
        }

        // POST: Circuits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,BaseURI,UserName,Password,Open,Public,CountryID,ContactID,MapPointID,UseLoyalty,OrderTimeoutSeconds,isActive,HeadOfficeID")] Circuit circuit)
        {
            if (string.IsNullOrEmpty(circuit.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
            }

            if (string.IsNullOrEmpty(circuit.BaseURI))
            {
                ModelState.AddModelError("BaseURI", "Base URI is required");
            }

            if (ModelState.IsValid)
            {
                circuit.isActive = true;
                db.Circuits.Add(circuit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContactID = new SelectList(db.Contacts, "Id", "BookingLine", circuit.ContactID);
            ViewBag.CountryID = new SelectList(db.Countries, "Id", "Name", circuit.CountryID);
            ViewBag.HeadOfficeID = new SelectList(db.HeadOffices, "Id", "Address", circuit.HeadOfficeID);
            ViewBag.MapPointID = new SelectList(db.MapPoints, "Id", "Lat", circuit.MapPointID);
            ViewBag.UseLoyalty = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text", circuit.UseLoyalty);
            return View(circuit);
        }

        // GET: Circuits/Edit/5
        public ActionResult Edit(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Int64 circuitId = Convert.ToInt64(CryptoProvider.Decrypt(EncId));
            Circuit circuit = db.Circuits.Find(circuitId);
            if (circuit == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContactID = new SelectList(db.Contacts, "Id", "BookingLine", circuit.ContactID);
            ViewBag.CountryID = new SelectList(db.Countries, "Id", "Name", circuit.CountryID);
            ViewBag.HeadOfficeID = new SelectList(db.HeadOffices, "Id", "Address", circuit.HeadOfficeID);
            ViewBag.MapPointID = new SelectList(db.MapPoints, "Id", "Lat", circuit.MapPointID);
            ViewBag.OldCircuitId = circuitId;
            ViewBag.UseLoyalty = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text", circuit.UseLoyalty);
            return View(circuit);
        }

        // POST: Circuits/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,BaseURI,UserName,Password,Open,Public,CountryID,ContactID,MapPointID,UseLoyalty,OrderTimeoutSeconds,isActive,HeadOfficeID")] Circuit circuit, FormCollection frm)
        {
            var oldCircuitId = Convert.ToInt64(frm["oldCircuitId"]);

            if (circuit.Id <= 0)
            {
                ModelState.AddModelError("Id", "Id is required");
            }

            if (string.IsNullOrEmpty(circuit.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
            }

            if (string.IsNullOrEmpty(circuit.BaseURI))
            {
                ModelState.AddModelError("BaseURI", "Base URI is required");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (oldCircuitId != circuit.Id)
                    {
                        db.UpdateCircuit(oldCircuitId, circuit.Id, circuit.Name, circuit.BaseURI, circuit.UserName, circuit.Password, circuit.Open, circuit.Public, circuit.HeadOfficeID, circuit.CountryID, circuit.ContactID, circuit.MapPointID, circuit.UseLoyalty, circuit.OrderTimeoutSeconds);
                    }
                    else
                    {
                        circuit.isActive = true;
                        db.Entry(circuit).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "This circuit id has already been taken";
                }
            }

            ViewBag.ContactID = new SelectList(db.Contacts, "Id", "BookingLine", circuit.ContactID);
            ViewBag.CountryID = new SelectList(db.Countries, "Id", "Name", circuit.CountryID);
            ViewBag.HeadOfficeID = new SelectList(db.HeadOffices, "Id", "Address", circuit.HeadOfficeID);
            ViewBag.MapPointID = new SelectList(db.MapPoints, "Id", "Lat", circuit.MapPointID);
            ViewBag.OldCircuitId = oldCircuitId;
            ViewBag.UseLoyalty = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text", circuit.UseLoyalty);
            return View(circuit);
        }

        // GET: Circuits/Delete/5
        public ActionResult Delete(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Int64 circuitId = Convert.ToInt64(CryptoProvider.Decrypt(EncId));
            Circuit circuit = db.Circuits.Find(circuitId);
            if (circuit == null)
            {
                return HttpNotFound();
            }
            return View(circuit);
        }

        // POST: Circuits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string EncId)
        {
            Int64 circuitId = Convert.ToInt64(CryptoProvider.Decrypt(EncId));
            Circuit circuit = db.Circuits.Find(circuitId);
            circuit.isActive = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetCircuitsByPaging()
        {
            int start = Convert.ToInt32(Request.QueryString["iDisplayStart"]);
            int length = Convert.ToInt32(Request.QueryString["iDisplayLength"]);

            int totalResultsCount;
            var result = GetAllCircuitData(length, start, out totalResultsCount);

            if (result != null && result.Any())
            {
                var res = result.Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name,
                    BaseURI = m.BaseURI,
                    Open = m.Open,
                    UserName = m.UserName,
                    Public = m.Public,
                    Password = m.Password,
                    HeadOffice = m.HeadOffice.Address,
                    Country = m.Country.Name,
                    Contact = m.Contact.BookingLine,
                    MapPoint = m.MapPoint.Lat + "-" + m.MapPoint.Long,
                    UseLoyalty = m.UseLoyalty,
                    OrderTimeoutSeconds = m.OrderTimeoutSeconds,
                    Actions = "<a class='edit-icon' href=\"/Circuits/Edit?EncId=" + CryptoProvider.Encrypt(m.Id) + " \"><span data-toggle='tooltip' data-placement='left' title='Edit' class='fa fa-pencil'></span> </a> <a class='delete-icon' href=\"/Circuits/Delete?EncId=" + CryptoProvider.Encrypt(m.Id) + " \"> <span data-toggle='tooltip' data-placement='right' title='Delete' class='fa fa-trash-o'></span> </a>"
                }).ToList();
                return Json(new { recordsFiltered = totalResultsCount, data = res.ToList(), recordsTotal = totalResultsCount }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { recordsFiltered = totalResultsCount, data = "", recordsTotal = totalResultsCount }, JsonRequestBehavior.AllowGet);
        }

        public List<Circuit> GetAllCircuitData(int take, int skip, out int totalResultsCount)
        {
            using (PeachAdministrationEntities con = new PeachAdministrationEntities())
            {
                var result = con.Circuits.Where(c => c.isActive).Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name,
                    BaseURI = m.BaseURI,
                    Open = m.Open,
                    UserName = m.UserName,
                    Public = m.Public,
                    Password = m.Password,
                    HeadOffice = m.HeadOffice,
                    Country = m.Country,
                    Contact = m.Contact,
                    MapPoint = m.MapPoint,
                    UseLoyalty = m.UseLoyalty,
                    OrderTimeoutSeconds = m.OrderTimeoutSeconds
                }).ToList().OrderByDescending(o => o.Id).Select(m => new Circuit()
                {
                    Id = m.Id,
                    Name = m.Name,
                    BaseURI = m.BaseURI,
                    Open = m.Open,
                    UserName = m.UserName,
                    Public = m.Public,
                    Password = m.Password,
                    HeadOffice = m.HeadOffice,
                    Country = m.Country,
                    Contact = m.Contact,
                    MapPoint = m.MapPoint,
                    UseLoyalty = m.UseLoyalty,
                    OrderTimeoutSeconds = m.OrderTimeoutSeconds
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
