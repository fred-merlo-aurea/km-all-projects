using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Standards.Models
{
    public class SearchFilter
    {

        public string Title { get; set; }
        public List<ContentViewModel> ContentList  {get; set;}
        public SearchFilter()
        {
            Title = "";
            ContentList = new List<ContentViewModel>();
        }
    }
}