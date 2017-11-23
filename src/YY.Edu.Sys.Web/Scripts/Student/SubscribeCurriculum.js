﻿
jQuery.support.cors = true;
function LoadMyCurriculum() {
    nextPagePlus();

    var CurrentDate = "";

    var myCuDate = new Date();
    CurrentDate = myCuDate.getFullYear() + "-" + (myCuDate.getMonth() + 1) + "-" + myCuDate.getDate();

    var requestPrm = " { PageIndex: " + $("#hdPageIndex").val() + ", PageSize:" + $("#hdPageSize").val() + ", RequestType: 1,CurrentDate:'" + CurrentDate + "',TSMID:1,PKID:1,PKType:1,SearchCondition:{StudentID: " + $("#hdSID").val() + "}}";
    $.ajax({
        type: "get",
        url: ApiUrl + "/Student/GetTeachingScheduleByStudentNEw/",
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
            //0 排课完成 1学生约课完成，2学校停课（需要判断，有没有学生预约）,3请假申请 4请假 5上课中 6上课完成 
            $.each(dataCu, function (i, c) {

                var KSstate = c.State;
                var PKType = c.PKType;
                var iyk = 0;
                if (KSstate == 0 || (KSstate == 1 && PKType == 2)) {
                    iyk = 1;
                }
               

                var ksInfo = "V" + c.VenueID + "_C" + c.CoachID + "_" + c.ClassNumber;//,VenueID_CoachID_ClassNumber

                //str += '<li>';
                ////str += ' <img src="http://s.amazeui.org/media/i/demos/bing-1.jpg" alt="" class="am-u-sm-3 am-u-md-3 am-u-lg-3">';
                ////str += ' <div class="am-u-sm-12 am-u-md-12 am-u-lg-12">';
                //str += ' <a href="order_detail.html">';
                //str += '<p class="time">' + dateformat(c.CurriculumDate, "MM-dd") + "    " + c.CurriculumBeginTime + "-" + c.CurriculumEndTime + '</p>';
                //str += '<p class="address"><span>' + c.VenueName + "-" + c.CampusName + '</span></p>';
                ////str += '<p>场馆：<span>' + c.VenueName + "-" + c.CampusName + '</span></p>';
                //str += '<p class="cx">科目：<span>' + c.Title + '</span></p>';
                //str += '<p class="cx">老师：<span>' + c.CoachFullName + '</span></p>';
                //str += ' </a>';
                ////str += ' </div>';
                //if (iyk == 1) {
                //    //  str += ' <input type="checkbox" data-pkid="' + c.PKID + '" data-vid="' + c.VenueID + '"  data-coachid="'+c.CoachID+'"  >'; //int sid, int pkid, int cid, int vid, string sname
                //    str += "  <button type='button' class='order-btn' onclick='Subscribe(" + c.PKID + ",1," + c.VenueID + "," + c.CoachID + ",this)' class='order-btn'> 约课 </button> ";
                //}
                //str += '  </li>';

                str += '  <li>';
               
                var CurriculumBeginDate = c.CurriculumBeginDate;
                var CurriculumEndDate = c.CurriculumEndDate;

                var CuDate = dateformat(new Date(), "yyyy-MM-dd");
                var Day = diy_time(c.CurriculumBeginDate, c.CurriculumEndDate);
               
                var sel = '  <select id="selDate' + c.TSMID + '" style="height:37px;width:135px;border:0;border:none;" onchange=GetPKTime(' + c.TSMID + ',this,"selTime' + c.TSMID + '")>';
                sel += '<option value="0">选择日期</option>';
                var strDate = "";
                for (var i =-1; i < Day; i++)
                {
                    var temp=change_date(CurriculumBeginDate, -1*i);
                    if (Date.parse(new Date(temp)) >=Date.parse(new Date(CuDate)))
                    {
                        strDate += '<option value="' + c.TSMID + '">' + dateformat(temp, "yyyy-MM-dd") + '</option>';
                    }
                  //  CurriculumBeginDate = temp;
                }              
             
                sel += strDate;
                sel += ' </select><select id="selTime' + c.TSMID + '" style="height:37px;width:135px;border:0;border:none;">';
                sel += '   <option value="0">选择时间</option>';

                sel += ' </select>';
                str += '  <p class="time">' + sel + '</p>';

                str += "<a href='#' onclick=detail(1,"+c.TSMID+",0)> ";
               // str += '  <p class="time">' + dateformat(c.CurriculumDate, "MM-dd") + "    " + c.CurriculumBeginTime + "-" + c.CurriculumEndTime + '</p>';
                str += '  <p class="address"><i class="iconfont">&#xe600;</i>'+ c.CampusName + '</p>';
                str += '  <p class="cx"><i class="iconfont">&#xe615;</i>' + c.Title + '</p>';
                str += '  <p class="cx"><i class="iconfont">&#xe612;</i>老师:' + c.CoachFullName + '</p>';
                str += '  </a>';
                str += "  <button type='button' class='order-btn' onclick='Subscribe(" + c.TSMID + ",1," + c.VenueID + "," + c.CoachID + ",this)'>约课</button>";
                str += '  </li>';

            });

            //if (str == "") {
            //    $('#btnMore a').text("没有课程啦");
            //} else { $('#btnMore a').text("更多课程 »"); }
            $("#panel0").append(str);

        }
    });
}

