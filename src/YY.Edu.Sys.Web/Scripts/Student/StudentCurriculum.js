﻿
jQuery.support.cors = true;
function LoadMyCurriculum() {

    nextPagePlus();

    // RequestType 请求类型，1今日，2未上，3已上

    var CurrentDate = "";

    var myCuDate = new Date();
    CurrentDate = myCuDate.getFullYear() + "-" + (myCuDate.getMonth() + 1) + "-" + myCuDate.getDate();

    var requestPrm = " { PageIndex: " + $("#hdPageIndex").val() + ", PageSize:" + $("#hdPageSize").val() + ", RequestType:1,CurrentDate:'" + CurrentDate + "',SearchCondition:{StudentID:" + $("#hdStudentID").val() + "}}";
    $.ajax({
        type: "get",
        url: ApiUrl + "/Student/GetStudentCurriculum/",
        dataType: "json",
        async: false,
        data: { query: requestPrm },
        success: function (data) {

            var str = "";
            var dataCu = data.data;
            if (dataCu.length >= parseInt($("#hdPageSize").val())) {
                $("#more").show();
            } else {
                $("#more").hide();
            }

            //0预约成功，1上课成功，2学生请假，3老师请假4场馆停课
            $.each(dataCu, function (i, c) {

                var KSstate = c.KSstate;
                var strState = "预约完成";
                if (KSstate == 0) {
                    var myDate = new Date();
                    var currTime = myDate.getFullYear() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes() + ":00";
                    var endTime = dateformat(c.CurriculumDate, "yyyy-MM-dd") + " " + c.CurriculumEndTime + ":00";

                    var dcTime = NewDateTime(currTime);
                    var deTime = NewDateTime(endTime);

                    if (dcTime >= deTime) {
                        strState = "未到";
                    }
                }
                else if (KSstate == 1 || KSstate == 5) {
                    strState = "已学完";
                    var myDate = new Date();
                    var currTime = myDate.getFullYear() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes() + ":00";
                    var beginTime = dateformat(c.CurriculumDate, "yyyy-MM-dd") + " " + c.CurriculumBeginTime + ":00";
                    var endTime = dateformat(c.CurriculumDate, "yyyy-MM-dd") + " " + c.CurriculumEndTime + ":00";

                    var dcTime = NewDateTime(currTime);
                    var dbTime = NewDateTime(beginTime);
                    var deTime = NewDateTime(endTime);
                    if (dcTime >= dbTime && dcTime <= deTime) {
                        strState = "上课中";
                    }

                    if (dcTime < dbTime) {
                        strState = "等待上课";
                    }
                }
                else if (KSstate == 2) {
                    strState = "请假";
                }
                else if (KSstate == 3) {
                    strState = "老师请假";
                }
                else if (KSstate == 4) {
                    strState = "场馆停课";
                }
                else if (KSstate == 6) {
                    strState = "申请请假";
                }

                //str += '<li class="courseT am-g">';
                //str += "<a href='CurriculumDetail/?pkid=" + c.PKID + "&cid=" + $("#hdStudentID").val() + "&cuid=" + c.CurriculumID + "'> ";
                //str += ' <div class="am-u-sm-9 am-u-md-9 am-u-lg-9">';
                //str += '<p class="courseName">' + c.Title + '     ' + c.CoachFullName + '</p>';
                //str += ' <p><span class="am-icon-clock-o"></span> <span>' + dateformat(c.CurriculumDate, "yyyy-MM-dd") + " " + c.CurriculumBeginTime + "-" + c.CurriculumEndTime + '</span></p>';
                //str += ' <p><span class="am-icon-map-marker"></span> <span>' + c.VenueName + "-" + c.CampusName + '</span></p>';
                //str += ' <p>教练：<span>1人</span> 状态: <span><font  style="color:red">' + strState + '</font></span></p></div>';
                //str += ' </li> ';
                //str += '</a>  ';

                str += '  <li>';
                str += "<a href='CurriculumDetail/?pkid=" + c.PKID + "&cid=" + $("#hdStudentID").val() + "&cuid=" + c.CurriculumID + "'> ";
                str += '  <p class="time">' + dateformat(c.CurriculumDate, "MM-dd") + "    " + c.CurriculumBeginTime + "-" + c.CurriculumEndTime + '</p>';
                str += '  <p class="address"><i class="iconfont">&#xe600;</i>' + c.VenueName + "-" + c.CampusName + '</p>';
                str += '  <p class="cx"><i class="iconfont">&#xe612;</i>' + c.Title + '</p>';
                str += '  <p class="cx"><i class="iconfont">&#xe612;</i>教练:' + c.CoachFullName + ' 状态:<span><font  style="color:red">' + strState + '</font></span></p>';
                str += '  </a>';
                str += '  </li>';
                str += ' </div>';

            });

            $("#panel0").append(str);
        }
    });
}

