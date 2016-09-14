using Com.Test.Core.DataAccess;
using Com.Test.Models.Data;
using Com.Test.Models.Model.AccessManagent.DataModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Com.Test.UPMS.Api.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class UserMenuController : BaseApiController
    {
        private BaseRepository<ModelInfo> ModelInfoRepository = new BaseRepository<ModelInfo>();

        // GET: api/UserMenu/5
        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<JsonResult<AjaxResult>> Get(string value)
        {
            JObject job = JObject.Parse(value).Value<JObject>();
            try
            {
                IEnumerable<ModelInfo> modelinfo = await GetCurrentUserModel(job.Value<string>("username"), job.Value<int>("systemId"));
                return Json(AjaxResult.SetResult(modelinfo));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }

        private Task<IEnumerable<ModelInfo>> GetCurrentUserModel(string name, int system = 0)
        {
            string sql = @"select distinct  T1.* from modelinfo T1
                            left join rolemodel T2 on T1.ModelId = T2.ModelId
                            left join userrole T3 on T2.RoleId = T3.RoleId
                            left join userinfo T4 on T3.UserId = T4.UserId
                            where T4.UserName='" + name + "'  and T1.SystemId =@SystemId and T1.IsDel=0";
            return ModelInfoRepository.GetOneAsync(sql, new ModelInfo { SystemId = system });
        }

        [HttpGet]
        public async Task<JsonResult<AjaxResult>> GetPageButton(string url, string name, string systemid = "0")
        {
            try
            {
                Dictionary<string, bool> dic = await GetCurrentUserPageButtonAsync(url, name, systemid);
                return Json(AjaxResult.SetResult(dic));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }
    }
}