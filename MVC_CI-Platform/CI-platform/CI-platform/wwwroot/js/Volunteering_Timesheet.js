//////date change according to database 
////function convertDate(inputFormat) {
////    function pad(s) { return (s < 10) ? '0' + s : s; }
////    var d = new Date(inputFormat)
////    return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('/')
////}



////const volunteerhour = () => {
////    var mission = document.getElementById("getmission").value
////    var date = convertDate($(`#edit-${id}`).find('#datepicker').val())
////    var hour = document.getElementById("userhour").value
////    var minites = document.getElementById("userminites").value
////    var massage = document.getElementById("usermessage").value

////    $ajax({
////        url: '/volunteering_timesheet',
////        type: 'POST',
////        data: { hour: hour, minities: minites, massage: massage },
////        success: function (result) {
////            alert("volunteering")
////        },
////        error: function () {
////            alert("something is wrong")
////        }
////    })
////}






function CreateorEdit(timesheetid, flag) {
    $.ajax({
        type: 'GET',
        url: '/volunteering_timesheet',
        data: { 'id': timesheetid },
        success: function (res) {
            if (flag == 0) {
                $('#timesheetpartial').html($($.parseHTML(res)).filter("#addtime")[0].innerHTML);
                //$('#addtime').modal('show')
            }
            else {
                $('#timesheetpartial').html($($.parseHTML(res)).filter("#addgoal")[0].innerHTML);
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