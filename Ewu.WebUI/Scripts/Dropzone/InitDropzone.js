Dropzone.options.dropzoneForm = {
    maxFiles: 2,
    init: function () {
        this.on("maxfilesexceeded", function (data) {
            var res = eval('(' + "超出限制了" + ')');
        });
        this.on("addedfile", function (file) {
            var removeButton = Dropzone.createElement("<button class='btn btn-danger'>Remove file</button>");

            var _this = this;

            removeButton.addEventListener("click", function (e){
                e.preventDefault();
                e.stopPropagation();

                _this.removeFile(file);
            });

            file.previewElement.appendChild(removeButton);
        });
    }
}