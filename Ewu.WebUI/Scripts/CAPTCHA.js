window.onload = function () {


    //短信验证码  
    var InterValObj; //timer变量，控制时间    
    var InterValObj2; //timer变量，控制时间    
    var curCount;//当前剩余秒数  
    var code = ""; //验证码    
    var codeLength = 4;//验证码长度   
    var errorMsg = "";  //错误信息

    $("#setCode").click(function () {

        //获取输入的手机号码
        var phoNum = $("#PhoneNumber").val();

        // 产生随记验证码    
        for (var i = 0; i < codeLength; i++) {
            code += parseInt(Math.random() * 9).toString();
        }

        // 设置按钮显示效果，倒计时   
        $("#setCode").attr("disabled", "true");

         //向后台发送处理数据    
        $.ajax({
            type: "POST", // 用POST方式传输    
            dataType: "text", // 数据格式:JSON    
            url: "/Register/GetCode", // 目标地址    
            data: { "Code": code, "phoNum": phoNum },
            error: function (msg) {
                alert(msg);
            },
            success: function (data) {
                console.log(data);
                //发送成功，定时60秒
                if (data == "\"OK\"") {
                    curCount = 60;
                    InterValObj = setInterval(SetRemainTime, 1000);
                }
                //发送失败时，定时10秒
                else {
                    errorMsg = data;
                    curCount = 10;
                    InterValObj2 = setInterval(SetRemainTime2, 1000);
                }
            }
        });

    });

    //timer处理函数    
    function SetRemainTime() {
        console.log("成功" + curCount);
        if (curCount == 0) {
            clearInterval(InterValObj);// 停止计时器    
            $("#setCode").removeAttr("disabled");// 启用按钮    
            $("#setCode").val("重新发送验证码");
            code = ""; // 清除验证码。如果不清除，过时间后，输入收到的验证码依然有效    
        }
        else {
            curCount--;
            $("#setCode").val("已发送，" + curCount + "秒后重试");
        }
    }

    //timer处理函数2  
    function SetRemainTime2() {
        console.log("失败" + curCount);
        if (curCount == 0) {
            clearInterval(InterValObj2);// 停止计时器    
            $("#setCode").removeAttr("disabled");// 启用按钮    
            $("#setCode").val("重新发送验证码");
            code = ""; // 清除验证码。如果不清除，过时间后，输入收到的验证码依然有效    
        }
        else {
            curCount--;
            $("#setCode").val(errorMsg + "," + curCount + "秒后重试");
        }
    }

    //提交注册按钮
    //$("#submit").click(function () {
    //    var CheckCode = $("#codename").val();
    //    // 向后台发送处理数据    
    //    $.ajax({
    //        url: "/Register/CheckCode",
    //        data: { "CheckCode": CheckCode },
    //        type: "POST",
    //        dataType: "text",
    //        success: function (data) {
    //            if (data == "true") {
    //                $("#codenameTip").html("<font color='#339933'>√</font>");
    //            } else {
    //                $("#codenameTip").html("<font color='red'>× 短信验证码有误，请核实后重新填写</font>");
    //                return;
    //            }
    //        }
    //    });
    //});
}