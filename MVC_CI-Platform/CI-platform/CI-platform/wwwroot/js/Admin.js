﻿

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

  
    $(`#deleterec-${id}`).modal('show');
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


function DeleteCms(cmsid) {
    debugger
    $('#deleter').modal('show');
    $('#cmsid').val(cmsid);
}

//##################################################################################################################

function ThemeDecline(id) {
    debugger
    $(`#deleterec-${id}`).modal('show');
    $('#id').val(id);
    $('#flag').val(flag);
    $('#title').val(title);

   

}



function ThemeGenerate() {
    debugger
    $('#addrec').modal('show');
    $('#title').val(title);

}


///////////////////////////////////////////////////////////////////////////////////////////////////////////

function SkillDecline(id, flag) {

    $(`#deleterec-${id}`).modal('show');
    $('#id').val(id);
    $('#flag').val(flag);
    $('#skillname').val(skillname);


}



function SkillGenerate() {
    debugger
    $('#addrec').modal('show');
    $('#skillname').val(skillname);

}


////story///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function StoryRecord(id, flag) {

    $(`#deleterec-${id}`).modal('show');
    $('#id').val(id);
    $('#flag').val(flag);
    $('#title').val(title);

}

function DeleteRecord(id) {

    $(`#deleter-${id}`).modal('show');
    $('#id').val(id);


}



/// /    / / / / / / / / /  / / / /             / / / / / / / / / / / mission @#@#@#@#@#@#@##222



function MissionDelete(id) {
    debugger
    $('#deleter').modal('show');
    $('#id').val(id);
}



  //  $(document).ready(function(){
  //      $('.datepicker').datepicker({
  //          format: 'yyyy/mm/dd',    // Or whatever format you want.
  //          startDate: '2015/01/01'  // Or whatever start date you want.
  //      });
  //});


const getcities = () => {
    var country = $('.country').find(":selected").val()
    if (parseInt(country) != 0) {
        $.ajax({
            url: '/profile',
            type: 'POST',
            data: { country: country },
            success: function (result) {
                $('.city').empty().append(result.cities.result)
                toastr.success("country selected")
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}