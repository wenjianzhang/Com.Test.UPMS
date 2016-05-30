using Com.Test.Core.DataAccess;
using Com.Test.Models.Data;
using Com.Test.Models.Model.AccessManagent.DataModel;
using Com.Test.UPMS.Web.Areas.Admin.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.Test.UPMS.Web.Areas.Admin.Controllers
{
    public class ModelInfoController : BaseController
    {
        private BaseRepository<ModelInfoViewData> ModelInfoRepository = new BaseRepository<ModelInfoViewData>();

        private BaseRepository<ModelButton> ModelButtonRepository = new BaseRepository<ModelButton>();

        // GET: Admin/ModelInfo
        public async Task<ActionResult> Index()
        {
            IEnumerable<PageModelButtonModel> PageModelButtons = await GetPermissions;
            ViewBag.PageModelButtons = PageModelButtons.Where(s => s.ModelUrl.ToLower() == Request.Url.AbsolutePath.ToLower());
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get(int pageSize = 20, int pageCurrent = 1, int SystemId = 0)
        {
            try
            {
                string sql = "select * from ModelInfo where 1=1 and IsDel =0 ";
                //if (SystemId != 0)
                //{
                sql += " and SystemId=" + SystemId + " ";
                //}
                IPagedList<ModelInfoViewData> ModelInfoList = await ModelInfoRepository.GetListAsync(sql, pageCurrent, pageSize);
                List<ModelInfoViewData> modelInfoListReslut = new List<ModelInfoViewData>();
                foreach (var itemp in ModelInfoList.Where(s => s.PModelId == 0))
                {
                    modelInfoListReslut.Add(itemp);
                    foreach (var item in ModelInfoList.Where(s => s.PModelId == itemp.ModelId))
                    {
                        modelInfoListReslut.Add(item);
                    }
                }

                return Json(AjaxResult.SetResult(new { list = modelInfoListReslut, pageCount = ModelInfoList.PageCount, totalItemCount = ModelInfoList.TotalItemCount, pageNumber = ModelInfoList.PageNumber, aa = ModelInfoList.Count }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetList(int pageSize = 20, int pageCurrent = 1, int SystemId = 0)
        {
            string sql = "select * from ModelInfo where 1=1 and IsDel =0 ";
            //if (SystemId != 0)
            //{
            sql += " and SystemId=" + SystemId + " ";
            //}
            IPagedList<ModelInfoViewData> ModelInfoList = await ModelInfoRepository.GetListAsync(sql, pageCurrent, pageSize);
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
                ModelInfoViewData entity = new ModelInfoViewData { ModelId = id };
                var result = await ModelInfoRepository.GetOneAsync("select * from ModelInfo where ModelId=@ModelId and IsDel=0", entity);

                var result2 = await ModelButtonRepository.GetListAsync("select ButtonId from ModelButton where ModelId='" + id + "'", 1, 1000);
                ModelInfoViewData resultEntity = result.FirstOrDefault();
                List<int> buttons = new List<int>();
                foreach (var item in result2)
                {
                    buttons.Add(item.ButtonId);
                }
                resultEntity.ModelButtons = buttons;

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
        public async Task<JsonResult> Post(ModelInfoViewData entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            string insertSQL = @"INSERT INTO ModelInfo (
                                ModelName ,
SystemId,
                                ModelCode ,
                                ModelIcon ,
                                PModelId ,
                                ModelDesc ,
                                Sort ,
                                CreateDate ,
                                Creater ,
                                UpdateDate ,
                                Updater ,
                                IsDel ) VALUES (@ModelName,@SystemId,@ModelCode,@ModelIcon,@PModelId,@ModelDesc,@Sort,@CreateDate,@Creater,@UpdateDate,@Updater,0);select @@IDENTITY";

            string insertButtonsSQL = @"Delete from AccessManagent.ModelButton where ModelId=@ModelId;";

            foreach (var item in entity.ModelButtons)
            {
                insertButtonsSQL += "INSERT INTO AccessManagent.ModelButton( ModelId ,ButtonId) VALUES ('" + entity.ModelId + " ','" + item + "' ); ";
            }

            try
            {
                int i = await ModelInfoRepository.ScalarAsync(insertSQL, entity);

                int a = await ModelButtonRepository.ScalarAsync(insertButtonsSQL, new ModelButton { ModelId = i });

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
            ModelInfoViewData entity = new ModelInfoViewData { ModelId = id, UpdateDate = DateTime.Now };
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

        public async Task<JsonResult> Edit(ModelInfoViewData entity)
        {
            //if (string.IsNullOrEmpty(entity.keyname) || string.IsNullOrEmpty(entity.valuecontent))
            //{
            //    return Json(AjaxResult.SetError("参数为空", ErrorCode.ErrorCodes.参数不能为null));
            //}
            entity.UpdateDate = DateTime.Now;

            string insertButtonsSQL = @"Delete from AccessManagent.ModelButton where ModelId=@ModelId;";

            foreach (var item in entity.ModelButtons)
            {
                insertButtonsSQL += "INSERT INTO AccessManagent.ModelButton( ModelId ,ButtonId) VALUES ('" + entity.ModelId + " ','" + item + "' ); ";
            }

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
                var a = await ModelButtonRepository.CreateAsync(insertButtonsSQL, new ModelButton { ModelId = entity.ModelId });

                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetCurrentModel()
        {
            try
            {
                IEnumerable<ModelInfo> modelInfo = await GetModel;
                return Json(AjaxResult.SetResult(modelInfo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误), JsonRequestBehavior.AllowGet);
            }
        }
    }
}