function diy_time(time1, time2) {
    time1 = Date.parse(new Date(time1));
    time2 = Date.parse(new Date(time2));
    return Math.abs(parseInt((time2 - time1) / 1000 / 3600 / 24));
}


var change_date = function (cudate,days) {
    // 参数表示在当前日期下要增加的天数  
    var now = new Date(cudate);
    // + 1 代表日期加，- 1代表日期减  
    now.setDate((now.getDate()) - 1 * days);
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    var day = now.getDate();
    if (month < 10) {
        month = '0' + month;
    }
    if (day < 10) {
        day = '0' + day;
    }

    return year + '-' + month + '-' + day;
};


function LoadMyCurriculum2() {

    nextPagePlusMore();

    var CurrentDate = "";

    var myCuDate = new Date();
    CurrentDate = myCuDate.getFullYear() + "-" + (myCuDate.getMonth() + 1) + "-" + myCuDate.getDate();

    var requestPrm = " { PageIndex: " + $("#hdPageIndex2").val() + ", PageSize:" + $("#hdPageSize2").val() + ", RequestType: 1,CurrentDate:'" + CurrentDate + "',PKType:2,SearchCondition:{StudentID: " + $("#hdSID").val() + "}}";
    $.ajax({
        type: "get",
        url: ApiUrl + "/Student/GetTeachingScheduleByStudentNEw/",
        dataType: "json",
        async: false,
        data: { query: requestPrm },
        success: function (data) {
            
            var str = "";
            var dataCu = data.data;
            if (dataCu.length >= parseInt($("#hdPageSize2").val())) {
                $("#more02").show();
            } else {
                $("#more02").hide();
            }

            //0 排课完成 1学生约课完成，2学校停课（需要判断，有没有学生预约）,3请假申请 4请假 5上课中 6上课完成 
            $.each(dataCu, function (i, c) {

                var KSstate = c.State;
                var PKType = c.PKType;
                var iyk = 0;
                if (KSstate == 0 || (KSstate == 1 && PKType == 2)) {
                    iyk = 1;
                }
               

                str += '  <li>';

                var CurriculumBeginDate = c.CurriculumBeginDate;
                var CurriculumEndDate = c.CurriculumEndDate;

                var CuDate = dateformat(new Date(), "yyyy-MM-dd");
                var Day = diy_time(c.CurriculumBeginDate, c.CurriculumEndDate);

                var sel = '  <select id="selDate2' + c.TSMID + '" style="height:37px;width:135px;border:0;border:none;" onchange=GetPKTime(' + c.TSMID + ',this,"selTime2' + c.TSMID + '")>';
                sel += '<option value="0">选择日期</option>';
                var strDate = "";
                for (var i = -1; i < Day; i++) {
                    var temp = change_date(CurriculumBeginDate, -1 * i);
                    if (Date.parse(new Date(temp)) >= Date.parse(new Date(CuDate))) {
                        strDate += '<option value="' + c.TSMID + '">' + dateformat(temp, "yyyy-MM-dd") + '</option>';
                    }
                    //  CurriculumBeginDate = temp;
                }

                sel += strDate;
                sel += ' </select><select id="selTime2' + c.TSMID + '" style="height:37px;width:135px;border:0;border:none;">';
                sel += '   <option value="0">选择时间</option>';

                sel += ' </select>';
                str += '  <p class="time">' + sel + '</p>';

                str += "<a href='#' onclick=detail(1," + c.TSMID + ",0)> ";
                // str += '  <p class="time">' + dateformat(c.CurriculumDate, "MM-dd") + "    " + c.CurriculumBeginTime + "-" + c.CurriculumEndTime + '</p>';
                str += '  <p class="address"><i class="iconfont">&#xe600;</i>' + c.CampusName + '</p>';
                str += '  <p class="cx"><i class="iconfont">&#xe615;</i>' + c.Title + '</p>';
                str += '  <p class="cx"><i class="iconfont">&#xe612;</i>老师:' + c.CoachFullName + '</p>';
                str += '  </a>';
                str += "  <button type='button' class='order-btn' onclick='Subscribe(" + c.TSMID + ",1," + c.VenueID + "," + c.CoachID + ",this)'>约课</button>";
                str += '  </li>';

            });

            //if (str == "") {
            //    $('#btnMore a').text("没有课程啦");
            //} else { $('#btnMore a').text("更多课程 »"); }
            $("#panel1").append(str);

        }
    });
}

