

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
    $('#profiletext').val(profiletext);
    $('#department').val(department);
    $('#employeeid').val(employeeid);
    $('#email').val(email);
    $('#lastname').val(lastname);
    $('#firstname').val(firstname);
    $('#role').val(role);
   


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
            toastr.success("Change Successfull")
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
    $('#deleter').modal('show');
    $('#id').val(id);
}
function changelayout() {
    $('#EditMission').addClass('d-none').removeClass('d-block');

}

function EditMission(id) {
  
    //$(`#edit-${id}`).modal('show');

    $.ajax({
        type: 'POST',
        url: '/Admin/Mission/GetEditMission',
        data: { id: id },
        success: function (result) {
       
            $('#EditMission').addClass('d-block').removeClass('d-none')
            $('.particalEditmission').addClass('d-block').removeClass('d-none')



        },
        error: function () {
            alert("Some Error From Cms.");
        }
    });


            //$('#editMission').html($($.parseHTML(result)).filter("#Editmission")[0].innerHTML);
            //$(`#editMission`).modal('show');

   // $('#id').val(id);
}




//  $(document).ready(function(){
//      $('.datepicker').datepicker({
//          format: 'yyyy/mm/dd',    // Or whatever format you want.
//          startDate: '2015/01/01'  // Or whatever start date you want.
//      });
//});


