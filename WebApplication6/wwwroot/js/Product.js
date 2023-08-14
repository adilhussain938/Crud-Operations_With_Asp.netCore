var datatable;

$(document).ready(function () {
    LoadDataTable();
});
function LoadDataTable()
{
    datatable = $('#TblData').DataTable({
        "ajax":
        {
            "url": "/Admin/Product/GetAll"
        },
        "columns":
            [
                { "data": "title", "width": "15%" },
                { "data": "isbn", "width": "15%" },
                { "data": "price", "width": "15%" },
                {
                    "data": "id", 
                    "render": function (data)
                    {
                        return `
                        
                        <div class= "w-75 btn-group" role="group">
                         <a onClick = Delete('/Admin/Product/Delete/${data}')  
                         class="btn btn-danger mx-2"><i class="bi bi-trash-fill"></i>Delete</a>
                         <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i>Edit</a>
                         </div>
                        `
                    },
                     "width": "15%"
                }
            ]
    });
}

function Delete(url)
{
    debugger;
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax(
                {
                    url: url,
                    type: 'DELETE',
                    success: function (data)
                    {
                        if (data.success) {
                            datatable.ajax.reload();
                            toastr.success(data.message);
                        }
                        else
                        {
                            toastr.error(data.message);
                        }
                    }
                    
                })
            
        }
    })
}