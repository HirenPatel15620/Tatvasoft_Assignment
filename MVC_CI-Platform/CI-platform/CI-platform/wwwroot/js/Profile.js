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
}

const saveskills = () => {
    $('.saved-skills').empty()
    skills_name.forEach((item, i) => {
        $('.saved-skills').append(`<span class="mt-1 ms-3">` + item + '</span>')
    })
}

const upload_profile_image = () => {
    var image = document.getElementById('profile-image').files[0]
    var fr = new FileReader()
    fr.onload = () => {
        $('#old-profile-image').attr('src', fr.result)
    }
    fr.readAsDataURL(image)
}

const save_details = () => { }
const validate = () => {
    first_name = document.getElementById('first-name').value
    last_name = document.getElementById('last-name').value

}
