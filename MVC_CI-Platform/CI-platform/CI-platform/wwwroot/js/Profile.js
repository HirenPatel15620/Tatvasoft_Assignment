toastr.options = {
    "closeButton": true,
    "debug": false, "newestOnTop": false, "progressBar": true, "positionClass": "toast-top-right", "preventDuplicates": false, "onclick": null, "showDuration": "300", "hideDuration": "1000",
    "timeOut": "2000", "extendedTimeOut": "1000", "showEasing": "swing", "hideEasing": "swing", "showMethod": "slideDown", "hideMethod": "slideUp"
}




var skills = []
var skills_name = []
var first_name
var last_name
var e_id
var m_details
var title
var department
var bio
var reason_volunteering
var country
var city
var availablity
var linkedin
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

const addskill = (skill_id, skill_name) => {
    var id = parseInt(skill_id.slice(6))
    if (!skills.includes(id)) {
        $(`#${skill_id}`).css("background-color", "#0000000D")
        $('.selected-skills').append(`<span class="mt-1" id=${id}>` + skill_name + '</span>')
        skills.push(id)
        skills_name.push(skill_name)
    }
    else {
        $(`#${skill_id}`).css("background-color", "white")
        $('.selected-skills').find(`#${id}`).remove()
        skills.splice(skills.indexOf(id), 1)
        skills_name.splice(skills_name.indexOf(skill_name), 1)
    }
    document.getElementById('selected_skills').value = skills

}

const saveskills = () => {
    $('.saved-skills').empty()
    skills_name.forEach((item, i) => {
        $('.saved-skills').append(`<span class="mt-1 ms-3">` + item + '</span>')
    })
    toastr.success("skill selected")
}

const upload_profile_image = () => {
    var image = document.getElementById('profile-image').files[0]
    var fr = new FileReader()
    fr.onload = () => {
        $('#old-profile-image').attr('src', fr.result)
    }
    fr.readAsDataURL(image)
}

const change_password = () => {
    var oldpassword = document.getElementById("oldpassword").value
    var newpassword = document.getElementById("newpassword").value
    var confirmpassword = document.getElementById("confirmpassword").value
    if (oldpassword.length < 8) {
        $('.wrong-oldpassword').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.wrong-oldpassword').addClass('d-none').removeClass('d-block')
    }
    if (newpassword.length < 8) {
        $('.newpassword').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.newpassword').addClass('d-none').removeClass('d-block')
    }
    if (confirmpassword != newpassword) {
        $('.confirmpassword').addClass('d-block').removeClass('d-none')
    }
    else {
        $('.confirmpassword').addClass('d-none').removeClass('d-block')
    }
    if (oldpassword.length >= 8 && newpassword.length >= 8 && confirmpassword.length >= 8 && confirmpassword == newpassword) {
        $.ajax({
            url: '/profile',
            type: 'POST',
            data: { oldpassword: oldpassword, newpassword: newpassword },
            success: function (result) {
                if (result.success) {
                    $("#changepassword").modal('hide')
                    document.getElementById("oldpassword").value = ""
                    document.getElementById("newpassword").value = ""
                    document.getElementById("confirmpassword").value = ""
                    $('.wrong-oldpassword').addClass('d-none').removeClass('d-block')
                    toastr.success("password change")
                }
                else {
                    $('.wrong-oldpassword').addClass('d-block').removeClass('d-none')
                    toastr.error("password error")
                }this
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}

function contactus(){


    var subject = document.getElementById("subject").value
    var message = document.getElementById("message").value
    $.ajax({
        type: 'POST',
        url: '/User/Home/contactus',
        data: { subject: subject, message: message },
        success: function (result) {
            if (result.success) {
                $("#contactus").modal('hide')
                document.getElementById("subject").value = ""
                document.getElementById("message").value = ""
                
                toastr.success("Thank You For Your Suggestion")
            }
            else {
                
                toastr.error(" Sorry,But Please send it again")
            } this
        },
        error: function () {
            console.log("Error updating variable");
        }
    })

}