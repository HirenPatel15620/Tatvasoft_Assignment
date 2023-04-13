

//common//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$(".sidebar ul li").on('click', function () {

    $(".sidebar ul li.active").removeClass('active');
    $(this).addClass('active');
});
$(".open-btn").on('click', function () {
    $(".sidebar").addClass('active');
});
$(".close-btn").on('click', function () {
    $(".sidebar").removeClass('active');
});
$(".c").on('click', function () {
    $(".sidebar").removeClass('active');
});
function LiveTime() {
    let now = new Date();
    document.getElementById("dateTime").innerHTML = now;
}
setInterval(LiveTime, 1000);





//missin application////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function Decline(id, flag) {

    $('#deleterec').modal('show');
    $('#id').val(id);
    $('#flag').val(flag);

}




//delete user//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function update(id, flag) {

    debugger
    $('#deleterec').modal('show');
    $('#id').val(id);
    $('#flag').val(flag);

}



//cms page@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

function updatecms(id) {

    var title = $(`#edit-${id}`).find('.title').val()
    var description = $(`#edit-${id}`).find('.discription').val()
    var slug = $(`#edit-${id}`).find('.slug').val()
    var status = $(`#edit-${id}`).find('.status').val()


    $.ajax({
        type: 'POST',
        url: '/Admin/User/Editcms',
        data: { id: id, title: title, description: description, slug: slug, status: status },
        success: function () {
            toastr.success("Cms Update")
            $(`#edit-${id}`).modal('hide');
            setTimeout(function () {
                location.reload();
            }, 1000);

            //$(`#edit-${id}`).addClass('d-none')
        },
        error: function () {
            alert("Some Error From Cms.");
        }
    });
}


function createcms(id = 0) {
    debugger
    var title = $(`#edit-${id}`).find('.title').val()
    var description = $(`#edit-${id}`).find('.discription').val()
    var slug = $(`#edit-${id}`).find('.slug').val()
    var status = $(`#edit-${id}`).find('.status').val()


    $.ajax({
        type: 'POST',
        url: '/Admin/User/Editcms',
        data: { id: id, title: title, description: description, slug: slug, status: status },
        success: function () {

            toastr.success("Cms Update")
            $(`#edit-${id}`).modal('hide');
            setTimeout(function () {
                location.reload();
            }, 1000);
            debugger
            //$(`#edit-${id}`).addClass('d-none')
        },
        error: function () {
            alert("Some Error From Cms.");
        }
    });
}
//delete cms details///////////////////////////////////////////////////////////////////////////////////////////////////////////

function DeleteRecord(cmsid) {

    $('#deleterec').modal('show');
    $('#cmsid').val(cmsid);
}

//##################################################################################################################

//function ThemeDecline(id, flag) {
//    debugger
//    $(`#deleterec-${id}`).modal('show');
//    $('#id').val(id);
//    $('#flag').val(flag);
//    //$('#title').val(title);
//    //$.ajax({
//    //    type: 'POST',
//    //    url: '/Admin/User/ThemeDecline',
//    //    data: { id: id, flag: flag, title: title },
//    //    success: function () {

//    //        toastr.success("reached");
//    //        debugger
//    //        //$(`#edit-${id}`).addClass('d-none')
//    //    },
//    //    error: function () {
//    //        alert("Some Error From Cms.");
//    //    }
//    //});
//}



//function ThemeGenerate() {
//    debugger
//    $('#deleterec').modal('show');
//    $('#title').val(title);

//}




function SkillDecline(id, flag) {

    $('#deleterec').modal('show');
    $('#id').val(id);
    $('#flag').val(flag);

}