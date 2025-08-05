window.initializeCoverDropzone = function (dotNetHelper) {
    Dropzone.autoDiscover = false;
    new Dropzone("#coverDropzone", {
        url: "http://localhost:5000/api/upload",
        paramName: "file",
        method: "post",
        maxFiles: 1,
        acceptedFiles: "image/*",
        addRemoveLinks: true,
        dictDefaultMessage: "Resmi buraya bırakın veya tıklayın",
        success: function (file, response) {
            console.log("Upload Başarılı: ", response.url);
            dotNetHelper.invokeMethodAsync("OnCoverUploaded", response.url);
        }
    });

  
}

// wwwroot/js/dropzone.js
window.initDropzoneWithExisting = function (dotNetHelper, existingImageUrl) {
    Dropzone.autoDiscover = false;

    var myDropzone = new Dropzone("#coverDropzone", {
        url: "http://localhost:5000/api/upload",
        maxFiles: 1,
        acceptedFiles: "image/*",
        init: function () {
            if (existingImageUrl) {
                var mockFile = {
                    name: "Existing Image",
                    size: 12345,
                    type: 'image/jpeg',
                    accepted: true
                };

                this.emit("addedfile", mockFile);
                this.emit("thumbnail", mockFile, existingImageUrl);
                this.files.push(mockFile);
            }
        },
        success: function (file, response) {
            dotNetHelper.invokeMethodAsync('OnImageUploaded', response.url);
        }
    });
};
  
