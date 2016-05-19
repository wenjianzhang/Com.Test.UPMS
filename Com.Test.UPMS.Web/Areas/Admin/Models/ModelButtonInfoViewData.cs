using Com.Test.Models.Model.AccessManagent.DataModel;

/**
* 作者：张文健
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Test.UPMS.Web.Areas.Admin.Models
{
    public class ModelButtonInfoViewData : ButtonInfo
    {
        public int Id { get; set; }

        public int SystemId { get; set; }

        public int ModelId { get; set; }

        public bool Checked { get; set; }

        public int RoleId { get; set; }
    }

    public class ModelRoleViewData
    {
        public int RoleId { get; set; }
        public int SystemId { get; set; }

        public List<ModelButtonInfoViewData> ModelButtonList { get; set; }
    }

    public class ModelButtonViewData
    {
        public int Id { get; set; }
        public int SystemId { get; set; }
        public int ModelId { get; set; }
        public bool Checked { get; set; }
        public int RoleId { get; set; }
        public string ButtonClass { get; set; }
        public string ButtonCode { get; set; }
        public string ButtonIcon { get; set; }
        public int ButtonId { get; set; }
        public string ButtonName { get; set; }
    }
}