function dateformat(date) {
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

function nextPagePlusMore() {

    var iPindex = parseInt($("#hdPageIndex2").val()) + 1;
    $("#hdPageIndex2").val(iPindex);

}

$('#btnMore a').bind('click', function () {
    //hdPageIndex
    var iPindex = parseInt($("#hdPageIndex").val()) + 1;
    $("#hdPageIndex").val(iPindex);
    LoadMyCurriculum();
    //hdPageSize
});


function GetPKTime(tsmid, obj, strid){
 
    var text = $(obj).find("option:selected").text();
    $.ajax({
        type: "get",
        url: ApiUrl + "/Student/GetTeachingScheduleTime/",
        contentType: "application/json",
        //data: { sid: sid, pkid: pkid, cid: cid, vid: VenueID, sname: FullName },
        data: { TSMID: tsmid, StudentID: parseInt($("#hdSID").val()), CDate: text },
        success: function (data) {
          
            $.each(data, function (i, c) {
                var info = ""
                info += '<option value="0">选择时间</option>';
                var CuDate = dateformat(new Date(), "yyyy-MM-dd");
                var icount = 0;
                $.each(data, function (i, c) {
                    if (Date.parse(new Date(text)) == Date.parse(new Date(CuDate))) {
                        var cTime = text+ " " + c.CurriculumEndTime + ":00";
                        if(Date.parse(new Date(cTime))>new Date())
                        {
                            icount++;
                            info += '<option value="' + c.PKID + '">' + c.CurriculumBeginTime + '~' + c.CurriculumEndTime + '</option>'
                        }
                    }
                    else {
                        icount++;
                        info += '<option value="' + c.PKID + '">' + c.CurriculumBeginTime + '~' + c.CurriculumEndTime + '</option>'
                    }
                });
                if (icount == 0)
                {
                    info = '<option value="0">没有可约的时间段</option>';
                    $("#" + strid).html(info);
                }
                else
                {
                    $("#" + strid).html(info);
                }
            });
        },
        error: function (e) {
         
        },
        complete: function () {

        }
    });
}

function detail(type,TSMID,CurriculumID)
{
    var PKID=0;
    if (type == 1) {
        if ($("#selDate" + TSMID).val() == "0") {
            model_alert("请选择要约的日期");
            return false;
        }
       
        if ($("#selTime" + TSMID).val() == "0") {
            model_alert("请选择要约的时间段");
            return false;
        }
        PKID = $("#selTime" + TSMID).val();
    }
    else {
        if ($("#selDate2" + TSMID).val() == "0") {
            model_alert("请选择要约的日期");
            return false;
        }

        if ($("#selTime2" + TSMID).val() == "0") {
            model_alert("请选择要约的时间段");
            return false;
        }
        PKID = $("#selTime2" + TSMID).val();
    }

          
    location.href = "CurriculumDetail/?pkid=" + PKID + "&cid=" + $("#hdSID").val() + "&cuid=" + CurriculumID + "&state=-1";
}


function Subscribe(TSMID, sid, VenueID, cid, obj) {

   
    //selTime selDate
    var FullName = $("#hdSName").val();
    var sid = $("#hdSID").val();
    if ($("#selDate" + TSMID).val() == "0")
    {
        model_alert("请选择要约的日期");
        return false;
    }

    if ($("#selTime" + TSMID).val() == "0") {
        model_alert("请选择要约的时间段");
        return false;
    }
    $(obj).attr("disabled", "disabled");
    $.ajax({
        type: "Post",
        url: ApiUrl + "/Student/SubscribeCurriculum/",
        contentType: "application/json",
        //data: { sid: sid, pkid: pkid, cid: cid, vid: VenueID, sname: FullName },
        data: JSON.stringify({ PKID: $("#selTime" + TSMID).val(), VenueID: VenueID, SID: sid, CID: cid, SName: FullName }),
        success: function (data, status) {

            if (status == "success") {

                if (!data.Error) {
                   
                    //$(obj).attr("disabled", "disabled");
                    $(obj).css({ 'background-color': '#C0C0C0' });
                    LoadMyCurriculum();
                    LoadMyCurriculum2();
                    model_alert("约课成功，快去我的课程里查看吧");
                   // $(obj).text("预约成功");
                }
                else {
                    $(obj).removeAttr("disabled");
                    model_alert("约课失败，刷新下课时列表再重新约下吧");
                }
            }
            else {
                $(obj).removeAttr("disabled");
                model_alert("约课失败，刷新下课时列表再重新约下吧");
            }
        },
        error: function (e) {
            $(obj).removeAttr("disabled");
            model_alert("约课失败，刷新下课时列表再重新约下吧!");
        },
        complete: function () {

        }
    });
}

$(function () {

    $(".o").hide();
    $(".o").first().show();

    $("#hdPageIndex").val('0');
    $("#hdPageIndex2").val('0');
    LoadMyCurriculum();

    LoadMyCurriculum2();

    $("#more").on("click", function () {
        setTimeout(function () {
            LoadMyCurriculum();
            //myscroll.refresh();
        }, 100)
    });

    $("#more02").on("click", function () {
        setTimeout(function () {
            LoadMyCurriculum2();
        }, 100)
    });

    $(".order li").on('click', function () {
        $(".order li").removeClass('current');
        $(this).addClass('current');
        $(".o").hide();
        $("#wrapper" + $(this).attr("id").replace("tab", "")).show();
    });

})