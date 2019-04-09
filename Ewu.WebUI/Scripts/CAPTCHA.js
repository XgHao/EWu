window.onload = function () {


    //短信验证码  
    var InterValObj; //timer变量，控制时间    
    var count = 60; //间隔函数，1秒执行    
    var curCount;//当前剩余秒数    
    var code = ""; //验证码    
    var codeLength = 4;//验证码长度   

    $("#setCode").click(function () {

        //获取输入的手机号码
        var phoNum = $("#PhoneNumber").val();
        curCount = count;

        // 产生随记验证码    
        for (var i = 0; i < codeLength; i++) {
            code += parseInt(Math.random() * 9).toString();
        }

        // 设置按钮显示效果，倒计时   
        $("#setCode").attr("disabled", "true");
        $("#setCode").val("已发送，" + curCount + "秒重试");
        InterValObj = setInterval(SetRemainTime, 1000);

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
                //前台给出提示语
                if (data) {
                    alert("发送成功，请注意查收！");
                }
                else {
                    alert("发送失败");
                    return false;
                }
            }
        });

    });

    //timer处理函数    
    function SetRemainTime() {
        console.log(curCount);
        if (curCount == 0) {
            clearInterval(InterValObj);// 停止计时器    
            $("#setCode").removeAttr("disabled");// 启用按钮    
            $("#setCode").val("重新发送验证码");
            code = ""; // 清除验证码。如果不清除，过时间后，输入收到的验证码依然有效    
        }
        else {
            curCount--;
            $("#setCode").val("已发送，" + curCount + "秒重试");
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