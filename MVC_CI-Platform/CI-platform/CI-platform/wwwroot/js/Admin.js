

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





//missin application////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function Decline(id, flag) {

    $('#deleterec').modal('show');
    $('#id').val(id);
    $('#flag').val(flag);

}







//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///create user


function usermake(id = 0) {
    
    var firstname = $(`#deleterec-${id}`).find('#firstname').val()
    var lastname = $(`#deleterec-${id}`).find('#lastname').val()
    var email = $(`#deleterec-${id}`).find('#email').val()
    var password = $(`#deleterec-${id}`).find('#password').val()
    var employeeid = $(`#deleterec-${id}`).find('#employeeid').val()
    var phone = $(`#deleterec-${id}`).find('#Phone').val()
    var department = $(`#deleterec-${id}`).find('#department').val()
    var status = $(`#deleterec-${id}`).find('#flag').val()
    var role = $(`#deleterec-${id}`).find('#role').val()
    if (firstname.length <= 0) {
        $('.first').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.first').addClass('d-none').removeClass('d-block')
    }
    if (lastname.length <= 0) {
        $('.last').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.last').addClass('d-none').removeClass('d-block')
    }
    if (email.length <= 0) {
        $('.email').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.email').addClass('d-none').removeClass('d-block')
    }
    if (password.length <= 0) {
        $('.password').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.password').addClass('d-none').removeClass('d-block')
    }
    if (phone.length <= 0) {
        $('.phone').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.phone').addClass('d-none').removeClass('d-block')
    }
    if (employeeid.length <= 0 || employeeid.length>16 ) {
        $('.emp').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.emp').addClass('d-none').removeClass('d-block')
    }
    if (department.length <= 0) {
        $('.department').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.department').addClass('d-none').removeClass('d-block')
    }

    if (firstname.length > 0 && lastname.length > 0 && email.length > 0 && employeeid.length > 0 && employeeid.length <16 && department.length > 0) {
       
        $.ajax({
            type: 'POST',
            url: '/Admin/User/GetEditUser',
            data: { id: id, firstname: firstname, lastname: lastname, employeeid: employeeid, department: department, status: status, role: role, email: email, password: password, phone: phone },
            success: function (result) {
                if (result.success) {
                    toastr.success("Change Successfull")
                    $(`#deleterec-${id}`).modal('hide');
                    setTimeout(function () {
                        location.reload();
                    }, 1000);
                }

            },
            error: function () {
                alert("Some Error From Cms.");
            }
        });
    }
}




//Update user/////////////////////////////////////////////////////
function update(id, flag) {

    $(`#deleterec-${id}`).modal('show');
}

function userchange(id) {

    var firstname = $(`#deleterec-${id}`).find('#firstname').val()
    var lastname = $(`#deleterec-${id}`).find('#lastname').val()
    var email = $(`#deleterec-${id}`).find('#email').val()
    var employeeid = $(`#deleterec-${id}`).find('#employeeid').val()
    var department = $(`#deleterec-${id}`).find('#department').val()
    var status = $(`#deleterec-${id}`).find('.flags').val()
    var role = $(`#deleterec-${id}`).find('.roles').val()
    if (firstname.length <= 0) {
        $('.first').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.first').addClass('d-none').removeClass('d-block')
    }
    if (lastname.length <= 0) {
        $('.last').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.last').addClass('d-none').removeClass('d-block')
    }
    if (email.length <= 0) {
        $('.email').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.email').addClass('d-none').removeClass('d-block')
    }
    if (employeeid.length <= 0 || employeeid.length > 16) {
        $('.emp').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.emp').addClass('d-none').removeClass('d-block')
    }
    if (department.length <= 0) {
        $('.department').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.department').addClass('d-none').removeClass('d-block')
    }

    if (firstname.length > 0 && lastname.length > 0 && email.length > 0 && employeeid.length > 0 && employeeid.length < 16 && department.length > 0) {

        $.ajax({
            type: 'POST',
            url: '/Admin/User/GetEditUser',
            data: { id: id, firstname: firstname, lastname: lastname, employeeid: employeeid, department: department, status: status, role: role },
            success: function (result) {
                if (result.success) {
                    toastr.success("Change Successfull")
                    $(`#deleterec-${id}`).modal('hide');
                    setTimeout(function () {
                        location.reload();
                    }, 1000);
                }

            },
            error: function () {
                alert("Some Error From Cms.");
            }
        });
    }
}



function removevalidationforuser() {

    $('.status').addClass('d-none').removeClass('d-block')
    $('.department').addClass('d-none').removeClass('d-block')
    $('.emp').addClass('d-none').removeClass('d-block')
    $('.phone').addClass('d-none').removeClass('d-block')
    $('.password').addClass('d-none').removeClass('d-block')
    $('.email').addClass('d-none').removeClass('d-block')
    $('.last').addClass('d-none').removeClass('d-block')
    $('.first').addClass('d-none').removeClass('d-block')

}






////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





//cms page@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

