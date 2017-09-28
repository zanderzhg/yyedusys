﻿

$(".save").bind("click", function () {

    if ($("#hdStudentID").val() == "")
    {
        alert("请选择要点评的学生");
        return false;
    }

    if ($("#hdCommentID").val() != "")//修改
    {
        var parm = { CommentID: $("#hdCommentID").val(), Info: $("#txtInfo").val() };
        $.ajax({
            type: "POST",
            url: "http://localhost:53262/api/Coash/UpdateCoachComment",
            contentType: "application/json",
            data: JSON.stringify(parm),
            success: function (data, status) {
                if (status == "success") {
                    alert("修改点评成功");                   
                }
            },
            error: function (e) {
                alert("修改点评信息失败，再操作一次吧");
            },
            complete: function () {

            }
        });
    }
    else
    {
        var parm = { CurriculumID: $("#hdCID").val(), StudentID: $("#hdStudentID").val(), Info: $("#txtInfo").val(), CoachID: $("#hdCoachID").val(), PKID: $("#hdPKID").val() };
        $.ajax({
            type: "POST",
            url: "http://localhost:53262/api/Coash/AddCoachComment",
            contentType: "application/json",
            data: JSON.stringify(parm),
            success: function (data, status) {
                if (status == "success") {
                    alert("添加点评信息成功");
                    console.log('ok');
                }
            },
            error: function (e) {
                alert("添加点评信息失败，再操作一次吧");
            },
            complete: function () {

            }
        });
    }

});

//取的当前学员信息
function LoadInfo() {
    var str = ""; 
    $.ajax({
        type: "get",
        url: "http://localhost:53262/api/Coach/GetCurriculumStudentByPKID/",
        dataType: "json",
        async: false,
        data: { PKID: $("#hdPKID").val() },
        //beforeSend: function () {
        //},
        success: function (data) {

            var str = ' <li class="am-dropdown-header">学员列表</li>';
            $.each(data, function (i, c) {
                str += '  <li><a href="#" data-id="'+c.StudentID+'">'+c.FullName+'</a></li>';
            });

            $("#cStudent").html(str);

            $("#cStudent li a").on("click", function () {
                $("#hdStudentID").val($(this).attr("data-id"));
             
                $.ajax({
                    type: "POST",
                    url: "http://localhost:53262/api/Coash/GetCoachCommentDetail",
                    contentType: "application/json",
                    data: {StudentID:$(this).attr("data-id"),CurriculumID:$("#hdCID").val()},
                    success: function (data) {
                        var c = data[0];
                        $("#txtInfo").text(c.Info);
                        $("#hdCommentID").val(c.CommentID);
                    },
                    error: function (e) {
                        console.log('error');
                    },
                    complete: function () {

                    }
                });
            }
            );
        }
    });


}

$(document).ready(function () {

    LoadInfo();

  
});

