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
    public class SystemInfoController : Controller
    {
        private BaseRepository<SystemInfo> repository = new BaseRepository<SystemInfo>();

        // GET: Admin/SystemInfo
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get(int pageSize = 20, int pageCurrent = 1)
        {
            string sql = "select * from SystemInfo where 1=1 and IsDel =0 ";
            IPagedList<SystemInfo> SystemInfoList = await repository.GetListAsync(sql, pageCurrent, pageSize);
            return Json(AjaxResult.SetResult(new { list = SystemInfoList, pageCount = SystemInfoList.PageCount, totalItemCount = SystemInfoList.TotalItemCount, pageNumber = SystemInfoList.PageNumber, aa = SystemInfoList.Count }), JsonRequestBehavior.AllowGet);
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
                SystemInfo entity = new SystemInfo { SystemId = id };
                var result = await repository.GetOneAsync("select * from SystemInfo where SystemId=@SystemId and IsDel=0", entity);
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
        public async Task<JsonResult> Post(SystemInfo entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            string insertSQL = @"INSERT INTO SystemInfo (
                                SystemName ,
                                SystemCode ,
                                SystemIcon ,
                                IsEnable ,
                                Sort ,
                                CreateDate ,
                                Creater ,
                                UpdateDate ,
                                Updater ,
                                IsDel ) VALUES (@SystemName,@SystemCode,@SystemIcon,@IsEnable ,@Sort,@CreateDate,@Creater,@UpdateDate,@Updater,0)";
            try
            {
                int i = await repository.CreateAsync(insertSQL, entity);
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
            SystemInfo entity = new SystemInfo { SystemId = id, UpdateDate = DateTime.Now };
            try
            {
                var result = await repository.DeleteAsync("update SystemInfo set IsDel=1,UpdateDate=@UpdateDate where SystemId=@SystemId  and IsDel=0", entity);
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }

        public async Task<JsonResult> Edit(SystemInfo entity)
        {
            //if (string.IsNullOrEmpty(entity.keyname) || string.IsNullOrEmpty(entity.valuecontent))
            //{
            //    return Json(AjaxResult.SetError("参数为空", ErrorCode.ErrorCodes.参数不能为null));
            //}
            entity.UpdateDate = DateTime.Now;
            try
            {
                var result = await repository.EditAsync("update SystemInfo Set SystemName=@SystemName,SystemCode=@SystemCode,SystemIcon=@SystemIcon,UpdateDate=@UpdateDate,Sort=@Sort where SystemId=@SystemId and IsDel=0", entity);
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }
    }
}