﻿
function bind_data() {

    columns_data = [
        { "data": "CoachID" },
        {
            "data": "HeadUrl",
            "render": function (data, type, full, meta) {
                var edithtml = "<img src='" + data + "' />"
                return edithtml;
            }
        },
        { "data": "FullName" },
        { "data": "UserName" },
        { "data": "NickName" },
        { "data": "Wage" },
        { "data": "Price" },
        { "data": "Address" },
        {
            "data": "Sex",
            "render": function (data, type, full, meta) {
                if (data == 1) {
                    var edithtml = '<span>男</span>';
                } else {
                    var edithtml = '<span>女</span>';
                }
                return edithtml;
            }
        },
        {
            "data": "State",
            "render": function (data, type, full, meta) {
                if (data == 0) {
                    var edithtml = '<span class="btn btn-xs btn-warning">未审核</span>';
                } else if (data == 1) {
                    var edithtml = '<span class="btn btn-xs btn-success">审核成功</span>';
                } else if (data == 2) {
                    var edithtml = '<span class="btn btn-xs btn-danger">审核失败</span>';
                } else {
                    var edithtml = '<span class="btn btn-xs btn-warning">未审核</span>';
                }
                return edithtml;
            }
        },
        { "data": "Introduce" },
        {
            "data": "State",
            "render": function (data, type, full, meta) {

                if (data == 1) {
                    //设置课时工资 课时价格 等级
                    var edithtml = '<button  class="btn btn-xs btn-success" onclick="return showcoachwage(' + full['CoachID'] + ');"><i class="fa fa-edit"> 设置课时工资 </i></button>&nbsp;&nbsp;';
                    edithtml += '<button  class="btn btn-xs btn-warning" onclick="return showcoachprice(' + full['CoachID'] + ');"><i class="fa fa-edit"> 设置课时费用 </i></button>&nbsp;&nbsp;';

                } else {
                    var edithtml = '<button class="btn btn-xs btn-success" onclick="return setcoachaudited(' + full['CoachID'] + ',' + full['VenueID'] + ');"><i class="fa fa-edit"> 审核通过 </i></button>&nbsp;&nbsp;';
                    edithtml += '<button class="btn btn-xs btn-danger" onclick="return deletecoach(' + full['CoachID'] + ',' + full['VenueID'] + ');"><i class="fa fa-edit"> 审核不通过 </i></button>&nbsp;&nbsp;';
                }

                edithtml += '<a href="/Coach/Details/' + full['CoachID'] + '" class="btn btn-xs btn-warning"><i class="fa fa-edit"> 查看 </i></a>&nbsp;&nbsp;';
                return edithtml;
            }
        },
    ];

    bindTable('coachtable', 'coachform', 'api/coach/Page4Venue', columns_data, false);
}

//审核通过
function setcoachaudited(coachId, venueId) {

    var flag = confirm('确定审核通过?');
    if (flag) {

        $.ajax({
            type: "post",
            url: ApiUrl + "api/coach/setcoachaudited",
            contentType: 'application/json',
            data: JSON.stringify({ CoachID: coachId, VenueID: venueId }),
            success: function (data, status) {
                if (status == "success") {
                    console.log(data);
                    if (data.Error) {
                        alert(data.Msg);
                    } else {
                        bind_data();
                        alert('设置成功');
                    }
                }
            },
            error: function (e) {
            },
            complete: function () {

            }
        });
    } else {

    }
}

//审核不通过
function deletecoach(coachId, venueId) {

    var flag = confirm('确定删除?');
    if (flag) {

        $.ajax({
            type: "post",
            url: ApiUrl + "api/coach/delete",
            contentType: 'application/json',
            data: JSON.stringify({ CoachID: coachId, VenueID: venueId }),
            success: function (data, status) {
                if (status == "success") {
                    console.log(data);
                    if (data.error) {
                        bind_data();
                        alert('设置成功');
                    } else {
                        alert(data.Msg);
                    }
                }
            },
            error: function (e) {
            },
            complete: function () {

            }
        });
    } else {

    }
}

//展示教练信息
function showcoachwage(coachId) {

    $('#setcoachwagemodal').on('show.bs.modal', function (e) {

        var coachid = e.relatedTarget.coachid;

        $.ajax({
            type: "get",
            url: ApiUrl + "api/coach/get/" + coachid,
            data: {},
            success: function (data, status) {
                if (status == "success") {
                    console.log(data);
                    if (data.Info == null) {
                        alert('教练未绑定');
                    } else {
                        $("[name='coachname']").html(data.Info.FullName);
                        $("[name='VenueID']").val(data.Info.VenueID);
                        $("[name='CoachID']").val(data.Info.CoachID);
                        $("#Wage").val(data.Info.Wage == null ? '0' : data.Info.Wage);
                    }
                }
            },
            error: function (e) {
            },
            complete: function () {

            }
        });

    })

    $('#setcoachwagemodal').modal({
        show: true
    }, {
        coachid: coachId
    });
}

