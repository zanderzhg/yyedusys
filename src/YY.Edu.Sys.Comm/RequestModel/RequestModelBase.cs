﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YY.Edu.Sys.Comm.RequestModel
{
    public class RequestModelBase<T>
    {

        public T SearchCondition { get; set; }


        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        /// <summary>
        /// 请求类型，1今日，2未上，3已上
        /// </summary>
        public int RequestType { get; set; }

        /// <summary>
        /// 当前日期
        /// </summary>
        public string CurrentDate { get; set; }
        /// <summary>
        ///开始日期
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        ///  结束日期
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 1:1对1，2：1对多
        /// </summary>
        public int PKType { get; set; }

        public int StudentID { get; set; }


        public int TSMID { get; set; }

        public int PKID { get; set; }
    }
}
