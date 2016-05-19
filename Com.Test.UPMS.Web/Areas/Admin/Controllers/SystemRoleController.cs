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
    public class SystemRoleController : Controller
    {
        private BaseRepository<SystemRole> SystemRoleRepository = new BaseRepository<SystemRole>();

        // GET: Admin/SystemRole
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get(int pageSize = 20, int pageCurrent = 1)
        {
            string sql = "select * from SystemRole where 1=1 ";
            IPagedList<SystemRole> SystemRoleList = await SystemRoleRepository.GetListAsync(sql, pageCurrent, pageSize);
            return Json(AjaxResult.SetResult(new { list = SystemRoleList, pageCount = SystemRoleList.PageCount, totalItemCount = SystemRoleList.TotalItemCount, pageNumber = SystemRoleList.PageNumber, aa = SystemRoleList.Count }), JsonRequestBehavior.AllowGet);
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
                SystemRole entity = new SystemRole { SystemId = id };
                var result = await SystemRoleRepository.GetOneAsync("select * from SystemRole where SystemId=@SystemId ", entity);
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
        public async Task<JsonResult> Post(SystemRole entity)
        {
            string insertSQL = @"Delete from SystemRole where SystemId=@SystemId;
                                INSERT INTO AccessManagent.SystemRole( SystemId ,RoleId) VALUES (@SystemId ,@RoleId );select @@IDENTITY ";
            try
            {
                int i = await SystemRoleRepository.ScalarAsync(insertSQL, entity);
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
            SystemRole entity = new SystemRole { SystemId = id };
            try
            {
                var result = await SystemRoleRepository.DeleteAsync("Delete from SystemRole where SystemId=@SystemId ", entity);
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }

        public async Task<JsonResult> Edit(SystemRole entity)
        {
            //if (string.IsNullOrEmpty(entity.keyname) || string.IsNullOrEmpty(entity.valuecontent))
            //{
            //    return Json(AjaxResult.SetError("参数为空", ErrorCode.ErrorCodes.参数不能为null));
            //}
            try
            {
                var result = await SystemRoleRepository.EditAsync("update SystemRole Set SystemId=@SystemId,RoleId=@RoleId where SystemId=@SystemId ", entity);
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }
    }
}