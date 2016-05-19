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
    public class ModelInfoController : Controller
    {
        private BaseRepository<ModelInfo> ModelInfoRepository = new BaseRepository<ModelInfo>();

        // GET: Admin/ModelInfo
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get(int pageSize = 20, int pageCurrent = 1)
        {
            string sql = "select * from ModelInfo where 1=1 and IsDel =0 ";
            IPagedList<ModelInfo> ModelInfoList = await ModelInfoRepository.GetListAsync(sql, pageCurrent, pageSize);
            return Json(AjaxResult.SetResult(new { list = ModelInfoList, pageCount = ModelInfoList.PageCount, totalItemCount = ModelInfoList.TotalItemCount, pageNumber = ModelInfoList.PageNumber, aa = ModelInfoList.Count }), JsonRequestBehavior.AllowGet);
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
                ModelInfo entity = new ModelInfo { ModelId = id };
                var result = await ModelInfoRepository.GetOneAsync("select * from ModelInfo where ModelId=@ModelId and IsDel=0", entity);
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
        public async Task<JsonResult> Post(ModelInfo entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            string insertSQL = @"INSERT INTO ModelInfo (
                                ModelName ,
                                ModelCode ,
                                ModelIcon ,
                                PModelId ,
                                ModelDesc ,
                                Sort ,
                                CreateDate ,
                                Creater ,
                                UpdateDate ,
                                Updater ,
                                IsDel ) VALUES (@ModelName,@ModelCode,@ModelIcon,@PModelId,@ModelDesc,@Sort,@CreateDate,@Creater,@UpdateDate,@Updater,0)";
            try
            {
                int i = await ModelInfoRepository.CreateAsync(insertSQL, entity);
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
            ModelInfo entity = new ModelInfo { ModelId = id, UpdateDate = DateTime.Now };
            try
            {
                var result = await ModelInfoRepository.DeleteAsync("update ModelInfo set IsDel=1,UpdateDate=@UpdateDate where ModelId=@ModelId  and IsDel=0", entity);
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }

        public async Task<JsonResult> Edit(ModelInfo entity)
        {
            //if (string.IsNullOrEmpty(entity.keyname) || string.IsNullOrEmpty(entity.valuecontent))
            //{
            //    return Json(AjaxResult.SetError("参数为空", ErrorCode.ErrorCodes.参数不能为null));
            //}
            entity.UpdateDate = DateTime.Now;
            try
            {
                var result = await ModelInfoRepository.EditAsync(@"update ModelInfo set
                                                                    ModelName = @ModelName,
                                                                    ModelCode = @ModelCode,
                                                                    ModelIcon = @ModelIcon,
                                                                    PModelId = @PModelId,
                                                                    ModelDesc = @ModelDesc,
                                                                    Sort = @Sort,
                                                                    UpdateDate = @UpdateDate,
                                                                    Updater = @Updater where ModelId=@ModelId and IsDel=0", entity);
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }
    }
}