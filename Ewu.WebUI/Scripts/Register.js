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
                alert("请求失败，请联系管理员");
            },
            success: function (data) {
                var dataStr = JSON.stringify(data);
                //发送成功，定时60秒
                if (data == "\"OK\"" || data == "OK") {
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
                alert("请求失败，请联系管理员" + msg);
            },
            success: function (data) {
                
                //发送成功，定时60秒
                if (data == "\"OK\"" || data == "OK") {
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
                    alert("请求失败，请联系管理员(zxh957553851@gmail.com)。错误信息：" + msg);
                },
                success: function (data) {
                    //验证成功
                    if (data == "\"OK\"" || data == "OK") {
                        //验证成功后修改验证样式
                        $("#validPho").attr("class", "btn btn--fullwidth btn-success");
                        $("#validPho").val("验证通过");

                        //并且禁止修改手机号码
                        $("#PhoneNumber").attr("readonly", "true");
                        $("#validPho").attr("disabled", "true");
                        $("#setPhoCode").attr("disabled", "true");
                        

                    }
                    //验证失败
                    else if (data == "\"Fail\"" || data == "Fail") {
                        //验证失败后修改验证样式
                        $("#validPho").attr("class", "btn btn--fullwidth btn-danger");
                        $("#validPho").val("验证失败，点击重试");
                    }
                    else if (data == "\"Error\"" || data == "Error") {
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
                    alert("请求失败，请联系管理员");
                },
                success: function (data) {
                    //验证成功
                    if (data == "\"OK\"" || data == "OK") {
                        //验证成功后修改验证样式
                        $("#validEmail").attr("class", "btn btn--fullwidth btn-success");
                        $("#validEmail").val("验证通过");

                        //并且禁止修改邮箱地址
                        $("#Email").attr("readonly", "true");
                        $("#validEmail").attr("disabled", "true");
                        $("#setEmailCode").attr("disabled", "true");

                        
                    }
                    //验证失败
                    else if (data == "\"Fail\"" || data == "Fail") {
                        //验证失败后修改验证样式
                        $("#validEmail").attr("class", "btn btn--fullwidth btn-danger");
                        $("#validEmail").val("验证失败，点击重试");
                    }
                    else if (data == "\"Error\"" || data == "Error") {
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

    //监听手机验证码框
    $("#PhoCAPTCHA").change(function () {
        if ($("#PhoCAPTCHA").val() != "") {
            $("#PhoCAPTCHA").attr("class", "text_field");
        } else {
            $("#PhoCAPTCHA").attr("class", "alert-danger");
        }
    });

    //监听邮箱验证码框
    $("#EmailCAPTCHA").change(function () {
        if ($("#EmailCAPTCHA").val() != "") {
            $("#EmailCAPTCHA").attr("class", "text_field");
        } else {
            $("#EmailCAPTCHA").attr("class", "alert-danger");
        }
    });

    //监听手机号码-是否已经注册
    $("#PhoneNumber").change(function () {
        var PhoneNumber = $("#PhoneNumber").val();
        if (PhoneNumber != "") {
            $.ajax({
                type: "POST",
                dataType: "text",
                url: "/Register/isExistPhoNum",
                data: { "PhoNum": PhoneNumber },
                error: function (msg) {
                    alert("请求失败，请联系管理员" + msg);
                },
                success: function (data) {
                    //存在YES
                    if (data == "\"YES\"" || data == "YES") {
                        alert("该手机已被注册，换一个吧");
                        $("#PhoneNumber").val("");
                        $("#setPhoCode").attr("disabled");
                    } else {
                        $("#setPhoCode").removeAttr("disabled");
                    }
                }
            });
        }
    });

    //监听邮箱-是否已经注册
    $("#Email").change(function () {
        var email = $("#Email").val();
        if (email != "") {
            $.ajax({
                type: "POST",
                dataType: "text",
                url: "/Register/isExistEmail",
                data: { "Email": email },
                error: function (msg) {
                    alert("请求失败，请联系管理员" + msg);
                },
                success: function (data) {
                    //存在YES
                    if (data == "\"YES\"" || data == "YES") {
                        alert("该邮箱已被注册，换一个吧");
                        $("#Email").val("");
                        $("#setEmailCode").attr("disabled");
                    } else {
                        $("#setEmailCode").removeAttr("disabled");
                    }
                }
            });
        }
    });

    //监听用户名-修改后检查是否已存在
    $("#Name").change(function () {
        //先检查是否验证通过
        IsValid();

        //获取用户名
        var username = $("#Name").val();

        if (username != "") {
            $.ajax({
                type: "POST",
                dataType: "text",
                url: "/Register/isExistUserName",
                data: { "Name": username },
                error: function (msg) {
                    alert("请求失败，请联系管理员。错误信息：" + msg);
                },
                success: function (isExist) {
                    //存在
                    if (isExist == "\"YES\"" || isExist == "YES") {
                        $("#NameIsExistValid").removeAttr("hidden");
                        $("#NameIsExistValid").text("该用户名已存在");
                    }
                    //不存在,可行
                    else if (isExist == "\"NO\"" || isExist == "NO") {
                        $("#NameIsExistValid").attr("hidden", "true");
                        $("#NameIsExistValid").text("验证通过");
                    }
                    //规则不通过
                    else {
                        var dataStr = JSON.stringify(isExist);
                        if (dataStr.length >= 6) {
                            var errorList = dataStr.substring(3, dataStr.length - 3);
                        }
                        $("#NameIsExistValid").removeAttr("hidden");
                        $("#NameIsExistValid").text(errorList);
                    }
                }
            });
        } else {
            $("#NameIsExistValid").removeAttr("hidden");
            $("#NameIsExistValid").text("请输入用户名");
        }

        //检测条件是否都满足
        CanCreate();
    });

    //监听密码-修改后检查
    $("#Password").change(function () {
        //先检查是否验证通过
        IsValid();

        //获取用户名
        var username = $("#Name").val();
        //获取密码
        var password = $("#Password").val();

        //获取电子邮件
        var email = $("#Email").val();

        $.ajax({
            type: "POST",
            dataType: "text",
            url: "/Register/ValidCreateUser",
            data: { "Name": username, "PassWD": password, "Email": email },
            error: function (msg) {
                alert("请求失败，请联系管理员");
            },
            success: function (data) {
                //可以创建
                if (data == "\"OK\"" || data == "OK") {
                    $("#PassWdIsExistValid").attr("hidden", "true");
                    $("#PassWdIsExistValid").text("验证通过");
                }
                //出错
                else if (data == "\"Error\"" || data == "Error") {
                }
                //条件不满足
                else {
                    var dataStr = JSON.stringify(data);
                    if (dataStr.length >= 6) {
                        var errorList = dataStr.substring(3, dataStr.length - 3);
                    }
                    $("#PassWdIsExistValid").removeAttr("hidden");
                    $("#PassWdIsExistValid").text(errorList);
                }
            }
        });

        //检测条件是否都满足
        CanCreate();
    });

    //监听重复输入密码
    $("#ConfirmedPassWd").change(function () {
        //先检查是否验证通过
        IsValid();

        var passwd = $("#Password").val();
        var repasswd = $("#ConfirmedPassWd").val();
        if (repasswd != "") {
            //验证通过
            if (repasswd == passwd) {
                $("#RepeatPassWd").attr("hidden", "true");
                $("#RepeatPassWd").text("验证通过");
            }
            else {
                $("#RepeatPassWd").removeAttr("hidden");
                $("#RepeatPassWd").text("两次密码不一致");
            }
        }
        else {
            $("#RepeatPassWd").removeAttr("hidden");
            $("#RepeatPassWd").text("请再次输入你的密码");
        }

        //检测条件是否都满足
        CanCreate();
    });

    //监听身份证文件
    $("#IdCard").change(function () {
        var idcard = $("#IdCard").val();    //文件路径
        var imgname = idcard.substring(idcard.lastIndexOf("\\") + 1);   //文件名
        var size = $("#IdCard")[0].files[0].size;   //文件大小
        var suffix = idcard.substring(idcard.lastIndexOf(".")); //后缀名
        //先判断后缀名
        if (suffix.toLowerCase() == ".png" || suffix.toLowerCase() == ".jpg") {
            //文件最大为2097152个字节(2MB)
            if (0 < size && size <= 2097152) {
                $("#upload-file-info").html(imgname);
                $("#validIdCardImg").text("");
            }
            else {
                $("#IdCard").val("");
                $("#upload-file-info").html("");
                $("#validIdCardImg").text("文件大小请保持在2MB以内");
            }
        } else {
            $("#IdCard").val("");
            $("#upload-file-info").html("");
            $("#validIdCardImg").text("请上传JPG或PNG图片文件");
        }

        //检测条件是否都满足
        CanCreate();
    });

    //验证方法-在填写账号信息前，要先验证手机号码和电子邮箱
    function IsValid() {
        //获取手机验证值
        var PhoIsValid = $("#validPho").val();
        //获取邮件验证值
        var EmailIsValid = $("#validEmail").val();
        //没有验证通过
        if (PhoIsValid != "验证通过" || EmailIsValid != "验证通过") {
            $("#Name").val("");
            $("#Password").val("");
            $("#ConfirmedPassWd").val("");
            alert("请先验证手机号码及电子邮箱地址");
        }
    }

    //验证方法-当创建帐号信息条件都满足时Create按钮可用
    function CanCreate() {
        //alert("CanCreate");
        if ($("#NameIsExistValid").text() == "验证通过" && $("#PassWdIsExistValid").text() == "验证通过" && $("#RepeatPassWd").text() == "验证通过" && $("#validIdCardImg").text() == "") {
            $("#Create").removeAttr("disabled");
        } else {
            $("#Create").attr("disabled", "true");
        }
    }

    //信息提示模块
    //添加提示框
    $("#Name").attr("data-container", "body").attr("data-toggle", "popover").attr("data-placement", "right").attr("data-content", "只能由数字和字母组成，长度为3-20字符");
    $("#Password").attr("data-container", "body").attr("data-toggle", "popover").attr("data-placement", "right").attr("data-content", "长度为6-16字符，大小写字母都需至少一个");
    $("#ConfirmedPassWd").attr("data-container", "body").attr("data-toggle", "popover").attr("data-placement", "right").attr("data-content", "再输入一次密码");
    $("#Email").attr("data-container", "body").attr("data-toggle", "popover").attr("data-placement", "left").attr("data-content", "如果接收不到邮件，请将Ewu@beishui.xyz添加到你的邮箱白名单或者将beishui.xyz添加到域名白名单");

    //提示框
    $(function () {
        $("[data-toggle='popover']").popover();
    });
}