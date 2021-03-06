﻿
jQuery.support.cors = true;
function LoadMyCurriculumV() {


    var CurrentDate = "";

    var myCuDate = new Date();
    CurrentDate = myCuDate.getFullYear() + "-" + (myCuDate.getMonth() + 1) + "-" + myCuDate.getDate();

    $.ajax({
        type: "get",
        url: ApiUrl + "/Student/GetStudentGrowth/",
        dataType: "json",
        async: false,
        data: { VenueID: $("#hdVenueID").val(), StudentID: $("#hdStudentID").val(), FCType: 2 },
        //beforeSend: function () {
        //},
        success: function (data) {
         
            var str = "";
            var dataCu = data.data;

            $.each(dataCu, function (i, c) {
//
                str += "<li> ";
                str += '<div class="am-close" onclick="Del(' + c.GrowthID + ',2)" style="position:absolute;float:right;opacity:0.9;margin-left:92%;margin-top:-5px">×</div>';
             //   str += "<div class='am-thumbnail'>";
                str += " <video src='/" + c.FCUrl + "'+ controls='controls' preload='preload' style='margin-top:15px;width:300px;height:150px' > ";
                str += "</video> ";
            //    str += ' <p class="textAlign">' + c.Title + '</p>';
                str += "  </li> ";

            });

    
            $("#imgLis2").html(str);

        }
    });
}

function LoadMyCurriculumImg() {


    $.ajax({
        type: "get",
        url: ApiUrl + "/Student/GetStudentGrowth/",
        dataType: "json",
        async: false,
        data: { VenueID: $("#hdVenueID").val(), StudentID: $("#hdStudentID").val(), FCType: 1 },
        //beforeSend: function () {
        //},
        success: function (data) {
          
            var str = "";
            var dataCu = data.data;

            $.each(dataCu, function (i, c) {
                
                var Ftype = 1;
                if (Ftype == 1) {

                    str += '<li><div class="am-close am-close-spin" onclick="Del(' + c.GrowthID + ',1)" style="position:absolute;float:right;color:#FFFFFF;opacity:0.9;margin-left:27%">×</div>';
                   

                    str += '<img src="/' + c.FCUrl + '" alt="' + c.Title + '" class="am-thumbnail" />';
                    str += ' <p class="textAlign">' + c.Title + '</p>';
                    str += ' </li>';
                }
                else {
                    str += "<li> ";
                    str += "<div class='am-thumbnail'>";
                    str += " <video controls='controls' autoplay='autoplay'> ";
                    str += "<source src='" + c.FCUrl + "' type='video/ogg' /> </video> ";
                    str += ' <p class="textAlign">' + c.Title + '</p>';
                    str += "    </div></li> ";
                }

            });

            $("#imgList").html(str);
          
        }
    });
}




$("#btnDel").on("click", function () {
    var id = $("#hdID").val();
    $.ajax({
        type: "get",
        url: ApiUrl + "/Student/DelStudentGrowth/",
        contentType: "application/json",
        data: { ID:id ,State:2},
        success: function (data, status) {          
            if (status == "success") {
                if (data.code == 1001) {
                    alert('删除成功');
                    if ($("#hdType").val() == "1") {
                        LoadMyCurriculumImg();
                    }
                    else {
                        LoadMyCurriculumV();
                    }
                }
                else { alert('删除失败，再来一次吧.'); }
            }
        },
        error: function (e) {
            alert('删除失败，再来一次吧');
        },
        complete: function () {

        }
    });
});

function Del(id,type)
{

    $("#hdType").val(type);
    $("#hdID").val(id);
    $('#my-confirm').modal();
}

$("#UpFile").on("click", function ()
{
    $('#my-alert').modal();
    $("#ulLoaing").hide();
});






$('#btnMore a').bind('click', function () {
    //hdPageIndex
    var iPindex = parseInt($("#hdPageIndex").val()) + 1;
    $("#hdPageIndex").val(iPindex);

    var datatype = $(".current").attr("data-type");
    if (datatype == "1") {
        LoadMyCurriculumImg();
    }
    else {
        LoadMyCurriculumV();
    }
    //hdPageSize
});

