using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Text.RegularExpressions;


namespace WattWebService
{
    public class HelperFunctions
    {

        public static bool isValidEmail(string emailaddress)
        {
            return Regex.IsMatch(emailaddress, @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }

        public static  Hashtable ToHashtable(object[][] oo)
        {
            Hashtable ht = new Hashtable(oo.Length);
            foreach (object[] pair in oo)
            {
                object key = pair[0].ToString().ToUpper();
                object value = pair[1];
                ht[key] = value;
            }
            return ht;
        }

        public static object[][] ToArray(Hashtable ht)
        {
            object[][] oo = new object[ht.Count][];
            int i = 0;
            foreach (object key in ht.Keys)
            {
                oo[i] = new object[] { key, ht[key] };
                i++;
            }
            return oo;
        }

        
    }
}