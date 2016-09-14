using Com.Test.Core.DataAccess;
using Com.Test.Models.Data;
using Com.Test.Models.Model.AccessManagent.DataModel;
using Com.Test.UPMS.Api.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Security;

namespace Com.Test.UPMS.Api.Controllers
{
    public class LoginController : ApiController
    {
        private BaseRepository<LoginOnModel> loginRepository = new BaseRepository<LoginOnModel>();
        private BaseRepository<RoleModel> RoleModelRepository = new BaseRepository<RoleModel>();

        // GET: api/Login/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Login
        [HttpPost]
        public JsonResult<AjaxResult> Post(LoginOnModel value)
        {
            //var UserName = JObject.Parse(value).Value<string>("UserName");
            //var Password = JObject.Parse(value).Value<string>("Password");
            //JObject
            //LoginOnModel
            var obj = loginRepository.GetOne(@"select userinfo.*,roleinfo.RoleCode from userinfo
                                                left join userrole on userinfo.UserId = userrole.UserId
                                                left join roleinfo on userrole.RoleId = roleinfo.RoleId
                                                where userinfo.UserName=@UserName and userinfo.UserPassword=@Password and userinfo.IsDel=0;", value).FirstOrDefault();

            if (obj != null)
            {
                return Json(AjaxResult.SetResult(obj));
            }
            else
            {
                return Json(AjaxResult.SetError("登陆失败！", ErrorCode.ErrorCodes.账号或密码错误));
            }
        }

        //[HttpPost]
        //public async Task<JsonResult<AjaxResult>> GetCurrentModel()
        //{
        //    try
        //    {
        //        IEnumerable<ModelInfo> modelInfo = await GetCurrentUserModel("");
        //        return Json(AjaxResult.SetResult(modelInfo));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
        //    }
        //}
    }
}