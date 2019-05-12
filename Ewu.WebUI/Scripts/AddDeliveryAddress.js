window.onload = function () {
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