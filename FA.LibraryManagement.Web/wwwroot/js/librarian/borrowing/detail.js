$(document).ready(function () {
    var status = $('#borrowing-status').text().split('-')[1].trim();

    if (status == 'Completed' || status == 'Cancelled') {
        $('.btn-borrowing-detail').prop('disabled', true);
    } else {
        $('.btn-borrowing-detail').prop('disabled', false);
    }

    $('.btn-borrowing-detail').on('click', function (e) {
        e.preventDefault();

        var id = $(this).closest('tr').find('td').first().text();
        var url = '/Librarian/Borrowing/BorrowingDetail/' + id;

        $.post(url).done(function (data) {
            $('#borrowing-detail .modal-body').html(data);

            if (status == 'Pending') {
                $('#borrowing-detail #status-select').attr('name', 'status-select-hidden').hide();
                $('<input>').attr({
                    type: 'text',
                    class: 'form-control',
                    id: 'status-pending',
                    name: 'Status',
                    value: 'Pending',
                    readonly: true
                }).appendTo('#update-form');
            } else {
                $('#borrowing-detail #status-select').attr('name', 'Status').show();
                $('#status-pending').remove();
            }

            $('#borrowing-detail').modal('show');
        });
    });

    $('#btn-update').on('click', function (e) {
        e.preventDefault();

        $.ajax({
            type: 'POST',
            url: $('#update-form').attr('action'),
            data: $('#update-form').serialize(),
            success: function (response) {
                // Close the modal
                $('#borrowing-detail').modal('hide');

                location.reload();
            },
            error: function (response) {
                // Log the error details to the console
                console.error('Update failed:', response);

                // Display a generic error message
                alert('Update failed!');
            }
        });
    });

    $('.btn-status').on('click', function (e) {
        e.preventDefault();

        var actionUrl = $(this).data('action-url');
        console.log(actionUrl);

        Swal.fire({
            title: "Are you sure?",
            text: "Please double-check before pressing the button!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'
        })
            .then((result) => {
                if (result.isConfirmed) {
                    $('#borrowing-form').attr('action', actionUrl);
                    $('#borrowing-form').submit();
                }
            });
    });
});
