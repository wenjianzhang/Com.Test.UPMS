using Com.Test.Core.DataAccess;
using Com.Test.Models.Data;
using Com.Test.Models.Model.AccessManagent.DataModel;
using Com.Test.UPMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Com.Test.UPMS.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        private BaseRepository<PageModelButtonModel> RoleModelRepository = new BaseRepository<PageModelButtonModel>();

        /// <summary>
        /// 控制按钮
        /// </summary>
        /// <param name="url">Request.Url.AbsolutePath</param>
        /// <param name="name"></param>
        /// <returns></returns>
        private Task<IEnumerable<PageModelButtonModel>> GetCurrentUserPermissionsAsync(string url, string name, string systemid = "0")
        {
            string[] urllist = url.ToLower().Split('/');
            string urlBase = "";
            for (int i = 1; i < urllist.Length - 1; i++)
            {
                urlBase += "/" + urllist[i];
            }
            //urlBase += "/Index";
            string sql = @"select T1.*,T5.ModelUrl,T4.ButtonCode from rolemodel T1
                            left join userrole T2 on T1.RoleId = T2.RoleId
                            left join userinfo T3 on T2.UserId = T3.UserId
                            left join buttoninfo T4 on T1.ButtonId = T4.ButtonId
                            left join modelinfo T5 on T1.ModelId = T5.ModelId
                            where T3.UserName='" + name + "' and T1.SystemId =" + systemid + "  and T5.ModelUrl='" + url.ToLower() + "'";
            var list = RoleModelRepository.GetOneAsync(sql, new PageModelButtonModel { });
            return list;
        }

        public async Task<Dictionary<string, bool>> GetCurrentUserPageButtonAsync(string url, string name = "", string systemid = "0")
        {
            //string name = "";
            if (User.Identity.IsAuthenticated)
            {
                name = User.Identity.Name;
            }

            IEnumerable<PageModelButtonModel> list = await GetCurrentUserPermissionsAsync(url, name, systemid);
            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            foreach (var item in list)
            {
                dic.Add(item.ButtonCode, true);
            }
            return dic;
        }
    }
}