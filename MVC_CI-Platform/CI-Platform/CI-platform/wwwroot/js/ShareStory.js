var count = 0
$(function () {
    $("#datepicker").datepicker();
});
CKEDITOR.replace('editor', {
        maxLength: 40000,
        toolbar: [
            { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike'] },
            { name: 'clipboard', items: ['RemoveFormat'] }

        ]
    });

function loadimages() {
    var image = document.getElementById('images').files
    var images_count = image.length
    if (images_count <= 20) {
        if (images_count == 1) {
            const div = document.createElement('div')
            const img = document.createElement('img')
            const close_div = document.createElement('div')
            close_div.className = "bg-black close d-flex justify-content-center align-items-center"
            const close_img = document.createElement('img')
            close_img.src = "/images/cancel.png"
            div.id = `image-${count}`
            div.className = "main-image-div"
            img.src = URL.createObjectURL(image[0])
            img.className = "main-image"
            $('.gallary').append(div)
            $(`#image-${count}`).append(img)
            close_div.append(close_img)
            close_div.onclick = function () { this.parentNode.remove() }
            $(`#image-${count}`).append(close_div)
            count++
        }
        else {
            for (var i = 0; i < images_count; i++) {
                const div = document.createElement('div')
                const img = document.createElement('img')
                const close_div = document.createElement('div')
                close_div.className = "bg-black close d-flex justify-content-center align-items-center"
                const close_img = document.createElement('img')
                close_img.src = "/images/cancel.png"
                div.id = `image-${i}`
                div.className = "main-image-div"
                img.src = URL.createObjectURL(image[i])
                img.className = "main-image"
                $('.gallary').append(div)
                $(`#image-${i}`).append(img)
                close_div.append(close_img)
                close_div.onclick = function () { this.parentNode.remove() }
                $(`#image-${i}`).append(close_div)
                count = i + 1
            }
        }
    }
}