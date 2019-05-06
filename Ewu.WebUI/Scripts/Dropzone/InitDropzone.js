//封面
Dropzone.options.CoverDropzone = {
    //最多上传的文件数
    maxFiles: 1,
    //最大文件大小
    maxFilesize: 3,
    //添加上传取消和删除预览图片的链接，默认不添加
    addRemoveLinks: true,

    //关闭自动上传功能，默认会true会自动上传
    //也就是添加一张图片向服务器发送一次请求
    autoProcessQueue: false,

    //允许上传多个照片
    uploadMultiple: false,

    //每次上传的最多文件数，经测试默认为2，坑啊
    //记得修改web.config 限制上传文件大小的节
    parallelUploads: 1,

    //重命名文件
    renameFilename: function (file) {
        return file.renameFilename = new Date().getTime() + "." + file.split('.').pop();
    },

    //以下为文字的翻译
    dictRemoveFile: "移除",   //删除按钮文本
    dictMaxFilesExceeded: "封面最多上传1张图片",    //文件超限的文本
    dictUploadCanceled: "你取消了图片上传",    //取消图片上传的文本
    dictInvalidFileType: "目前仅支持JPG和PNG的格式，往后添加更多格式的支持",
    dictFileTooBig: "图片太大了，仅支持3M以下的图片",
    dictFallbackMessage: "浏览器版本不支持，请升级浏览器",
    dictDefaultMessage: "目前无图片，请拖拽或者点击上传",
    dictResponseError: '未知原因，上传失败!',

    //接受的文件类型
    acceptedFiles: "image/jpeg,image/png,image/jpg",

    init: function () {
        var submitButton = document.querySelector("#CoverBtn")
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
            $("#CoverBtn").removeAttr("disabled");
        });

        //当上传完成后的事件，接受的数据为JSON格式
        this.on("complete", function (data) {
            if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                var res = eval('(' + data.xhr.responseText + ')');
                var msg;
                if (res.Result) {
                    msg = "封面图片上传成功";
                    //下一步按钮显示
                    $("#CoverBtn").attr("hidden", "hidden");
                    $("#ToDetail").removeAttr("hidden");
                } else {
                    msg = "上传失败，错误原因：" + res.Message;
                }
                alert(msg);
            }
        });

        //删除图片的事件，当上传的图片为空时，使上传按钮不可用状态
        this.on("removedfile", function () {
            if (this.getAcceptedFiles().length === 0) {
                $("#CoverBtn").attr("disabled", true);
            }
        });
    }
};

//细节图
Dropzone.options.DetailImgDropzone = {
    //最大上传数
    maxFiles: 4,
    //最大文件大小
    maxFilesize: 3,

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

    //重命名
    renameFilename: function (file) {
        return file.renameFilename = new Date().getTime() + "." + file.split('.').pop();
    },

    //以下为文字的翻译
    dictRemoveFile: "移除",   //删除按钮文本
    dictMaxFilesExceeded:"细节图最多上传4张图片",    //文件超限的文本
    dictUploadCanceled: "你取消了图片上传",    //取消图片上传的文本
    dictInvalidFileType: "目前仅支持JPG和PNG的格式，往后添加更多格式的支持",
    dictFileTooBig: "图片太大了，仅支持3M以下的图片",
    dictFallbackMessage: "浏览器版本不支持，请升级浏览器",
    dictDefaultMessage: "目前无图片，请拖拽或者点击上传",
    dictResponseError: '未知原因，上传失败!',

    //接受的文件类型
    acceptedFiles: "image/jpeg,image/png,image/jpg",

    init: function () {
        var submitButton = document.querySelector("#DetailImgBtn")
        var myDropzone = this; // closure

        submitButton.addEventListener("click", function () {
            //手动上传所有图片
            myDropzone.processQueue();
        });

        //当添加图片后的事件，上传按钮恢复可用
        this.on("addedfile", function (file) {
            if (this.getAcceptedFiles().length == 4) {
                alert("目前已有" + this.getAcceptedFiles().length + "张图片，已超上限");
                this.removeFile(file);
            }
            $("#DetailImgBtn").removeAttr("disabled");
        });

        //当上传完成后的事件，接受的数据为JSON格式
        this.on("complete", function (data) {
            if (this.getQueuedFiles().length > 0 && this.getUploadingFiles().length == 0) {
                myDropzone.processQueue();
            }
            else if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                var res = eval('(' + data.xhr.responseText + ')');
                var msg;
                if (res.Result) {
                    msg = "细节图片上传成功";
                    //下一步按钮显示
                    $("#DetailImgBtn").attr("hidden", "hidden");
                    $("#Success").removeAttr("hidden");
                } else {
                    msg = "上传失败，错误原因：" + res.Message;
                }
                alert(msg);
            }
        });

        //删除图片的事件，当上传的图片为空时，使上传按钮不可用状态
        this.on("removedfile", function () {
            if (this.getAcceptedFiles().length === 0) {
                $("#DetailImgBtn").attr("disabled", true);
            }
        });
    }
};
