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
    public class ModelInfoViewData : ModelInfo
    {
        public List<int> ModelButtons { get; set; }
    }
}