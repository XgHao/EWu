//设置收货地址
function SetDeliveryAddress(DeliveryAddressUID) {
    //获取当前登录人的角色
    var role = $("#CurrentRole").val();

    //获取当前交易订单号
    var LogDealUID = $("#CurrentLogDeal_DLogUID").val();

    //信息都不为空
    if ((role != null) && (role != "") && (LogDealUID != null) && (LogDealUID != "") && (DeliveryAddressUID != null) && (DeliveryAddressUID != "")) {
        $.ajax({
            type: "POST",
            dataType: "text",
            url: "/Deal/SetDeliveryAddress",
            data: {
                "CurrentRole": role,
                "DLogUID": LogDealUID,
                "DeliveryAddressUID": DeliveryAddressUID
            },
            error: function (msg) {
                alert("请求失败，请联系管理员(zxh957553851@gmail.com)。错误信息：" + msg);
            },
            success: function (result) {
                //成功
                if (result == "\"OK\"") {
                    //跳转到正在交易页面
                    window.location.href = "/Account/DealingLog";
                }
                else {
                    //请求失败,刷新页面
                    alert("未知错误，将刷新页面");
                    window.location.reload();
                }
            }
        });
    }
}

//删除收货地址
function DeleteDeliveryAddress(DeliveryAddressUID) {

    //信息都不为空
    if ((DeliveryAddressUID != null) && (DeliveryAddressUID != "")) {
        $.ajax({
            type: "POST",
            dataType: "text",
            url: "/Deal/DeleteDeliveryAddress",
            data: {
                "DeliveryAddressUID": DeliveryAddressUID
            },
            error: function (msg) {
                alert("请求失败，请联系管理员(zxh957553851@gmail.com)。错误信息：" + msg);
            },
            success: function (result) {
                //成功
                if (result == "\"OK\"") {
                    alert("删除成功！");
                    window.location.reload();
                }
                else {
                    //请求失败,刷新页面
                    alert("未知错误，将刷新页面");
                    window.location.reload();
                }
            }
        });
    }
}

//验证填写物流单号
function SetDeliveryNum() {
    //获取物流单号,订单号,当前角色
    var DeliveryNum = $("#DeliveryNum").val();
    var DLogUID = $("#DLogUID").val();
    var CurrentRole = $("#CurrentRole").val();

    $.ajax({
        type: "POST",
        dataType: "text",
        url: "/Deal/InquireDeliveryNum",
        data: {
            "CurrentRole": CurrentRole,
            "DLogUID": DLogUID,
            "DeliveryNum": DeliveryNum
        },
        error: function (msg) {
            alert("请求失败，请联系管理员(zxh957553851@gmail.com)。错误信息：" + msg);
        },
        success: function (result) {
            $("#ValidDeliveryNum").text(result);
            if (result == "\"查询成功\"") {
                //跳转到正在交易页面
                window.location.href = "/Account/DealingLog";
            }
        }
    });
}

window.onload = function () {
    //清空信息
    $("#userProvinceId").text("");
    $("#userCityId").text("");
    $("#userDistrictId").text("");
    $("#NewdeliveryAddress_MoreLocation").text("");

    //增加收货地址按钮
    $("#AddDeliveryAddress").click(function () {
        //获取地址
        var Line1 = $("#userProvinceId").val();         //省
        if (Line1 != null && Line1 != "") {
            var Line2 = $("#userCityId").val();         //市
            var Line3 = $("#userDistrictId").val();     //区、县
            var MoreLine = $("#NewdeliveryAddress_MoreLocation").val();
            if (Line2 != "" && Line3 != "") {
                if (MoreLine == null || MoreLine == "") {
                    alert("请填写完整信息");
                }
                else {
                    $("#NewdeliveryAddress_LocationProvince").val(Line1);
                    $("#NewdeliveryAddress_LocationCity").val(Line2);
                    $("#NewdeliveryAddress_LocationDistrict").val(Line3);
                    //触发提交按钮
                    $("#SubmitForm").click();
                }
            }
        } else {
            alert("请选择城市");
        }
    });

}