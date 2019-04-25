$(function () {
    $(".carousel-content").carousel({
        carousel: ".carousel",//轮播图容器
        indexContainer: ".img-index",//下标容器
        prev: ".carousel-prev",//左按钮
        next: ".carousel-next",//右按钮
        timing: 3000,//自动播放间隔
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