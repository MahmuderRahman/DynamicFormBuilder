var Message = {

    Success: function (msg) {
        Swal.fire({
            icon: 'success',
            title: 'Success!',
            text: msg,
            timer: 2000,
            showConfirmButton: false
        });
    },

    Error: function (msg) {
        Swal.fire({
            icon: 'error',
            title: 'Error!',
            text: msg,
            showConfirmButton: true
        });
    },

    Warning: function (msg) {
        Swal.fire({
            icon: 'warning',
            title: 'Warning!',
            text: msg,
            showConfirmButton: true
        });
    },

    Prompt: function () {
        return confirm("Do you want to proceed?.");
    }
};
