var messages = new Array();

function PLInitialize() {
    // Client side form validation
    $('form').submit(PLValidate);

    //Handles Event after Upload of a File
    $('#uploader').pluploadQueue().bind('FileUploaded', PLFileUploaded);

    //Handles Event after Upload of all File
    $('#uploader').pluploadQueue().bind('UploadComplete', PLUploadComplete);
}

function PLValidate(e) {
    var uploader = $('#uploader').pluploadQueue();

    // Files in queue upload them first
    if (uploader.files.length > 0) {
        // When all files are uploaded submit form
        uploader.bind('StateChanged', function () {
            if (uploader.files.length === (uploader.total.uploaded + uploader.total.failed)) {
                $('form')[0].submit();
            }
        });

        uploader.start();
    } else {
        alert('You must queue at least one file.');
    }

    return false;
}

function PLFileUploaded(up, file, info) {

    var object = $.parseJSON(info.response)
    if (object.MessageType == "Error") {
        //up.trigger("Error", { message: "'" + object.Text + "'", file: file });
        file.status = plupload.FAILED;
    }

    if (!(object.ThumbPath == null || object.ThumbPath == 'undefined' || object.ThumbPath == '')) {
        $('#' + file.id).prepend(PLGetThumbnail(object));
    }

    messages[file.id] = info.response;
    $("a[rel^='lightbox']").lightBox();
    //$.post('/MultipleFileUploader/FileUploaded');
}

function PLUploadComplete(up, files) {
    var parentBody = $(document.body);

    for (var i = 0; i < files.length; i++) {
        var object = $.parseJSON(messages[files[i].id])
        if (!(object.ImagePath == null || object.ImagePath == 'undefined' || object.ImagePath == '')) {
            $('#' + files[i].id).prepend(PLGetThumbnail(object));
            //parentBody.append(PLGetThumbnail(object));
        }
    }

    $("a[rel^='lightbox']").lightBox();
    //parentBody.find("a[rel^='lightbox']").lightBox();
}

function PLGetThumbnail(imageData) {
    return "<div class= srk_image><a title=" + imageData.FileName + " href=" + imageData.ImagePath + " rel='lightbox'><img src='" + imageData.ThumbPath + "' alt='" + imageData.FileName + "'/></a></div>";
}

//function UnloadPlupload()