﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YY.Edu.Sys.Comm.Helper;

namespace YY.Edu.Sys.Api.Controllers
{

    [Authorize]
    [RoutePrefix("api/Curriculum")]
    public class CurriculumController : BaseController
    {

        private static readonly ILog logs = Comm.Helper.LogHelper.GetInstance();

        /// <summary>
        /// 查询请假申请的课程
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Page4Venue(string query)
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest();

                Comm.RequestModel.RequestModelBase<Models.RequestModel.CurriculumRequest> oData = Newtonsoft.Json.JsonConvert.DeserializeObject<Comm.RequestModel.RequestModelBase<Models.RequestModel.CurriculumRequest>>(query);

                if (oData.PageIndex < 0 || oData.PageSize <= 0)
                    return BadRequest();

                PageCriteria criteria = new PageCriteria();
                criteria.Condition = "1=1";

                if (oData.SearchCondition.VenueID > 0)
                    criteria.Condition += string.Format(" and c.VenueID = {0}", oData.SearchCondition.VenueID);
                if (oData.SearchCondition.CampusID > 0)
                    criteria.Condition += string.Format(" and t.CampusID = {0}", oData.SearchCondition.CampusID);
                if (oData.SearchCondition.StudentID > 0)
                    criteria.Condition += string.Format(" and c.StudentID = {0}", oData.SearchCondition.StudentID);
                if (oData.SearchCondition.State > -1)
                    criteria.Condition += string.Format(" and c.State = {0}", oData.SearchCondition.State);
                if (oData.SearchCondition.PKType > 0)
                    criteria.Condition += string.Format(" and t.PKType = {0}", oData.SearchCondition.PKType);
                if (!string.IsNullOrEmpty(oData.SearchCondition.CurriculumBeginTime))
                    criteria.Condition += string.Format(" and t.CurriculumDate >= '{0}'", oData.SearchCondition.CurriculumBeginTime);
                if (!string.IsNullOrEmpty(oData.SearchCondition.CurriculumEndTime))
                    criteria.Condition += string.Format(" and t.CurriculumDate < '{0}'", Convert.ToDateTime(oData.SearchCondition.CurriculumEndTime).AddDays(1));

                criteria.CurrentPage = oData.PageIndex + 1;
                criteria.Fields = "c.CurriculumID,t.PKID,t.VenueID,t.CampusID,t.CoachID,t.Title,t.CurriculumDate,t.CurriculumBeginTime,t.CurriculumEndTime,t.AddTime,t.Price,t.CoachPrice,t.PKType,c.State,t.Info,v.VenueName,c1.CampusName,c2.FullName as CoachFullName,c.StudentFullName";
                criteria.PageSize = oData.PageSize;
                criteria.TableName = "[dbo].[Curriculum] as c join [dbo].[TeachingSchedule] t on c.pkid=t.pkid join Venue as v on t.VenueID = v.VenueID join Campus as c1 on c1.CampusID = t.CampusID join Coach as c2 on t.CoachID = c2.CoachID ";
                criteria.PrimaryKey = "c.CurriculumID";

                var r = Comm.Helper.DapperHelper.GetPageData<Models.ResponseModel.TeachingScheduleResponse>(criteria);

                return Ok(new Comm.ResponseModel.ResponseModel4Page<Models.ResponseModel.TeachingScheduleResponse>()
                {
                    data = r.Items,
                    recordsFiltered = r.TotalNum,
                    recordsTotal = r.TotalNum,
                });
            }
            catch (Exception ex)
            {
                logs.Error("课程查询失败", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// 确认学生请假申请
        /// </summary>
        /// <param name="teachSche"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AgreeLeave(dynamic teachSche)
        {

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (teachSche == null || teachSche.CurriculumID <= 0)
                    return BadRequest();

                bool result = false;
                result = Services.TeachingScheduleService.CancelTeachingSche4Student(
                    Convert.ToInt32(teachSche.CurriculumID),
                    Convert.ToInt32(teachSche.VenueID),
                    Sys.Models.StateEnum.TeachingSchedule.StudentLeaveTeachingScheDone,
                    Sys.Models.StateEnum.Curriculum.StudentLeaveTeachingScheDone,
                    Sys.Models.StateEnum.ClassHoursDetailed.StudentLeaveTeachingScheDone, "学生请假"
                    );

                return result ? Ok(Comm.ResponseModel.ResponseModelBase.Success()) : Ok(Comm.ResponseModel.ResponseModelBase.SysError());

            }
            catch (Comm.YYException.YYException ex)
            {
                return Ok(Comm.ResponseModel.ResponseModelBase.GetRes(ex.Message));
            }
            catch (Exception ex)
            {
                logs.Error("同意请假失败", ex);
                return BadRequest();
            }
        }


    }
}
