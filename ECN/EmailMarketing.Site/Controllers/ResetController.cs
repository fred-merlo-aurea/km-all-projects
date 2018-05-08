using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using EmailMarketing.Site.Infrastructure.Abstract.Settings;
using EmailMarketing.Site.Models;
using EmailMarketing.Site.Infrastructure.Abstract;
using EmailMarketing.Site.Infrastructure.Authorization;

namespace EmailMarketing.Site.Controllers
{
    [AllowAnonymous]
    public class ResetController : ControllerBase
    {
        IAuthenticationProvider AuthenticationProvider { get; set; }
        IAccountProvider AccountProvider { get; set; }

        private void AddCookie(HttpCookie cookie)
        {
            Response.SetCookie(cookie);
        }

        public ResetController(
            IUserSessionProvider userSessionProvider,
            IPathProvider pathProvider,
            IAuthenticationProvider authenticationProvider,
            IBaseChannelProvider baseChannelProvider,
            IAccountProvider accountProvider
            )
            : base(userSessionProvider, pathProvider, baseChannelProvider)
        {
            AuthenticationProvider = authenticationProvider;
            AccountProvider = accountProvider;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(int id = -1)
        {
            if (id > 0)
            {
                try
                {
                    KMPlatform.Entity.User currentUser = KMPlatform.BusinessLogic.User.GetByUserID(id, false);
                    if (currentUser.RequirePasswordReset)
                    {
                        Models.ResetPasswordViewModel rpvm = new ResetPasswordViewModel();
                        rpvm.UserName = currentUser.UserName;
                        rpvm.UserID = currentUser.UserID;
                        ModelState.Clear();
                        return View(rpvm);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(ResetPasswordViewModel rpvm)
        {

            KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();
            KMPlatform.Entity.User currentUser = KMPlatform.BusinessLogic.User.GetByUserID(rpvm.UserID, false);
            if (currentUser.Password == rpvm.TempPassword)
            {
                if (rpvm.ConfirmPassword == rpvm.NewPassword && (!string.IsNullOrEmpty(rpvm.ConfirmPassword) && !string.IsNullOrEmpty(rpvm.NewPassword)))
                {
                    currentUser.Password = rpvm.NewPassword;
                    currentUser.RequirePasswordReset = false;
                    uWorker.Save(currentUser);

                    try
                    {
                        KMPlatform.Entity.User authenticationSuccess = AuthenticationProvider.Authenticate(
                            AddCookie, // action to add auth cookies
                            currentUser.UserName,
                            rpvm.NewPassword,
                            true); // not displayed on the form but defaulted true in the model

                        if (authenticationSuccess != null)
                        {
                            return View("ResetSuccess", rpvm);
                        }
                        else
                        {
                            return View(rpvm);
                        }
                    }
                    catch (KMPlatform.Object.UserLoginException ule)
                    {
                        if (ule.UserStatus == KMPlatform.Enums.UserLoginStatus.LockedUser)
                        {
                            rpvm.UserIsLocked = true;
                            return View(rpvm);
                        }
                        else if (ule.UserStatus == KMPlatform.Enums.UserLoginStatus.DisabledUser)
                        {
                            rpvm.UserIsDisabled = true;
                            return View(rpvm);
                        }
                        else if (ule.UserStatus == KMPlatform.Enums.UserLoginStatus.NoRoles)
                        {
                            rpvm.NoActiveRoles = true;
                            return View(rpvm);
                        }
                        else if (ule.UserStatus == KMPlatform.Enums.UserLoginStatus.InvalidPassword)
                        {
                            rpvm.InvalidUsername_Password = true;
                            return View(rpvm);
                        }
                    }
                    return View(rpvm);
                }
                else
                {
                    rpvm.PasswordCompare = true;
                    return View(rpvm);
                }
            }
            else
            {
                rpvm.InvalidTemp_Password = true;
                return View(rpvm);
            }
        }
    }

}