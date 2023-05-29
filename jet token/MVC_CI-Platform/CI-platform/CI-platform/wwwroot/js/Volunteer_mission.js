
toastr.options = {
    "closeButton": true,
    "debug": false, "newestOnTop": false, "progressBar": true, "positionClass": "toast-top-right", "preventDuplicates": false, "onclick": null, "showDuration": "300", "hideDuration": "1000",
    "timeOut": "2000", "extendedTimeOut": "1000", "showEasing": "swing", "hideEasing": "swing", "showMethod": "slideDown", "hideMethod": "slideUp"
}




var count = 1
var co_workers = []
const tabs = (id) => {
    let active_mission = document.getElementsByClassName("active-detail")
    active_mission[0].classList.add("detail")
    active_mission[0].classList.remove("active-detail")
    let user_mission = document.getElementsByClassName(id)
    user_mission[0].classList.remove("detail")
    user_mission[0].classList.add("active-detail")
    let element = document.getElementById(id)
    let remaining_tab = document.getElementsByClassName("tab")
    for (let i = 0; i < remaining_tab.length; i++) {
        if (!remaining_tab[i].classList.contains("inner-content")) {
            remaining_tab[i].classList.add("inner-content")
        }
    }
    element.classList.remove("inner-content")
}

const view_detail_onmouseover = (id, img) => {
    let image = document.getElementById(img)
    image.classList.add("story-image")
    let item = document.getElementById(id);
    item.style.display = "block";
}
const view_detail_onmouseout = (id, img) => {
    let image = document.getElementById(img)
    image.classList.remove("story-image")
    let item = document.getElementById(id);
    item.style.display = "none";
}


