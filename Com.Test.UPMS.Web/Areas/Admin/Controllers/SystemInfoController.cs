using Com.Test.Core.DataAccess;
using Com.Test.Models.Data;
using Com.Test.Models.Model.AccessManagent.DataModel;
using Com.Test.UPMS.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.Test.UPMS.Web.Areas.Admin.Controllers
{
    public class SystemInfoController : BaseController
    {
        private BaseRepository<SystemInfoViewData> repository = new BaseRepository<SystemInfoViewData>();
        private BaseRepository<SystemRole> SystemRoleRepository = new BaseRepository<SystemRole>();

        // GET: Admin/SystemInfo
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get(int pageSize = 20, int pageCurrent = 1)
        {
            try
            {
                string sql = "select * from systeminfo where 1=1 and IsDel =0 ";
                IPagedList<SystemInfoViewData> SystemInfoList = await repository.GetListAsync(sql, pageCurrent, pageSize);
                return Json(AjaxResult.SetResult(new { list = SystemInfoList, pageCount = SystemInfoList.PageCount, totalItemCount = SystemInfoList.TotalItemCount, pageNumber = SystemInfoList.PageNumber, aa = SystemInfoList.Count }), JsonRequestBehavior.AllowGet);
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
                SystemInfoViewData entity = new SystemInfoViewData { SystemId = id };
                var result = await repository.GetOneAsync("select * from systeminfo where SystemId=@SystemId and IsDel=0", entity);

                var result2 = await SystemRoleRepository.GetListAsync("select RoleId from systemrole where SystemId='" + id + "'", 1, 1000);
                SystemInfoViewData resultEntity = result.FirstOrDefault();
                List<int> roles = new List<int>();
                foreach (var item in result2)
                {
                    roles.Add(item.RoleId);
                }
                resultEntity.SystemRoles = roles;
                if (result == null)
                {
                    return Json(AjaxResult.SetError("未找到符合条件数据！", ErrorCode.ErrorCodes.获取数据失败), JsonRequestBehavior.AllowGet);
                }
                return Json(AjaxResult.SetResult(resultEntity), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Post(SystemInfoViewData entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            string insertSQL = @"INSERT INTO systeminfo (
                                SystemName ,
                                SystemCode ,
                                SystemIcon ,
                                IsEnable ,
                                Sort ,
                                CreateDate ,
                                Creater ,
                                UpdateDate ,
                                Updater ,
                                IsDel ) VALUES (@SystemName,@SystemCode,@SystemIcon,@IsEnable ,@Sort,@CreateDate,@Creater,@UpdateDate,@Updater,0);select @@IDENTITY";

            string insertRolesSQL = @"Delete from SystemRole where SystemId=@SystemId;";

            foreach (var item in entity.SystemRoles)
            {
                insertRolesSQL += "INSERT INTO systemrole( SystemId ,RoleId) VALUES (@SystemId,'" + item + "' ); ";
            }

            try
            {
                int i = await repository.ScalarAsync(insertSQL, entity);

                int a = await SystemRoleRepository.ScalarAsync(insertRolesSQL, new SystemRole { SystemId = i });
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
            SystemInfoViewData entity = new SystemInfoViewData { SystemId = id, UpdateDate = DateTime.Now };
            try
            {
                var result = await repository.DeleteAsync("update systeminfo set IsDel=1,UpdateDate=@UpdateDate where SystemId=@SystemId  and IsDel=0", entity);
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }

        public async Task<JsonResult> Edit(SystemInfoViewData entity)
        {
            //if (string.IsNullOrEmpty(entity.keyname) || string.IsNullOrEmpty(entity.valuecontent))
            //{
            //    return Json(AjaxResult.SetError("参数为空", ErrorCode.ErrorCodes.参数不能为null));
            //}

            string insertRolesSQL = @"Delete from systemrole where SystemId=@SystemId;";

            foreach (var item in entity.SystemRoles)
            {
                insertRolesSQL += "INSERT INTO systemrole( SystemId ,RoleId) VALUES ('" + entity.SystemId + " ','" + item + "' ); ";
            }

            entity.UpdateDate = DateTime.Now;
            try
            {
                var result = await repository.EditAsync("update systeminfo Set SystemName=@SystemName,SystemCode=@SystemCode,SystemIcon=@SystemIcon,UpdateDate=@UpdateDate,Sort=@Sort where SystemId=@SystemId and IsDel=0", entity);

                int a = await SystemRoleRepository.CreateAsync(insertRolesSQL, new SystemRole { SystemId = entity.SystemId });

                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }
    }
}