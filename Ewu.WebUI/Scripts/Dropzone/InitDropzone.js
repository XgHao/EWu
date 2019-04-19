var CoverCnt = 0;

Dropzone.options.CoverDropzone = {

    maxFiles:1,
    //添加上传取消和删除预览图片的链接，默认不添加
    addRemoveLinks: true,

    //关闭自动上传功能，默认会true会自动上传
    //也就是添加一张图片向服务器发送一次请求
    autoProcessQueue: false,

    //允许上传多个照片
    uploadMultiple: true,

    //每次上传的最多文件数，经测试默认为2，坑啊
    //记得修改web.config 限制上传文件大小的节
    parallelUploads: 1,

    init: function () {
        var submitButton = document.querySelector("#submit-all")
        var myDropzone = this; // closure

        submitButton.addEventListener("click", function () {
            //手动上传所有图片
            myDropzone.processQueue();
        });

        //当添加图片后的事件，上传按钮恢复可用
        this.on("addedfile", function (file) {
            //alert("getAcceptedFiles" + this.getAcceptedFiles().length + "\ngetRejectedFiles" + this.getRejectedFiles().length + "\ngetQueuedFiles" + this.getQueuedFiles().length + "\ngetUploadingFiles" + this.getUploadingFiles().length);

            if (this.getAcceptedFiles().length == 1) {
                alert("目前已有" + this.getAcceptedFiles().length + "张图片，已超上限");
                this.removeFile(file);
            }
            $("#submit-all").removeAttr("disabled");
        });

        //当上传完成后的事件，接受的数据为JSON格式
        this.on("complete", function (file) {
            
        });

        //删除图片的事件，当上传的图片为空时，使上传按钮不可用状态
        this.on("removedfile", function () {
            if (this.getAcceptedFiles().length === 0) {
                $("#submit-all").attr("disabled", true);
            }
        });
    }
};
