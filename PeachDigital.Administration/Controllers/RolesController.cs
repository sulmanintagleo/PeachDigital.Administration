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
    public class RolesController : Controller
    {
        private PeachAdministrationEntities db = new PeachAdministrationEntities();

        // GET: Roles
        [AuthorizeUser("Roles", "View")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Roles/Details/5
        [AuthorizeUser("Roles", "View")]
        public ActionResult Details(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int roleId = Convert.ToInt32(CryptoProvider.Decrypt(EncId));
            Role role = db.Roles.Find(roleId);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // GET: Roles/Create
        [AuthorizeUser("Roles", "Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AuthorizeUser("Roles", "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Role role)
        {
            if (string.IsNullOrEmpty(role.Name))
            {
                ModelState.AddModelError("Name", "Role name is required");
            }

            if (ModelState.IsValid)
            {
                //Duplication Check
                var roleExist = db.Roles.Any(u => u.Name == role.Name);
                if (roleExist)
                {
                    ViewBag.ErrorMessage = "Role already Exist in the system";
                    return View(role);
                }

                role.isActive = true;
                db.Roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        [AuthorizeUser("Roles", "Update")]
        public ActionResult Edit(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int roleId = Convert.ToInt32(CryptoProvider.Decrypt(EncId));
            Role role = db.Roles.Find(roleId);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AuthorizeUser("Roles", "Update")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Role role)
        {
            if (string.IsNullOrEmpty(role.Name))
            {
                ModelState.AddModelError("Name", "Role name is required");
            }
            if (ModelState.IsValid)
            {
                //Duplication Check
                var roleExist = db.Roles.Any(u => u.Name == role.Name && u.Id != role.Id);
                if (roleExist)
                {
                    ViewBag.ErrorMessage = "Role already Exist in the system";
                    return View(role);
                }

                role.isActive = true;
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        [AuthorizeUser("Roles", "Delete")]
        public ActionResult Delete(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int roleId = Convert.ToInt32(CryptoProvider.Decrypt(EncId));
            Role role = db.Roles.Find(roleId);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [AuthorizeUser("Roles", "Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string EncId)
        {
            int roleId = Convert.ToInt32(CryptoProvider.Decrypt(EncId));
            Role role = db.Roles.Find(roleId);
            role.isActive = false;
            //db.Roles.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Roles Permissions
        [AuthorizeUser("Permissions", "View")]
        public ActionResult Permissions(string EncId)
        {
            if (string.IsNullOrEmpty(EncId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            ViewBag.Modules = new SelectList(db.Modules.Where(m=>m.isActive), "Id", "Name");
            ViewBag.PermissionTypes = new SelectList(db.PermissionTypes, "Id", "Type");
            ViewBag.StringRoleId = EncId;

            int intRoleId = Convert.ToInt32(CryptoProvider.Decrypt(EncId));
            Role role = db.Roles.Find(intRoleId);
            if (role != null)
            {
                ViewBag.RoleName = role.Name;
            }
            else
            {
                ViewBag.RoleName = "";
            }
            List<ModulePermission> permission = db.ModulePermissions.Where(r => r.RoleId == intRoleId).ToList();
            if (permission != null && permission.Count > 0)
            {
                return View(permission);
            }

            return View(new List<ModulePermission>());
        }

        //Update Permissions
        [AuthorizeUser("Permissions", "Update")]
        [HttpPost]
        public JsonResult UpdatePermissions(string roleId, string permissions)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                return Json(new { success = false, message = "Access denied, Please contact to admin to get access." }, JsonRequestBehavior.AllowGet);
            }

            int role = Convert.ToInt32(CryptoProvider.Decrypt(roleId));

            if (!string.IsNullOrEmpty(permissions))
            {
                string[] tmpPermissionsAry = permissions.Split(',');
                if (tmpPermissionsAry.Count() > 0)
                {
                    List<ModulePermission> permission = db.ModulePermissions.Where(r => r.RoleId == role).ToList();
                    if (permission != null && permission.Count > 0)
                    {
                        db.ModulePermissions.RemoveRange(permission);
                        db.SaveChanges();
                    }
                    foreach (var strPermission in tmpPermissionsAry)
                    {
                        string[] tempModulePermAry = strPermission.Split('-');
                        if (tempModulePermAry.Count() > 0)
                        {
                            ModulePermission modulePermissions = new ModulePermission();
                            modulePermissions.ModuleId = Convert.ToInt32(tempModulePermAry[0]);
                            modulePermissions.PermissionId = Convert.ToInt16(tempModulePermAry[1]);
                            modulePermissions.RoleId = role;
                            db.ModulePermissions.Add(modulePermissions);
                        }
                    }
                    db.SaveChanges();
                }
            }

            return Json(new { success = true, message = "Permisssions has been updated successfully" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRolesByPaging()
        {
            int start = Convert.ToInt32(Request.QueryString["iDisplayStart"]);
            int length = Convert.ToInt32(Request.QueryString["iDisplayLength"]);

            int totalResultsCount;
            var result = GetAllRoleData(length, start, out totalResultsCount);

            if (result != null && result.Any())
            {
                var res = result.Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name,
                    Actions = "<a class='edit-icon' href=\"/Roles/Edit?EncId=" + CryptoProvider.Encrypt(m.Id) + " \"><span data-toggle='tooltip' data-placement='left' title='Edit' class='fa fa-pencil'></span> </a><a class='edit-icon' href=\"/Roles/Permissions?EncId=" + CryptoProvider.Encrypt(m.Id) + " \"> <span data-toggle='tooltip' data-placement='top' title='Permissions' class='fa fa-bars'></span> </a> <a class='delete-icon' href=\"/Roles/Delete?EncId=" + CryptoProvider.Encrypt(m.Id) + " \"> <span data-toggle='tooltip' data-placement='right' title='Delete' class='fa fa-trash-o'></span> </a>"
                }).ToList();
                return Json(new { recordsFiltered = totalResultsCount, data = res.ToList(), recordsTotal = totalResultsCount }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { recordsFiltered = totalResultsCount, data = result.ToList(), recordsTotal = totalResultsCount }, JsonRequestBehavior.AllowGet);
        }

        public List<Role> GetAllRoleData(int take, int skip, out int totalResultsCount)
        {
            using (PeachAdministrationEntities con = new PeachAdministrationEntities())
            {
                var result = con.Roles.Where(c => c.isActive).Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList().OrderByDescending(o => o.Id).Select(m => new Role()
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