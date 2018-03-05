using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachDigital.Administration.Common.Helper
{
    public static class CryptoProvider
    {
        public static string Encrypt(int id)
        {
            try
            {
                string text = id + "$" + (id);
                byte[] encoded = System.Text.Encoding.UTF8.GetBytes(text);
                return Convert.ToBase64String(encoded);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static string Encrypt(Int64 id)
        {
            try
            {
                string text = id + "$" + (id);
                byte[] encoded = System.Text.Encoding.UTF8.GetBytes(text);
                return Convert.ToBase64String(encoded);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static string Encrypt(string id)
        {
            try
            {
                string text = id + "$" + (id);
                byte[] encoded = System.Text.Encoding.UTF8.GetBytes(text);
                return Convert.ToBase64String(encoded);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static string EncryptNew(string id)
        {
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(id);
                return System.Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public static string DecryptNew(string encryptedText)
        {
            string result = string.Empty;
            try
            {
                if (!Globals.IsNumeric(encryptedText))
                {

                    byte[] encoded = Convert.FromBase64String(encryptedText);
                    string dec = System.Text.ASCIIEncoding.ASCII.GetString(encoded);
                    string[] array = dec.Split('$');
                    result = array[0];
                }
                return result;
                // Commented by Mehmood Ahmed on Dated : 14-Feb-2017
                //throw new Exception();
            }
            catch (Exception)
            {
                throw new Exception("Invalid request found.");
            }
        }


        public static string Decrypt(string encryptedText)
        {
            string result = string.Empty;
            try
            {
                if (!Globals.IsNumeric(encryptedText))
                {

                    byte[] encoded = Convert.FromBase64String(encryptedText);
                    string dec = System.Text.Encoding.UTF8.GetString(encoded);
                    string[] array = dec.Split('$');
                    int recordId = Convert.ToInt32(array[0]);
                    int salt = Convert.ToInt32(array[1]);
                    if (recordId == (salt))
                    {
                        result = array[0];
                        return result;
                    }
                }
                return result;
                // Commented by Mehmood Ahmed on Dated : 14-Feb-2017
                //throw new Exception();
            }
            catch (Exception)
            {
                throw new Exception("Invalid request found.");
            }
        }
    }
}
