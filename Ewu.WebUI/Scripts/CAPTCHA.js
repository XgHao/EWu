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

        if (phoCode != "") {
            //向后台发送处理数据    
            $.ajax({
                type: "POST", // 用POST方式传输    
                dataType: "text", // 数据格式:JSON    
                url: "/Register/validCode", // 目标地址    
                data: { "Code": phoCode, "Type": "Pho" },
                error: function (msg) {
                    alert(msg);
                },
                success: function (data) {
                    //验证成功
                    if (data == "\"OK\"") {
                        //验证成功后修改验证样式
                        $("#validPho").attr("class", "btn btn--fullwidth btn-success");
                        $("#validPho").val("验证通过");

                        //并且禁止修改手机号码
                        $("#PhoneNumber").attr("readonly", "true");
                        $("#validPho").attr("disabled", "true");

                        

                    }
                    //验证失败
                    else if (data == "\"Fail\"") {
                        //验证失败后修改验证样式
                        $("#validPho").attr("class", "btn btn--fullwidth btn-danger");
                        $("#validPho").val("验证失败，点击重试");
                    }
                    else if (data == "\"Error\"") {
                        alert("请先获取验证码！");
                    }
                    else {
                    }
                }
            });
        }
        else {
            //提示输入验证码
            $("#PhoCAPTCHA").attr("class", "alert-danger");
        }
    });

    //验证按钮-电子邮件
    $("#validEmail").click(function () {

        //获取输入框的验证码
        var emailCode = $("#EmailCAPTCHA").val();

        if (emailCode != "") {
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
                        $("#validEmail").attr("class", "btn btn--fullwidth btn-success");
                        $("#validEmail").val("验证通过");

                        //并且禁止修改手机号码
                        $("#Email").attr("readonly", "true");
                        $("#validEmail").attr("disabled", "true");

                        
                    }
                    //验证失败
                    else if (data == "\"Fail\"") {
                        //验证失败后修改验证样式
                        $("#validEmail").attr("class", "btn btn--fullwidth btn-danger");
                        $("#validEmail").val("验证失败，点击重试");
                    }
                    else if (data == "\"Error\"") {
                        //出错，验证码为空
                        alert("请先获取验证码！");
                    }
                    else {}
                }
            });
        }
        else {
            //提示输入验证码
            $("#EmailCAPTCHA").attr("class", "alert-danger");
        }
        
    });

    //监听验证码框
    $("#PhoCAPTCHA").change(function () {
        if ($("#PhoCAPTCHA").val() != "") {
            $("#PhoCAPTCHA").attr("class", "text_field");
        } else {
            $("#PhoCAPTCHA").attr("class", "alert-danger");
        }
    });

    //监听验证码框
    $("#EmailCAPTCHA").change(function () {
        if ($("#EmailCAPTCHA").val() != "") {
            $("#EmailCAPTCHA").attr("class", "text_field");
        } else {
            $("#EmailCAPTCHA").attr("class", "alert-danger");
        }
    });

    //监听用户名-修改后检查是否已存在
    $("#Name").change(function () {
        //获取用户名
        var username = $("#Name").val();

        if (username != "") {
            $.ajax({
                type: "POST",
                dataType: "text",
                url: "/Register/isExistUserName",
                data: { "Name": username },
                error: function (msg) {
                    alert(msg);
                },
                success: function (isExist) {
                    //存在
                    if (isExist == "\"YES\"") {
                        $("#NameIsExist").removeAttr("hidden");
                    }
                    //不存在
                    else if (isExist == "\"NO\"") {
                        $("#NameIsExist").attr("hidden", "true");
                    }
                    else { }
                }
            });
        }
    });

    //监听密码-修改后检查
    $("#Password").change(function () {
        //获取用户名
        var username = $("#Name").val();
        //获取密码
        var password = $("#Password").val();
        //获取电子邮件
        var email = $("#Email").val();

        if (username != "") {
            $.ajax({
                type: "POST",
                dataType: "text",
                url: "/Register/ValidCreateUser",
                data: { "Name": username, "PassWD": password, "Email": email },
                error: function (msg) {
                    alert(msg);
                },
                success: function (data) {
                    //可以创建
                    if (data == "\"OK\"") {
                        //检查是否双重验证通过
                        if ($("#validPho").val() == "验证通过" && $("#validEmail").val() == "验证通过") {
                            $("#Create").removeAttr("disabled");
                        }
                    }
                    //出错
                    else if (data == "\"Error\"") {
                    }
                    //条件不满足
                    else {
                        alert(data);
                    }
                }
            });
        }
    });


    //信息提示模块
    //添加提示框
    $("#Name").attr("data-container", "body").attr("data-toggle", "popover").attr("data-placement", "right").attr("data-content", "只能由数字和字母组成，长度为3-20字符");
    $("#Password").attr("data-container", "body").attr("data-toggle", "popover").attr("data-placement", "right").attr("data-content", "长度为6-16字符，大小写字母都需至少一个");
    $("#ConfirmedPassWd").attr("data-container", "body").attr("data-toggle", "popover").attr("data-placement", "right").attr("data-content", "再输入一次密码");

    $(function () {
        $("[data-toggle='popover']").popover();
    });
}