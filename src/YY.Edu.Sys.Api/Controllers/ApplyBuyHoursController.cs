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
    [Authorize]
    [RoutePrefix("api/ApplyBuyHours")]
    public class ApplyBuyHoursController : BaseController
    {

        private static readonly ILog logs = Comm.Helper.LogHelper.GetInstance();

        /// <summary>
        /// 添加缴费记录
        /// </summary>
        /// <param name="applyBuyHours"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Create(Sys.Models.ApplyBuyHours applyBuyHours)
        {

            if (!ModelState.IsValid)
                return Ok(Comm.ResponseModel.ResponseModelBase.GetRes(ValidationMsg));

            try
            {
                applyBuyHours.AddTime = DateTime.Now;
                Comm.Helper.DapperHelper.Instance.Insert<Sys.Models.ApplyBuyHours>(applyBuyHours);
                return Ok(Comm.ResponseModel.ResponseModelBase.Success());
            }
            catch (Exception ex)
            {
                logs.Error("添加缴费记录失败", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// 设置缴费记录审核成功
        /// </summary>
        /// <param name="coach"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SetPayRecordAudited(dynamic applyBuyHours)
        {

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (applyBuyHours == null || applyBuyHours.ApplyID <= 0 || applyBuyHours.VenueID <= 0)
                    return BadRequest();

                if (!IsExist(Convert.ToInt32(applyBuyHours.ApplyID), Convert.ToInt32(applyBuyHours.VenueID)))
                    return Ok(Comm.ResponseModel.ResponseModelBase.GetRes("场馆下没有要审核的支付记录"));

                var flag = Services.ApplyBuyHoursService.SetPayRecordAudited(applyBuyHours);

                return flag ? Ok(Comm.ResponseModel.ResponseModelBase.Success()) : Ok(Comm.ResponseModel.ResponseModelBase.SysError());

            }
            catch (Exception ex)
            {
                logs.Error("确认缴费成功失败", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// 分页查询
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

                Comm.RequestModel.RequestModelBase<Models.RequestModel.ApplyBuyHoursRequest> oData = Newtonsoft.Json.JsonConvert.DeserializeObject<Comm.RequestModel.RequestModelBase<Models.RequestModel.ApplyBuyHoursRequest>>(query);

                if (oData.PageIndex < 0 || oData.PageSize <= 0)
                    return BadRequest();

                PageCriteria criteria = new PageCriteria();
                criteria.Condition = "1=1";

                criteria.Condition += string.Format(" and VenueID = {0}", oData.SearchCondition.VenueID);

                if (oData.SearchCondition.CoachID > 0)
                    criteria.Condition += string.Format(" and CoachID = {0}", oData.SearchCondition.CoachID);
                if (oData.SearchCondition.StudentID > 0)
                    criteria.Condition += string.Format(" and StudentID = {0}", oData.SearchCondition.StudentID);
                if (oData.SearchCondition.Status > -1)
                    criteria.Condition += string.Format(" and Status = {0}", oData.SearchCondition.Status);
                if (oData.SearchCondition.PKType > 0)
                    criteria.Condition += string.Format(" and PKType = {0}", oData.SearchCondition.PKType);
                if (!string.IsNullOrEmpty(oData.SearchCondition.StartDate))
                    criteria.Condition += string.Format(" and AddTime >= '{0}'", oData.SearchCondition.StartDate);
                if (!string.IsNullOrEmpty(oData.SearchCondition.EndDate))
                    criteria.Condition += string.Format(" and AddTime < '{0}'", Convert.ToDateTime(oData.SearchCondition.EndDate).AddDays(1));

                criteria.CurrentPage = oData.PageIndex + 1;
                criteria.Fields = "[ApplyID],[StudentID],[CoachID],[ClassNumber],[AddTime],[PayMoney],[Status],[PKType],[PaidMoney],[VenueID],[StudentFullName],[CoachFullName]";
                criteria.PageSize = oData.PageSize;
                criteria.TableName = "[ApplyBuyHours]";
                criteria.PrimaryKey = "ApplyID";

                var r = Comm.Helper.DapperHelper.GetPageData<Sys.Models.ApplyBuyHours>(criteria);

                return Ok(new Comm.ResponseModel.ResponseModel4Page<Sys.Models.ApplyBuyHours>()
                {
                    data = r.Items,
                    recordsFiltered = r.TotalNum,
                    recordsTotal = r.TotalNum,
                });
            }
            catch (Exception ex)
            {
                logs.Error("缴费记录查询失败", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// 是否已经录入缴费记录
        /// </summary>
        /// <param name="applyID"></param>
        /// <param name="venueId"></param>
        /// <returns></returns>
        private bool IsExist(int applyID, int venueId)
        {

            var sql = "SELECT count([ApplyID]) FROM [dbo].[ApplyBuyHours] where ApplyID=@ApplyID and VenueID=@VenueID ";
            var count = DapperHelper.Instance.Query<int>(sql, new { ApplyID = applyID, VenueID = venueId });

            return count.FirstOrDefault() > 0;
        }

    }
}