function showcoachprice(coachId) {

    $('#setcoachpricemodal').on('show.bs.modal', function (e) {

        var coachid = e.relatedTarget.coachid;

        $.ajax({
            type: "get",
            url: ApiUrl + "api/coach/get/" + coachid,
            data: {},
            success: function (data, status) {
                if (status == "success") {
                    console.log(data);
                    if (data.Info == null) {
                        alert('教练未绑定');
                    } else {
                        $("[name='coachname']").html(data.Info.FullName);
                        $("[name='VenueID']").val(data.Info.VenueID);
                        $("[name='CoachID']").val(data.Info.CoachID);
                        $("#Price").val(data.Info.Price == null ? '0' : data.Info.Price);
                    }
                }
            },
            error: function (e) {
            },
            complete: function () {

            }
        });

    })

    $('#setcoachpricemodal').modal({
        show: true
    }, {
        coachid: coachId
    });
}

function setcoachwage() {

    console.log($('#setcoachwageform').serializeJSON());
    //添加
    $.ajax({
        type: "POST",
        url: ApiUrl + "api/coach/SetCoachWages",
        contentType: "application/json",
        data: JSON.stringify($('#setcoachwageform').serializeJSON()),
        success: function (data, status) {
            if (status == "success") {
                if (data.Error) {
                    alert(data.Msg);
                } else {
                    bind_data();
                    alert('设置工资成功');
                }
            }
        },
        error: function (e) {
            console.log('error');
        },
        complete: function () {

        }
    });

}

function setcoachprice() {

    //添加
    $.ajax({
        type: "POST",
        url: ApiUrl + "api/coach/SetCoachPrice",
        contentType: "application/json",
        data: JSON.stringify($('#setcoachpriceform').serializeJSON()),
        success: function (data, status) {
            if (status == "success") {
                if (data.Error) {
                    alert(data.Msg);
                } else {
                    bind_data();
                    alert('设置课时费成功');
                }
            }
        },
        error: function (e) {
            console.log('error');
        },
        complete: function () {

        }
    });

}

function showdetail(coachId) {

    var coachid = $("#CoachID").val();

    $.ajax({
        type: "get",
        url: ApiUrl + "api/coach/get/" + coachid,
        data: {},
        success: function (data, status) {
            if (status == "success") {
                console.log(data);
                if (data.Info == null) {
                    alert('教练未绑定');
                } else {
                    $("#CoachID").html(data.Info.CoachID);
                    $("#FullName").html(data.Info.FullName);
                    $("#UserName").html(data.Info.UserName);
                    $("#HeadUrl").val(data.Info.HeadUrl);
                    $("#CardPositiveUrl").val(data.Info.CardPositiveUrl);
                    $("#CardReverseUrl").val(data.Info.CardReverseUrl);
                    if (data.Info.State == 0) {
                        $("#State").html("未审核");
                    } else if (data.Info.State == 1) {
                        $("#State").html("审核成功");
                    } else if (data.Info.State == 2) {
                        $("#State").html("审核失败");
                    } else {
                        $("#State").html("未审核");
                    }

                    $("#NickName").html(data.Info.NickName);
                    $("#Address").html(data.Info.Address);
                    $("#Introduce").html(data.Info.Introduce);
                    $("#Sex").html(data.Info.Sex == 1 ? "男" : "女");
                    $("#Price").html(data.Info.Price == null ? '0' : data.Info.Price);
                    $("#Wage").html(data.Info.Wage == null ? '0' : data.Info.Wage);
                }
            }
        },
        error: function (e) {
        },
        complete: function () {

        }
    });
}

$("#import").on('click', function () {

    var fd = new FormData();
    fd.append("venueID", $("#VenueID").val());
    fd.append("file", $("#file").get(0).files[0]);
    $.ajax({
        url: ApiUrl + "api/Coach/Import",
        type: "POST",
        processData: false,
        contentType: false,
        data: fd,
        success: function (data, status) {
            if (status == "success") {
                if (data.Error) {
                    alert(data.Msg);
                } else {
                    alert("导入成功");
                }
            } else {
                alert("导入异常");
            }
        }
    });
});
