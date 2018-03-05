using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace PeachDigital.Administration.Common.Helper
{
    public static class Globals
    {

        public static bool IsNumeric(string s)
        {
            int val;
            return Int32.TryParse(s, out val);
        }

        public static string StandarizedPermalink(string pageTitle)
        {
            try
            {
                string permalink = Regex.Replace(pageTitle, @"-+", "-");
                permalink = Regex.Replace(permalink, @"\s+", " ");
                permalink = permalink.Trim().ToLower().Replace(" ", "-");
                if (permalink.IndexOf("--") > 0)
                {
                    permalink = StandarizedPermalink(permalink);
                }
                return permalink;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        
    }
}
