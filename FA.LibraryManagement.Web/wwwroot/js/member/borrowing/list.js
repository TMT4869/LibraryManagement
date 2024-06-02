var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    switch (url) {
        case "?status=pending":
            loadDataTable("pending");
            break;
        case "?status=incomplete":
            loadDataTable("incomplete");
            break;
        case "?status=completed":
            loadDataTable("completed");
            break;
        case "?status=cancelled":
            loadDataTable("cancelled");
            break;
        default:
            loadDataTable("all");
            break;
    }
});

function loadDataTable(status) {
    dataTable = $('#borrowing-table').DataTable({
        "ajax": {
            "url": `http://localhost:5055/api/Borrowing/get-all-borrowing-by-user-id/${userId}?status=${status}`
        },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'userVM.fullName', "width": "25%" },
            { data: 'userVM.phoneNumber', "width": "20%" },
            { data: 'userVM.email', "width": "20%" },
            {
                data: 'borrowedTime',
                "width": "10%",
                "render": function (data) {
                    var date = new Date(data);
                    var day = date.getDate();
                    var month = date.getMonth() + 1;
                    var year = date.getFullYear();
                    return `${day.toString().padStart(2, '0')}/${month.toString().padStart(2, '0')}/${year}`;
                }
            },
            { data: 'status', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                 <a href="Detail/?borrowingId=${data}" class="btn btn-primary mx-2"> 
                 <i class="bi bi-pencil-square"></i></a>
                </div>`
                },
                "width": "10%"
            }
        ]
    });
}