var AjaxManager = {

    SendJson: function (serviceUrl, formData, successCallback, errorCallback) {
        $.ajax({
            type: "POST",
            url: serviceUrl,
            data: formData,
            processData: false,
            contentType: false,
            success: successCallback,
            error: errorCallback
        });
    },

};
