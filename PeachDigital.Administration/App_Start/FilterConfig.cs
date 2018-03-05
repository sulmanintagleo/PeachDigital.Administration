using PeachDigital.Administration.Models;
using PeachDigital.Administration.Models.Custome;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using System.Linq;

namespace PeachDigital.Administration
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    public class AuthorizeUser : AuthorizeAttribute, IAuthenticationFilter
    {
        private readonly string _moduleName;
        private readonly string _Action;
        private readonly SessionManager _sessionManager;
        PeachAdministrationEntities context = new PeachAdministrationEntities();

        public AuthorizeUser()
        {
            _sessionManager = new SessionManager();
        }

        public AuthorizeUser(string moduleName, string action)
        {
            _moduleName = moduleName;
            _Action = action;
            _sessionManager = new SessionManager();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Items.Contains("isAuthentication"))
            {
                httpContext.Items.Add("isAuthentication", false);
            }

            var user = (Models.Custome.User)_sessionManager.Get("UserSession");
            //check if user contains any module permission.
            if (!string.IsNullOrEmpty(_moduleName) && !string.IsNullOrEmpty(_Action))
            {
                if (user.Role.Permissions.Any(m => m.ModuleName.Equals(_moduleName, StringComparison.OrdinalIgnoreCase) && m.PermissionaName.Equals(_Action, StringComparison.OrdinalIgnoreCase)))
                {
                    httpContext.Items["isAuthentication"] = true;
                    return true;
                }
                httpContext.Items["isAuthentication"] = false;
                return false;
            }
            else
            {
                httpContext.Items["isAuthentication"] = true;
                return true;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Items.Contains("isAuthentication"))
            {
                RouteValueDictionary routeValues = null;
                if (filterContext.HttpContext.Request.Url != null)
                {
                    routeValues = new RouteValueDictionary(new
                    {
                        controller = "Access",
                        action = "Index",
                    });
                }

                filterContext.Result = new RedirectToRouteResult(routeValues);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (_sessionManager.Get("UserSession") == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                new
                {
                    controller = "Account",
                    action = "Login",
                }));
            }
        }

    }

    public class SessionManager
    {
        private bool IsInitialize()
        {
            try
            {
                return HttpContext.Current.Session != null;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public object Get(string key)
        {
            try
            {
                return IsExists(key) ? HttpContext.Current.Session[key] : null;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void Set(string key, object value)
        {
            try
            {
                if (IsInitialize())
                    HttpContext.Current.Session[key] = value;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public object GetValue(string key)
        {
            try
            {
                return HttpContext.Current.Session[key] != null ? HttpContext.Current.Session[key] : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SetValue(string key, object value)
        {
            try
            {
                HttpContext.Current.Session[key] = value;
            }
            catch (Exception)
            {

            }
        }

        public void Clear()
        {
            try
            {
                HttpContext.Current.Session["UserSession"] = null;
                HttpContext.Current.Session.Clear();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void Logout()
        {
            try
            {
                HttpContext.Current.Session["UserSession"] = null;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private bool IsExists(string key)
        {
            try
            {
                return (HttpContext.Current.Session != null && HttpContext.Current.Session[key] != null);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        

    }

}