function LoadMyCurriculum4WaitClass() {

    nextPageWaitClassPlus();
    var CurrentDate = "";

    var myCuDate = new Date();
    CurrentDate = myCuDate.getFullYear() + "-" + (myCuDate.getMonth() + 1) + "-" + myCuDate.getDate();

    var requestPrm = " { PageIndex: " + $("#hdPageIndex4WaitClass").val() + ", PageSize:" + $("#hdPageSize4WaitClass").val() + ", RequestType:2,CurrentDate:'" + CurrentDate + "',SearchCondition:{StudentID:" + $("#hdStudentID").val() + "}}";
    $.ajax({
        type: "get",
        url: ApiUrl + "/Student/GetStudentCurriculum/",
        dataType: "json",
        async: false,
        data: { query: requestPrm },
        success: function (data) {

            var str = "";
            var dataCu = data.data;
            if (dataCu.length >= parseInt($("#hdPageSize4WaitClass").val())) {
                $("#moreWaitClass").show();
            } else {
                $("#moreWaitClass").hide();
            }

            //0预约成功，1上课成功，2学生请假，3老师请假4场馆停课
            $.each(dataCu, function (i, c) {

                var KSstate = c.KSstate;
                var strState = "预约完成";
                if (KSstate == 0) {
                    var myDate = new Date();
                    var currTime = myDate.getFullYear() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes() + ":00";
                    var endTime = dateformat(c.CurriculumDate, "yyyy-MM-dd") + " " + c.CurriculumEndTime + ":00";

                    var dcTime = NewDateTime(currTime);
                    var deTime = NewDateTime(endTime);

                    if (dcTime >= deTime) {
                        strState = "未到";
                    }
                }
                else if (KSstate == 1||KSstate==5) {
                    strState = "已学完";
                    var myDate = new Date();
                    var currTime = myDate.getFullYear() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes() + ":00";
                    var beginTime = dateformat(c.CurriculumDate, "yyyy-MM-dd") + " " + c.CurriculumBeginTime + ":00";
                    var endTime = dateformat(c.CurriculumDate, "yyyy-MM-dd") + " " + c.CurriculumEndTime + ":00";

                    var dcTime = NewDateTime(currTime);
                    var dbTime = NewDateTime(beginTime);
                    var deTime = NewDateTime(endTime);
                    if (dcTime >= dbTime && dcTime <= deTime) {
                        strState = "上课中";
                    }

                    if (dcTime < dbTime) {
                        strState = "等待上课";
                    }
                }
                else if (KSstate == 2) {
                    strState = "请假";
                }
                else if (KSstate == 3) {
                    strState = "老师请假";
                }
                else if (KSstate == 4) {
                    strState = "场馆停课";
                }
                else if (KSstate == 6) {
                    strState = "申请请假";
                }
                //str += '<li class="courseT am-g">';
                //str += "<a href='CurriculumDetail/?pkid=" + c.PKID + "&cid=" + $("#hdStudentID").val() + "&cuid=" + c.CurriculumID + "'> ";
                //str += '<img src="http://s.amazeui.org/media/i/demos/bing-1.jpg" alt="" class="am-u-sm-3 am-u-md-3 am-u-lg-3">';
                //str += ' <div class="am-u-sm-9 am-u-md-9 am-u-lg-9">';

                //str += '<p class="courseName">' + c.Title + '     ' + c.CoachFullName + '</p>';
                //str += ' <p><span class="am-icon-clock-o"></span> <span>' + dateformat(c.CurriculumDate, "yyyy-MM-dd") + " " + c.CurriculumBeginTime + "-" + c.CurriculumEndTime + '</span></p>';
                //str += ' <p><span class="am-icon-map-marker"></span> <span>' + c.VenueName + "-" + c.CampusName + '</span></p>';
                //str += ' <p>教练：<span>1人</span> 状态: <span><font  style="color:red">' + strState + '</font></span></p></div>';
                //str += ' </li> </a> ';

                str += '  <li>';
                str += "<a href='CurriculumDetail/?pkid=" + c.PKID + "&cid=" + $("#hdStudentID").val() + "&cuid=" + c.CurriculumID + "'> ";
                str += '  <p class="time">' + dateformat(c.CurriculumDate, "MM-dd") + "    " + c.CurriculumBeginTime + "-" + c.CurriculumEndTime + '</p>';
                str += '  <p class="address"><i class="iconfont">&#xe600;</i>' + c.VenueName + "-" + c.CampusName + '</p>';
                str += '  <p class="cx"><i class="iconfont">&#xe612;</i>' + c.Title + '</p>';
                str += '  <p class="cx"><i class="iconfont">&#xe612;</i>教练:' + c.CoachFullName + ' 状态:<span><font  style="color:red">' + strState + '</font></span></p>';
                str += '  </a>';
                str += '  </li>';
                str += ' </div>';

            });

            $("#panelWaitClass").append(str);
        }
    });
}

