#region License

/**
 * 作者：张文健
 */

#endregion License

using Com.Test.Models.Model.AccessManagent.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Test.UPMS.Api.Models
{
    public class PageModelButtonModel : RoleModel
    {
        public string ModelUrl { get; set; }

        public string ButtonCode { get; set; }
    }
}