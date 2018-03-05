using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PeachDigital.Administration.Models;
using PeachDigital.Administration.Common.Helper;
using System.Collections.ObjectModel;

namespace PeachDigital.Administration.Controllers
{
    public class CinemasController : Controller
    {
        private PeachAdministrationEntities db = new PeachAdministrationEntities();

        // GET: Cinemas
        [AuthorizeUser("Cinema", "View")]
        public ActionResult Index()
        {
            ViewBag.Circuits = new SelectList(db.Circuits.Where(c => c.isActive).OrderBy(s => s.Name), "Id", "Name", "Select Circuit");
            return View();
        }

        // GET: Cinemas/Create
        [AuthorizeUser("Cinema", "Create")]
        public ActionResult Create()
        {
            ViewBag.Circuit_Id = new SelectList(db.Circuits.Where(c => c.isActive), "Id", "Name");
            ViewBag.RegionId = new SelectList(db.Regions, "Id", "Name");
            ViewBag.Id = new SelectList(db.CinemaAddresses, "CinemaId", "Address1");
            ViewBag.Timezone = new SelectList(TimeZoneInfo.GetSystemTimeZones(), "Id", "DisplayName");
            ViewBag.BookingSystemID = new SelectList(db.BookingSystems, "Id", "BookingURI");
            ViewBag.GiftStore = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser("Cinema", "Create")]
        public ActionResult Create([Bind(Include = "Id,Name,Timezone,Open,Booking,Public,ThreeDSecure,Order,ManagerName,ManagerDesc,External_Id,BookingSystemID,RegionId,Circuit_Id,DayStart,GiftStore,SecondExternal_Id,IsPaymentWithGiftCardEnabled,IsLoyaltyEarnPointsEnabled,TicketingUrl")] Cinema cinema)
        {

            if (string.IsNullOrEmpty(cinema.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
            }

            if (string.IsNullOrEmpty(cinema.Timezone))
            {
                ModelState.AddModelError("Timezone", "Time zone is required");
            }

            if (cinema.DayStart <= 0)
            {
                ModelState.AddModelError("DayStart", "Day Start is required");
            }

            if (cinema.Order <= 0)
            {
                ModelState.AddModelError("Order", "Order is required");
            }

            if (ModelState.IsValid)
            {
                cinema.isActive = true;
                db.Cinemas.Add(cinema);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Circuit_Id = new SelectList(db.Circuits.Where(c => c.isActive), "Id", "Name", cinema.Circuit_Id);
            ViewBag.RegionId = new SelectList(db.Regions, "Id", "Name", cinema.RegionId);
            ViewBag.Id = new SelectList(db.CinemaAddresses, "CinemaId", "Address1", cinema.Id);
            ViewBag.Timezone = new SelectList(TimeZoneInfo.GetSystemTimeZones(), "Id", "DisplayName", cinema.Timezone);
            ViewBag.BookingSystemID = new SelectList(db.BookingSystems, "Id", "BookingURI", cinema.BookingSystemID);
            ViewBag.GiftStore = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text", cinema.GiftStore);
            return View(cinema);
        }

        // GET: Cinemas/Edit/5
        [AuthorizeUser("Cinema", "Update")]
        public ActionResult Edit(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Int64 cinemaId = Convert.ToInt64(CryptoProvider.Decrypt(EncId));
            Cinema cinema = db.Cinemas.Find(cinemaId);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            ViewBag.Circuit_Id = new SelectList(db.Circuits.Where(c => c.isActive), "Id", "Name", cinema.Circuit_Id);
            ViewBag.RegionId = new SelectList(db.Regions, "Id", "Name", cinema.RegionId);
            ViewBag.Id = new SelectList(db.CinemaAddresses, "CinemaId", "Address1", cinema.Id);
            ViewBag.Timezone = new SelectList(TimeZoneInfo.GetSystemTimeZones(), "Id", "DisplayName", cinema.Timezone);
            ViewBag.BookingSystemID = new SelectList(db.BookingSystems, "Id", "BookingURI", cinema.BookingSystemID);
            ViewBag.OldCinemaId = cinemaId;
            ViewBag.GiftStore = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text", cinema.GiftStore);
            return View(cinema);
        }

        // POST: Cinemas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser("Cinema", "Update")]
        public ActionResult Edit([Bind(Include = "Id,Name,Timezone,Open,Booking,Public,ThreeDSecure,Order,ManagerName,ManagerDesc,External_Id,BookingSystemID,RegionId,Circuit_Id,DayStart,GiftStore,SecondExternal_Id,IsPaymentWithGiftCardEnabled,IsLoyaltyEarnPointsEnabled,TicketingUrl")] Cinema cinema, FormCollection frm)
        {
            var oldCinemaId = Convert.ToInt64(frm["oldCinemaId"]);

            if (cinema.Id <= 0)
            {
                ModelState.AddModelError("Id", "Id is required");
            }

            if (string.IsNullOrEmpty(cinema.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
            }

            if (string.IsNullOrEmpty(cinema.Timezone))
            {
                ModelState.AddModelError("Timezone", "Time zone is required");
            }

            if (cinema.DayStart <= 0)
            {
                ModelState.AddModelError("DayStart", "Day Start is required");
            }

            if (cinema.Order <= 0)
            {
                ModelState.AddModelError("Order", "Order is required");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (oldCinemaId != cinema.Id)
                    {
                        db.UpdateCinema(oldCinemaId, cinema.Id, cinema.Name, cinema.Timezone, cinema.Open, cinema.Booking, cinema.Public, cinema.ThreeDSecure, cinema.Order, cinema.ManagerName, cinema.ManagerDesc, cinema.External_Id, cinema.BookingSystemID, cinema.RegionId, cinema.Circuit_Id, cinema.DayStart, cinema.GiftStore, cinema.SecondExternal_Id, cinema.IsPaymentWithGiftCardEnabled, cinema.IsLoyaltyEarnPointsEnabled, cinema.TicketingUrl);
                    }
                    else
                    {
                        db.Entry(cinema).State = EntityState.Modified;
                        cinema.isActive = true;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "This cinema id has already been taken";
                }
            }
            ViewBag.Circuit_Id = new SelectList(db.Circuits.Where(c => c.isActive), "Id", "Name", cinema.Circuit_Id);
            ViewBag.RegionId = new SelectList(db.Regions, "Id", "Name", cinema.RegionId);
            ViewBag.Id = new SelectList(db.CinemaAddresses, "CinemaId", "Address1", cinema.Id);
            ViewBag.Timezone = new SelectList(TimeZoneInfo.GetSystemTimeZones(), "Id", "DisplayName", cinema.Timezone);
            ViewBag.BookingSystemID = new SelectList(db.BookingSystems, "Id", "BookingURI", cinema.BookingSystemID);
            ViewBag.OldCinemaId = oldCinemaId;
            ViewBag.GiftStore = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "True"},
                new SelectListItem { Text = "No", Value = "False"},
            }, "Value", "Text", cinema.GiftStore);
            return View(cinema);
        }

