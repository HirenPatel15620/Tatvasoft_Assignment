





function CreateorEdit(timesheetid, flag) {
    $.ajax({
        type: 'POST',
        url: '/User/Home/GetEditData',
        data: { 'id': timesheetid },
        success: function (result) {
            if (flag == 0) {
                $('#timesheetpartial').html($($.parseHTML(result)).filter("#addtime")[0].innerHTML);
             
            }
            else {
                $('#timesheetpartial').html($($.parseHTML(result)).filter("#addgoal")[0].innerHTML);
            }
            console.log("Hii")
            
            $('#timesheetpartial').modal('show');
        },
        error: function (data) {
            alert("Some Error From timesheet.");
        }
    });
}


function DeleteRecord(timesheetid) {
   
    
    $('#deleterec').modal('show');
    $('#timesheetid').val(timesheetid);
}


function showthis() {
    toastr.error("Volunteer Delete Successfull")
    setTimeout(function () {
        location.reload();
    }, 4000);
}