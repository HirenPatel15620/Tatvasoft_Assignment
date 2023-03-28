//mouse hover effect=======================================================================
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



var count = 0

const editor = (StoryId) => {
    CKEDITOR.replace(`editor-${StoryId}`, {
        maxLength: 40000,
        //toolbar: [
        //    { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike'] },
        //    { name: 'clipboard', items: ['RemoveFormat'] }
        //]
    });
}

const next = () => {
    $('.prev').removeClass('d-block').addClass('d-none')
    $('.next').removeClass('d-none').addClass('d-block')
    $('.page-1').addClass('d-none').removeClass('d-block')
    $('.page-2').removeClass('d-none').addClass('d-block')
}
const prev = () => {
    $('.prev').removeClass('d-none').addClass('d-block')
    $('.next').removeClass('d-block').addClass('d-none')
    $('.page-1').addClass('d-block').removeClass('d-none')
    $('.page-2').removeClass('d-block').addClass('d-none')
}
const remove = (id) => {
    document.getElementById(id).remove()
}

function convertDate(inputFormat) {
    function pad(s) { return (s < 10) ? '0' + s : s; }
    var d = new Date(inputFormat)
    return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('/')
}
function load_userimages(id) {
    var image = document.getElementById(`images-${id}`).files
    var images_count = $(`.gallary-${id}`).find('.main-image').length
    if (images_count + image.length <= 20 && image.length <= 20) {
        if (images_count == 1) {
            var fr = new FileReader()
            const div = document.createElement('div')
            const img = document.createElement('img')
            const close_div = document.createElement('div')
            close_div.className = "bg-black close d-flex justify-content-center align-items-center"
            const close_img = document.createElement('img')
            close_img.src = "/images/cancel.png"
            div.id = `story-${id}-user-newimage-${count}`
            div.className = "main-image-div"
            fr.readAsDataURL(image[0])
            fr.onload = () => {
                img.src = fr.result
            }
            img.className = "main-image"
            $(`.gallary-${id}`).append(div)
            $(`#story-${id}-user-newimage-${count}`).append(img)
            close_div.append(close_img)
            close_div.onclick = function () { this.parentNode.remove() }
            $(`#story-${id}-user-newimage-${count}`).append(close_div)
            count++
        }
        else {
            for (var i = 0; i < image.length; i++) {
                let fr = new FileReader()
                fr.onload = () => {
                    const div = document.createElement('div')
                    const img = document.createElement('img')
                    const close_div = document.createElement('div')
                    close_div.className = "bg-black close d-flex justify-content-center align-items-center"
                    const close_img = document.createElement('img')
                    close_img.src = "/images/cancel.png"
                    div.id = `story-${id}-user-newimage-${count}`
                    div.className = "main-image-div"
                    img.src = fr.result
                    img.className = "main-image"
                    $(`.gallary-${id}`).append(div)
                    $(`#story-${id}-user-newimage-${count}`).append(img)
                    close_div.append(close_img)
                    close_div.onclick = function () { this.parentNode.remove() }
                    $(`#story-${id}-user-newimage-${count}`).append(close_div)
                    count++
                }
                fr.readAsDataURL(image[i])
            }
        }
    }
}

function poststory(type, id, mission_id) {
    var current_date = new Date()
    var title = $(`#edit-${id}`).find('.title').val()
    var date = convertDate($(`#edit-${id}`).find('#datepicker').val())
    var compareddate = new Date($(`#edit-${id}`).find('#datepicker').val())
    var mystory = CKEDITOR.instances[`editor-${id}`].getData();
    var video = $(`#edit-${id}`).find(`.video`).val()
    var media = []
    if (video.trim().length > 3) {
        media.push(video_url)
    }
    $(`.gallary-${id}`).find('.main-image').each(function (i, item) {
        media.push(item.src)
    })
    if (title.trim().length > 50 && title.trim().length < 255 && date.length != 0
        && Date.parse(current_date) >= Date.parse(compareddate) && mystory.trim().length > 70 && mystory.trim().length < 40000 && $(`.gallary-${id}`).find('.main-image').length != 0) {
        $.ajax({
            url: '/stories/share',
            type: 'POST',
            data: { story_id: id, mission_id: mission_id, title: title, published_date: date.toString(), mystory: mystory, media: media, type: type },
            success: function (result) {
                if (result.success) {
                    $("#edit-story").addClass('d-none')
                    $(`#edit-${id}`).modal('hide');
                }
            },
            error: function () {
                alert('some error accured')
            }
        })
        $('#alert').removeClass('d-block').addClass('d-none')

    }
    else {
        $('#alert').removeClass('d-none').addClass('d-block')
    }

}



