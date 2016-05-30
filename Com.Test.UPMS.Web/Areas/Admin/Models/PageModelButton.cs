using Com.Test.Models.Model.AccessManagent.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Test.UPMS.Web.Areas.Admin.Models
{
    public class PageModelButtonModel : RoleModel
    {
        public string ModelUrl { get; set; }

        public string ButtonCode { get; set; }
    }
}