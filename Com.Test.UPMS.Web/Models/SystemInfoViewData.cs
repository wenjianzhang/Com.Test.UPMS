using Com.Test.Models.Model.AccessManagent.DataModel;

/**
* 作者：张文健
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Test.UPMS.Web.Models
{
    public class SystemInfoViewData : SystemInfo
    {
        public List<int> SystemRoles { get; set; }
    }

    public class SystemRoles
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }
}