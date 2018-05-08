using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAD.Web.Admin.Infrastructure
{
    public static class ConvenienceMethods
    {
        public static object GetTempData(this ControllerBase cont, string index)
        {
            var temp = cont.TempData[index];
            cont.TempData[index] = null;
            return temp;
        }
        public static void SetTempData(this ControllerBase cont, string index, object value)
        {
            cont.TempData[index] = value;
        }
    }
}