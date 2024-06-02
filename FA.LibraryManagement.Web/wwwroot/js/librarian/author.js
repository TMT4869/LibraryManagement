let authorTable = null;

const AuthorObj = {
    init: function () {

        console.log('AuthorObj init')

        let global = this;

        authorTable = $('#author-table');

        authorTable = authorTable.DataTable({
            "processing": '<div class="spinner-border text-primary" role="status"><span class="sr-only">Loading...</span></div>',
            "serverSide": true,
            "paging": true,
            "lengthChange": true,
            "searching": true,
            "ordering": true,
            "autoWidth": false,
            "responsive": true,
            "pagingType": "full_numbers",
            "info": true,
            "sort": true,
            "ajax": {
                "url": "http://localhost:5055/api/Author/get-all-authors-by-paging", "type": "POST", "datatype": "json"
            },
            "columnDefs": [{
                "targets": [0], "visible": true, "searchable": true
            }],
            "columns": [{"data": "number", "name": "#"}, {"data": "name", "name": "Name"}, {
                "render": function (data, type, row) {
                    //render action buttons
                    return `
                            <div class="btn-group">
                            <button class="btn btn-primary btn-edit me-2" data-bs-toggle="tooltip" data-bs-placement="top" title="Edit"><i class="bi bi-pencil-square"></i></button>
                            <button class="btn btn-danger btn-delete" data-bs-toggle="tooltip" data-bs-placement="top" title="Delete"><i class="bi bi-trash"></i></button>
                            </div>
                        `;
                },
                "width": "16%"
            }],
            "createdRow": function (row, data, dataIndex) {
                // Add a data-id attribute to the tr element
                $(row).attr('data-id', data.id);

                $(row).find('.btn-edit').on('click', function () {
                    return global.editAuthor(data);
                });

                $(row).find('.btn-delete').on('click', function () {
                    return global.deleteAuthor(data.id);
                });
            },
            "drawCallback": function () {
                var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
                var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
                    return new bootstrap.Tooltip(tooltipTriggerEl)
                })
            }
        });

        $('body').on('click', '#btn-submit', function (event) {
            return global.submitForm(this, event);
        });

        $('body').on('click', '#btn-reset', function (event) {
            return global.resetForm(event);
        });
    },

    resetForm: function (e) {
        e.preventDefault();
        $('#Name').val('');
        const btnSubmit = $('#btn-submit');
        btnSubmit.removeAttr('data-id');
        btnSubmit.text('Create');
    },

    submitForm: function (eThis, e) {
        e.preventDefault();
        const form = $('#category-form');

        if (!form.valid()) {
            return;
        }

        let api = "";

        let id = $(eThis).attr('data-id');

        if (id === undefined) {
            api = "http://localhost:5055/api/Author/add-author";
        } else {
            api = "http://localhost:5055/api/Author/update-author-by-id";
        }

        const authorVM = {
            Name: $('#Name').val()
        };

        $.ajax({
            url: id === undefined ? api : `${api}/${id}`,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(authorVM),
            success: function (data) {
                if (data.success) {
                    showToast(data.message, 'green', () => {
                        authorTable.ajax.reload();
                        $(eThis).removeAttr('data-id');
                        $('#Name').val('');
                        $(eThis).text('Create');
                    });
                }
            },
            error: function (error) {
                console.log(error)
            }
        });
    },

    editAuthor: function (data) {
        const btnSubmit = $('#btn-submit');
        const name = data.name;
        const id = data.id;
        $('#Name').val(name);
        btnSubmit.attr('data-id', id);
        btnSubmit.text('Update');
    },

    deleteAuthor: function (id) {
        showConfirmDialog('Are you sure ?', "You won't be able to revert this!", 'question', () => {
            $.ajax({
                url: `http://localhost:5055/api/Author/delete-author-by-id/${id}`,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        showToast(data.message, 'green', () => {
                            authorTable.ajax.reload();
                        });
                    }
                },
                error: function (error) {
                    console.log(error)
                }
            });
        });
    }
}

$(document).ready(function ($) {
    const AuthorClass = Object.create(AuthorObj);
    AuthorClass.init();
});