        // GET: Cinemas/Delete/5
        [AuthorizeUser("Cinema", "Delete")]
        public ActionResult Delete(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Int64 cinemaId = Convert.ToInt64(CryptoProvider.Decrypt(EncId));
            Cinema cinema = db.Cinemas.Find(cinemaId);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            return View(cinema);
        }

        // POST: Cinemas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeUser("Cinema", "Delete")]
        public ActionResult DeleteConfirmed(string EncId)
        {
            Int64 cinemaId = Convert.ToInt64(CryptoProvider.Decrypt(EncId));
            Cinema cinema = db.Cinemas.Find(cinemaId);
            cinema.isActive = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AuthorizeUser("Cinema", "View")]
        public JsonResult GetCinemasByPaging(int circuitId = 0)
        {
            int start = Convert.ToInt32(Request.QueryString["iDisplayStart"]);
            int length = Convert.ToInt32(Request.QueryString["iDisplayLength"]);

            int totalResultsCount;
            var result = GetAllCinemaData(length, start, out totalResultsCount, circuitId);

            if (result != null && result.Any())
            {
                var res = result.Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name,
                    Timezone = m.Timezone,
                    Open = m.Open,
                    Booking = m.Booking,
                    Public = m.Public,
                    ThreeDSecure = m.ThreeDSecure,
                    Order = m.Order,
                    ManagerName = m.ManagerName,
                    ManagerDesc = m.ManagerDesc,
                    External_Id = m.External_Id,
                    BookingSystemID = m.BookingSystemID,
                    DayStart = m.DayStart,
                    GiftStore = m.GiftStore,
                    SecondExternal_Id = m.SecondExternal_Id,
                    IsPaymentWithGiftCardEnabled = m.IsPaymentWithGiftCardEnabled,
                    IsLoyaltyEarnPointsEnabled = m.IsLoyaltyEarnPointsEnabled,
                    TicketingUrl = m.TicketingUrl,
                    Region = m.Region != null ? m.Region.Name : "",
                    Circuit = m.Circuit != null ? m.Circuit.Name : "",
                    Actions = "<a class='edit-icon' href=\"/Cinemas/Edit?EncId=" + CryptoProvider.Encrypt(m.Id) + " \"> <span data-toggle='tooltip' data-placement='left' title='Edit' class='fa fa-pencil'></span> </a> <a class='delete-icon' href=\"/Cinemas/Delete?EncId=" + CryptoProvider.Encrypt(m.Id) + " \"> <span data-toggle='tooltip' data-placement='right' title='Delete' class='fa fa-trash-o'></span> </a>"
                }).ToList();
                return Json(new { recordsFiltered = totalResultsCount, data = res.ToList(), recordsTotal = totalResultsCount }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { recordsFiltered = totalResultsCount, data = "", recordsTotal = totalResultsCount }, JsonRequestBehavior.AllowGet);
        }

