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
    public class RoleInfoController : BaseController
    {
        // GET: Admin/RoleInfo
        private BaseRepository<RoleInfo> RoleInfoRepository = new BaseRepository<RoleInfo>();

        // GET: Admin/RoleInfo
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get(int pageSize = 20, int pageCurrent = 1)
        {
            try
            {
                string sql = "select * from roleinfo where 1=1 and IsDel =0 ";
                IPagedList<RoleInfo> RoleInfoList = await RoleInfoRepository.GetListAsync(sql, pageCurrent, pageSize);
                return Json(AjaxResult.SetResult(new { list = RoleInfoList, pageCount = RoleInfoList.PageCount, totalItemCount = RoleInfoList.TotalItemCount, pageNumber = RoleInfoList.PageNumber, aa = RoleInfoList.Count }), JsonRequestBehavior.AllowGet);
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
                RoleInfo entity = new RoleInfo { RoleId = id };
                var result = await RoleInfoRepository.GetOneAsync("select * from roleinfo where RoleId=@RoleId and IsDel=0", entity);
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
        public async Task<JsonResult> Post(RoleInfo entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            string insertSQL = @"INSERT INTO roleinfo (
                                RoleName ,
                                RoleCode ,
                                RoleDesc ,
                                RoleIcon ,
                                Sort ,
                                CreateDate ,
                                Creater ,
                                UpdateDate ,
                                Updater ,
                                IsDel ) VALUES (@RoleName,@RoleCode,@RoleDesc,@RoleIcon,@Sort,@CreateDate,@Creater,@UpdateDate,@Updater,0);select @@IDENTITY";
            try
            {
                int i = await RoleInfoRepository.ScalarAsync(insertSQL, entity);
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
            RoleInfo entity = new RoleInfo { RoleId = id, UpdateDate = DateTime.Now };
            try
            {
                var result = await RoleInfoRepository.DeleteAsync("update roleinfo set IsDel=1,UpdateDate=@UpdateDate where RoleId=@RoleId  and IsDel=0", entity);
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }

        public async Task<JsonResult> Edit(RoleInfo entity)
        {
            //if (string.IsNullOrEmpty(entity.keyname) || string.IsNullOrEmpty(entity.valuecontent))
            //{
            //    return Json(AjaxResult.SetError("参数为空", ErrorCode.ErrorCodes.参数不能为null));
            //}
            entity.UpdateDate = DateTime.Now;
            try
            {
                var result = await RoleInfoRepository.EditAsync("update roleinfo Set RoleName=@RoleName,RoleCode=@RoleCode,RoleIcon=@RoleIcon,RoleDesc=@RoleDesc,UpdateDate=@UpdateDate,Sort=@Sort where RoleId=@RoleId and IsDel=0", entity);
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }
    }
}