$(document).ready(function () {
    $("#grid").click(function () {
        $(".missions-view").addClass("col-sm-6 col-lg-4");
        $(".col-listview-img").removeClass("col-4");
        $(".col-listview-body").removeClass("col-8");
        $(".col-listview-img").addClass("col-12");
        $(".col-listview-body").addClass("col-12");
        $(".list").removeClass('list-view');
        $("#list").removeClass('grid-active');
        $("#grid").addClass('grid-active');
    });
    $("#list").click(function () {
        $(".missions-view").removeClass("col-sm-6 col-lg-4");
        $(".col-listview-img").addClass("col-4");
        $(".col-listview-body").addClass("col-8 pb-3 ps-0");
        $(".col-listview-img").removeClass("col-12");
        $(".col-listview-body").removeClass("col-12");
        $(".list").addClass('list-view');
        $("#list").addClass('grid-active');
        $("#grid").removeClass('grid-active');
    });
});