function updatecms(id) {

    var title = $(`#edit-${id}`).find('.title').val()
    var description = $(`#edit-${id}`).find('.discription').val()
    var slug = $(`#edit-${id}`).find('.slug').val()
    var status = $(`#edit-${id}`).find('.status').val()
    if (title.length <= 0) {
        $('.tit').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.tit').addClass('d-none').removeClass('d-block')
    }
    if (description.length <= 0) {
        $('.des').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.des').addClass('d-none').removeClass('d-block')
    }
    if (slug.length <= 0) {
        $('.slu').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.slu').addClass('d-none').removeClass('d-block')
    }
    //if (status.value != 0 && status.value != 1) {
    //    $('.sta').addClass('d-block').removeClass('d-none')
    //}
    //else {
    //    $('.sta').addClass('d-none').removeClass('d-block')
    //}
    if (title.length > 0 && description.length > 0 && description.length > 0) {

        $.ajax({
            type: 'POST',
            url: '/Admin/User/Editcms',
            data: { id: id, title: title, description: description, slug: slug, status: status },
            success: function (result) {
                if (result.success) {
                    toastr.success("Change Successfull")
                    $(`#edit-${id}`).modal('hide');
                    setTimeout(function () {
                        location.reload();
                    }, 1000);
                }
                //$(`#edit-${id}`).addClass('d-none')
            },
            error: function () {
                alert("Some Error From Cms.");
            }
        });
    }
}


function createcms(id = 0) {

    var title = $(`#edit-${id}`).find('.title').val()
    var description = $(`#edit-${id}`).find('.discription').val()
    var slug = $(`#edit-${id}`).find('.slug').val()
    var status = $(`#edit-${id}`).find('.status').val()
    if (title.length <= 0) {
        $('.tit').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.tit').addClass('d-none').removeClass('d-block')
    }
    if (description.trim().length <= 0) {
        $('.des').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.des').addClass('d-none').removeClass('d-block')
    }
    if (slug.length <= 0) {
        $('.slu').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.slu').addClass('d-none').removeClass('d-block')
    }

    if (title.length > 0 && description.length > 0 && description.length > 0) {
        $.ajax({
            type: 'POST',
            url: '/Admin/User/Editcms',
            data: { id: id, title: title, description: description, slug: slug, status: status },
            success: function (result) {

                if (result.success) {
                    toastr.success("Cms Update")
                    $(`#edit-${id}`).modal('hide');
                    setTimeout(function () {
                        location.reload();
                    }, 1000);
                    debugger
                }

            },
            error: function () {
                alert("Some Error From Cms.");
            }
        });
    }
}


function removevalidation() {

    $('.tit').addClass('d-none').removeClass('d-block')
    $('.des').addClass('d-none').removeClass('d-block')
    $('.slu').addClass('d-none').removeClass('d-block')
}


function DeleteCms(cmsid) {

    $('#deleter').modal('show');
    $('#cmsid').val(cmsid);
}

//##################################################################################################################////////////////////////////////////////////////////////////////////////////////////////////

function ThemeDecline(id) {

    $(`#deleterec-${id}`).modal('show');
    $('#id').val(id);
    //$('#flag').val(flag);
    //$('#title').val(title);



}



function ThemeGenerate() {

    $('#addrec').modal('show');
    $('#title').val(title);

}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function SkillDecline(id, flag) {

    $(`#deleterec-${id}`).modal('show');
    $('#id').val(id);
    $('#flag').val(flag);
    //$('#skillname').val(skillname);


}



function SkillGenerate() {

    $('#addrec').modal('show');
    //$('#skillname').val(skillname);

}
///banner////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function BannerRecord(id) {

    $(`#deleterec-${id}`).modal('show');
    $('#id').val(id);

}



function DeleteBanner(id) {

    $('#id').val(id);
    $(`#deleter-${id}`).modal('show');

}


function AddBanner() {

    $('#addbanner').modal('show');

}

////story////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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



/// /    / / / / / / / / /  / / / /             / / / / / / / / / / / mission @#@#@#@#@#@#@##222///////////////////////////////////////////////////////////////////////////////////////////////



function MissionDelete(id) {

    $(`#deleter-${id}`).modal('show');
    $('#id').val(id);
}

function changelayout() {
    $('#EditMission').addClass('d-none').removeClass('d-block');
    $('#editmis').addClass('d-block').removeClass('d-none');


}

function EditMission(id) {



    $.ajax({
        type: 'POST',
        url: '/Admin/Mission/GetEditMission',
        data: { id: id },
        success: function (result) {

            $('#EditMission').empty().append(result)
            $('#EditMission').addClass('d-block').removeClass('d-none')
            $('#editmis').addClass('d-none').removeClass('d-block')
            $('.particalEditmission').addClass('d-block').removeClass('d-none')



        },
        error: function () {
            alert("Some Error From Cms.");
        }
    });



    $("#MissionType").change((e) => {
        if (e.target.value === "GOAL")
            $(".goal-item").removeClass("d-none");
        else
            $(".goal-item").addClass("d-none");
    });




}

function Declinedocument(id) {

    $.ajax({
        url: '/Admin/Mission/Declinedocument',
        type: 'POST',
        data: { id: id },
        success: function () {
            $(`#document-${id}`).remove()
            toastr.error("Successfully Deleted")
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}


function Declinemedia(id) {

    $.ajax({
        url: '/Admin/Mission/Declinemedia',
        type: 'POST',
        data: { id: id },
        success: function (response) {
            $('#particalEditmission').html(response);
            $(`#image-${id}`).remove()
            toastr.error("Successfully Deleted")
        },
        error: function () {
            console.log("Error updating variable");
        }
    })

}

function displaycities(Class) {
    var id = $(`.${Class}`).find(":selected").val()
    $(".modelcities").each(function (i, item) {
        if (item.classList.contains(`country-${id}`)) {
            item.classList.remove("d-none")
        }
        else {
            item.classList.add("d-none")
        }
    })
    
}