//search mission=================================================================================

const search_missions = () => {
    
    var key = document.getElementById('floatingSearch').value
    if (key.length > 3) {
        key = key.toLowerCase();
        $.ajax({
            url: '/home',
            type: 'POST',

            data: { key: key },
            success: function (result) {
                loadmissions(result.mission.result, result.length)
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
    else {
        alert("search is not loading");
    }
}
key.addEventListener("keydown", function (e) {
    if (e.code === "Enter") {
        search_missions()
    }
})







    // pageindex
let pageindex = 0;
const loadstories = (stories) => {
    $('.stories').empty().append(stories)
}


//pagination=============================================================================




const pagination = (page_index) => {
    pageindex = page_index - 1;
    $('.pagination li span').each(function (i, item) {
        item.classList.remove('page-active')
    })
    $(`#page-${page_index}`).addClass('page-active')
    $.ajax({
        url: '/stories',
        type: 'POST',
        data: { page_index: page_index - 1 },
        success: function (result) {
            loadstories(result.next_stories.result)
        },
        error: function (e) {
            console.log(e);
        }
    })
}


const prev_page = () => {
    var current_page;
    $('.pagination li span').each(function (i, item) {
        if (item.classList.contains('page-active')) {
            current_page = i - 1;
            if (current_page !== 1) {
                $('.pagination li span').eq(i - 1).addClass('page-active')
                item.classList.remove('page-active')
            }
        }
    })
    if (current_page !== 1) {
        pageindex = current_page - 2
        $.ajax({
            url: '/stories',
            type: 'POST',
            data: { page_index: current_page - 2 },
            success: function (result) {
                loadstories(result.next_stories.result)
            },
            error: function (e) {
                console.log(e);
            }
        })
    }
}


const next_page = (max_page) => {
    var current_page;
    $('.pagination li span').each(function (i, item) {
        if (item.classList.contains('page-active')) {
            current_page = i - 1;
            if (current_page !== max_page) {
                $('.pagination li span').eq(i + 1).addClass('page-active')
                item.classList.remove('page-active')
                return false
            }
        }
    })
    if (current_page !== max_page) {
        pageindex = current_page
        $.ajax({
            url: '/stories',
            type: 'POST',
            data: { page_index: current_page },
            success: function (result) {
                loadstories(result.next_stories.result)
            },
            error: function (e) {
                console.log(e);
            }
        })
    }
}



const first_page = () => {
    var current_page;
    $('.pagination li span').each(function (i, item) {
        if (item.classList.contains('page-active')) {
            current_page = i - 1;
            if (current_page !== 1) {
                $('.pagination li span').eq(2).addClass('page-active')
                item.classList.remove('page-active')
            }
        }
    })
    if (current_page !== 1) {
        pageindex = 0
        $.ajax({
            url: '/stories',
            type: 'POST',
            data: { page_index: 0 },
            success: function (result) {
                loadstories(result.next_stories.result)
            },
            error: function (e) {
                console.log(e);
            }
        })
    }
}


const last_page = (max_page) => {
    var current_page;
    $('.pagination li span').each(function (i, item) {
        if (item.classList.contains('page-active')) {
            current_page = i - 1;
            if (current_page !== max_page) {
                $('.pagination li span').eq(max_page + 1).addClass('page-active')
                item.classList.remove('page-active')
                return false
            }
        }
    })
    if (current_page !== max_page) {
        pageindex = max_page - 1
        $.ajax({
            url: '/stories',
            type: 'POST',
            data: { page_index: max_page - 1 },
            success: function (result) {
                loadstories(result.next_stories.result)
            },
            error: function (e) {
                console.log(e);
            }
        })
    }
}
