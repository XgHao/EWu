function Score(element) {
    //选择推荐
    if (element.id == "Yes") {
        //更改图标
        $("#Up").removeClass("fa-thumbs-o-up").addClass("fa-thumbs-up");
        $("#Down").removeClass("fa-thumbs-down").addClass("fa-thumbs-o-down");

        //赋值推荐值
        $("#NowEvaluation_IsRecommend").val("True");
        $("#IsChoose").val("Yes");
        $("#valid").removeAttr("disabled");
    } else if (element.id == "No") {
        //更改图标
        $("#Up").removeClass("fa-thumbs-up").addClass("fa-thumbs-o-up");
        $("#Down").removeClass("fa-thumbs-o-down").addClass("fa-thumbs-down");

        //赋值推荐值
        $("#NowEvaluation_IsRecommend").val("False");
        $("#IsChoose").val("Yes");
        $("#valid").removeAttr("disabled");
    }
}

function Submit() {
    var ischoose = $("#IsChoose").val();
    if (ischoose == "") {
        alert("请选择“推荐”或者“不推荐”");
    }
    else if (ischoose == "Yes") {
        $("#score").click();
    }
}