function LoadMyCurriculum4History() {

    nextPageHistoryPlus();

    var CurrentDate = "";

    var myCuDate = new Date();
    CurrentDate = myCuDate.getFullYear() + "-" + (myCuDate.getMonth() + 1) + "-" + myCuDate.getDate();

    var requestPrm = " { PageIndex: " + $("#hdPageIndex4History").val() + ", PageSize:" + $("#hdPageSize4History").val() + ", RequestType:3,CurrentDate:'" + CurrentDate + "',SearchCondition:{StudentID:" + $("#hdStudentID").val() + "}}";
    $.ajax({
        type: "get",
        url: ApiUrl + "/Student/GetStudentCurriculum/",
        dataType: "json",
        async: false,
        data: { query: requestPrm },
        success: function (data) {

            var str = "";
            var dataCu = data.data;
            if (dataCu.length >= parseInt($("#hdPageSize4History").val())) {
                $("#moreHistory").show();
            } else {
                $("#moreHistory").hide();
            }
            //0预约成功，1上课成功，2学生请假，3老师请假4场馆停课
            $.each(dataCu, function (i, c) {

                var KSstate = c.KSstate;
              
                var strState = "预约完成";
                if (KSstate == 0) {
                    var myDate = new Date();
                    var currTime = myDate.getFullYear() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes() + ":00";
                    var endTime = dateformat(c.CurriculumDate, "yyyy-MM-dd") + " " + c.CurriculumEndTime + ":00";

                    var dcTime = NewDateTime(currTime);
                    var deTime = NewDateTime(endTime);

                    if (dcTime >= deTime) {
                        strState = "未到";
                    }
                }
                else if (KSstate == 1||KSstate==5) {
                    strState = "已学完";
                    var myDate = new Date();
                    var currTime = myDate.getFullYear() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes() + ":00";
                    var beginTime = dateformat(c.CurriculumDate, "yyyy-MM-dd") + " " + c.CurriculumBeginTime + ":00";
                    var endTime = dateformat(c.CurriculumDate, "yyyy-MM-dd") + " " + c.CurriculumEndTime + ":00";

                    var dcTime = NewDateTime(currTime);
                    var dbTime = NewDateTime(beginTime);
                    var deTime = NewDateTime(endTime);
                    if (dcTime >= dbTime && dcTime <= deTime) {
                        strState = "上课中";
                    }

                    if (dcTime < dbTime) {
                        strState = "等待上课";
                    }
                }
                else if (KSstate == 2) {
                    strState = "请假";
                }
                else if (KSstate == 3) {
                    strState = "老师请假";
                }
                else if (KSstate == 4) {
                    strState = "场馆停课";
                } else if (KSstate == 6) {
                    strState = "申请请假";
                }
                str += '  <li>';
                str += "<a href='CurriculumDetail/?pkid=" + c.PKID + "&cid=" + $("#hdStudentID").val() + "&cuid=" + c.CurriculumID + "'> ";
                str += '  <p class="time">' + dateformat(c.CurriculumDate, "MM-dd") + "    " + c.CurriculumBeginTime + "-" + c.CurriculumEndTime + '</p>';
                str += '  <p class="address"><i class="iconfont">&#xe600;</i>' + c.VenueName + "-" + c.CampusName + '</p>';
                str += '  <p class="cx"><i class="iconfont">&#xe612;</i>' + c.Title + '</p>';
                str += '  <p class="cx"><i class="iconfont">&#xe612;</i>教练:' + c.CoachFullName + ' 状态:<span><font  style="color:red">' + strState + '</font></span></p>';
                str += '  </a>';
                str += '  </li>';
                str += ' </div>';

            });
            
            $("#panelHistory").append(str);
        }
    });
}

