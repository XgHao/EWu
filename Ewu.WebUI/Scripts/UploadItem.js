window.onload = function () {
    $("#SubmitForm").click(function () {
        alert("Your Click It");
        //获取地址
        var Line1 = $("#userProvinceId").val();
        var Line2 = $("#userCityId").val();
        var Line3 = $("#userDistrictId").val();
        if ((Line1 != null && Line2 != null && Line3 != null) && (Line1 != "" && Line2 != "" && Line3 != "")) {
            var location = Line1 + Line2 + Line3;
            alert(location);
            $("#Location").text(location);
        }
    });
}