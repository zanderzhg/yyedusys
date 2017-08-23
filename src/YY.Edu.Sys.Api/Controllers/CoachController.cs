﻿using Dapper;
using DapperExtensions;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using YY.Edu.Sys.Api.Models.ResponseModel;
using YY.Edu.Sys.Comm.Helper;


namespace YY.Edu.Sys.Api.Controllers
{
    [RoutePrefix("api/Coach")]
    public class CoachController : ApiController
    {
        private static readonly ILog logs = Comm.Helper.LogHelper.GetInstance();

        // GET: api/Student
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        #region 基础信息
        [HttpGet]
        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            try
            {
                //查询

                var sql = @"select c.CoachID,c.FullName,c.UserName,c.State,c.Introduce,c.NickName,c.HeadUrl,c.Address,c.Mobile,c.Sex,v.VenueName,cv.Wage,cv.Price 
                    from Coach as c join Venue as v on c.VenueID = v.VenueID left join Coach_Venue as cv on c.CoachID = cv.CoachID where c.CoachID = @CoachID";

                var result = DapperHelper.Instance.Query<Models.ResponseModel.CoachResponse, Sys.Models.Coach_Venue, Sys.Models.Venue, Sys.Models.Coach>(sql,
                                        splitOn: "VenueName",
                                        param: new { CoachID = id },
                                        map: (coach, coach_venue, venue) =>
                                        {
                                            coach.VenueName = venue.VenueName;
                                            coach.Price = coach_venue.Price;
                                            coach.Wage = coach_venue.Wage;
                                            return coach;
                                        });

                return Ok(new Comm.ResponseModel.ResponseModel4Res<YY.Edu.Sys.Models.Coach>()
                {
                    Info = result.AsList().FirstOrDefault()
                });
            }
            catch (Exception ex)
            {
                logs.Error("获取教练详情失败", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// 获取校区下可以任教的教练
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCoach4Teaching(int venueId)
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest();

                if (venueId <= 0)
                    return BadRequest();

                IList<ISort> sort = new List<ISort>();
                sort.Add(new Sort { PropertyName = "CoachID", Ascending = false });

                IList<IPredicate> predList = new List<IPredicate>();

                predList.Add(Predicates.Field<YY.Edu.Sys.Models.Coach>(f => f.VenueID, Operator.Eq, venueId));
                predList.Add(Predicates.Field<YY.Edu.Sys.Models.Coach>(f => f.State, Operator.Eq, 1));

                var result = Comm.Helper.DapperHelper.Instance.GetList<YY.Edu.Sys.Models.Coach>(predList, sort);

                return Ok(new Comm.ResponseModel.ResponseModel4Res<YY.Edu.Sys.Models.Coach>()
                {
                    data = result.AsList(),
                });

            }
            catch (Exception ex)
            {
                logs.Error("获取排课教练查询失败", ex);
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult Create(YY.Edu.Sys.Models.Coach coach)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {

                var result = Comm.Helper.DapperHelper.Instance.Insert(coach);

                if (result > 0)
                {
                    return Ok(Comm.ResponseModel.ResponseModelBase.Success());
                }
                else
                {
                    return Content(HttpStatusCode.OK, Comm.ResponseModel.ResponseModelBase.GetRes(Comm.ResponseModel.ResponseModelErrorEnum.SystemError));
                }

            }
            catch (Exception ex)
            {
                logs.Error("教练信息添加失败", ex);
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult Edit(YY.Edu.Sys.Models.Coach coach)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                //Models.VenueInfo venueInfo = Comm.Helper.DapperHelper.Instance.Get<Models.VenueInfo>(cityId);

                var result = Comm.Helper.DapperHelper.Instance.Update(coach);

                if (result)
                {
                    return Ok(Comm.ResponseModel.ResponseModelBase.Success());
                }
                else
                {
                    return Content(HttpStatusCode.OK, Comm.ResponseModel.ResponseModelBase.GetRes(Comm.ResponseModel.ResponseModelErrorEnum.SystemError));
                }
            }
            catch (Exception ex)
            {
                logs.Error("场馆信息编辑失败", ex);
                return BadRequest();
            }

        }

        /// <summary>
        /// 教练信息分页查询
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

                Comm.RequestModel.RequestModelBase<Sys.Models.Coach> oData = Newtonsoft.Json.JsonConvert.DeserializeObject<Comm.RequestModel.RequestModelBase<Sys.Models.Coach>>(query);

                if (oData.PageIndex < 0 || oData.PageSize <= 0)
                    return BadRequest();

                PageCriteria criteria = new PageCriteria();
                criteria.Condition = "1=1";

                if (oData.SearchCondition.VenueID > 0)
                    criteria.Condition += string.Format(" and v.VenueID = {0}", oData.SearchCondition.VenueID);
                if (!string.IsNullOrEmpty(oData.SearchCondition.UserName))
                    criteria.Condition += string.Format(" and c.UserName = '{0}'", oData.SearchCondition.UserName);
                if (!string.IsNullOrEmpty(oData.SearchCondition.FullName))
                    criteria.Condition += string.Format(" and c.FullName like '%{0}%'", oData.SearchCondition.FullName);

                criteria.CurrentPage = oData.PageIndex + 1;//adminlte 加载的datatable起始页为0
                criteria.Fields = "c.CoachID,c.FullName,c.UserName,c.State,c.Introduce,c.NickName,c.HeadUrl,c.Address,c.Mobile,c.Sex,v.VenueName,cv.Wage,cv.Price";
                criteria.PageSize = oData.PageSize;
                criteria.TableName = "Coach as c join Venue as v on c.VenueID=v.VenueID left join Coach_Venue as cv on c.CoachID=cv.CoachID";
                criteria.PrimaryKey = "c.CoachID";

                var r = Comm.Helper.DapperHelper.GetPageData<Sys.Models.Coach>(criteria);

                return Ok(new Comm.ResponseModel.ResponseModel4Page<Sys.Models.Coach>()
                {
                    data = r.Items,
                    recordsFiltered = r.TotalNum,
                    recordsTotal = r.TotalNum,
                });
            }
            catch (Exception ex)
            {
                logs.Error("教练查询失败", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// 教练绑定场馆
        /// </summary>
        /// <param name="coach"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult BindVenue(YY.Edu.Sys.Models.Coach_Venue coach)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (coach == null || coach.VenueID == null || coach.VenueID <= 0 ||
                    coach.CoachID == null || coach.CoachID <= 0)
                    return BadRequest();

                var result = DapperHelper.Instance.Get<Sys.Models.Coach>(coach.CoachID);

                //判断教练状态
                if (result.State != 1)
                    return Content(HttpStatusCode.OK, Comm.ResponseModel.ResponseModelBase.GetRes("此教练未审核通过"));

                IList<IPredicate> predList = new List<IPredicate>();
                predList.Add(Predicates.Field<Sys.Models.Coach_Venue>(f => f.CoachID, Operator.Eq, coach.CoachID));
                predList.Add(Predicates.Field<Sys.Models.Coach_Venue>(f => f.VenueID, Operator.Eq, coach.VenueID));
                IPredicateGroup predGroup = Predicates.Group(GroupOperator.And, predList.ToArray());

                //查询该学校下是否有此教练
                int venue_info_count = DapperHelper.Instance.Count<Sys.Models.Coach_Venue>(predGroup);
                //没有绑定过此学校
                if (venue_info_count > 0)
                {
                    return Ok(Comm.ResponseModel.ResponseModelBase.GetRes("已经绑定过此场馆"));
                }
                else
                {
                    coach.AddTime = DateTime.Now;
                    bool flag = DapperHelper.Instance.Insert(coach);
                    return flag ? Ok(Comm.ResponseModel.ResponseModelBase.Success()) : Ok(Comm.ResponseModel.ResponseModelBase.SysError());
                }
            }
            catch (Exception ex)
            {
                logs.Error("设置教练工资失败", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// 设置教练工资
        /// </summary>
        /// <param name="coach"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SetCoachWages(YY.Edu.Sys.Models.Coach_Venue coach)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (coach == null || coach.VenueID == null || coach.VenueID <= 0 ||
                    coach.CoachID == null || coach.CoachID <= 0 || coach.Wage <= 0)
                    return BadRequest();

                var result = DapperHelper.Instance.Get<Sys.Models.Coach>(coach.CoachID);

                //判断教练状态
                if (result.State != 1)
                    return Content(HttpStatusCode.OK, Comm.ResponseModel.ResponseModelBase.GetRes("此教练未审核通过"));

                //查询该学校下是否有此教练
                var venue_info = DapperHelper.Instance.Get<Sys.Models.Coach_Venue>(
                    Predicates.Field<Sys.Models.Coach_Venue>(f => f.CoachID, Operator.Eq, coach.CoachID)
                );

                if (venue_info == null)
                    return Content(HttpStatusCode.OK, Comm.ResponseModel.ResponseModelBase.GetRes("此教练未绑定场馆"));

                if (venue_info.VenueID != coach.VenueID)
                    return Content(HttpStatusCode.OK, Comm.ResponseModel.ResponseModelBase.GetRes("该场馆下没有此教练"));

                //设置工资
                venue_info.Wage = coach.Wage;
                bool flag = DapperHelper.Instance.Update(venue_info);

                return flag ? Ok(Comm.ResponseModel.ResponseModelBase.Success()) : Ok(Comm.ResponseModel.ResponseModelBase.SysError());
            }
            catch (Exception ex)
            {
                logs.Error("设置教练工资失败", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// 设置教练课时费
        /// </summary>
        /// <param name="coach"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SetCoachPrice(YY.Edu.Sys.Models.Coach_Venue coach)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (coach == null || coach.VenueID == null || coach.VenueID <= 0 ||
                    coach.CoachID == null || coach.CoachID <= 0 || coach.Price <= 0)
                    return BadRequest();

                //查询该学校下是否有此教练
                var venue_info = DapperHelper.Instance.Get<Sys.Models.Coach_Venue>(
                    Predicates.Field<Sys.Models.Coach_Venue>(f => f.CoachID, Operator.Eq, coach.CoachID)
                );

                if (venue_info == null)
                    return Content(HttpStatusCode.OK, Comm.ResponseModel.ResponseModelBase.GetRes("此教练未绑定场馆"));

                if (venue_info.VenueID != coach.VenueID)
                    return Content(HttpStatusCode.OK, Comm.ResponseModel.ResponseModelBase.GetRes("该场馆下没有此教练"));

                var result = DapperHelper.Instance.Get<Sys.Models.Coach>(coach.CoachID);

                //判断教练状态
                if (result.State != 1)
                    return Content(HttpStatusCode.OK, Comm.ResponseModel.ResponseModelBase.GetRes("此教练未审核通过"));

                //设置教练课时费
                venue_info.Price = coach.Price;
                bool flag = DapperHelper.Instance.Update(venue_info);

                return flag ? Ok(Comm.ResponseModel.ResponseModelBase.Success()) : Ok(Comm.ResponseModel.ResponseModelBase.SysError());
            }
            catch (Exception ex)
            {
                logs.Error("设置教练课时费失败", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// 设置教练审核通过
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SetCoachAudited(dynamic coach)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (coach == null || coach.CoachID <= 0)
                    return BadRequest();

                //查询该学校下是否有此教练
                int coachId = coach.CoachID;
                var result = DapperHelper.Instance.Get<Sys.Models.Coach>(coachId);
                if (result.VenueID != coach.VenueID)
                    return Content(HttpStatusCode.OK, Comm.ResponseModel.ResponseModelBase.GetRes("该场馆下没有此教练"));

                result.State = 1;
                bool flag = DapperHelper.Instance.Update(result);

                return flag ? Ok(Comm.ResponseModel.ResponseModelBase.Success()) : Ok(Comm.ResponseModel.ResponseModelBase.SysError());
            }
            catch (Exception ex)
            {
                logs.Error("设置教练审核通过失败", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// 删除教练
        /// </summary>
        /// <param name="coach"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Delete(dynamic coach)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (coach == null || coach.CoachID <= 0)
                    return BadRequest();

                //查询该学校下是否有此教练
                int coachId = coach.CoachID;
                var result = DapperHelper.Instance.Get<Sys.Models.Coach>(coachId);
                if (result.VenueID != coach.VenueID)
                    return Content(HttpStatusCode.OK, Comm.ResponseModel.ResponseModelBase.GetRes("该场馆下没有此教练"));

                result.State = 2;
                bool flag = DapperHelper.Instance.Update(result);

                return flag ? Ok(Comm.ResponseModel.ResponseModelBase.Success()) : Ok(Comm.ResponseModel.ResponseModelBase.SysError());
            }
            catch (Exception ex)
            {
                logs.Error("删除教练失败", ex);
                return BadRequest();
            }
        }


        #endregion


        #region //教师风采

        [HttpGet]
        // GET api/<controller>
        public IHttpActionResult GetCoachingPresence()
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest();

                System.Web.HttpContextBase context = (System.Web.HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
                System.Web.HttpRequestBase request = context.Request;//定义传统request对象
                string UserName = Comm.Helper.ParamHelper<string>.GetParam(request["UserName"], "");
                string ParentMobile = Comm.Helper.ParamHelper<string>.GetParam(request["ParentMobile"], "");
                string FullName = Comm.Helper.ParamHelper<string>.GetParam(request["FullName"], "");
                string ParentFullName = Comm.Helper.ParamHelper<string>.GetParam(request["ParentFullName"], "");
                int start = Comm.Helper.ParamHelper<int>.GetParam(request["start"], 0);
                start += 1;//adminlte 加载的datatable起始页为0
                int length = Comm.Helper.ParamHelper<int>.GetParam(request["length"], 0);
                int venueId = Comm.Helper.ParamHelper<int>.GetParam(request["venueId"], 0);

                if (venueId <= 0 || start < 0 || length <= 0)
                    return BadRequest();

                PageCriteria criteria = new PageCriteria();
                criteria.Condition = "1=1";

                criteria.Condition += string.Format(" and VenueID = {0} and FCState=1 ", venueId);



                criteria.CurrentPage = start;
                criteria.Fields = "*";
                criteria.PageSize = length;
                criteria.TableName = "CoachingPresence";
                criteria.PrimaryKey = "FCID";

                var r = Comm.Helper.DapperHelper.GetPageData<YY.Edu.Sys.Models.CoachingPresence>(criteria);

                return Ok(new Comm.ResponseModel.ResponseModel4Page<YY.Edu.Sys.Models.CoachingPresence>()
                {
                    data = r.Items,
                    recordsFiltered = r.TotalNum,
                    recordsTotal = r.TotalNum,
                });

            }
            catch (Exception ex)
            {
                logs.Error("教师风采", ex);
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult AddCoachingPresence(YY.Edu.Sys.Models.CoachingPresence cp)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {

                var result = Comm.Helper.DapperHelper.Instance.Insert(cp);

                if (result > 0)
                {
                    return Ok(Comm.ResponseModel.ResponseModelBase.Success());
                }
                else
                {
                    return Content(HttpStatusCode.OK, Comm.ResponseModel.ResponseModelBase.GetRes(Comm.ResponseModel.ResponseModelErrorEnum.SystemError));
                }

            }
            catch (Exception ex)
            {
                logs.Error("风采信息添加失败", ex);
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult DelCoachingPresence(int ID, int State)
        {
            string sql = "update CoachingPresence set FCState=@FCState where FCID=@FCID";

            if (!ModelState.IsValid)
                return BadRequest();

            //单条添加
            var result = Comm.Helper.DapperHelper.Instance.Execute(sql.ToString(),
                                   new { FCState = State, FCID = ID });

            if (result > 0)
            {
                return Ok(new { error = false, code = "1000", message = "操作成功" });
            }
            else
            {
                return Content(HttpStatusCode.OK, new { error = true, code = "1001", message = "操作失败,请重新操作" });
            }
        }

        #endregion


        #region 课程

        /// <summary>
        /// 取的教练下的所有课程
        /// </summary>
        /// <param name="CoachID"></param>
        /// <returns></returns>
        public IHttpActionResult GetCoachCurriculum(int CoachID)
        {
            //
            //select T.*,v.VenueName,c.CampusName from  TeachingSchedule t with(nolock) inner join Venue v with(nolock) on t.VenueID=v.VenueID             left join Campus c with(nolock) on t.CampusID = t.CampusID  where PKID in(select PKID from Curriculum c with(nolock) where CoachID = 0 ) and CoachID = 0
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest();

                System.Web.HttpContextBase context = (System.Web.HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
                System.Web.HttpRequestBase request = context.Request;//定义传统request对象
                string UserName = Comm.Helper.ParamHelper<string>.GetParam(request["UserName"], "");
                string ParentMobile = Comm.Helper.ParamHelper<string>.GetParam(request["ParentMobile"], "");
                string FullName = Comm.Helper.ParamHelper<string>.GetParam(request["FullName"], "");
                string ParentFullName = Comm.Helper.ParamHelper<string>.GetParam(request["ParentFullName"], "");
                int start = Comm.Helper.ParamHelper<int>.GetParam(request["start"], 0);
                start += 1;//adminlte 加载的datatable起始页为0
                int length = Comm.Helper.ParamHelper<int>.GetParam(request["length"], 0);
                int venueId = Comm.Helper.ParamHelper<int>.GetParam(request["venueId"], 0);

                if (venueId <= 0 || start < 0 || length <= 0)
                    return BadRequest();

                PageCriteria criteria = new PageCriteria();
                criteria.Condition = "1=1";

                criteria.Condition += string.Format(" and t.CoachID = {0} ", CoachID);

                //  select T.*,v.VenueName,c.CampusName from  TeachingSchedule t with(nolock) inner join Venue v with(nolock) on t.VenueID = v.VenueID             left join Campus c with(nolock) on t.CampusID = t.CampusID  where PKID in(select PKID from Curriculum c with(nolock) where CoachID = 0 ) and CoachID = 0

                criteria.CurrentPage = start;
                criteria.Fields = "*";
                criteria.PageSize = length;
                criteria.TableName = " TeachingSchedule t with(nolock) inner join Venue v with(nolock) on t.VenueID = v.VenueID             left join Campus c with(nolock) on t.CampusID = t.CampusID  where PKID in(select PKID from Curriculum c with(nolock) where CoachID = '" + CoachID + "' )";
                criteria.PrimaryKey = "PKID";

                var r = Comm.Helper.DapperHelper.GetPageData<TeachingScheduleResponse>(criteria);

                return Ok(new Comm.ResponseModel.ResponseModel4Page<TeachingScheduleResponse>()
                {
                    data = r.Items,
                    recordsFiltered = r.TotalNum,
                    recordsTotal = r.TotalNum,
                });

            }
            catch (Exception ex)
            {
                logs.Error("取的教练下的所有课程", ex);
                return BadRequest();
            }
        }


        /// <summary>
        /// 取的课程下预约的学生
        /// </summary>
        /// <param name="pkid"></param>
        /// <returns></returns>
        public IHttpActionResult GetCurriculumStudentByPKID(int pkid)
        {

            string sql = "select c.CurriculumID,s.FullName,s.Mobile,s.ParentFullName,s.ParentMobile from Curriculum c with(nolock) inner join Student s with(nolock) on c.StudentID=s.StudentID  where PKID=@PKID ";
            var query = Comm.Helper.DapperHelper.Instance.Query<StudentCurriculumResponse>(sql, new { PKID = pkid });
            return Ok(query);
        }


        /// <summary>
        ///课程表 学生进行签到（由教练发起）df
        /// </summary>
        /// <param name="State"></param>
        /// <param name="StudentID"></param>
        /// <param name="pkid"></param>
        /// <returns></returns>
        public IHttpActionResult UpdateCurriculumState(int State, string StudentIDs, int pkid)
        {
            string sql = "update Curriculum set State=@State,ModifyTime=GETDATE() where PKID=@PKID and StudentID in(@StudentID) ";
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                //Models.VenueInfo venueInfo = Comm.Helper.DapperHelper.Instance.Get<Models.VenueInfo>(cityId);

                var result = Comm.Helper.DapperHelper.Instance.Execute(sql, new { State = State, PKID = pkid, StudentID = StudentIDs });

                if (result > 0)
                {
                    return Ok(Comm.ResponseModel.ResponseModelBase.Success());
                }
                else
                {
                    return Content(HttpStatusCode.OK, Comm.ResponseModel.ResponseModelBase.GetRes(Comm.ResponseModel.ResponseModelErrorEnum.SystemError));
                }
            }
            catch (Exception ex)
            {
                logs.Error("学生进行签到失败", ex);
                return BadRequest();
            }
        }



        // 薪酬列表、明细


        [HttpGet]
        // GET api/<controller>
        public IHttpActionResult GetCoachWages(int CoachID, int VenueID)
        {
            //传参 http://www.cnblogs.com/landeanfen/p/5337072.html
            //dynamic

            //返回值 http://www.cnblogs.com/landeanfen/p/5501487.html
            //1 return Json<obj>;
            //2 return Ok<obj>;
            //3 return NotFound<obj>;
            //4 return Content<HttpStatusCode.OK,obj>;
            //5 return BadRequest(); 返回400错误


            try
            {

                if (!ModelState.IsValid)
                    return BadRequest();

                System.Web.HttpContextBase context = (System.Web.HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
                System.Web.HttpRequestBase request = context.Request;//定义传统request对象
                string UserName = Comm.Helper.ParamHelper<string>.GetParam(request["UserName"], "");
                string ParentMobile = Comm.Helper.ParamHelper<string>.GetParam(request["ParentMobile"], "");
                string FullName = Comm.Helper.ParamHelper<string>.GetParam(request["FullName"], "");
                string ParentFullName = Comm.Helper.ParamHelper<string>.GetParam(request["ParentFullName"], "");
                int start = Comm.Helper.ParamHelper<int>.GetParam(request["start"], 0);
                start += 1;//adminlte 加载的datatable起始页为0
                int length = Comm.Helper.ParamHelper<int>.GetParam(request["length"], 0);
                int venueId = Comm.Helper.ParamHelper<int>.GetParam(request["venueId"], 0);

                if (venueId <= 0 || start < 0 || length <= 0)
                    return BadRequest();

                PageCriteria criteria = new PageCriteria();
                criteria.Condition = "1=1";

                criteria.Condition += string.Format(" and CoachID={0} and VenueID={1} ", CoachID, VenueID);

                //  select T.*,v.VenueName,c.CampusName from  TeachingSchedule t with(nolock) inner join Venue v with(nolock) on t.VenueID = v.VenueID             left join Campus c with(nolock) on t.CampusID = t.CampusID  where PKID in(select PKID from Curriculum c with(nolock) where CoachID = 0 ) and CoachID = 0

                criteria.CurrentPage = start;
                criteria.Fields = "c.*,v.VenueName";
                criteria.PageSize = length;
                criteria.TableName = " CoachWages c with(nolock) inner join Venue v with(nolock)";
                criteria.PrimaryKey = "WagesID";

                var r = Comm.Helper.DapperHelper.GetPageData<YY.Edu.Sys.Models.CoachWages>(criteria);

                return Ok(new Comm.ResponseModel.ResponseModel4Page<YY.Edu.Sys.Models.CoachWages>()
                {
                    data = r.Items,
                    recordsFiltered = r.TotalNum,
                    recordsTotal = r.TotalNum,
                });

            }
            catch (Exception ex)
            {
                logs.Error("取的教练下的所有课程", ex);
                return BadRequest();
            }

        }


        [HttpGet]
        // GET api/<controller>
        public IHttpActionResult GetCoachWagesByWagesID(int WagesID)
        {
            //传参 http://www.cnblogs.com/landeanfen/p/5337072.html
            //dynamic

            //返回值 http://www.cnblogs.com/landeanfen/p/5501487.html
            //1 return Json<obj>;
            //2 return Ok<obj>;
            //3 return NotFound<obj>;
            //4 return Content<HttpStatusCode.OK,obj>;
            //5 return BadRequest(); 返回400错误

            string sql = "select T.*,v.VenueName,c.CampusName,s.CoachPrice from CoachWages_sub s with(nolock) inner join TeachingSchedule t with(nolock) on s.PKID=t.PKID inner join Venue v with(nolock) on t.VenueID = v.VenueID left join Campus c with(nolock) on t.CampusID = t.CampusID where  s.WagesID=@WagesID ";
            var query = Comm.Helper.DapperHelper.Instance.Query<YY.Edu.Sys.Models.TeachingSchedule>(sql, new { WagesID = WagesID });
            return Ok(query);
        }

        #endregion



    }
}
