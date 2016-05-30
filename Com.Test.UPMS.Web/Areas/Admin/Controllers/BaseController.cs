using Com.Test.Core.DataAccess;
using Com.Test.Models.Model.AccessManagent.DataModel;
using Com.Test.UPMS.Web.Areas.Admin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace Com.Test.UPMS.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public BaseRepository<PageModelButtonModel> RoleModelRepository = new BaseRepository<PageModelButtonModel>();
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

        public Task<IEnumerable<PageModelButtonModel>> GetPermissions
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
        private Task<IEnumerable<PageModelButtonModel>> GetCurrentUserPermissions(string name)
        {
            string[] urllist = Request.Url.AbsolutePath.ToLower().Split('/');
            string urlBase = "";
            for (int i = 1; i < urllist.Length - 1; i++)
            {
                urlBase += "/" + urllist[i];
            }
            urlBase += "/Index";
            string sql = @"select T1.*,T5.ModelUrl,T4.ButtonCode from RoleModel T1
                            left join UserRole T2 on T1.RoleId = T2.RoleId
                            left join UserInfo T3 on T2.UserId = T3.UserId
                            left join ButtonInfo T4 on T1.ButtonId = T4.ButtonId
                            left join ModelInfo T5 on T1.ModelId = T5.ModelId
                            where T3.UserName='" + name + "' and T1.SystemId =1 and T5.ModelUrl='" + urlBase.ToLower() + "'";
            return RoleModelRepository.GetOneAsync(sql, new PageModelButtonModel { });
        }

        public Dictionary<string, bool> GetCurrentUserPageButton()
        {
            string name = "";
            if (User.Identity.IsAuthenticated)
            {
                name = User.Identity.Name;
            }

            IEnumerable<PageModelButtonModel> list = GetCurrentUserPermissions(name).Result;
            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            foreach (var item in list)
            {
                dic.Add(item.ButtonCode, true);
            }
            return dic;
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