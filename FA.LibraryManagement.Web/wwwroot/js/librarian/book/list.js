let bookTable = null;

const BookObj = {
    init: function () {

        console.log('BookObj init');

        let global = this;

        bookTable = $('#book-table');

        bookTable = bookTable.DataTable({
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
                "url": "http://localhost:5055/api/Book/get-all-books-by-paging",
                "type": "POST",
                "datatype": "json",
                "data": function (d) {
                    d.quantity = $('#quantity').val();
                }
            },
            "columnDefs": [{
                "targets": [0], "visible": true, "searchable": true
            }],
            "columns": [
                {"data": "number", "name": "#"},
                {"data": "title", "name": "Title"},
                {"data": "isbn", "name": "ISBN"},
                {"data": "publisher", "name": "Publisher"},
                {"data": "quantity", "name": "Quantity"},
                {"data": "categoryName", "name": "CategoryName"},
                {"data": "publishedDateString", "name": "PublishedDateString"},
                {"data": "authorName", "name": "AuthorName"},
                {
                    "render": function (data, type, row) {
                        return `                            
                            <div class="btn-group">
                            <button class="btn btn-primary btn-edit me-2" data-bs-toggle="tooltip" data-bs-placement="top" title="Edit"><i class="bi bi-pencil-square"></i></button>
                            <button class="btn btn-danger btn-delete" data-bs-toggle="tooltip" data-bs-placement="top" title="Delete"><i class="bi bi-trash"></i></button>
</div>                                                                                `
                    },
                    "orderable": false
                }
            ],
            "createdRow": function (row, data, dataIndex) {
                // Add a data-id attribute to the tr element
                $(row).attr('data-id', data.id);

                $(row).find('.btn-edit').on('click', function () {
                    return global.editBook(data.id);
                });

                $(row).find('.btn-delete').on('click', function () {
                    return global.deleteBook(data.id);
                });
            },
            "drawCallback": function () {
                var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
                var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
                    return new bootstrap.Tooltip(tooltipTriggerEl)
                })
            }
        });
        
        $('body').on('change','#quantity', function (event) {
            event.preventDefault();
            let quantity = $(this).val();
            console.log('quantity', quantity);
            bookTable.ajax.params({
                filter: {
                    quantity: quantity
                }
            });
            bookTable.ajax.reload();
        });
    },
    editBook: function (id) {
        window.location.href = `http://localhost:5150/Librarian/Book/Edit?bookId=${id}`;
    },
    deleteBook: function (id) {
        console.log('deleteBook', id)
        showConfirmDialog('Are you sure ?', "You won't be able to revert this!", 'question', () => {
            $.ajax({
                url: `/Librarian/Book/Delete/${id}`,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        showToast(data.message, 'green', () => {
                            bookTable.ajax.reload();
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
    const BookClass = Object.create(BookObj);
    BookClass.init();
});

