using System;
using System.Web;

namespace PeachDigital.Administration.Common.Helper
{
    //public class SessionManager
    //{
    //    private bool IsInitialize()
    //    {
    //        try
    //        {
    //            return HttpContext.Current.Session != null;
    //        }
    //        catch (Exception exception)
    //        {
    //            throw exception;
    //        }
    //    }

    //    public object Get(string key)
    //    {
    //        try
    //        {
    //            return IsExists(key) ? HttpContext.Current.Session[key] : null;
    //        }
    //        catch (Exception exception)
    //        {
    //            throw exception;
    //        }
    //    }

    //    public void Set(string key, object value)
    //    {
    //        try
    //        {
    //            if (IsInitialize())
    //                HttpContext.Current.Session[key] = value;
    //        }
    //        catch (Exception exception)
    //        {
    //            throw exception;
    //        }
    //    }


    //    public object GetValue(string key)
    //    {
    //        try
    //        {
    //            return HttpContext.Current.Session[key] != null ? HttpContext.Current.Session[key] : null;
    //        }
    //        catch (Exception exception)
    //        {
    //            return null;
    //        }
    //    }

    //    public void SetValue(string key, object value)
    //    {
    //        try
    //        {
    //            HttpContext.Current.Session[key] = value;
    //        }
    //        catch (Exception exception)
    //        {

    //        }
    //    }

    //    public void Clear()
    //    {
    //        try
    //        {
    //            HttpContext.Current.Session[SessionConstants.FranchiseAdminUser] = null;
    //            HttpContext.Current.Session[SessionConstants.HeadofficeAdminUser] = null;
    //            HttpContext.Current.Session.Clear();
    //        }
    //        catch (Exception exception)
    //        {
    //            throw exception;
    //        }
    //    }

    //    public void FranchiseLogout()
    //    {
    //        try
    //        {
    //            HttpContext.Current.Session[SessionConstants.FranchiseAdminUser] = null;
    //        }
    //        catch (Exception exception)
    //        {
    //            throw exception;
    //        }
    //    }

    //    public void HeadOfficeLogout()
    //    {
    //        try
    //        {
    //            HttpContext.Current.Session[SessionConstants.HeadofficeAdminUser] = null;
    //        }
    //        catch (Exception exception)
    //        {
    //            throw exception;
    //        }
    //    }
    //    private bool IsExists(string key)
    //    {
    //        try
    //        {
    //            return (HttpContext.Current.Session != null && HttpContext.Current.Session[key] != null);
    //        }
    //        catch (Exception exception)
    //        {
    //            throw exception;
    //        }
    //    }

    //    public int GetFranchiseId()
    //    {
    //        try
    //        {
    //            var so = (HttpContext.Current.Session[SessionConstants.FranchiseAdminUser]);
    //            if (so != null)
    //            {
    //                var parsedSO = (User)so;
    //                return (HttpContext.Current.Session != null && parsedSO.FrachiseId != null) ? (int)parsedSO.FrachiseId : 0;
    //            }
    //            return 0;
    //        }
    //        catch (Exception exception)
    //        {
    //            throw exception;
    //        }

    //        //            try
    //        //            {
    //        //                var frachiseId = ((User)HttpContext.Current.Session[SessionConstants.FranchiseAdminUser]).FrachiseId;
    //        //                if (frachiseId != null)
    //        //                    return (HttpContext.Current.Session != null && HttpContext.Current.Session[SessionConstants.FranchiseAdminUser] != null) ? (int)frachiseId : 0;
    //        //                return 0;
    //        //            }
    //        //            catch (Exception exception)
    //        //            {
    //        //                throw exception;
    //        //            }
    //    }

    //    public string GetFranchiseName()
    //    {
    //        try
    //        {
    //            var so = (HttpContext.Current.Session[SessionConstants.FranchiseAdminUser]);
    //            if (so != null)
    //            {
    //                var parsedSO = (User)so;
    //                return (HttpContext.Current.Session != null) ? parsedSO.FirstName + " " + parsedSO.Surname : string.Empty;
    //            }
    //            return string.Empty;
    //        }
    //        catch (Exception exception)
    //        {
    //            throw exception;
    //        }
    //    }

    //    public int GetFranchiseVAT()
    //    {

    //        return 10;
    //    }

    //    /// <summary>
    //    /// Developer: Mehmood Ahmed
    //    /// Date : 09-Feb-2017
    //    /// Reason: To Get the Session value Globally
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="name"></param>
    //    /// <returns></returns>
    //    public static T GetValue<T>(string name) where T : class
    //    {
    //        try
    //        {

    //            var so = (HttpContext.Current.Session[name]);
    //            if (so != null)
    //            {
    //                return (T)so;
    //            }
    //            return null;
    //        }
    //        catch (Exception exception)
    //        {
    //            throw exception;
    //        }
    //    }

    //}
}