toastr.options = {
    "closeButton": true,
    "debug": false, "newestOnTop": false, "progressBar": true, "positionClass": "toast-top-right", "preventDuplicates": false, "onclick": null, "showDuration": "300", "hideDuration": "1000",
    "timeOut": "2000", "extendedTimeOut": "1000", "showEasing": "swing", "hideEasing": "swing", "showMethod": "slideDown", "hideMethod": "slideUp"
}



const co_workers = []
const add_coworkers = (id) => {
    id = parseInt(id.slice(9))
    if (!co_workers.includes(id)) {
        co_workers.push(id)
    }
    else {
        co_workers.splice(co_workers.indexOf(id), 1)
    }
}

const recommend_to_coworkers = (user_id, story_id) => {
    if (co_workers.length > 0) {
        $.ajax({
            url: `/stories/detail/${story_id}`,
            type: 'POST',
            data: { co_workers: co_workers, user_id: user_id, story_id: story_id }, 
            success: function (result) {
                toastr.success("mail sent successfully")
            },
            error: function () {
                console.log("Error updating variable");

            }
        })
    }
}