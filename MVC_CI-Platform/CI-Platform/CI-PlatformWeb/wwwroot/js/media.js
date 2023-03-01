$(window).resize(function () {
    if($(window).width() < 871){
        $(".missions-view").addClass("col-sm-6 col-lg-4");
        $(".col-listview-img").removeClass("col-4");
        $(".col-listview-body").removeClass("col-8");
        $(".col-listview-img").addClass("col-12");
        $(".col-listview-body").addClass("col-12");
        $(".list").removeClass('list-view');
        $("#list").removeClass('grid-active');
        $("#grid").addClass('grid-active');
    }
});