toastr.options = {
    "closeButton": true,
    "debug": false, "newestOnTop": false, "progressBar": true, "positionClass": "toast-top-right", "preventDuplicates": false, "onclick": null, "showDuration": "300", "hideDuration": "500",
    "timeOut": "4000", "extendedTimeOut": "1000", "showEasing": "swing", "hideEasing": "swing", "showMethod": "slideDown", "hideMethod": "slideUp"
}


//mouse over & out effect=======================================================


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

//limatation of story lenght===========================================================
const editor = (StoryId) => {
    CKEDITOR.replace(`editor-${StoryId}`, {
        maxLength: 40000,
        toolbar: [
            { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike'] },
            { name: 'clipboard', items: ['RemoveFormat'] }
        ]
    });
}




//pagination class changes=================================================================


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





//date change according to database 
function convertDate(inputFormat) {
    function pad(s) { return (s < 10) ? '0' + s : s; }
    var d = new Date(inputFormat)
    return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('/')
}





const SearchStories = () => {
  
    var key = document.getElementById('SearchStory').value
    if (key.length > 3) {
        console.log("das")
        key = key;
        $.ajax({
            url: '/stories',
            type: 'POST',
            data: { key: key },
            success: function (result) {
                $(".stories").empty().append(result.stories.result)
                setpages();

            },
            error: function () {
                console.log("error updating variable");
            }
        })
    }
    else {
        $.ajax({
            url: '/stories',
            type: 'POST',
            data: {  },
            success: function (result) {
                Stories(result.stories.result, result.length)
                setpages();

            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
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
        media.push(video)
    }
    $(`.gallary-${id}`).find('.main-image').each(function (i, item) {
        media.push(item.src)
    })
    if (video.trim().length > 0 && video.trim().length < 300) {
        if (video.split('.').includes("youtube")) {
            if (title.trim().length > 50 && title.trim().length < 255 && date.length != 0
                && Date.parse(current_date) >= Date.parse(compareddate) && mystory.trim().length > 70 && mystory.trim().length < 40000 && $(`.gallary-${id}`).find('.main-image').length != 0) {
                $.ajax({
                    url: '/stories/share',
                    type: 'POST',
                    data: { story_id: id, mission_id: mission_id, title: title, published_date: date.toString(), mystory: mystory, media: media, type: type },
                    success: function (result) {
                        if (result.success) {
                            $(`#edit-story-${id}`).addClass('d-none')
                            $(`#edit-${id}`).modal('hide');
                            toastr.success("story change")
                        }
                    },
                    error: function () {
                        alert('some error accured')
                    }
                })
                $('#alert').removeClass('d-block').addClass('d-none')
                $('#alert-video-url').removeClass('d-block').addClass('d-none')

            }
            else {
                $('#alert').removeClass('d-none').addClass('d-block')
                $('#alert-video-url').removeClass('d-block').addClass('d-none')
            }
        }
        else {
            $('#alert').removeClass('d-block').addClass('d-none')
            $('#alert-video-url').removeClass('d-none').addClass('d-block')
        }
    }
    else {
        if (title.trim().length > 50 && title.trim().length < 255 && date.length != 0
            && Date.parse(current_date) >= Date.parse(compareddate) && mystory.trim().length > 70 && mystory.trim().length < 40000 && $(`.gallary-${id}`).find('.main-image').length != 0) {
            $.ajax({
                url: '/stories/share',
                type: 'POST',
                data: { story_id: id, mission_id: mission_id, title: title, published_date: date.toString(), mystory: mystory, media: media, type: type },
                success: function (result) {
                    if (result.success) {
                       
                        if (type == "PENDING") {
                            toastr.success("story approve request send")
                        }
                        toastr.info("shortly, you are re-direct to the story page")
                        setTimeout(function () {
                            location.reload();
                        }, 4000);
                        //$(`#edit-story-${id}`).addClass('d-none')
                        //$(`#edit-${id}`).modal('hide');
                    }
                },
                error: function () {
                    alert('some error accured')
                }
            })
            $('#alert').removeClass('d-block').addClass('d-none')
            $('#alert-video-url').removeClass('d-block').addClass('d-none')
        }
        else {
            $('#alert').removeClass('d-none').addClass('d-block')
            $('#alert-video-url').removeClass('d-block').addClass('d-none')
        }
    }

}


let pageindex = 0;
const loadstories = (stories) => {
    $('.stories').empty().append(stories)
}
//pagination==========================================================================================================


//page number=============================================================================================
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

//previous page and next page===============================================================================
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


//first and last page======================================================================================================
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
