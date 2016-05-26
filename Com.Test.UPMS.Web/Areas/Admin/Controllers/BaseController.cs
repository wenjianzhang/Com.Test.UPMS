using Com.Test.Core.DataAccess;
using Com.Test.Models.Model.AccessManagent.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace Com.Test.UPMS.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        private BaseRepository<RoleModel> RoleModelRepository = new BaseRepository<RoleModel>();

        public virtual string CurrentUserID
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    FormsIdentity id = (FormsIdentity)User.Identity;
                    FormsAuthenticationTicket ticket = id.Ticket;
                    return ticket.UserData;
                }
                return null;
            }
        }

        public virtual string CurrentUserName
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    return User.Identity.Name;
                }
                return null;
            }
        }

        public Task<IEnumerable<RoleModel>> GetPermissions
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    return GetCurrentUserPermissions(User.Identity.Name);
                }
                return null;
            }
        }

        private Task<IEnumerable<RoleModel>> GetCurrentUserPermissions(string name)
        {
            string sql = @"select T1.* from RoleModel T1
                            left join UserRole T2 on T1.RoleId = T2.RoleId
                            left join UserInfo T3 on T2.UserId = T3.UserId
                            where T3.UserName='" + name + "' and T1.SystemId =1";
            return RoleModelRepository.GetOneAsync(sql, new RoleModel { });
        }
    }
}