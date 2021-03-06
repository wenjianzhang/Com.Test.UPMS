﻿using Com.Test.Core.DataAccess;
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
    public class ButtonInfoController : BaseController
    {
        // GET: Admin/ButtonInfo
        private BaseRepository<ButtonInfo> ButtonInfoRepository = new BaseRepository<ButtonInfo>();

        // GET: Admin/ButtonInfo
        public async Task<ActionResult> Index()
        {
            //try
            //{
            //    IEnumerable<PageModelButtonModel> PageModelButtons = await GetPermissions;
            //    ViewBag.PageModelButtons = PageModelButtons.Where(s => s.ModelUrl.ToLower() == Request.Url.AbsolutePath.ToLower());
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get(int pageSize = 20, int pageCurrent = 1)
        {
            string sql = "select * from buttoninfo where 1=1 and IsDel =0 ";
            try
            {
                IPagedList<ButtonInfo> ButtonInfoList = await ButtonInfoRepository.GetListAsync(sql, pageCurrent, pageSize);
                return Json(AjaxResult.SetResult(new { list = ButtonInfoList, pageCount = ButtonInfoList.PageCount, totalItemCount = ButtonInfoList.TotalItemCount, pageNumber = ButtonInfoList.PageNumber, aa = ButtonInfoList.Count }), JsonRequestBehavior.AllowGet);
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
                ButtonInfo entity = new ButtonInfo { ButtonId = id };
                var result = await ButtonInfoRepository.GetOneAsync("select * from buttoninfo where ButtonId=@ButtonId and IsDel=0", entity);
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
        public async Task<JsonResult> Post(ButtonInfo entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            string insertSQL = @"INSERT INTO buttoninfo (
                                 ButtonName ,
                                 ButtonCode ,
                                 ButtonIcon ,
                                 ButtonDesc ,
                                 Sort ,
                                 CreateDate ,
                                 Creater ,
                                 UpdateDate ,
                                 Updater ,
                                 IsDel ) VALUES (@ButtonName,@ButtonCode,@ButtonIcon,@ButtonDesc,@Sort,@CreateDate,@Creater,@UpdateDate,@Updater,0);select @@IDENTITY";
            try
            {
                int i = await ButtonInfoRepository.ScalarAsync(insertSQL, entity);
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
            ButtonInfo entity = new ButtonInfo { ButtonId = id, UpdateDate = DateTime.Now };
            try
            {
                var result = await ButtonInfoRepository.DeleteAsync("update buttoninfo set IsDel=1,UpdateDate=@UpdateDate where ButtonId=@ButtonId  and IsDel=0", entity);
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }

        public async Task<JsonResult> Edit(ButtonInfo entity)
        {
            //if (string.IsNullOrEmpty(entity.keyname) || string.IsNullOrEmpty(entity.valuecontent))
            //{
            //    return Json(AjaxResult.SetError("参数为空", ErrorCode.ErrorCodes.参数不能为null));
            //}
            entity.UpdateDate = DateTime.Now;
            try
            {
                var result = await ButtonInfoRepository.EditAsync("update buttoninfo Set ButtonName=@ButtonName,ButtonCode=@ButtonCode,ButtonIcon=@ButtonIcon,ButtonDesc=@ButtonDesc,UpdateDate=@UpdateDate where ButtonId=@ButtonId and IsDel=0", entity);
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }

        [HttpGet]
        public JsonResult GetPageButton()
        {
            try
            {
                Dictionary<string, bool> dic = GetCurrentUserPageButton();
                return Json(AjaxResult.SetResult(dic), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误), JsonRequestBehavior.AllowGet);
            }
        }
    }
}