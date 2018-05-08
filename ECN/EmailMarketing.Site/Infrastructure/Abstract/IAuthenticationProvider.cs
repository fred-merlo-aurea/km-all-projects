using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//using ECN_Framework_BusinessLayer.Accounts;
using KMPlatform.Entity;
using ECN_Framework_Entities.Application;


namespace EmailMarketing.Site.Infrastructure.Abstract
{
    public interface IAuthenticationProvider
    {
        KMPlatform.Entity.User Authenticate(Action<HttpCookie> addCookie, string username, string password, bool persist);
        bool Authenticate(Action<HttpCookie> addCookie, User u, KMPlatform.Entity.Client c, bool persist);
        void Deauthenticate(Action<HttpCookie> addCookie);

    }
}