using Com.Test.Core.DataAccess;
using Com.Test.Models.Data;
using Com.Test.Models.Model.AccessManagent.DataModel;
using Com.Test.UPMS.Web.Areas.Admin.Models;
using PagedList;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Com.Test.UPMS.Web.Areas.Admin.Controllers
{
    public class UserInfoController : BaseController
    {
        private BaseRepository<UserInfoView> userInfoRepository = new BaseRepository<UserInfoView>();
        private BaseRepository<UserRole> UserRoleRepository = new BaseRepository<UserRole>();

        private string insertUserRoleSQL = @"Delete from UserRole where UserId=@UserId;
                                INSERT INTO AccessManagent.UserRole(
                                   UserId
                                  ,RoleId
                                ) VALUES (
                                   @UserId  -- UserName - IN varchar(200)
                                  ,@RoleId  -- UserPassword - IN varchar(200)
                                );select @@IDENTITY";

        // GET: Admin/UserInfo
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get(int pageSize = 20, int pageCurrent = 1)
        {
            try
            {
                string sql = @"select * from UserInfo where 1 = 1 and IsDel = 0 ";
                IPagedList<UserInfoView> userInfoList = await userInfoRepository.GetListAsync(sql, pageCurrent, pageSize);
                return Json(AjaxResult.SetResult(new { list = userInfoList, pageCount = userInfoList.PageCount, totalItemCount = userInfoList.TotalItemCount, pageNumber = userInfoList.PageNumber, aa = userInfoList.Count }), JsonRequestBehavior.AllowGet);
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
                UserInfoView entity = new UserInfoView { UserId = id };
                var result = await userInfoRepository.GetOneAsync(@"select T1.*,T2.RoleId from UserInfo T1
                            left join UserRole T2 on T1.UserId = T2.UserId
                            where 1 = 1 and  T1.UserId=@UserId and IsDel=0", entity);
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
        public async Task<JsonResult> Post(UserInfoView entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            string insertSQL = @"INSERT INTO AccessManagent.UserInfo(
                                   UserName
                                  ,UserPassword
                                  ,Salt
                                  ,Sort
                                  ,CreateDate
                                  ,Creater
                                  ,UpdateDate
                                  ,Updater
                                  ,IsDel
                                ) VALUES (
                                   @UserName  -- UserName - IN varchar(200)
                                  ,@UserPassword  -- UserPassword - IN varchar(200)
                                  ,@Salt  -- Salt - IN varchar(200)
                                  ,@Sort   -- Sort - IN int(11)
                                  ,@CreateDate  -- CreateDate - IN datetime
                                  ,''  -- Creater - IN varchar(200)
                                  ,@UpdateDate  -- UpdateDate - IN datetime
                                  ,''  -- Updater - IN varchar(200)
                                  ,0   -- IsDel - IN tinyint(4)
                                ) ;select @@IDENTITY";

            try
            {
                int i = await userInfoRepository.ScalarAsync(insertSQL, entity);
                int a = await UserRoleRepository.ScalarAsync(insertUserRoleSQL, new UserRole { RoleId = entity.RoleId, UserId = i });
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
            UserInfoView entity = new UserInfoView { UserId = id, UpdateDate = DateTime.Now };
            try
            {
                var result = await userInfoRepository.DeleteAsync("update UserInfo set IsDel=1,UpdateDate=@UpdateDate where UserId=@UserId  and IsDel=0", entity);
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }

        public async Task<JsonResult> Edit(UserInfoView entity)
        {
            //if (string.IsNullOrEmpty(entity.keyname) || string.IsNullOrEmpty(entity.valuecontent))
            //{
            //    return Json(AjaxResult.SetError("参数为空", ErrorCode.ErrorCodes.参数不能为null));
            //}
            entity.UpdateDate = DateTime.Now;
            try
            {
                var result = await userInfoRepository.EditAsync("update UserInfo Set UserName=@UserName,UserPassword=@UserPassword,UpdateDate=@UpdateDate where UserId=@UserId and IsDel=0", entity);
                int a = await UserRoleRepository.ScalarAsync(insertUserRoleSQL, new UserRole { RoleId = entity.RoleId, UserId = entity.UserId });
                return Json(AjaxResult.SetResult(result));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.SetError(ex.Message, ErrorCode.ErrorCodes.系统错误));
            }
        }
    }
}