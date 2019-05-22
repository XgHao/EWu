function AddFavorite(treasureid) {
    //获取对象
    var Heart = $("#Favorite_" + treasureid);
    var Heart1 = $("#Favorite1_" + treasureid);
    var Heart2 = $("#Favorite2_" + treasureid);

    //根据class判断引用的方法
    //添加收藏
    if (Heart.hasClass("fa-heart-o")) {
        //修改样式避免重复提交
        if (!Heart.hasClass("fa-clock-o")) {
            Heart.removeClass("fa-heart-o").addClass("fa-clock-o");
            Heart1.removeClass("fa-heart-o").addClass("fa-clock-o");
            Heart2.removeClass("fa-heart-o").addClass("fa-clock-o");
        }

        $.ajax({
            type: "POST",
            dateType: "text",
            url: "/Account/AddFavorite",
            data: {
                "TreaUID": treasureid
            },
            error: function (msg) {
                alert("请求失败，请联系957553851@qq.com" + msg);
                Heart.removeClass("fa-clock-o").addClass("fa-heart");
                Heart1.removeClass("fa-clock-o").addClass("fa-heart");
                Heart2.removeClass("fa-clock-o").addClass("fa-heart");
            },
            success: function (data) {
                //收藏成功
                if (data == "\"OK\"" || data == "OK") {
                    //修改图标样式
                    Heart.removeClass("fa-clock-o").addClass("fa-heart");
                    //修改数据
                    Heart.text(parseInt(Heart.text()) + 1);
                    //修改图标样式
                    Heart1.removeClass("fa-clock-o").addClass("fa-heart");
                    //修改数据
                    Heart1.text(parseInt(Heart1.text()) + 1);
                    //修改图标样式
                    Heart2.removeClass("fa-clock-o").addClass("fa-heart");
                    //修改数据
                    Heart2.text(parseInt(Heart2.text()) + 1);
                }
                //失败，按钮复原
                else {
                    Heart.removeClass("fa-clock-o").addClass("fa-heart-o");
                    Heart1.removeClass("fa-clock-o").addClass("fa-heart-o");
                    Heart2.removeClass("fa-clock-o").addClass("fa-heart-o");
                    alert("好像出错了");
                }
            }
        });
    }
    //取消收藏
    else if (Heart.hasClass("fa-heart")) {
        //修改样式避免重复提交
        if (!Heart.hasClass("fa-clock-o")) {
            Heart.removeClass("fa-heart").addClass("fa-clock-o");
            Heart1.removeClass("fa-heart").addClass("fa-clock-o");
            Heart2.removeClass("fa-heart").addClass("fa-clock-o");
        }

        $.ajax({
            type: "POST",
            dateType: "text",
            url: "/Account/CancelFavorite",
            data: {
                "TreaUID": treasureid
            },
            error: function (msg) {
                alert("请求失败，请联系957553851@qq.com" + msg);
                Heart.removeClass("fa-clock-o").addClass("fa-heart");
                Heart1.removeClass("fa-clock-o").addClass("fa-heart");
                Heart2.removeClass("fa-clock-o").addClass("fa-heart");
            },
            success: function (data) {
                //收藏成功
                if (data == "\"OK\"" || data == "OK") {
                    //修改图标样式
                    Heart.removeClass("fa-clock-o").addClass("fa-heart-o");
                    //修改数据
                    Heart.text(parseInt(Heart.text()) - 1);
                    //修改图标样式
                    Heart1.removeClass("fa-clock-o").addClass("fa-heart-o");
                    //修改数据
                    Heart1.text(parseInt(Heart1.text()) - 1);
                    //修改图标样式
                    Heart2.removeClass("fa-clock-o").addClass("fa-heart-o");
                    //修改数据
                    Heart2.text(parseInt(Heart2.text()) - 1);
                }
                //失败，按钮复原
                else {
                    Heart.removeClass("fa-clock-o").addClass("fa-heart");
                    Heart1.removeClass("fa-clock-o").addClass("fa-heart");
                    Heart2.removeClass("fa-clock-o").addClass("fa-heart");
                    alert("好像出错了");
                }
            }
        });
    }
}
function AddFavorite1(treasureid) {
    //获取对象
    var Heart = $("#Favorite_" + treasureid);
    var Heart1 = $("#Favorite1_" + treasureid);
    var Heart2 = $("#Favorite2_" + treasureid);

    //根据class判断引用的方法
    //添加收藏
    if (Heart1.hasClass("fa-heart-o")) {
        //修改样式避免重复提交
        if (!Heart1.hasClass("fa-clock-o")) {
            Heart.removeClass("fa-heart-o").addClass("fa-clock-o");
            Heart1.removeClass("fa-heart-o").addClass("fa-clock-o");
            Heart2.removeClass("fa-heart-o").addClass("fa-clock-o");
        }

        $.ajax({
            type: "POST",
            dateType: "text",
            url: "/Account/AddFavorite",
            data: {
                "TreaUID": treasureid
            },
            error: function (msg) {
                alert("请求失败，请联系957553851@qq.com" + msg);
                Heart.removeClass("fa-clock-o").addClass("fa-heart");
                Heart1.removeClass("fa-clock-o").addClass("fa-heart");
                Heart2.removeClass("fa-clock-o").addClass("fa-heart");
            },
            success: function (data) {
                //收藏成功
                if (data == "\"OK\"" || data == "OK") {
                    //修改图标样式
                    Heart.removeClass("fa-clock-o").addClass("fa-heart");
                    //修改数据
                    Heart.text(parseInt(Heart.text()) + 1);
                    //修改图标样式
                    Heart1.removeClass("fa-clock-o").addClass("fa-heart");
                    //修改数据
                    Heart1.text(parseInt(Heart1.text()) + 1);
                    //修改图标样式
                    Heart2.removeClass("fa-clock-o").addClass("fa-heart");
                    //修改数据
                    Heart2.text(parseInt(Heart2.text()) + 1);
                }
                //失败，按钮复原
                else {
                    Heart.removeClass("fa-clock-o").addClass("fa-heart-o");
                    Heart1.removeClass("fa-clock-o").addClass("fa-heart-o");
                    Heart2.removeClass("fa-clock-o").addClass("fa-heart-o");
                    alert("好像出错了");
                }
            }
        });
    }
    //取消收藏
    else if (Heart1.hasClass("fa-heart")) {
        //修改样式避免重复提交
        if (!Heart1.hasClass("fa-clock-o")) {
            Heart.removeClass("fa-heart").addClass("fa-clock-o");
            Heart1.removeClass("fa-heart").addClass("fa-clock-o");
            Heart2.removeClass("fa-heart").addClass("fa-clock-o");
        }

        $.ajax({
            type: "POST",
            dateType: "text",
            url: "/Account/CancelFavorite",
            data: {
                "TreaUID": treasureid
            },
            error: function (msg) {
                alert("请求失败，请联系957553851@qq.com" + msg);
                Heart.removeClass("fa-clock-o").addClass("fa-heart");
                Heart1.removeClass("fa-clock-o").addClass("fa-heart");
                Heart2.removeClass("fa-clock-o").addClass("fa-heart");
            },
            success: function (data) {
                //收藏成功
                if (data == "\"OK\"" || data == "OK") {
                    //修改图标样式
                    Heart.removeClass("fa-clock-o").addClass("fa-heart-o");
                    //修改数据
                    Heart.text(parseInt(Heart.text()) - 1);
                    //修改图标样式
                    Heart1.removeClass("fa-clock-o").addClass("fa-heart-o");
                    //修改数据
                    Heart1.text(parseInt(Heart1.text()) - 1);
                    //修改图标样式
                    Heart2.removeClass("fa-clock-o").addClass("fa-heart-o");
                    //修改数据
                    Heart2.text(parseInt(Heart2.text()) - 1);
                }
                //失败，按钮复原
                else {
                    Heart.removeClass("fa-clock-o").addClass("fa-heart");
                    Heart1.removeClass("fa-clock-o").addClass("fa-heart");
                    Heart2.removeClass("fa-clock-o").addClass("fa-heart");
                    alert("好像出错了");
                }
            }
        });
    }
}
function AddFavorite2(treasureid) {
    //获取对象
    var Heart = $("#Favorite_" + treasureid);
    var Heart1 = $("#Favorite1_" + treasureid);
    var Heart2 = $("#Favorite2_" + treasureid);

    //根据class判断引用的方法
    //添加收藏
    if (Heart2.hasClass("fa-heart-o")) {
        //修改样式避免重复提交
        if (!Heart2.hasClass("fa-clock-o")) {
            Heart.removeClass("fa-heart-o").addClass("fa-clock-o");
            Heart1.removeClass("fa-heart-o").addClass("fa-clock-o");
            Heart2.removeClass("fa-heart-o").addClass("fa-clock-o");
        }

        $.ajax({
            type: "POST",
            dateType: "text",
            url: "/Account/AddFavorite",
            data: {
                "TreaUID": treasureid
            },
            error: function (msg) {
                alert("请求失败，请联系957553851@qq.com" + msg);
                Heart.removeClass("fa-clock-o").addClass("fa-heart");
                Heart1.removeClass("fa-clock-o").addClass("fa-heart");
                Heart2.removeClass("fa-clock-o").addClass("fa-heart");
            },
            success: function (data) {
                //收藏成功
                if (data == "\"OK\"" || data == "OK") {
                    //修改图标样式
                    Heart.removeClass("fa-clock-o").addClass("fa-heart");
                    //修改数据
                    Heart.text(parseInt(Heart.text()) + 1);
                    //修改图标样式
                    Heart1.removeClass("fa-clock-o").addClass("fa-heart");
                    //修改数据
                    Heart1.text(parseInt(Heart1.text()) + 1);
                    //修改图标样式
                    Heart2.removeClass("fa-clock-o").addClass("fa-heart");
                    //修改数据
                    Heart2.text(parseInt(Heart2.text()) + 1);
                }
                //失败，按钮复原
                else {
                    Heart.removeClass("fa-clock-o").addClass("fa-heart-o");
                    Heart1.removeClass("fa-clock-o").addClass("fa-heart-o");
                    Heart2.removeClass("fa-clock-o").addClass("fa-heart-o");
                    alert("好像出错了");
                }
            }
        });
    }
    //取消收藏
    else if (Heart2.hasClass("fa-heart")) {
        //修改样式避免重复提交
        if (!Heart2.hasClass("fa-clock-o")) {
            Heart.removeClass("fa-heart").addClass("fa-clock-o");
            Heart1.removeClass("fa-heart").addClass("fa-clock-o");
            Heart2.removeClass("fa-heart").addClass("fa-clock-o");
        }

        $.ajax({
            type: "POST",
            dateType: "text",
            url: "/Account/CancelFavorite",
            data: {
                "TreaUID": treasureid
            },
            error: function (msg) {
                alert("请求失败，请联系957553851@qq.com" + msg);
                Heart.removeClass("fa-clock-o").addClass("fa-heart");
                Heart1.removeClass("fa-clock-o").addClass("fa-heart");
                Heart2.removeClass("fa-clock-o").addClass("fa-heart");
            },
            success: function (data) {
                //收藏成功
                if (data == "\"OK\"" || data == "OK") {
                    //修改图标样式
                    Heart.removeClass("fa-clock-o").addClass("fa-heart-o");
                    //修改数据
                    Heart.text(parseInt(Heart.text()) - 1);
                    //修改图标样式
                    Heart1.removeClass("fa-clock-o").addClass("fa-heart-o");
                    //修改数据
                    Heart1.text(parseInt(Heart1.text()) - 1);
                    //修改图标样式
                    Heart2.removeClass("fa-clock-o").addClass("fa-heart-o");
                    //修改数据
                    Heart2.text(parseInt(Heart2.text()) - 1);
                }
                //失败，按钮复原
                else {
                    Heart.removeClass("fa-clock-o").addClass("fa-heart");
                    Heart1.removeClass("fa-clock-o").addClass("fa-heart");
                    Heart2.removeClass("fa-clock-o").addClass("fa-heart");
                    alert("好像出错了");
                }
            }
        });
    }
}

function Search() {
    var key = $("#wordkey").val();
    if (key != "" && key != null) {
        window.location.href = "/Treasure/Search?KeyWord=" + key;
    }
}