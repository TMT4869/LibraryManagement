let userTable = null;

const UserObj = {
    init: function () {

        console.log('UserObj init');

        let global = this;

        userTable = $('#user-table');

        userTable = userTable.DataTable({
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
                "url": "http://localhost:5055/api/User/get-all-users-by-paging",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs": [{
                "targets": [0], "visible": true, "searchable": true
            }],
            "columns": [
                {"data": "number", "name": "#"},
                {
                    "render": function (data, type, row) {
                        return `<img src="${row.imageUrl}"" class="rounded-circle" style="width: 30px; height: 30px;"> ${row.userName}`;
                    },
                    "name": "UserName"
                },
                {"data": "fullName", "name": "FullName"},
                {"data": "email", "name": "Email"},
                {"data": "phoneNumber", "name": "PhoneNumber"},
                {"data": "role", "name": "RoleName"},
                {
                    "render": function (data, type, row) {
                        return row.lockoutEnabled == true ? `<span class="badge bg-danger">Locked</span>` : `<span class="badge bg-success">Unlocked</span>`;
                    },
                },
                {
                    "render": function (data, type, row) {
                        var style = row.lockoutEnabled == true ? 'btn-success' : 'btn-danger';
                        var icon = row.lockoutEnabled == true ? 'bi-unlock' : 'bi-lock';
                        return `                            
                            <div class="btn-group">
                            <button class="btn btn-primary btn-change-role me-1" data-bs-toggle="tooltip" data-bs-placement="top" title="Edit">
                            <i class="bi bi-pencil-square"></i>                           
                            </button>
                            <button class="btn btn-danger btn-delete me-1" data-bs-toggle="tooltip" data-bs-placement="top" title="Delete">
                            <i class="bi bi-trash"></i>
                            </button>
                            <button class="btn ${style} btn-change-status" data-bs-toggle="tooltip" data-bs-placement="top" title="Lock/Unlock">
                               <i class="bi ${icon}"></i>
                            </button>
                            </div>                            
                        `;
                    },
                    "orderable": false
                }],
            "createdRow": function (row, data, dataIndex) {
                // Add a data-id attribute to the tr element
                $(row).attr('data-id', data.id);

                $(row).find('.btn-change-status').on('click', function () {
                    return global.changeStatus(data.id);
                });

                $(row).find('.btn-delete').on('click', function () {
                    return global.deleteUser(data.id);
                });

                $(row).find('.btn-change-role').on('click', function () {
                    return global.changeRole(data.id);
                });
            },
            "drawCallback": function () {
                var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
                var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
                    return new bootstrap.Tooltip(tooltipTriggerEl)
                })
            }
        });

        $('body').on('click', '#btn-submit-role', function () {
            return global.submitRole();
        });
    },

    submitRole: function () {
        const userId = $('#userId').data('id');
        const roleId = $('#role-select').val();

        const userRoleVM = {
            userId: userId,
            roleId: roleId
        };

        $.ajax({
            url: 'http://localhost:5055/api/Role/update-role-for-user',
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(userRoleVM),
            success: function (response) {
                if (response.success) {
                    showToast(response.message, 'green', () => {
                        $('#user-detail').modal('hide');
                        userTable.ajax.reload();
                    });
                } else {
                    showToast(response.message, 'red');
                }
            }
        });
    },


    changeRole: function (id) {
        const userDetal = $('#user-detail');
        $.ajax({
            url: `/Librarian/User/Detail/${id}`,
            type: 'POST',
            success: function (response) {
                if (response) {
                    userDetal.find(".modal-body").html(response);
                    //set choices.js for multiple select
                    let choices = $('.choices');
                    let initChoice;
                    for (let i = 0; i < choices.length; i++) {
                        if (choices[i].classList.contains("multiple-remove")) {
                            initChoice = new Choices(choices[i], {
                                delimiter: ",",
                                editItems: true,
                                maxItemCount: -1,
                                removeItemButton: true
                            })
                        } else {
                            initChoice = new Choices(choices[i])
                        }
                    }
                    userDetal.modal('show');
                }
            },
            error: function (error) {
                alert(error);
            }
        });


        const modal = $('#user-detail');
        modal.modal('show');
    },

    changeStatus: function (id) {
        $.ajax({
            url: `http://localhost:5055/api/User/change-status/${id}`,
            type: 'POST',
            success: function (data) {
                if (data.success) {
                    showToast(data.message, 'green', () => {
                        userTable.ajax.reload();
                    });
                }
            },
            error: function (error) {
                alert(error);
            }
        });
    },

    deleteUser: function (id) {
        console.log('deleteUser');
        showConfirmDialog('Are you sure ?', "You won't be able to revert this!", 'question', () => {
            $.ajax({
                url: `http://localhost:5055/api/User/delete-user/${id}`,
                type: 'POST',
                success: function (data) {
                    if (data.success) {
                        showToast(data.message, 'green', () => {
                            userTable.ajax.reload();
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
    const UserClass = Object.create(UserObj);
    UserClass.init();
});

