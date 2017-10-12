﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YY.Edu.Sys.Models
{
    public class FeedBackLog
    {
        public FeedBackLog()
        { }
        #region Model
        private int _fdid;
        private int _userid;
        private string _username;
        private string _fdmsg;
        private DateTime? _addtime = DateTime.Now;
        private int _venueid;
        private int _studentid;
        private string _fullname;
        /// <summary>
        /// 
        /// </summary>
        public int FDId
        {
            set { _fdid = value; }
            get { return _fdid; }
        }
        /// <summary>
        /// 添加公告的后台用户
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 添加公告的后台用户名
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "反馈内容不能为空")]
        public string FDMsg
        {
            set { _fdmsg = value; }
            get { return _fdmsg; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 主键ID
        /// </summary>
        [Required(ErrorMessage = "场馆编号不能为空")]
        public int VenueID
        {
            set { _venueid = value; }
            get { return _venueid; }
        }
        /// <summary>
        /// 学生表主键ID
        /// </summary>
        [Required(ErrorMessage = "学生不能为空")]
        public int StudentID
        {
            set { _studentid = value; }
            get { return _studentid; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string FullName
        {
            set { _fullname = value; }
            get { return _fullname; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 反馈内容
        /// </summary>
        public string ReplyMsg { get; set; }
        #endregion Model
    }
}
