let categoryTable = null;

const CategoryObj = {
    init: function () {

        let global = this;

        categoryTable = $('#category-table');

        categoryTable = categoryTable.DataTable({
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
                "url": "http://localhost:5055/api/Category/get-all-categories-by-paging",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs": [{
                "targets": [0], "visible": true, "searchable": true
            }],
            "columns": [
                {"data": "number", "name": "#"},
                {"data": "name", "name": "Name"},
                {
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
                    return global.editCategory(data);
                });

                $(row).find('.btn-delete').on('click', function () {
                    return global.deleteCategory(data.id);
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
            return global.resetCategory(event);
        });
    },

    resetCategory: function (e) {
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
            api = "http://localhost:5055/api/Category/add-category";
        } else {
            api = "http://localhost:5055/api/Category/update-category-by-id";
        }

        const categoryVM = {
            Name: $('#Name').val()
        };

        $.ajax({
            url: id === undefined ? api : `${api}/${id}`,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(categoryVM),
            success: function (data) {
                if (data.success) {
                    showToast(data.message, 'green', () => {
                        categoryTable.ajax.reload();
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

    editCategory: function (data) {
        const btnSubmit = $('#btn-submit');
        const name = data.name;
        const id = data.id;
        $('#Name').val(name);
        btnSubmit.attr('data-id', id);
        btnSubmit.text('Update');
    },

    deleteCategory: function (id) {
        showConfirmDialog('Are you sure ?', "You won't be able to revert this!", 'question', () => {
            $.ajax({
                url: `http://localhost:5055/api/Category/delete-category-by-id/${id}`,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        showToast(data.message, 'green', () => {
                            categoryTable.ajax.reload();
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
    const UserClass = Object.create(CategoryObj);
    UserClass.init();
});
