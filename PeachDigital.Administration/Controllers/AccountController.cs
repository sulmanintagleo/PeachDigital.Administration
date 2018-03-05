using PeachDigital.Administration.Common.Helper;
using PeachDigital.Administration.Models;
using PeachDigital.Administration.Models.Custome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeachDigital.Administration.Controllers
{
    public class AccountController : Controller
    {
        private readonly SessionManager _sessionManager = new SessionManager();
        private readonly PeachAdministrationEntities _db = new PeachAdministrationEntities();
        
        [HttpGet]
        public ActionResult Login()
        {
            _sessionManager.Clear();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var pass = CustomeEncryptDecrypt.Encrypt(model.Password,true);
                        var user = _db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == pass);

                        if (user == null)
                        {
                            ModelState.AddModelError("*", "User Not Found!");
                            return View(model);
                        }

                        var record = new Models.Custome.User
                        {
                            Address = user.Address1 + user.Address2,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            Id = user.Id,
                            Password = user.Password,
                            PhoneNo = user.PhoneNo,
                            Role = new Models.Custome.Role
                            {
                                Id = user.RoleId,
                                Name = user.Role.Name
                            },
                            RoleId = user.RoleId,
                            isActive = user.isActive,
                            Lastname = user.LastName,
                            County = user.County
                        };

                        record.Role.Permissions = new List<ModulePermissions>();

                        foreach (var access in user.Role.ModulePermissions)
                        {
                            var obj = new ModulePermissions();
                            obj.Id = access.Id;
                            obj.ModuleId = access.ModuleId;
                            obj.ModuleName = access.Module.Name;
                            obj.RoleId = access.RoleId;
                            obj.RoleName = access.Role.Name;
                            obj.PermissionId = access.PermissionId;
                            obj.PermissionaName = access.PermissionType.Type;

                            record.Role.Permissions.Add(obj);
                        }

                        _sessionManager.Set("UserSession", record);

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LogOut()
        {
            try
            {
                _sessionManager.Clear();
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}