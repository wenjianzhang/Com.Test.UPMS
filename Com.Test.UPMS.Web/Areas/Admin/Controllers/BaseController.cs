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
        private BaseRepository<ModelInfo> ModelInfoRepository = new BaseRepository<ModelInfo>();

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

        public Task<IEnumerable<ModelInfo>> GetModel
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    return GetCurrentUserModel(User.Identity.Name);
                }
                return null;
            }
        }

        /// <summary>
        /// 控制按钮
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Task<IEnumerable<RoleModel>> GetCurrentUserPermissions(string name)
        {
            string sql = @"select T1.* from RoleModel T1
                            left join UserRole T2 on T1.RoleId = T2.RoleId
                            left join UserInfo T3 on T2.UserId = T3.UserId
                            where T3.UserName='" + name + "' and T1.SystemId =1";
            return RoleModelRepository.GetOneAsync(sql, new RoleModel { });
        }

        private Task<IEnumerable<ModelInfo>> GetCurrentUserModel(string name)
        {
            string sql = @"select distinct  T1.* from ModelInfo T1
                            left join RoleModel T2 on T1.ModelId = T2.ModelId
                            left join UserRole T3 on T2.RoleId = T3.RoleId
                            left join UserInfo T4 on T3.UserId = T4.UserId
                            where T4.UserName='" + name + "' and T1.IsDel=0";
            return ModelInfoRepository.GetOneAsync(sql, new ModelInfo { });
        }
    }
}