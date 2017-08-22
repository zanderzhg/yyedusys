﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dapper;
using DapperExtensions;
using System.Collections.Generic;

namespace YY.Edu.Sys.Test
{
    [TestClass]
    public class UnitTest1
    {
        public void TestMethod1()
        {

            //https://github.com/tmsmith/Dapper-Extensions
            //https://github.com/tmsmith/Dapper-Extensions/wiki/Predicates
            try
            {
                string _connectionString = "Data Source=.;Initial Catalog=SportsDB;Persist Security Info=True;User ID=sa;Password=11111111;Integrated Security=True";
                using (System.Data.IDbConnection cn = new System.Data.SqlClient.SqlConnection(_connectionString))
                {
                    cn.Open();

                    //添加
                    //var multiKey = cn.Insert<City>(new City() { CityName = "北京5" });
                    //Console.WriteLine(multiKey);

                    //分页

                    //分页查询
                    IList<ISort> sort = new List<ISort>();
                    sort.Add(new Sort { PropertyName = "CityID", Ascending = false });

                    long allRowsCount = 0;
                    //cn.GetPage<City>(1, 10, out allRowsCount, new { ID = 1 }, sort);
                    var predicate_page = Predicates.Field<City>(f => f.CityName, Operator.Like, "%北京%");
                    IEnumerable<City> list_page = cn.GetList<City>(predicate_page);

                    var result = cn.GetPage<City>(predicate_page, sort, 1, 1);
                    allRowsCount = cn.Count<City>(predicate_page);
                    Console.WriteLine(result.AsList().Count);

                    //查询
                    var predicate = Predicates.Field<City>(f => f.CityName, Operator.Eq, "北京2");
                    IEnumerable<City> list = cn.GetList<City>(predicate);
                    Console.WriteLine(list.AsList()[0].CityName);

                    //查询
                    int cityId = 1;
                    City person = cn.Get<City>(cityId);

                    person.CityName = "上海";
                    cn.Update(person);

                    cn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {

            string _connectionString = "Data Source=.;Initial Catalog=SportsDB;Persist Security Info=True;User ID=sa;Password=11111111;Integrated Security=True";
            System.Data.IDbConnection connection = new System.Data.SqlClient.SqlConnection(_connectionString);

            var sql = @"select * from Venue inner join City on Venue.CityID=City.CityID where venue.venueid=@id";

            //var result = connection.Query<Venue, City, Venue>(sql,
            //                        (product, users) =>
            //                        {
            //                            product. = users; return product;
            //                        }, splitOn: "UserName");

            var result = connection.Query(sql, new { id = 1 });
            Console.WriteLine(result);

        }
    }


    public class City
    {
        public int CityID { get; set; }

        public string CityName { get; set; }

    }

    public partial class Venue
    {
        public Venue()
        { }
        #region Model
        private int _venueid;
        private int? _cityid;
        private string _username;
        private string _pwd;
        private string _venuecode;
        private string _venuename;
        private string _venueaddress;
        private string _linkman;
        private string _linkmanmobile;
        private string _linkmanwx;
        private string _venuefax;
        private string _legalperson;
        private string _cardnumber;
        private DateTime? _addtime = DateTime.Now;
        private string _businesslicense;
        private string _logourl;
        private string _adduser;
        private int? _status = 0;
        private string _systemroleids;
        /// <summary>
        /// 主键ID
        /// </summary>
        public int VenueID
        {
            set { _venueid = value; }
            get { return _venueid; }
        }
        /// <summary>
        /// 城市ID
        /// </summary>
        public int? CityID
        {
            set { _cityid = value; }
            get { return _cityid; }
        }
        /// <summary>
        /// 用户名（手机号）
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd
        {
            set { _pwd = value; }
            get { return _pwd; }
        }
        /// <summary>
        /// 场馆代码（生成唯的一4～6位数）
        /// </summary>
        public string VenueCode
        {
            set { _venuecode = value; }
            get { return _venuecode; }
        }
        /// <summary>
        /// 场馆名称
        /// </summary>
        public string VenueName
        {
            set { _venuename = value; }
            get { return _venuename; }
        }
        /// <summary>
        /// 场馆地址
        /// </summary>
        public string VenueAddress
        {
            set { _venueaddress = value; }
            get { return _venueaddress; }
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan
        {
            set { _linkman = value; }
            get { return _linkman; }
        }
        /// <summary>
        /// 联系人手机号
        /// </summary>
        public string LinkManMobile
        {
            set { _linkmanmobile = value; }
            get { return _linkmanmobile; }
        }
        /// <summary>
        /// 联系人微信
        /// </summary>
        public string LinkManWX
        {
            set { _linkmanwx = value; }
            get { return _linkmanwx; }
        }
        /// <summary>
        /// 传真
        /// </summary>
        public string VenueFax
        {
            set { _venuefax = value; }
            get { return _venuefax; }
        }
        /// <summary>
        /// 法人姓名
        /// </summary>
        public string LegalPerson
        {
            set { _legalperson = value; }
            get { return _legalperson; }
        }
        /// <summary>
        /// 法人身份证号
        /// </summary>
        public string CardNumber
        {
            set { _cardnumber = value; }
            get { return _cardnumber; }
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 营业执照（图片地址）
        /// </summary>
        public string BusinessLicense
        {
            set { _businesslicense = value; }
            get { return _businesslicense; }
        }
        /// <summary>
        /// 场馆LOGO
        /// </summary>
        public string LogoUrl
        {
            set { _logourl = value; }
            get { return _logourl; }
        }
        /// <summary>
        /// 注册人
        /// </summary>
        public string AddUser
        {
            set { _adduser = value; }
            get { return _adduser; }
        }
        /// <summary>
        /// 状态0待审核，1审核通过，2删除
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 运营系统给场馆分配的权限，为空代表所有权限
        /// </summary>
        public string SystemRoleIDS
        {
            set { _systemroleids = value; }
            get { return _systemroleids; }
        }
        #endregion Model

    }
}