function dateformat(date)
{
    var myDate = new Date(date);
    return myDate.getFullYear() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getDate();
}

function NewDateTime(str) {
    str = str.replace("-", "/").replace("-", "/");
    var arr = str.split(' ');

    var strY = arr[0].split('/');
    var date = new Date();
    date.setFullYear(Number(strY[0]));
    date.setMonth(Number(strY[1]) - 1);
    date.setDate(Number(strY[2]));
    if (arr.length > 1) {
        var strH = arr[1].split(':');
        date.setHours(Number(strH[0]));
        date.setMinutes(Number(strH[1]));
        date.setSeconds(Number(strH[2]));
        date.setMilliseconds(0);
    } else {
        date.setHours(0, 0, 0, 0);
    }
    return date;
}

function nextPagePlus() {
    var iPindex = parseInt($("#hdPageIndex").val()) + 1;
    $("#hdPageIndex").val(iPindex);
}

function nextPageWaitClassPlus() {
    var iPindex = parseInt($("#hdPageIndex4WaitClass").val()) + 1;
    $("#hdPageIndex4WaitClass").val(iPindex);
}

function nextPageHistoryPlus() {
    var iPindex = parseInt($("#hdPageIndex4History").val()) + 1;
    $("#hdPageIndex4History").val(iPindex);
}


function CurriculumDetail(pkid, sid, cid) {
    location.href = "../CurriculumDetail/?pkid=" + pkid + "&cid=" + sid + "&cuid=" + cid;
}

$(function () {

    $(".o").hide();
    $(".o").first().show();

    LoadMyCurriculum();
    LoadMyCurriculum4WaitClass();
    LoadMyCurriculum4History();

    $("#more").on("click", function () {
        setTimeout(function () {
            LoadMyCurriculum();
            //myscroll.refresh();
        }, 100)
    });

    $("#moreWaitClass").on("click", function () {
        setTimeout(function () {
            LoadMyCurriculum4WaitClass();
            //myscrollWaitClass.refresh();
        }, 100)
    });

    $("#moreHistory").on("click", function () {
        setTimeout(function () {
            LoadMyCurriculum4History();
            //myscrollHistory.refresh();
        }, 100)
    });

    $(".order li").on('click', function () {
        $(".order li").removeClass('current');
        $(this).addClass('current');
        $(".o").hide();
        $("#wrapper" + $(this).attr("id").replace("tab", "")).show();
    });

})