// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//list & grid view JS

//$(document).ready(function () {
//    $("#grid").click(function () {
//        $(".missions-view").addClass("col-sm-6 col-lg-4 d-flex");
//        $(".col-listview-img").removeClass("col-4");
//        $(".col-listview-body").removeClass("col-8");
//        $(".col-listview-img").addClass("col-12");
//        $(".col-listview-body").addClass("col-12");
//        $(".list").removeClass('list-view');
//        $("#list").removeClass('grid-active');
//        $("#grid").addClass('grid-active');
//    });
//    $("#list").click(function () {
//        $(".missions-view").removeClass("col-sm-6 col-lg-4 d-flex");
//        $(".col-listview-img").addClass("col-4");
//        $(".col-listview-body").addClass("col-8 pb-3 ps-0");
//        $(".col-listview-img").removeClass("col-12");
//        $(".col-listview-body").removeClass("col-12");
//        $(".list").addClass('list-view');
//        $("#list").addClass('grid-active');
//        $("#grid").removeClass('grid-active');
//    });
//});



function GetAllNotification() {
 

    var userid = $('#usrid').val();
  
    if (userid != "0") {
        $.ajax({
            type: 'POST',
            url: "/home/GetNotificationofUser",
            data: { 'userid': parseInt(userid) },
            success: function (res) {
                $('#notification').html($($.parseHTML(res)).filter("#notificationpartial")[0].innerHTML);
                //$('#notification').modal('show');
            },
            error: function (data) {
                alert("Error Get Notification for user");
            }
        });
    }
}
function ClearAllNotification() {

    var userid = $('#usrid').val();
    if (userid != "0") {
        $.ajax({
            type: 'POST',
            url: "/home/ClearAllNotification",
            data: { 'userid': parseInt(userid) },
            success: function (res) {
                $(".notification-div").addClass("d-none");

            },
            error: function (data) {
                alert("some Error from Notification Partial");
            }
        });
    }
}



function MakeReadedNotification(usernotificationid) {
    $.ajax({
        url: "/home/MakeReadedNotification",
        type: 'POST',
        data: { 'usernotificationid': usernotificationid },
        success: function (res) {
            $('.unreaded-' + res).addClass('d-none');
            $('.readed-' + res).removeClass('d-none');
            GetNotificationCount();

        },
        error: function (data) {
            alert("some error from read-unread.");
        }
    });
}

function MakeReadUnreadNotification(usernotificationid) {
    $.ajax({
        url: "/Home/MakeReadedNotification",
        type: 'POST',
        data: { 'usernotificationid': usernotificationid },
        success: function (res) {
            $('.unreaded-' + res).toggleClass('d-none');
            $('.readed-' + res).toggleClass('d-none');
            GetNotificationCount();
            //toastr.success("Notification marked as read", "EVPP Says", { timeOut: 5000, "positionClass": "toast-top-right", "closeButton": true, "progressBar": true });
        },
        error: function (data) {
            alert("some error from read-unread.");
        }
    });
}

function SaveNotificationSettings() {
    var settingsarray = [];
    $("input:checkbox[name=settingoption]:checked").each(function () {
        settingsarray.push($(this).val());
    });
    $.ajax({
        url: "/home/SaveNotificationSettings",
        type: 'POST',
        data: { 'settingsarray': settingsarray },
        success: function (res) {
            $('.notification-setting').addClass('d-none');
            $('.notification-list').removeClass('d-none');
        },
        error: function (data) {
            alert("some error from SaveNotificationSettings.");
        }
    });
    console.log(settingsarray);
}




function GotoNotificationSettings() {
    $('.notification-list').addClass('d-none');
    $('.notification-setting').removeClass('d-none');
}
function GotoNotificationList() {
    $('.notification-setting').addClass('d-none');
    $('.notification-list').removeClass('d-none');
}
$("#notification-btn").on("click", function (e) {
    e.stopPropagation()
    $(".notification-div").toggleClass("d-none");
    GetAllNotification();
});
$(document).on("click", function (e) {
    if ($(e.target).is(".notification-div, .notification-div *") === false) {
        $(".notification-div").addClass("d-none");
    }
});