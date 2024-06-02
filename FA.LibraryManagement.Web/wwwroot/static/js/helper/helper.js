function showToast(message, backgroundColor, callback) {
    Toastify({
        text: message,
        duration: 1000,
        gravity: "top",
        position: 'right',
        backdrop: false,
        style: {
            background: backgroundColor,
        },
        callback: callback
    }).showToast();
}

function showConfirmDialog(title, text, icon, callback) {
    Swal.fire({
        title: title,
        text: text,
        icon: icon || 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        customClass: {
            confirmButton: 'btn btn-danger',
            cancelButton: 'btn btn-secondary'
        },
    }).then((result) => {
        if (result.isConfirmed) {
            callback();
        }
    });
}

