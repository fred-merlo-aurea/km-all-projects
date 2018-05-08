using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecn.communicator.mvc.Models;
using ecn.communicator.mvc.Infrastructure;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;
using System.Configuration;
using System.Data;
using System.Xml;
using System.IO;

namespace ecn.communicator.mvc.Infrastructure
{
    public static class ConvenienceMethods
    {
        public static KMPlatform.Entity.User GetCurrentUser()
        {
            return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
        }
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