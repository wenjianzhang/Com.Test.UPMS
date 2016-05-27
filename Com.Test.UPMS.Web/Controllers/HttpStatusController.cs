using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.Test.UPMS.Web.Controllers
{
    public class HttpStatusController : Controller
    {
        // GET: HttpStatus
        public ActionResult Http403()
        {
            return View();
        }

        public ActionResult Http404()
        {
            return View();
        }

        public ActionResult Http500()
        {
            return View();
        }
    }
}