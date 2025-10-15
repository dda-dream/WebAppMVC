var dataTable;



$(document).ready(function () {

    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").dataTable({
        "ajax": {
            "url": "/sales/GetOrdersList"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "orderDate", "width": "10%"},
            { "data": "fullName", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "finalOrderTotal", "width": "5%" },
            { "data": "transactionId", "width": "5%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center" >
                            <a href="/Sales/Details/${data}" class="btn btn-success text-white" style="cursor:pointer">Edit</a>
                        </div>
                    `;
                },
                "width":"5%"
            }
        ]
    })

}