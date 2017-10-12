﻿using System;
using System.ComponentModel.DataAnnotations;

namespace YY.Edu.Sys.Models
{
	/// <summary>
	/// Campus:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	//[Serializable]
	public partial class Campus
	{
		public Campus()
		{}
		#region Model
		private int _campusid;
		private int? _venueid;
		private string _campusname;
		private string _linkman;
		private string _linktel;
		private int? _cityid;
		private int? _campusstatus=1;
		/// <summary>
		/// 
		/// </summary>
		public int CampusID
		{
			set{ _campusid=value;}
			get{return _campusid;}
		}
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "场馆编号不能为空")]
        public int? VenueID
		{
			set{ _venueid=value;}
			get{return _venueid;}
		}
		/// <summary>
		/// 
		/// </summary>
        [Required(ErrorMessage = "校区名称不能为空")]
        public string CampusName
		{
			set{ _campusname=value;}
			get{return _campusname;}
		}
		/// <summary>
		/// 联系人
		/// </summary>
        [Required(ErrorMessage = "联系人不能为空")]
        public string LinkMan
		{
			set{ _linkman=value;}
			get{return _linkman;}
		}
		/// <summary>
		/// 联系人手机号
		/// </summary>
        [Required(ErrorMessage = "联系人手机号不能为空")]
        public string LinkTel
		{
			set{ _linktel=value;}
			get{return _linktel;}
		}
		/// <summary>
		/// 城市ID
		/// </summary>
        [Required(ErrorMessage = "城市不能为空")]
        public int? CityID
		{
			set{ _cityid=value;}
			get{return _cityid;}
		}
		/// <summary>
		/// 校区状态1有效，，2停用
		/// </summary>
		public int? CampusStatus
		{
			set{ _campusstatus=value;}
			get{return _campusstatus;}
		}
        #endregion Model

        /// <summary>
        /// 校区地址
        /// </summary>
        [Required(ErrorMessage = "校区地址不能为空")]
        public string Address { get; set; }
    }
}

