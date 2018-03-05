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
using System.Text.RegularExpressions;

namespace PeachDigital.Administration.Controllers
{
    public class UsersController : Controller
    {
        private PeachAdministrationEntities db = new PeachAdministrationEntities();

        // GET: Users
        [AuthorizeUser("Users", "View")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Users/Details/5
        [AuthorizeUser("Users", "View")]
        public ActionResult Details(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int userId = Convert.ToInt32(CryptoProvider.Decrypt(EncId));
            User user = db.Users.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Password = CustomeEncryptDecrypt.Decrypt(user.Password,true);
            return View(user);
        }

        // GET: Users/Create
        [AuthorizeUser("Users", "Create")]
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.Roles.Where(r=>r.isActive), "Id", "Name");
            return View();
        }

        [AuthorizeUser("Users", "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,Password,PhoneNo,Address1,Address2,County,ZipCode,RoleId")] User user)
        {
            if (string.IsNullOrEmpty(user.FirstName))
            {
                ModelState.AddModelError("FirstName", "First Name is required");
            }

            if (string.IsNullOrEmpty(user.LastName))
            {
                ModelState.AddModelError("LastName", "Last Name is required");
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);
                if (!re.IsMatch(user.Email))
                {
                    ModelState.AddModelError("Email", "Email is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Email is required");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                ModelState.AddModelError("Password", "Password is required");
            }

            if (user.RoleId <= 0)
            {
                ModelState.AddModelError("RoleId", "Role is required");
            }
            
            if (ModelState.IsValid)
            {
                //Duplication Check
                var userExist = db.Users.Any(u => u.Email == user.Email || u.PhoneNo == user.PhoneNo);
                if (userExist)
                {
                    ViewBag.ErrorMessage = "Please enter unique email and phone to register";
                    ViewBag.RoleId = new SelectList(db.Roles.Where(r => r.isActive), "Id", "Name", user.RoleId);
                    return View(user);
                }
                user.Password = CustomeEncryptDecrypt.Encrypt(user.Password,true);
                user.isActive = true;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleId = new SelectList(db.Roles.Where(r=>r.isActive), "Id", "Name", user.RoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        [AuthorizeUser("Users", "Update")]
        public ActionResult Edit(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int userId = Convert.ToInt32(CryptoProvider.Decrypt(EncId));
            User user = db.Users.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Password = CustomeEncryptDecrypt.Decrypt(user.Password,true);
            ViewBag.RoleId = new SelectList(db.Roles.Where(r=>r.isActive), "Id", "Name", user.RoleId);
            return View(user);
        }

        [AuthorizeUser("Users", "Update")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,Password,PhoneNo,Address1,Address2,County,ZipCode,RoleId")] User user)
        {
            if (string.IsNullOrEmpty(user.FirstName))
            {
                ModelState.AddModelError("FirstName", "First Name is required");
            }

            if (string.IsNullOrEmpty(user.LastName))
            {
                ModelState.AddModelError("LastName", "Last Name is required");
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);
                if (!re.IsMatch(user.Email))
                {
                    ModelState.AddModelError("Email", "Email is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Email is required");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                ModelState.AddModelError("Password", "Password is required");
            }

            if (user.RoleId <= 0)
            {
                ModelState.AddModelError("RoleId", "Role is required");
            }
            if (ModelState.IsValid)
            {
                //Duplication Check
                var userExist = db.Users.Any(u => (u.Email == user.Email || u.PhoneNo == user.PhoneNo) && u.Id != user.Id);
                if (userExist)
                {
                    ViewBag.ErrorMessage = "Please enter unique email and phone to register";
                    ViewBag.RoleId = new SelectList(db.Roles.Where(r => r.isActive), "Id", "Name", user.RoleId);
                    return View(user);
                }
                user.Password = CustomeEncryptDecrypt.Encrypt(user.Password,true);
                user.isActive = true;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(db.Roles.Where(r=>r.isActive), "Id", "Name", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        [AuthorizeUser("Users", "Delete")]
        public ActionResult Delete(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int userId = Convert.ToInt32(CryptoProvider.Decrypt(EncId));
            User user = db.Users.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Password = CustomeEncryptDecrypt.Decrypt(user.Password,true);
            return View(user);
        }

        // POST: Users/Delete/5
        [AuthorizeUser("Users", "Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string EncId)
        {
            int userId = Convert.ToInt32(CryptoProvider.Decrypt(EncId));
            User user = db.Users.Find(userId);
            user.isActive = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetUsersByPaging()
        {
            int start = Convert.ToInt32(Request.QueryString["iDisplayStart"]);
            int length = Convert.ToInt32(Request.QueryString["iDisplayLength"]);

            int totalResultsCount;
            var result = GetAllUsersData(length, start, out totalResultsCount);

            if (result != null && result.Any())
            {
                var res = result.Select(m => new
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Email = m.Email,
                    Password = CustomeEncryptDecrypt.Decrypt(m.Password, true),
                    PhoneNo = m.PhoneNo,
                    Address1 = m.Address1,
                    Address2 = m.Address2,
                    County = m.County,
                    ZipCode = m.ZipCode,
                    RoleName = m.Role.Name,
                    Actions = "<a class='edit-icon' href=\"/Users/Edit?EncId=" + CryptoProvider.Encrypt(m.Id) + " \"><span data-toggle='tooltip' data-placement='left' title='Edit' class='fa fa-pencil'></span> </a> <a class='delete-icon' href=\"/Users/Delete?EncId=" + CryptoProvider.Encrypt(m.Id) + " \"> <span data-toggle='tooltip' data-placement='right' title='Delete' class='fa fa-trash-o'></span> </a>"
                }).ToList();
                return Json(new { recordsFiltered = totalResultsCount, data = res.ToList(), recordsTotal = totalResultsCount }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { recordsFiltered = totalResultsCount, data = result.ToList(), recordsTotal = totalResultsCount }, JsonRequestBehavior.AllowGet);
        }

        public List<User> GetAllUsersData(int take, int skip, out int totalResultsCount)
        {
            using (PeachAdministrationEntities con = new PeachAdministrationEntities())
            {
                var result = con.Users.Where(c => c.isActive).Select(m => new
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Email = m.Email,
                    Password = m.Password,
                    PhoneNo = m.PhoneNo,
                    Address1 = m.Address1,
                    Address2 = m.Address2,
                    County = m.County,
                    ZipCode = m.ZipCode,
                    Role = m.Role
                }).ToList().OrderByDescending(o => o.Id).Select(m => new User()
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Email = m.Email,
                    Password = m.Password,
                    PhoneNo = m.PhoneNo,
                    Address1 = m.Address1,
                    Address2 = m.Address2,
                    County = m.County,
                    ZipCode = m.ZipCode,
                    Role = m.Role
                });
                if (result != null && result.Any())
                {
                    totalResultsCount = result.Count();
                    var res = result.Skip(skip).Take(take).ToList();
                    return res;
                }
                else
                {
                    totalResultsCount = 0;
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
