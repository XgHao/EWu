window.onload = function () {


    //短信验证码  
    var InterValObjPho; //timer变量，控制时间    
    var InterValObjPho2; //timer变量，控制时间    
    var InterValObjEmail; //timer变量，控制时间    
    var InterValObjEmail2; //timer变量，控制时间    
    var phocurCount;//当前剩余秒数-手机号码
    var emailcurCount;  //当前剩余秒数-电子邮件
    var phocode = ""; //手机号码验证码    
    var emailcode = ""; //邮件验证码
    var codeLength = 4;//验证码长度   
    var errorMsg = "";  //错误信息

    //先禁用验证按钮
    //$("#validPho").attr("disabled", "true");
    //$("#validEmail").attr("disabled", "true");

    //手机号码验证
    $("#setPhoCode").click(function () {

        //获取输入的手机号码
        var phoNum = $("#PhoneNumber").val();

        // 产生随记验证码    
        for (var i = 0; i < codeLength; i++) {
            phocode += parseInt(Math.random() * 9).toString();
        }

        // 设置按钮显示效果，倒计时   
        $("#setPhoCode").attr("disabled", "true");

         //向后台发送处理数据    
        $.ajax({
            type: "POST", // 用POST方式传输    
            dataType: "text", // 数据格式:JSON    
            url: "/Register/GetPhoCode", // 目标地址    
            data: { "Code": phocode, "phoNum": phoNum },
            error: function (msg) {
                alert(msg);
            },
            success: function (data) {
                var dataStr = JSON.stringify(data);
                //发送成功，定时60秒
                if (data == "\"OK\"") {
                    //$("#validPho").removeAttr("disabled");// 启用按钮 
                    phocurCount = 60;
                    InterValObjPho = setInterval(SetRemainTimePho, 1000);
                }
                //发送失败时，定时10秒
                else {
                    errorMsg = "发送失败";
                    if (dataStr.length >= 6) {
                        errorMsg = dataStr.substring(3, dataStr.length - 3);
                    }
                    phocurCount = 10;
                    InterValObjPho2 = setInterval(SetRemainTimePho2, 1000);
                }
            }
        });
    });

    //邮箱验证
    $("#setEmailCode").click(function () {

        //获取输入的电子邮件
        var email = $("#Email").val();

        // 产生随记验证码    
        for (var i = 0; i < codeLength; i++) {
            emailcode += parseInt(Math.random() * 9).toString();
        }

        // 设置按钮显示效果，倒计时   
        $("#setEmailCode").attr("disabled", "true");

        //向后台发送处理数据    
        $.ajax({
            type: "POST", // 用POST方式传输    
            dataType: "text", // 数据格式:JSON    
            url: "/Register/GetEmailCode", // 目标地址    
            data: { "Code": emailcode, "Email": email },
            error: function (msg) {
                alert(msg);
            },
            success: function (data) {
                
                //发送成功，定时60秒
                if (data == "\"OK\"") {
                    emailcurCount = 60;
                    InterValObjEmail = setInterval(SetRemainTimeEmail, 1000);
                }
                //发送失败时，定时10秒
                else {
                    errorMsg = data;
                    emailcurCount = 10;
                    InterValObjEmail2 = setInterval(SetRemainTimeEmail2, 1000);
                }
            }
        });
    });

    //timer处理函数pho
    function SetRemainTimePho() {
        //console.log("成功" + phocurCount);
        if (phocurCount == 0) {
            clearInterval(InterValObjPho);// 停止计时器    
            $("#setPhoCode").removeAttr("disabled");// 启用按钮    
            $("#setPhoCode").val("重新发送验证码");
            phocode = ""; // 清除验证码。如果不清除，过时间后，输入收到的验证码依然有效    
        }
        else {
            phocurCount--;
            $("#setPhoCode").val("已发送，" + phocurCount + "秒后重试");
        }
    }

    //timer处理函数pho2 
    function SetRemainTimePho2() {
        //console.log("失败" + curCount);
        if (phocurCount == 0) {
            clearInterval(InterValObjPho2);// 停止计时器    
            $("#setPhoCode").removeAttr("disabled");// 启用按钮    
            $("#setPhoCode").val("重新发送验证码");
            phocode = ""; // 清除验证码。如果不清除，过时间后，输入收到的验证码依然有效    
        }
        else {
            phocurCount--;
            $("#setPhoCode").val(errorMsg + "，" + phocurCount + "秒后重试");
        }
    }

    //timer处理函数email 
    function SetRemainTimeEmail() {
        //console.log("失败" + emailcurCount);
        if (emailcurCount == 0) {
            clearInterval(InterValObjEmail);// 停止计时器    
            $("#setEmailCode").removeAttr("disabled");// 启用按钮    
            $("#setEmailCode").val("重新发送验证码");
            emailcode = ""; // 清除验证码。如果不清除，过时间后，输入收到的验证码依然有效    
        }
        else {
            emailcurCount--;
            $("#setEmailCode").val("已发送，" + emailcurCount + "秒后重试");
        }
    }

    //timer处理函数email
    function SetRemainTimeEmail2() {
        //console.log("失败" + emailcurCount);
        if (emailcurCount == 0) {
            clearInterval(InterValObjEmail2);// 停止计时器    
            $("#setEmailCode").removeAttr("disabled");// 启用按钮    
            $("#setEmailCode").val("重新发送验证码");
            emailcode = ""; // 清除验证码。如果不清除，过时间后，输入收到的验证码依然有效    
        }
        else {
            emailcurCount--;
            $("#setEmailCode").val(errorMsg + "," + emailcurCount + "秒后重试");
        }
    }


    //验证按钮-手机号码
    $("#validPho").click(function () {

        //获取输入框的验证码
        var phoCode = $("#PhoCAPTCHA").val();
        
        //向后台发送处理数据    
        $.ajax({
            type: "POST", // 用POST方式传输    
            dataType: "text", // 数据格式:JSON    
            url: "/Register/validCode", // 目标地址    
            data: { "Code": phoCode, "Type": "Pho"},
            error: function (msg) {
                alert(msg);
            },
            success: function (data) {
                //验证成功
                if (data == "\"OK\"") {
                    //验证成功后修改验证样式
                    $("validPho").attr("class", "btn btn--fullwidth btn-success");

                    //并且禁止修改手机号码
                    $("#PhoneNumber").attr("readonly", "true");

                    console.log(data + "+验证成功");
                }
                //验证失败
                else if (data == "\"Fail\"") {
                    //验证失败
                    console.log(data + "+验证失败");
                }
                else if (data == "\"Error\"") {
                    //出错，验证码为空
                    console.log(data + "+验证为空");
                }
            }
        });
    });

    //验证按钮-电子邮件
    $("#validEmail").click(function () {

        //获取输入框的验证码
        var emailCode = $("#EmailCAPTCHA").val();

        //向后台发送处理数据    
        $.ajax({
            type: "POST", // 用POST方式传输    
            dataType: "text", // 数据格式:JSON    
            url: "/Register/validCode", // 目标地址    
            data: { "Code": emailCode, "Type": "Email" },
            error: function (msg) {
                alert(msg);
            },
            success: function (data) {
                //验证成功
                if (data == "\"OK\"") {
                    //验证成功后修改验证样式
                    $("validEmail").attr("class", "btn btn--fullwidth btn-success");

                    //并且禁止修改电子邮件
                    $("#Email").attr("readonly", "true");

                    alert(data + "+验证成功");
                }
                //验证失败
                else if (data == "\"Fail\"") {
                    //验证失败
                    alert(data + "+验证失败");
                }
                else if (data == "\"Error\"") {
                    //出错，验证码为空
                    alert(data + "+验证为空");
                }
            }
        });
    });


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