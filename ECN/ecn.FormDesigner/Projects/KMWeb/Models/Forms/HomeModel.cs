using KMModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMWeb.Models.Forms
{
    public class HomeModel
    {
        public IEnumerable<FormViewModel> ActiveForms { get; set; }

        public IEnumerable<FormViewModel> InactiveForms { get; set; }
    }
}