        public List<Cinema> GetAllCinemaData(int take, int skip, out int totalResultsCount, int circuitId = 0)
        {
            using (PeachAdministrationEntities con = new PeachAdministrationEntities())
            {

                var result = con.Cinemas.Where(c => c.isActive && c.Circuit_Id == (circuitId > 0 ? circuitId : c.Circuit_Id)).Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name,
                    Timezone = m.Timezone,
                    Open = m.Open,
                    Booking = m.Booking,
                    Public = m.Public,
                    ThreeDSecure = m.ThreeDSecure,
                    Order = m.Order,
                    ManagerName = m.ManagerName,
                    ManagerDesc = m.ManagerDesc,
                    External_Id = m.External_Id,
                    BookingSystemID = m.BookingSystemID,
                    DayStart = m.DayStart,
                    GiftStore = m.GiftStore,
                    SecondExternal_Id = m.SecondExternal_Id,
                    IsPaymentWithGiftCardEnabled = m.IsPaymentWithGiftCardEnabled,
                    IsLoyaltyEarnPointsEnabled = m.IsLoyaltyEarnPointsEnabled,
                    TicketingUrl = m.TicketingUrl,
                    Region = m.Region ?? null,
                    Circuit = m.Circuit ?? null
                }).ToList().OrderByDescending(o => o.Id).Select(m => new Cinema()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Timezone = m.Timezone,
                    Open = m.Open,
                    Booking = m.Booking,
                    Public = m.Public,
                    ThreeDSecure = m.ThreeDSecure,
                    Order = m.Order,
                    ManagerName = m.ManagerName,
                    ManagerDesc = m.ManagerDesc,
                    External_Id = m.External_Id,
                    BookingSystemID = m.BookingSystemID,
                    DayStart = m.DayStart,
                    GiftStore = m.GiftStore,
                    SecondExternal_Id = m.SecondExternal_Id,
                    IsPaymentWithGiftCardEnabled = m.IsPaymentWithGiftCardEnabled,
                    IsLoyaltyEarnPointsEnabled = m.IsLoyaltyEarnPointsEnabled,
                    TicketingUrl = m.TicketingUrl,
                    Region = m.Region ?? null,
                    Circuit = m.Circuit ?? null
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