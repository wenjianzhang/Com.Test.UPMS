using Com.Test.Core.DataAccess;
using Com.Test.Models.Data;
using Com.Test.Models.Model.AccessManagent.DataModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.Test.UPMS.Web.Areas.Admin.Controllers
{
    public class UserRoleController : BaseController
    {
        private BaseRepository<UserRole> UserRoleRepository = new BaseRepository<UserRole>();

        // GET: Admin/UserRole
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get(int pageSize = 20, int pageCurrent = 1)
        {
            try
            {
                string sql = "select * from UserRole where 1=1 ";
                IPagedList<UserRole> UserRoleList = await UserRoleRepository.GetListAsync(sql, pageCurrent, pageSize);
                return Json(AjaxResult.SetResult(new { list = UserRoleList, pageCount = UserRoleList.PageCount, totalItemCount = UserRoleList.TotalItemCount, pageNumber = UserRoleList.PageNumber, aa = UserRoleList.Count }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetOne(int id)
        {
            if (id == 0)
            {
                return Json(AjaxResult.SetError("参数为空", ErrorCode.ErrorCodes.参数不能为null));
            }
            try
            {
                UserRole entity = new UserRole { UserId = id };
                var result = await UserRoleRepository.GetOneAsync("select * from UserRole where UserId=@UserId ", entity);
                if (result == null)
                {
                    return Json(AjaxResult.SetError("未找到符合条件数据！", ErrorCode.ErrorCodes.获取数据失败), JsonRequestBehavior.AllowGet);
                }
                return Json(AjaxResult.SetResult(result.FirstOrDefault()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Post(UserRole entity)
        {
            string insertSQL = @"Delete from UserRole where UserId=@UserId;
                                INSERT INTO AccessManagent.UserRole(
                                   UserId
                                  ,RoleId
,SystemId
                                ) VALUES (
                                   @UserId  -- UserName - IN varchar(200)
                                  ,@RoleId  -- UserPassword - IN varchar(200)
,@SystemId
                                );select @@IDENTITY";
            try
            {
                int i = await UserRoleRepository.ScalarAsync(insertSQL, entity);
                return Json(AjaxResult.SetResult(i));
            }
            catch (System.Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }

        public async Task<JsonResult> Delete(int id)
        {
            if (id == 0)
            {
                return Json(AjaxResult.SetError("参数为空", ErrorCode.ErrorCodes.参数不能为null));
            }
            UserRole entity = new UserRole { UserId = id };
            try
            {
                var result = await UserRoleRepository.DeleteAsync("Delete from UserRole where UserId=@UserId ", entity);
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }

        public async Task<JsonResult> Edit(UserRole entity)
        {
            //if (string.IsNullOrEmpty(entity.keyname) || string.IsNullOrEmpty(entity.valuecontent))
            //{
            //    return Json(AjaxResult.SetError("参数为空", ErrorCode.ErrorCodes.参数不能为null));
            //}
            try
            {
                var result = await UserRoleRepository.EditAsync("update UserRole Set UserId=@UserId,RoleId=@RoleId,SystemId=@SystemId where UserId=@UserId ", entity);
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }
    }
}