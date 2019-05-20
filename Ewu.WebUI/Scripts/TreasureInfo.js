function AddFavorite(treasureid) {
    //获取对象
    var Heart = $("#Favorite_" + treasureid);

    //根据class判断引用的方法
    //添加收藏
    if (Heart.hasClass("fa-heart-o")) {
        //修改样式避免重复提交
        if (!Heart.hasClass("fa-clock-o")) {
            Heart.removeClass("fa-heart-o").addClass("fa-clock-o");
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
            },
            success: function (data) {
                //收藏成功
                if (data == "\"OK\"" || data == "OK") {
                    //修改图标样式
                    Heart.removeClass("fa-clock-o").addClass("fa-heart");
                    //修改数据
                    Heart.text("取消收藏");
                }
                //失败，按钮复原
                else {
                    Heart.removeClass("fa-clock-o").addClass("fa-heart-o");
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
            },
            success: function (data) {
                //收藏成功
                if (data == "\"OK\"" || data == "OK") {
                    //修改图标样式
                    Heart.removeClass("fa-clock-o").addClass("fa-heart-o");
                    //修改数据
                    Heart.text("添加收藏");
                }
                //失败，按钮复原
                else {
                    Heart.removeClass("fa-clock-o").addClass("fa-heart");
                    alert("好像出错了");
                }
            }
        });
    }




}


$(function () {
    $(".carousel-content").carousel({
        carousel: ".carousel",//轮播图容器
        indexContainer: ".img-index",//下标容器
        prev: ".carousel-prev",//左按钮
        next: ".carousel-next",//右按钮
        timing: 5000,//自动播放间隔
        animateTime: 700,//动画时间
        autoPlay: true,//是否自动播放 true/false
        direction: "left",//滚动方向 right/left
    });

    $(".carousel-content").hover(function () {
        $(".carousel-prev,.carousel-next").fadeIn(300);
    }, function () {
        $(".carousel-prev,.carousel-next").fadeOut(300);
    });

    $(".carousel-prev").hover(function () {
        $(this).find("img").attr("src", "/images/test/left2.png");
    }, function () {
        $(this).find("img").attr("src", "/images/test/left1.png");
    });
    $(".carousel-next").hover(function () {
        $(this).find("img").attr("src", "/images/test/right2.png");
    }, function () {
        $(this).find("img").attr("src", "/images/test/right1.png");
    });
});