const add_comments = (user_id, mission_id) => {
    var comment = document.getElementById('usercomment').value
    var length = $('.user-comments').find('.usercomment-image').length
    if (comment.length > 3 && comment.length < 600) {
        $.ajax({
            url: `/volunteering_mission/${mission_id}`,
            type: 'POST',
            data: { user_id: user_id, mission_id: mission_id, comment: comment, length: length },
            success: function (result) {
                load_comments(result.comments.result)
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
    else {
        alert("comment lenth must be between 3 to 600");
    }
}


const load_comments = (comments) => {
    $('.user-comments').append(comments);
    $('#usercomment').val("")
}


const add_to_favourite = (user_id, mission_id) => {
    $.ajax({
        url: `/volunteering_mission/${mission_id}`,
        type: 'POST',
        data: { request_for: "add_to_favourite", mission_id: mission_id, user_id: user_id },
        success: function (result) {
            if (result.success) {
                
                //alert("add to favourite");
                $('.heart').removeAttr('src').attr('src', '/images/red-heart.png')
           


                toastr.success("add to favourite");
            }
            else {
                $('.heart').removeAttr('src').attr('src', '/images/heart1.png')
        

                toastr.error("Remove from favourite");
            }
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}


//related mission
const add_to_favourited = (user_id, mission_id) => {
    $.ajax({
        url: `/volunteering_mission/${mission_id}`,
        type: 'POST',
        data: { request_for: "add_to_favourite", mission_id: mission_id, user_id: user_id },
        success: function (result) {
            if (result.success) {

                //alert("add to favourite");

                $(`.heart-${mission_id}`).removeAttr('src').attr('src', '/images/red-heart.png')
                $(`.heart-${mission_id}`).css('height', 24)


                toastr.success("add to favourite");
            }
            else {
             
                $(`.heart-${mission_id}`).removeAttr('src').attr('src', '/images/heart1.png')
                $(`.heart-${mission_id}`).css('height', 20)

                toastr.error("Remove from favourite");
            }
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}





const rating = (rating, user_id, mission_id) => {
    if (rating == 1) {
        $('.rating').find('img').each(function (i, item) {
            if (i == (rating - 1)) {
                if (item.id == `${i + 1}-star-empty`) {
                    item.src = '/images/selected-star.png'
                    item.id = `${i + 1}-star`
                    $.ajax({
                        url: `/volunteering_mission/${mission_id}`,
                        type: 'POST',
                        data: { request_for: "rating", mission_id: mission_id, user_id: user_id, rating: rating },
                        success: function (result) {
                            setTimeout(function () {
                                location.reload();
                            }, 1000);
                        },
                        error: function () {
                            console.log("Error updating variable");
                        }
                    })
                }
                else {
                    item.src = '/images/star-empty.png'
                    item.id = `${i + 1}-star-empty`
                    $.ajax({
                        url: `/volunteering_mission/${mission_id}`,
                        type: 'POST',
                        data: { request_for: "rating", mission_id: mission_id, user_id: user_id, rating: 0 },
                        success: function (result) {

                        },
                        error: function () {
                            console.log("Error updating variable");
                        }
                    })
                }
            }
            else {
                item.src = '/images/star-empty.png'
                item.id = `${i + 1}-star-empty`
            }
        })
    }
    else {
        $('.rating').find('img').each(function (i, item) {
            if (i <= (rating - 1)) {
                if (item.id == `${i + 1}-star-empty` || i <= (rating - 1)) {
                    item.src = '/images/selected-star.png'
                    item.id = `${i + 1}-star`
                }
                else {
                    item.src = '/images/star-empty.png'
                    item.id = `${i + 1}-star-empty`
                }
            }
            else {
                item.src = '/images/star-empty.png'
                item.id = `${i + 1}-star-empty`
            }
        })
        $.ajax({
            url: `/volunteering_mission/${mission_id}`,
            type: 'POST',
            data: { request_for: "rating", mission_id: mission_id, user_id: user_id, rating: rating },
            success: function (result) {
                setTimeout(function () {
                    location.reload();
                }, 1000);
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }

}






const add_coworkers = (id) => {
    id = parseInt(id.slice(9))
    if (!co_workers.includes(id)) {
        co_workers.push(id)
    }
    else {
        co_workers.splice(co_workers.indexOf(id), 1)
    }
}




const recommend = (user_id, mission_id) => {
   
    if (co_workers.length > 0) {
        $.ajax({
            url: `/volunteering_mission/${mission_id}`,
            type: 'POST',
            data: { co_workers: co_workers, user_id: user_id, mission_id: mission_id, request_for: "recommend" },
            success: function (result) {
                console.log(result)
                toastr.success("send mail successly");
                $(`coworker-${user_id}`).addClass('d-none')


            },
            error: function () {
                console.log("Error updating variable");
                toastr.error("Fail To Send Mail ");
            }
        })
    }
}







const apply_for_mission = (user_id, mission_id) => {
    $.ajax({
        url: `/volunteering_mission/${mission_id}`,
        type: 'POST',
        data: { user_id: user_id, mission_id: mission_id, request_for: "mission" },
        success: function (result) {
            if (result.success) {
                $('.apply-button').empty().append('<button  class="applyButton btn" disabled>Applied<img src="images/right-arrow.png" alt="">' + '</button >')
                $('.validate-recommend').removeClass('d-none').addClass('d-flex')
                toastr.success("successfully Applied");
            }
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}







const prev_volunteers = (user_id, mission_id) => {
    var one_page_volunteers = 9
    if (count > 1) {
        count--;
        $.ajax({
            url: `/volunteering_mission/${mission_id}`,
            type: 'POST',
            data: { count: count - 1, request_for: "next_volunteers", user_id: user_id, mission_id: mission_id },
            success: function (result) {
                $('.volunteers').empty().append(result.recent_volunteers.result)
                $('.current_volunteers').html(`${one_page_volunteers * (count - 1) == 0 ? 1 : one_page_volunteers * (count - 1)}-${one_page_volunteers * count} of recent ${result.total_volunteers} volunteers`)
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}
const next_volunteers = (max_page, user_id, mission_id) => {
    var one_page_volunteers = 9
    if (count < max_page) {
        count++;
        $.ajax({
            url: `/volunteering_mission/${mission_id}`,
            type: 'POST',
            data: { count: count - 1, request_for: "next_volunteers", mission_id: mission_id, user_id: user_id },
            success: function (result) {
                $('.volunteers').empty().append(result.recent_volunteers.result)
                $('.current_volunteers').html(`${one_page_volunteers * (count - 1) + 1}-${one_page_volunteers * count >= result.total_volunteers ? result.total_volunteers : one_page_volunteers * count} of recent ${result.total_volunteers} volunteers`)
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}

