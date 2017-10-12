﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YY.Edu.Sys.Models
{
    public class ApplyBuyHours
    {
        /// <summary>
        /// 
        /// </summary>
        public int ApplyID { get; set; }
        /// <summary>
        /// 场馆编号
        /// </summary>

        [Required(ErrorMessage = "场馆编号不能为空")]
        public int VenueID { get; set; }
        /// <summary>
        /// 学生ID
        /// </summary>
        [Required(ErrorMessage = "学生编号不能为空")]
        public int StudentID { get; set; }
        /// <summary>
        /// 教练ID
        /// </summary>
        [Required(ErrorMessage = "教练编号不能为空")]
        public int CoachID { get; set; }
        /// <summary>
        /// 课时数量
        /// </summary>
        [Required(ErrorMessage = "课时数量不能为空")]
        public int ClassNumber { get; set; }

        private DateTime? _addtime = DateTime.Now;
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 支付总金额
        /// </summary>
        [Required(ErrorMessage = "支付总金额不能为空")]
        public decimal PayMoney { get; set; }
        /// <summary>
        /// 0申请成功，待支付（学生前端选择完成），1支付成功，并加入学生课次（场馆审核通过）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 类型1，1对1，2，1对多
        /// </summary>
        [Required(ErrorMessage = "课程类型不能为空")]
        public int PKType { get; set; }
        /// <summary>
        /// 实付金额(用于折扣、活动类的结算)
        /// </summary>
        [Required(ErrorMessage = "实付金额不能为空")]
        public decimal PaidMoney { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentFullName { get; set; }
        /// <summary>
        /// 教练姓名
        /// </summary>
        public string CoachFullName { get; set; }
    }
}
