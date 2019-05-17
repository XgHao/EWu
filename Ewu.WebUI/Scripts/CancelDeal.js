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
                if (data == "\"OK\"") {
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

