﻿using Dapper;
using DapperExtensions;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YY.Edu.Sys.Comm.Helper;

namespace YY.Edu.Sys.Api.Controllers
{
    [RoutePrefix("api/TeachingSchedule")]
    public class TeachingScheduleController : ApiController
    {
        private static readonly ILog logs = Comm.Helper.LogHelper.GetInstance();

        // GET: api/TeachingSchedule
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/TeachingSchedule/5
        [HttpGet]
        public IHttpActionResult Get(int pkId)
        {

            var sql = @"select t.PKID,t.VenueID,t.CampusID,t.CoachID,t.Title,t.CurriculumDate,t.CurriculumBeginTime,t.CurriculumEndTime,t.AddTime,t.Price,t.CoachPrice,t.PKType,t.State,t.Info,c2.FullName as CoachFullName,StudentFullName = (
STUFF((SELECT    ',' + c.studentfullname FROM[Curriculum] as c WHERE  t.pkid = c.pkid  FOR XML PATH('')), 1, 1, '') )  
from TeachingSchedule as t join Coach as c2 on t.CoachID = c2.CoachID where t.pkid = @pkid ";

            try
            {

                var result = DapperHelper.Instance.Query(sql, new { pkid = pkId });

                return Ok(new Comm.ResponseModel.ResponseModel4Res<dynamic>()
                {
                    Info = result.AsList().FirstOrDefault(),
                });

            }
            catch (Exception ex)
            {
                logs.Error("查询排课详情异常", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetTeachingScheDetail(int pkId, int venueId)
        {

            var sql = @"select t.PKID,t.VenueID,t.CampusID,t.CoachID,t.Title,t.CurriculumDate,t.CurriculumBeginTime,t.CurriculumEndTime,t.AddTime,t.Price,t.CoachPrice,t.PKType,t.State,t.Info,c2.FullName as CoachFullName,StudentFullName = (
STUFF((SELECT    ',' + c.studentfullname FROM[Curriculum] as c WHERE  t.pkid = c.pkid  FOR XML PATH('')), 1, 1, '') )  
from TeachingSchedule as t join Coach as c2 on t.CoachID = c2.CoachID where t.pkid = @pkid and t.venueid=@venueid";

            try
            {

                var result = DapperHelper.Instance.Query(sql, new { pkid = pkId, venueid = venueId });

                return Ok(new Comm.ResponseModel.ResponseModel4Res<dynamic>()
                {
                    Info = result.AsList().FirstOrDefault(),
                });

                //return Ok(Comm.ResponseModel.ResponseModelBase.GetRes("失败"));

            }
            catch (Exception ex)
            {
                logs.Error("查询排课详情异常", ex);
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult Create(Models.RequestModel.TeachingScheduleRequest teachingschedule)
        {

            logs.Info(Newtonsoft.Json.JsonConvert.SerializeObject(teachingschedule));

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                //上课周期
                string[] curriculumDateArr = teachingschedule.CurriculumDateStr
                    .Split("to".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                DateTime startDate = Convert.ToDateTime(curriculumDateArr[0]);
                DateTime endDate = Convert.ToDateTime(curriculumDateArr[1]);

                List<Sys.Models.TeachingSchedule> entityList = new List<Sys.Models.TeachingSchedule>();

                bool isContain = true;
                while (isContain)
                {
                    try
                    {
                        //上午上课
                        entityList.AddRange(BuildClass(teachingschedule.TimepickerAMCheckd, startDate, teachingschedule.CurriculumAMTime, teachingschedule));
                        //下午上课
                        entityList.AddRange(BuildClass(teachingschedule.TimepickerPMCheckd, startDate, teachingschedule.CurriculumPMTime, teachingschedule));
                    }
                    catch (Exception ex)
                    {
                        logs.Error("排课失败", ex);
                        return Ok(Comm.ResponseModel.ResponseModelBase.GetRes(ex.Message));
                    }
                    startDate = startDate.AddDays(1);
                    if (startDate > endDate)
                        isContain = false;
                }

                bool isOk = false;

                System.Data.IDbConnection connection = Comm.Helper.DapperHelper.Instance;
                using (connection)
                {
                    connection.Open();
                    System.Data.IDbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        connection.Insert<Sys.Models.TeachingSchedule>(entityList, transaction);
                        transaction.Commit();
                        isOk = true;
                    }
                    catch (Exception ex)
                    {
                        logs.Error("排课失败", ex);
                        transaction.Rollback();
                        return BadRequest();
                    }
                }

                return isOk ? Ok(Comm.ResponseModel.ResponseModelBase.Success()) : Ok(Comm.ResponseModel.ResponseModelBase.SysError());
            }
            catch (Exception ex)
            {
                logs.Error("排课失败", ex);
                return BadRequest();
            }
        }

        private IList<Sys.Models.TeachingSchedule> BuildClass(bool isClass, DateTime startDate, string CurriculumTime, Models.RequestModel.TeachingScheduleRequest teachingschedule)
        {

            IList<Sys.Models.TeachingSchedule> entityList = new List<Sys.Models.TeachingSchedule>();
            if (!isClass) return entityList;

            string[] curriculumAMTimeArr = CurriculumTime.Split('-');
            //上课开始时间
            DateTime classStartTime = Convert.ToDateTime("2008-08-08 " + curriculumAMTimeArr[0]);
            //上课结束时间
            DateTime classOverTime = Convert.ToDateTime("2008-08-08 " + curriculumAMTimeArr[1]);

            //检查是否有重复课程
            IEnumerable<dynamic> coachTeachingSches = Services.CoachService.GetCoachTeachingScheByDay(teachingschedule.CoachID.Value, startDate.ToString("yyyy-MM-dd"), startDate.AddDays(1).ToString("yyyy-MM-dd"));

            bool classIsContain = true;
            while (classIsContain)
            {
                //单节课下课时间
                DateTime singleClassOverTime = classStartTime.AddMinutes(teachingschedule.ClassLength);

                Sys.Models.TeachingSchedule info = new Sys.Models.TeachingSchedule()
                {
                    AddTime = DateTime.Now,
                    CurriculumDate = startDate,
                    CurriculumBeginTime = classStartTime.ToString("HH:mm"),
                    CurriculumEndTime = singleClassOverTime.ToString("HH:mm"),
                    Info = teachingschedule.Info,
                    CampusID = teachingschedule.CampusID,
                    CoachID = teachingschedule.CoachID,
                    CoachPrice = teachingschedule.CoachPrice,
                    PKType = teachingschedule.PKType,
                    Price = teachingschedule.Price,
                    State = 0,
                    Title = teachingschedule.Title,
                    VenueID = teachingschedule.VenueID,
                };

                //设置下一节课上课时间
                if (singleClassOverTime >= classOverTime)
                {
                    singleClassOverTime = classOverTime;
                    info.CurriculumEndTime = classOverTime.ToString("HH:mm");
                    classIsContain = false;
                }

                //上课开始时间必须小于下课时间
                if (classStartTime < singleClassOverTime)
                {
                    IsContainClass(coachTeachingSches, classStartTime, singleClassOverTime, info);

                    entityList.Add(info);
                    classStartTime = classStartTime.AddMinutes(teachingschedule.ClassLength).AddMinutes(teachingschedule.RestLength);
                }

            }

            return entityList;
        }

        private void IsContainClass(IEnumerable<dynamic> coachTeachingSches, DateTime inClassStart, DateTime inClassOver, Sys.Models.TeachingSchedule info)
        {

            if (coachTeachingSches != null || coachTeachingSches.Count() > 0)
            {
                string timeStr = info.CurriculumBeginTime + "-" + info.CurriculumEndTime;

                foreach (var item in coachTeachingSches)
                {
                    //已经添加的上课时间
                    DateTime classStart = Convert.ToDateTime("2008-08-08 " + item.CurriculumBeginTime);
                    //已经添加的下课时间
                    DateTime classOver = Convert.ToDateTime("2008-08-08 " + item.CurriculumEndTime);

                    if ((inClassStart >= classStart && inClassStart <= classStart)||
                        (inClassStart >= classOver && inClassStart <= classOver))
                    {
                        throw new Exception(info.CurriculumDate.Value.ToString("yyyy-MM-dd") + " " + timeStr + "已排课");
                    }
                }
            }
        }

        [HttpGet]
        public IHttpActionResult Page4Venue(string query)
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest();

                Comm.RequestModel.RequestModelBase<Sys.Models.TeachingSchedule> oData = Newtonsoft.Json.JsonConvert.DeserializeObject<Comm.RequestModel.RequestModelBase<Sys.Models.TeachingSchedule>>(query);

                if (oData.PageIndex < 0 || oData.PageSize <= 0)
                    return BadRequest();

                PageCriteria criteria = new PageCriteria();
                criteria.Condition = "1=1";

                if (oData.SearchCondition.VenueID > 0)
                    criteria.Condition += string.Format(" and t.VenueID = {0}", oData.SearchCondition.VenueID);
                if (oData.SearchCondition.CampusID > 0)
                    criteria.Condition += string.Format(" and t.CampusID = {0}", oData.SearchCondition.CampusID);
                if (oData.SearchCondition.CoachID > 0)
                    criteria.Condition += string.Format(" and t.CoachID = {0}", oData.SearchCondition.CoachID);
                if (oData.SearchCondition.State > -1)
                    criteria.Condition += string.Format(" and t.State = {0}", oData.SearchCondition.State);
                if (oData.SearchCondition.PKType > 0)
                    criteria.Condition += string.Format(" and t.PKType = {0}", oData.SearchCondition.PKType);
                if (!string.IsNullOrEmpty(oData.SearchCondition.CurriculumBeginTime))
                    criteria.Condition += string.Format(" and t.CurriculumDate >= '{0}'", oData.SearchCondition.CurriculumBeginTime);
                if (!string.IsNullOrEmpty(oData.SearchCondition.CurriculumEndTime))
                    criteria.Condition += string.Format(" and t.CurriculumDate < '{0}'", Convert.ToDateTime(oData.SearchCondition.CurriculumEndTime).AddDays(1));

                criteria.CurrentPage = oData.PageIndex + 1;
                criteria.Fields = "t.PKID,t.VenueID,t.CampusID,t.CoachID,t.Title,t.CurriculumDate,t.CurriculumBeginTime,t.CurriculumEndTime,t.AddTime,t.Price,t.CoachPrice,t.PKType,t.State,t.Info,v.VenueName,c.CampusName,c2.FullName as CoachFullName,c3.StudentFullName";
                criteria.PageSize = oData.PageSize;
                criteria.TableName = "TeachingSchedule as t join Venue as v on t.VenueID=v.VenueID join Campus as c on c.CampusID=t.CampusID join Coach as c2 on t.CoachID=c2.CoachID left join Curriculum as c3 on t.PKID=c3.PKID";
                criteria.PrimaryKey = "t.PKID";

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
                logs.Error("排课查询失败", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// 停课
        /// </summary>
        /// <param name="teachSche"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult StopTeachingSche(dynamic teachSche)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (teachSche == null || teachSche.PKID <= 0)
                    return BadRequest();

                return ChangeTeachingScheState(Convert.ToInt32(teachSche.PKID), Convert.ToInt32(teachSche.VenueID), Convert.ToInt32(Sys.Models.StateEnum.TeachingSchedule.StopTeachingSche));
            }
            catch (Exception ex)
            {
                logs.Error("停课失败", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// 同意请假
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

                if (teachSche == null || teachSche.PKID <= 0)
                    return BadRequest();

                return ChangeTeachingScheState(Convert.ToInt32(teachSche.PKID), Convert.ToInt32(teachSche.VenueID), Convert.ToInt32(Sys.Models.StateEnum.TeachingSchedule.LeaveTeachingScheDone));
            }
            catch (Exception ex)
            {
                logs.Error("同意请假失败", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// 上课完成
        /// </summary>
        /// <param name="teachSche"></param>
        /// <returns></returns>
        public IHttpActionResult Done(dynamic teachSche)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (teachSche == null || teachSche.PKID <= 0)
                    return BadRequest();

                return ChangeTeachingScheState(Convert.ToInt32(teachSche.PKID), Convert.ToInt32(teachSche.VenueID), Convert.ToInt32(Sys.Models.StateEnum.TeachingSchedule.ClassOverTeachingSche));
            }
            catch (Exception ex)
            {
                logs.Error("确认上课完成失败", ex);
                return BadRequest();
            }
        }

        private IHttpActionResult ChangeTeachingScheState(int pkId, int venueID, int state)
        {
            try
            {
                if (pkId <= 0)
                    return BadRequest();

                try
                {

                    var result = DapperHelper.Instance.Get<Sys.Models.TeachingSchedule>(pkId);
                    if (result.VenueID != venueID)
                        return Ok(Comm.ResponseModel.ResponseModelBase.GetRes("操作非法"));

                    result.State = state;
                    bool flag = DapperHelper.Instance.Update(result);

                    return flag ? Ok(Comm.ResponseModel.ResponseModelBase.Success()) : Ok(Comm.ResponseModel.ResponseModelBase.SysError());
                }
                catch (Exception ex)
                {
                    return Ok(Comm.ResponseModel.ResponseModelBase.GetRes(ex.Message));
                }
            }
            catch (Exception ex)
            {
                logs.Error("停课失败", ex);
                return BadRequest();
            }
        }

        // POST: api/TeachingSchedule
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/TeachingSchedule/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TeachingSchedule/5
        public void Delete(int id)
        {
        }
    }
}
