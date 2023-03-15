var count=1
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
    if (comment.length > 3) {
        $.ajax({
            url: `/volunteering_mission/${mission_id}`,
            type: 'POST',
            data: { user_id: user_id, mission_id: mission_id, comment: comment, length: length },
            success: function (result) {
                load_comments(result.comments)
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}
const load_comments = (comments) => {
    $.each(comments, function (i, item) {
        var comment = "<div class='d-flex mt-3 bg-white align-items-center'>" +
            "<img class='rounded-circle usercomment-image ms-2' src='/images/volunteer1.png' alt='' />" +
            "<div class='d-flex flex-column ms-2'>" +
            " <span>" + `${item.user.firstName} ${item.user.lastName}` + "</span>" +
            "<span>" + item.user_Comment.createdAt.slice(0,10) + "</span>" +
            "<span class='mt-2'>" + item.user_Comment.user_Comment + "</span>" +
            "</div>" +
            " </div>"
        $('.user-comments').append(comment);
    })
}

const apply_for_mission = (user_id, mission_id) => {
    $.ajax({
        url: `/volunteering_mission/${mission_id}`,
        type: 'POST',
        data: { user_id: user_id, mission_id: mission_id, request_for: "mission" },
        success: function (result) {
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}

const prev_volunteers = (mission_id) => {
    var one_page_volunteers = 9
    if (count > 1) {
        count--;
        $.ajax({
            url: `/volunteering_mission/${mission_id}`,
            type: 'POST',
            data: { count: count - 1, request_for: "next_volunteers", mission_id: mission_id },
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
const next_volunteers = (max_page, mission_id) => {
    var one_page_volunteers=9
    if (count < max_page) {
        count++;
        $.ajax({
            url: `/volunteering_mission/${mission_id}`,
            type: 'POST',
            data: { count: count - 1, request_for: "next_volunteers", mission_id: mission_id },
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
const add_to_favourite = (user_id,mission_id) => {
    $.ajax({
        url: `/volunteering_mission/${mission_id}`,
        type: 'POST',
        data: { request_for: "add_to_favourite", mission_id: mission_id, user_id: user_id },
        success: function (result) {
            if (result.success) {
                $('.heart').removeAttr('src').attr('src', '/images/red-heart.png')
            }
            else {
                $('.heart').removeAttr('src').attr('src', '/images/heart1.png')
            }
           },
        error: function () {
            console.log("Error updating variable");
        }
    })
}
const rating = (rating,user_id,mission_id) => {
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
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }

}
