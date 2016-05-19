using Com.Test.Core.DataAccess;
using Com.Test.UPMS.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Com.Test.UPMS.Web.Areas.Admin.Controllers
{
    public class LoginController : BaseController
    {
        private BaseRepository<LoginOnModel> loginRepository = new BaseRepository<LoginOnModel>();

        // GET: Admin/Login
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public void Login(LoginOnModel collection)
        {
            bool isPersistent = true;
            var obj = loginRepository.GetOne("select * from UserInfo where UserName=@UserName and UserPassword=@Password and IsDel=0;", collection).FirstOrDefault();

            if (obj != null)
            {
                SetFormAuthentication(isPersistent, obj);
                //GetCurrentUserPermissions(obj.ID);

                Response.Redirect("~/Home/Index");
            }
        }

        private void SetFormAuthentication(bool isPersistent, LoginOnModel user)
        {
            #region

            ////////////////////////////////////////////////////////////////////
            DateTime utcNow = DateTime.Now;
            DateTime cookieUtc = isPersistent ? utcNow.AddDays(7) : utcNow.Add(FormsAuthentication.Timeout);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.UserName,
                utcNow,
                cookieUtc,
                isPersistent,
                user.UserName.ToString(), FormsAuthentication.FormsCookiePath);

            string ticketEncrypted = FormsAuthentication.Encrypt(ticket);
            if (string.IsNullOrEmpty(ticketEncrypted))
            {
                throw new InvalidOperationException("FormsAuthentication.Encrypt failed.");
            }

            HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketEncrypted)
            {
                Domain = FormsAuthentication.CookieDomain,
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath,
            };

            var name = Uri.EscapeDataString(user.UserName);
            HttpCookie nameCookie = new HttpCookie("name", name)
            {
                Domain = FormsAuthentication.CookieDomain,
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath,
            };
            //if (isPersistent)
            {
                httpCookie.Expires = utcNow.AddDays(7);
                nameCookie.Expires = utcNow.AddDays(7);
            }

            // FormsAuthentication.SetAuthCookie(name, false);
            HttpContext.Response.Cookies.Add(httpCookie);

            HttpContext.Response.Cookies.Add(nameCookie);
            ////////////////////////////////////////////////////////////
            #endregion
            //ViewBag.aaa = Request.Cookies[".FA"].Value;
        }
    }
}