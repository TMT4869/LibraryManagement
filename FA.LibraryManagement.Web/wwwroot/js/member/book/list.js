var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    switch (url) {
        case "?status=borrowing":
            loadDataTable("borrowing");
            break;
        case "?status=borrowed":
            loadDataTable("borrowed");
            break;
        default:
            loadDataTable("all");
            break;
    }
});

function loadDataTable(status) {
    dataTable = $('#book-history-table').DataTable({
        "ajax": {
            "url": `http://localhost:5055/api/History/get-history-by-user-id/${userId}?status=${status}`,
            "dataSrc": "data",
            "error": function (xhr, error, thrown) {
                console.error('Error occurred while fetching data:', error, thrown);
                console.error('Response:', xhr.responseText);
            }
        },
        "columns": [
            {
                data: 'bookVM.bookImages[0].imageUrl', "width": "10%", "render": function (data) {
                    return `<img src="${data}" alt="Book Image" style="width:50px; height:auto;"/>`;
                }
            },
            {
                data: 'bookVM.title', "width": "20%", "render": function (data, type, row) {
                    return `<a href="/Book/Detail?bookId=${row.bookVM.id}">${data}</a>`;
                }
            },
            { data: 'bookVM.isbn', "width": "15%" },
            {
                data: 'bookVM.authorNames', "width": "10%", "render": function (data) {
                    return data ? data.join(', ') : 'No authors';
                }
            },
            {
                data: 'borrowedTime', "width": "10%", "render": function (data) {
                    var date = new Date(data);
                    return date.toLocaleDateString();
                }
            },
            {
                data: 'returnedTime', "width": "10%", "render": function (data) {
                    if (data) {
                        var date = new Date(data);
                        return date.toLocaleDateString();
                    }
                    return 'N/A';
                }
            },
            {
                data: 'dueTime', "width": "10%", "render": function (data) {
                    var date = new Date(data);
                    return date.toLocaleDateString();
                }
            },
            { data: 'fine', "width": "5%" },
            { data: 'status', "width": "10%" }
        ]
    });
}
