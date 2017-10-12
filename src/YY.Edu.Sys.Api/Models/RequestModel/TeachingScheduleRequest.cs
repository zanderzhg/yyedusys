﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YY.Edu.Sys.Api.Models.RequestModel
{
    public class TeachingScheduleRequest : Sys.Models.TeachingSchedule
    {

        /// <summary>
        /// 上课日期 2017-08-25 to 2017-09-25
        /// </summary>
        [Required(ErrorMessage = "上课日期不能为空")]
        public string CurriculumDateStr { get; set; }
        
        /// <summary>
        /// 上午是否有课
        /// </summary>
        [Required(ErrorMessage = "上午是否有课不能为空")]
        public bool TimepickerAMCheckd { get; set; }

        /// <summary>
        /// 下午是否有课
        /// </summary>
        [Required(ErrorMessage = "下午是否有课不能为空")]
        public bool TimepickerPMCheckd { get; set; }
        /// <summary>
        /// 上午上课时间
        /// </summary>
        [Required(ErrorMessage = "上午上课时间不能为空")]
        public string CurriculumAMTime { get; set; }

        /// <summary>
        /// 下午上课时间
        /// </summary>
        [Required(ErrorMessage = "下午上课时间不能为空")]
        public string CurriculumPMTime { get; set; }

        /// <summary>
        /// 上课时长
        /// </summary>
        [Required(ErrorMessage = "上课时长不能为空")]
        public int ClassLength { get; set; }
        /// <summary>
        /// 休息时长
        /// </summary>
        [Required(ErrorMessage = "休息时长不能为空")]
        public int RestLength { get; set; }

    }
}