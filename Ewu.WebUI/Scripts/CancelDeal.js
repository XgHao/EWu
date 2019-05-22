//取消交易
function CancelDeal(element) {
    //获取当前按钮id
    var id = element.id;
    if (confirm("你确定取消交易申请吗？")) {
        $.ajax({
            type: "POST",
            dataType: "text",
            url: "/Deal/CancelDeal",
            data: { "DealLogUID": id },
            error: function (msg) {
                alert(msg + "请求失败");
            },
            success: function (data) {
                //删除成功
                if (data == "\"OK\"" || data == "OK") {
                    //刷新页面
                    location.reload();
                }
                else {
                    alert("请求失败，请刷新页面");
                }
            }
        });
    }
    else {
        
    }
}



//回复留言
function Comment() {
    //获取评论框对象,按钮对象
    var userid = $("#BasicUserInfoTa_UserID").val();
    var Noticeid = $("#CurrMessUID").val();
    var comBox = $("#comment");
    var btn = $("#Icon");

    if (btn.hasClass("fa-paper-plane-o")) {
        //获取评论信息
        var comment = comBox.val();
        if (comment != "" || comment != null) {
            btn.removeClass("fa-paper-plane-o").addClass("fa-circle-o-notch fa-spin fa-fw");

            //添加留言信息
            $.ajax({
                type: "POST",
                dateType: "text",
                url: "/Account/Reply",
                data: {
                    "UserID": userid,
                    "Comment": comment,
                    "NoticeId": Noticeid,
                },
                error: function (msg) {
                    alert("请求失败，请联系957553851@qq.com" + msg);
                    btn.removeClass("fa-circle-o-notch fa-spin fa-fw").addClass("fa-paper-plane-o");
                },
                success: function (data) {
                    //成功
                    if (data == "OK" || data == "\"OK\"") {
                        //清空留言栏
                        comBox.val("");
                        //更改样式
                        btn.removeClass("fa-circle-o-notch fa-spin fa-fw").addClass("fa-check");
                    }
                    //失败
                    else {
                        btn.removeClass("fa-circle-o-notch fa-spin fa-fw").addClass("fa-paper-plane-o");
                    }
                }
            });
        }
    }
}



//选择头像
function ChooseImg(id) {
    //遍历所有头像,首先全部设为未选
    for (var i = 1; i <= 16; i++) {
        if (i == parseInt(id)) {
            if ($("#choose" + i).hasClass("fa-circle-o")) {
                $("#choose" + i).removeClass("fa-circle-o").addClass("fa-circle");
            }
        }
        else {
            if ($("#choose" + i).hasClass("fa-circle")) {
                $("#choose" + i).removeClass("fa-circle").addClass("fa-circle-o");
            }
        }
    }
    $("#HeadPortrait").val("/images/HeadImg/HeadImg" + id + ".png");
}


//留言
function Message(userid) {
    //获取评论框对象,按钮对象
    var comBox = $("#comment");
    var btn = $("#Icon");

    if (btn.hasClass("fa-paper-plane-o")) {
        //获取评论信息
        var comment = comBox.val();
        if (comment != "" || comment != null) {
            btn.removeClass("fa-paper-plane-o").addClass("fa-circle-o-notch fa-spin fa-fw");

            //添加留言信息
            $.ajax({
                type: "POST",
                dateType: "text",
                url: "/Account/Comment",
                data: {
                    "UserID": userid,
                    "Comment": comment,
                },
                error: function (msg) {
                    alert("请求失败，请联系957553851@qq.com" + msg);
                    btn.removeClass("fa-circle-o-notch fa-spin fa-fw").addClass("fa-paper-plane-o");
                },
                success: function (data) {
                    //成功
                    if (data == "OK" || data == "\"OK\"") {
                        //清空留言栏
                        comBox.val("");
                        //更改样式
                        btn.removeClass("fa-circle-o-notch fa-spin fa-fw").addClass("fa-check");
                    }
                    //失败
                    else {
                        btn.removeClass("fa-circle-o-notch fa-spin fa-fw").addClass("fa-paper-plane-o");
                    }
                }
            });
        }
    }
}