$('.order li a').bind('click', function () {

    $('.order li a').removeClass("current");
    $(this).addClass("current");
    $("#hdPageIndex").val(1);
    $("#imgList").html('');
    $("#imgList2").html('');
    var datatype = $(".current").attr("data-type");
    if (datatype == "1") {
        LoadMyCurriculumImg();
    }
    else {
        LoadMyCurriculumV();
    }
});



//读取图片实例,并上传到服务器
var fileBox = document.getElementById('file');

function uploadClick() {
    var fileList = fileBox.files;
    for (var i = 0; i < fileList.length; i++) {
        var file = fileList[i];
        //图片类型验证第二种方式
        uploadFile(file);
        //if (/image\/\w+/.test(file.type))
        //{ }
        //else
        //{ console.log(file.name + ':不是图片'); }
    }
}

//关键代码上传到服务器
function uploadFile(file) {

    var ext = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();
  
    if ($("input[name='docInlineRadio']:checked").val() == 1) {

        var strExt = ".jpg.png.jpeg.bmp";
        if (strExt.indexOf(ext) < 0) {
            alert('只能上传' + strExt + "格式的图片文件");
            return false;
        }
    }
    else {
        var strExt = ".mp4";
        if (strExt.indexOf(ext) < 0) {
            alert('只能上传' + strExt + "格式的视频文件");
            return false;
        }
    }

    var fileSize = 0;
    var isIE = /msie/i.test(navigator.userAgent) && !window.opera;
    if (isIE) {
        var filePath = $(file).val();
        var fileSystem = new ActiveXObject("Scripting.FileSystemObject");
        var file = fileSystem.GetFile(filePath);
        fileSize = file.Size;
    } else {
        fileSize = file.size;
    }
    fileSize = Math.round(fileSize / 1024 * 100) / 100; //单位为KB
    if ($("input[name='docInlineRadio']:checked").val() == 1) {

        if (fileSize >= 300) {
            alert("照片最大尺寸为300KB，请重新上传!");
            return false;
        }
    }
    else {
        if (fileSize >=6000) {
            alert("视频最大尺寸为6MB，请重新上传!");
            return false;
        }
    }

    var reader = new FileReader();
    reader.readAsArrayBuffer(file);
    reader.onload = function () {
        var blob = new Blob([reader.result]);
        //提交到服务器
        var fd = new FormData();
        fd.append('file', blob);
        fd.append('utype', 1);
       
        fd.append('extention', ext);
        fd.append('maxsize', 1024 * 1024 * 40);//Asp.net 默认一次上传最大4MB
        fd.append('isClip', -1);
        var xhr = new XMLHttpRequest();
        xhr.open('post', '../data/UpLoad.ashx', true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var data = eval('(' + xhr.responseText + ')');
                if (data.success == 1) {
                    var parm = { Title: $("#txtTitle").val(), FCType: $("input[name='docInlineRadio']:checked").val(), FCUrl: data.msg, StudentID: $("#hdStudentID").val(), FCState: 1, VenueID: $("#hdVenueID").val() };
                    $.ajax({
                        type: "POST",
                        url: ApiUrl + "/Student/AddStudentGrowth",
                        contentType: "application/json",
                        data: JSON.stringify(parm),
                        success: function (data, status) {
                            if (status == "success") {
                                if (data.Code == 1001) {
                                    alert('添加成功');
                                    $("#my-alert").modal('close');

                                    if ($("input[name='docInlineRadio']:checked").val() == 1)
                                    {
                                        LoadMyCurriculumImg();
                                    }
                                    else
                                    {
                                        LoadMyCurriculumV();
                                    }
                                }
                                else { alert('添加失败，再来一次吧'); }
                            }
                        },
                        error: function (e) {
                            alert('添加出错了，再来一次吧');
                        },
                        complete: function () {

                        }
                    });
                    //上传成功

                } else {
                    alert('上传失败：' + data.msg + ",重新操作下试试吧");
                }


            }
        }
        //开始发送
        xhr.send(fd);
    }
}

$('#btnSave').on('click', function () {

    if ($("#txtTitle").val() == "") {
        alert('标题不能为空');
        $("#txtTitle").focus();
        return false;
    }

    if ($("#file").val() == "") {
        alert('请选择要上传的文件');
        $("#file").focus();
        return false;
    }
    $("#ulLoaing").show();
    uploadClick();
});
