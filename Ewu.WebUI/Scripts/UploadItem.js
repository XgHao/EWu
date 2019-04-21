$(function () {
    $("[data-toggle='popover']").popover();
});

window.onload = function () {
    //验证提交按钮
    $("#VaildForm").click(function () {
        alert("Your Click VaildForm");
        //获取名称
        var Name = $("#TreasureName").val();
        //获取地址
        var Line1 = $("#userProvinceId").val();     //省
        //获取描述
        var DetailInfo = $("#DetailContent").val();

        //信息填写完成后才可提交
        if (Line1 != null && Line1 != "" && Name != "" && DetailInfo != "") {
            var Line2 = $("#userCityId").val();         //市
            var Line3 = $("#userDistrictId").val();     //区、县
            if (Line2 != "" && Line3 != "") {
                var location = Line1 + Line2 + Line3;
                alert(location);
                $("#Location").text(location);
            }

            //触发提交按钮
            $("#SubmitForm").click();
        }
        else {
            alert("请确保信息填写完成");
        }
    });

    //监听名称
    $("#TreasureName").change(function () {
        var TName = $("#TreasureName").val();
        if (TName == "") {
            $("#TreasureNameVaild").removeAttr("hidden");
            $("#TreasureNameVaild").text("请填写名称");
        }
        else if (TName.length >= 30) {
            $("#TreasureNameVaild").removeAttr("hidden");
            $("#TreasureNameVaild").text("名称长度超限了");
        }
        else {
            $("#TreasureNameVaild").attr("hidden", "true");
            $("#TreasureNameVaild").text("验证通过");
        }
    });

    //监听名称
    $("#DetailContent").change(function () {
        var TName = $("#DetailContent").val();
        if (TName == "") {
            $("#DetailContentVaild").removeAttr("hidden");
            $("#DetailContentVaild").text("请填写名称");
        }
        else if (TName.length >= 200) {
            $("#DetailContentVaild").removeAttr("hidden");
            $("#DetailContentVaild").text("请限制长度在200个字符以内");
        }
        else {
            $("#DetailContentVaild").attr("hidden", "true");
            $("#DetailContentVaild").text("验证通过");
        }
    });
}

