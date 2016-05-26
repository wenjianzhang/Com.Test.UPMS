﻿using Com.Test.Models.Model.AccessManagent.DataModel;
using Com.Test.UPMS.Web.Areas.Admin.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Com.Test.UPMS.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            IEnumerable<RoleModel> list = GetPermissions.Result;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}