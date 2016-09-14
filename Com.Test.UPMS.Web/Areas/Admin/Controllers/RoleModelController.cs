using Com.Test.Core.DataAccess;
using Com.Test.Models.Data;
using Com.Test.Models.Model.AccessManagent.DataModel;
using Com.Test.UPMS.Web.Areas.Admin.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Com.Test.UPMS.Web.Areas.Admin.Controllers
{
    public class RoleModelController : BaseController
    {
        private BaseRepository<RoleModelViewData> RoleModelRepository = new BaseRepository<RoleModelViewData>();
        private BaseRepository<RoleModel> RoleModelBaseRepository = new BaseRepository<RoleModel>();
        private BaseRepository<ModelButtonInfoViewData> ModelButtonInfoRepository = new BaseRepository<ModelButtonInfoViewData>();
        private BaseRepository<ModelButtonViewData> ModelButtonRepository = new BaseRepository<ModelButtonViewData>();
        private BaseRepository<ModelInfo> ModelInfoRepository = new BaseRepository<ModelInfo>();
        private BaseRepository<ModelRoleViewData> ModelRoleRepository = new BaseRepository<ModelRoleViewData>();

        // GET: Admin/RoleModel
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetRoleModel(int SystemId = 0)
        {
            try
            {
                string sql = "select T2.* from systemrole T1 left join roleinfo T2 on T1.RoleId = T2.RoleId where T1.SystemId = @SystemId ";
                IEnumerable<RoleModelViewData> RoleModelList = await RoleModelRepository.GetOneAsync(sql, new RoleModelViewData { SystemId = SystemId });
                return Json(AjaxResult.SetResult(new { list = RoleModelList }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetModelInfo(int SystemId = 0)
        {
            string sql = "select * from modelinfo where IsDel=0 and SystemId= @SystemId ";
            IEnumerable<ModelInfo> RoleModelList = await ModelInfoRepository.GetOneAsync(sql, new ModelInfo { SystemId = SystemId });
            return Json(AjaxResult.SetResult(new { list = RoleModelList }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetModelButtonInfo(int SystemId = 0)
        {
            string sql = "select T1.SystemId,T1.ModelId,T3.* from modelinfo T1  left join modelbutton T2 on T1.ModelId = T2.ModelId left join buttoninfo T3 on T2.ButtonId = T3.ButtonId where T1.SystemId = @SystemId ";
            IEnumerable<ModelButtonViewData> ModelButtonInfoList = await ModelButtonRepository.GetOneAsync(sql, new ModelButtonViewData { SystemId = SystemId });
            return Json(AjaxResult.SetResult(new { list = ModelButtonInfoList }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> SetModelButtonInfo(int SystemId = 0, int RoleId = 0)
        {//select T1.*,T3.* from SystemModel T1
            string sql = "select T1.SystemId,T1.ModelId,T3.* from modelinfo T1  left join modelbutton T2 on T1.ModelId = T2.ModelId left join buttoninfo T3 on T2.ButtonId = T3.ButtonId where T1.SystemId = @SystemId ";

            string sql2 = "select * from rolemodel where SystemId=@SystemId and RoleId=@RoleId;";
            try
            {
                IEnumerable<RoleModel> RoleModelList = await RoleModelBaseRepository.GetOneAsync(sql2, new RoleModel { SystemId = SystemId, RoleId = RoleId });

                IEnumerable<ModelButtonViewData> ModelButtonInfoList = await ModelButtonRepository.GetOneAsync(sql, new ModelButtonViewData { SystemId = SystemId });

                //ModelButtonInfoList = ModelButtonInfoList.Where(s => s.ButtonId > 0);

                foreach (var item in ModelButtonInfoList.Where(s => s.ButtonId > 0))
                {
                    IEnumerable<RoleModel> temp = RoleModelList.Where(s => s.ModelId == item.ModelId && s.ButtonId == item.ButtonId);
                    if (temp.Count() > 0)
                    {
                        item.Checked = true;
                    }
                }

                return Json(AjaxResult.SetResult(new { list = ModelButtonInfoList }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateRoleModel(string ModelButtonList, int RoleId, int SystemId)
        {
            string sql = "delete from rolemodel where SystemId=@SystemId; ";

            //string insert_PModel = "select PModelId,SystemId from ModelInfo where ModeId in( 0";
            JavaScriptSerializer Serializer = new JavaScriptSerializer();

            List<ModelButtonInfoViewData> ModelButton = Serializer.Deserialize<List<ModelButtonInfoViewData>>(ModelButtonList);

            foreach (var item in ModelButton)
            {
                sql += string.Format("insret into rolemodel(SystemId,ModelId,RoleId,ButtonId)values(@SystemId,@ModelId,'" + RoleId + "',@ButtonId);", item);
            }
            //foreach (var item in ModelButton)
            //{
            //    insert_PModel += "," + item.ModelId;
            //}
            //insert_PModel += ");";
            if (ModelButton.Count() > 0)
            {
                var i = await ModelRoleRepository.CreateAsync(sql, new ModelRoleViewData { SystemId = ModelButton.FirstOrDefault().SystemId });
                return Json(AjaxResult.SetResult(i));
            }
            return Json(AjaxResult.SetResult(0));
        }

        [HttpPost]
        public async Task<JsonResult> UpdateRM(int SystemId, int ModelId, int ButtonId, int RoleId, bool Checked)
        {
            string sql = "";
            if (Checked)
            {
                sql = "delete from rolemodel where SystemId=@SystemId and ModelId='" + ModelId + "' and RoleId='" + RoleId + "' and ButtonId='" + ButtonId + "'; ";

                sql += "insert into rolemodel(SystemId,ModelId,RoleId,ButtonId)values(@SystemId,'" + ModelId + "','" + RoleId + "','" + ButtonId + "');";
            }
            else
            {
                sql = "delete from rolemodel where SystemId=@SystemId and ModelId='" + ModelId + "' and RoleId='" + RoleId + "' and ButtonId='" + ButtonId + "'; ";
            }

            try
            {
                var result = await ModelRoleRepository.CreateAsync(sql, new ModelRoleViewData { SystemId = SystemId });
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }
    }
}