using System;
using System.ComponentModel.DataAnnotations;

namespace KMEnums
{
    public enum FormType
    {
        [Display(Name = "Newsletter")]
        Newsletter = 0,

        [Display(Name = "Simple")]
        Simple = 1,

        [Display(Name = "Auto-Submit")]
        AutoSubmit = 2,

       [Display(Name = "Subscription")]
        Subscription = 3
    }
}