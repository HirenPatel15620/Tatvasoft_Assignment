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



function DeleteRecord(userid) {


    $('#deleterec').modal('show');
    $('#userid').val(userid);
}

function DeleteRecord(cmsid) {

    $('#deleterec').modal('show');
    $('#cmsid').val(cmsid);
}
