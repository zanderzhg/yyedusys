﻿jQuery.support.cors = true;

function LoadInfo() {
    
    var str = "";
    $.ajax({
        type: "get",
        url: ApiUrl + "/Student/GetStudentCurriculumByID/",
        dataType: "json",
        async: false,
        data: { PKID: $("#hdID").val() },
        //beforeSend: function () {
        //},
        success: function (data) {         
            var c = data[0];           
          
            str += ' <p>' + c.Title + '</p> ';
            str += '<p> <span>课时：</span> <span>' + dateformat(c.CurriculumDate, "yyyy-MM-dd") + '</span> <span>' + c.CurriculumBeginTime + "-" + c.CurriculumEndTime + '</span></p> ';
            str += ' <img src="' + c.HeadUrl + '" alt="" class="am-circle"> ';
            str += '<p class="textAlign reFirName">' + c.CoachFullName + '</p> ';
         
            $(".remaekFir").html(str);
        }
    });
}




function GetPJ()
{
    var str = "";
    $.ajax({
        type: "get",
        url: ApiUrl + "/Student/GetCurriculumEvaluateByID/",
        dataType: "json",
        async: false,
        data: { StudentID: $("#hdSID").val(), CurriculumID: $("#hdCID").val() },
        //beforeSend: function () {
        //},
        success: function (data) {
            var c = data[0];
            if (c != null && c != "") {
                $("#save").hide();
                var starT = c.StarLevel - 1;
                var starG = c.EffectStarLevel - 1;

                $("#Typj span").each(function (index) {
                    if (starT >= index) {
                        $(this).attr("class", "am-icon-star");
                    }
                    else { $(this).attr("class", "am-icon-star-o"); }
                });

                $("#Ggpj span").each(function (index) {
                    if (starG >= index) {
                        $(this).attr("class", "am-icon-star");
                    }
                    else { $(this).attr("class", "am-icon-star-o"); }
                });

                $("#Tpf").text(c.StarLevel * 2);
                $("#Gpf").text(c.EffectStarLevel * 2);

                $("#doc-ta-1").val(c.Info);

              
            }
        }
    });
}

function checkTSart(c) {
    var tsnumber = 0;
    if ($(c).attr("class") == 'am-icon-star-o') {
        $(c).attr("class", "am-icon-star");
        tsnumber = 2;
    }
    else {
        $(c).attr("class", "am-icon-star-o");
    }
    var tez = c;

    while (1 == 1) {
        var te = $(tez).prev();
        tez = te;
        var strclass = $(te).attr("class");
        if (strclass != "am-icon-star" && strclass != "am-icon-star-o") {
            break;
        }
        else {
            tsnumber = tsnumber + 2;
            $(te).attr("class", "am-icon-star");
        }
    }

    var tez2 = c;
    while (1 == 1) {
        var te = $(tez2).next();
        tez2 = te;
        var strclass = $(te).attr("class");
        if (strclass != "am-icon-star" && strclass != "am-icon-star-o") {
            break;
        }
        else {
            $(te).attr("class", "am-icon-star-o");
        }
    }

    $("#Tpf").text(tsnumber);

}



function checkSSart(c) {
    var tsnumber = 0;
    if ($(c).attr("class") == 'am-icon-star-o') {
        $(c).attr("class", "am-icon-star");
        tsnumber = 2;
    }
    else {
        $(c).attr("class", "am-icon-star-o");
    }
    var tez = c;

    while (1 == 1) {
        var te = $(tez).prev();
        tez = te;
        var strclass = $(te).attr("class");
        if (strclass != "am-icon-star" && strclass != "am-icon-star-o") {
            break;
        }
        else {
            tsnumber = tsnumber + 2;
            $(te).attr("class", "am-icon-star");
        }
    }

    var tez2 = c;
    while (1 == 1) {
        var te = $(tez2).next();
        tez2 = te;
        var strclass = $(te).attr("class");
        if (strclass != "am-icon-star" && strclass != "am-icon-star-o") {
            break;
        }
        else {
            $(te).attr("class", "am-icon-star-o");
        }
    }

    $("#Gpf").text(tsnumber);

}

$('#Typj span').bind('click', function () {
    var c = $(this);

    checkTSart(c);
});


$('#Ggpj span').bind('click', function () {

    var c = $(this);
    checkSSart(c);
});


$("#save").bind("click", function () {
    var icount1 = 0;
    var icount2 = 0;
    $('#Typj span').each(function () {
        var c = $(this).attr("class");
        if (c == 'am-icon-star') {
            icount1 = icount1 + 1;
        }
    });

    $('#Ggpj span').each(function () {
        var c = $(this).attr("class");
        if (c == 'am-icon-star') {
            icount2 = icount2 + 1;
        }
    });

   
    var parm = { CurriculumID: $("#hdCID").val(), StudentID: $("#hdSID").val(), Info: $("#doc-ta-1").val(), StarLevel: icount1, EffectStarLevel: icount2, PKID:$("#hdID").val() };
    $.ajax({
        type: "POST",
        url: ApiUrl + "/Student/AddCurriculumEvaluate",
        contentType: "application/json",
        data: JSON.stringify(parm),
        success: function (data, status) {
            if (status == "success") {
                alert("评价成功");
                $("#save").hide();
            }
        },
        error: function (e) {
            console.log('error');
        },
        complete: function () {

        }
    });

});



function dateformat(date) {
    var myDate = new Date(date);
    return myDate.getFullYear() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getDate();
}
