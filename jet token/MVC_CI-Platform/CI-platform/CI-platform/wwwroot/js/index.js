toastr.options = {
    "closeButton": true,
    "debug": false, "newestOnTop": false, "progressBar": true, "positionClass": "toast-top-right", "preventDuplicates": false, "onclick": null, "showDuration": "300", "hideDuration": "1000",
    "timeOut": "2000", "extendedTimeOut": "1000", "showEasing": "swing", "hideEasing": "swing", "showMethod": "slideDown", "hideMethod": "slideUp"
}




let items = document.getElementsByClassName("item")
let cards = document.getElementsByClassName("thumbnail")
let img_event = document.getElementsByClassName('img-event')
let list = document.getElementById("list")
let grid = document.getElementById("grid")
//let cities = [document.getElementsByClassName("user-city")[0].innerHTML.trim()]
let cities = [document.getElementsByClassName("user-city")[0].innerHTML.trim()]
let countries = [document.getElementsByClassName("user-country")[0].innerHTML.trim()]
let themes = []
let skills = []
let country_count = 1;
let city_count = 1;
let theme_count = 0;
let skill_count = 0;
let pageindex = 0;
var view = "grid"
var clearall = "<span class='ms-1 clear-all'>" + "Clear All" + "</span>";
var key = document.getElementById('floatingSearch')






//mouse hover effect
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




//list and grid view
function listview() {
    for (var i = 0; i < items.length; i++) {
        items[i].classList.remove("col-lg-4")
        items[i].classList.add("col-lg-12")
        items[i].classList.remove("col-md-6")
        items[i].classList.add("col-md-12")
        items[i].classList.remove("col-sm-6")
        items[i].classList.add("col-sm-12")
        items[i].classList.add("mb-5")
        cards[i].classList.remove("card")
        cards[i].classList.add("d-flex")
        img_event[i].classList.add('list_image')
    }
    grid.classList.remove("view")
    list.classList.add("view")
    list.style.marginLeft = 20 + "px";
    view = "list"
}
function gridview() {
    for (var i = 0; i < items.length; i++) {
        items[i].classList.add("col-lg-4")
        items[i].classList.remove("col-lg-12")
        items[i].classList.add("col-md-6")
        items[i].classList.remove("col-md-12")
        items[i].classList.add("col-sm-6")
        items[i].classList.remove("col-sm-12")
        items[i].classList.remove("mb-5")
        cards[i].classList.remove("d-flex")
        cards[i].classList.add("card")
        img_event[i].classList.remove('list_image')
    }
    grid.classList.add("view")
    list.classList.remove("view")
    list.style.marginLeft = 0 + "px";
    view = "grid"
}


//Apply for mission

const apply_for_mission = (user_id, mission_id) => {
    $.ajax({
        url: `/volunteering_mission/${mission_id}`,
        type: 'POST',
        data: { user_id: user_id, mission_id: mission_id, request_for: "mission" },
        success: function (result) {
            if (result.success) {
                $(`.app-${mission_id}`).empty().append('Request sent<img src="images/right-arrow.png" alt="">' )
                toastr.success("successfully Applied");

            }
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}





//filters by cities
function addcities(name, type) {
    var selected = $('#sort').find(':selected').text();
    var city;
    if (type == 'mobile') {
        city = document.getElementsByClassName(name)[0]
    }
    else {
        city = document.getElementById(name)
    }
    if (city.checked) {
        if (!cities.includes(name)) {
            cities.push(name)
            var badge = "<div class='border rounded-pill mt-3'>"
                + "<span class='p-2'>" + cities[city_count] + "</span>"
                + "<img src='images/cancel.png' class='p-2' alt='not found' />"
                + "</div>";
            var $mybadge = $(badge)
            $mybadge.attr('id', `badge-${name.replace(/\s/g, '')}`)
            $mybadge.find('img').attr('onclick', `remove_badges("badge-${name}","city")`)
            badge = $mybadge
            $('.all-choices').append(badge)
            if ($('.all-choices').find('div').length > 1 && $('.clear-all').length == 0) {
                $myclear = $(clearall)
                $myclear.attr('onclick', 'clear_all()')
                clearall = $myclear
                $('#clear-all').append(clearall)
            }
            city_count++;
        }
    }
    else {
        if (cities.includes(name)) {
            cities.splice(cities.indexOf(name), 1)
            $('.all-choices').find(`#badge-${name.replace(/\s/g, '')}`).remove()
            if ($('.all-choices').find("span").length == 0) {
                page = 0
                $(".clear-all").remove()
            }
            city_count--;
        }
    }
    $.ajax({
        url: '/home',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
        success: function (result) {
            loadmissions(result.mission.result, result.length)
            setpages();

        },
        error: function () {
            console.log("Error updating variable");
        }
    });
}

//filters by countryies
const addcountries = (name, type) => {
    var selected = $('#sort').find(':selected').text();
    var country;
    if (type == 'mobile') {
        country = document.getElementsByClassName(name)[0]
    }
    else {
        country = document.getElementById(name)
    }
    if (country.checked) {
        if (!countries.includes(name)) {
            countries.push(name)
            var badge = "<div class='border rounded-pill mt-3'>"
                + "<span class='p-2'>" + countries[country_count] + "</span>"
                + "<img src='images/cancel.png' class='p-2' alt='not found' />"
                + "</div>";
            var $mybadge = $(badge)
            $mybadge.attr('id', `badge-${name.replace(/\s/g, '')}`)
            //$mybadge.find('img').attr('onclick', `remove_badges("badge-${name}","country")`)
            badge = $mybadge
            $('.all-choices').append(badge)
            if ($('.all-choices').find('div').length > 1 && $('.clear-all').length == 0) {
                $myclear = $(clearall)
                $myclear.attr('onclick', 'clear_all()')
                clearall = $myclear
                $('#clear-all').append(clearall)
            }
            country_count++;
        }
    }
    else {
        if (countries.includes(name)) {
            countries.splice(countries.indexOf(name), 1)
            $('.all-choices').find(`#badge-${name.replace(/\s/g, '')}`).remove()
            if ($('.all-choices').find("span").length == 0) {
                page = 0
                $(".clear-all").remove()
            }
            country_count--;

        }
    }
    $.ajax({
        url: '/home',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
        success: function (result) {
            loadmissions(result.mission.result, result.length)
            loadcities(result.city.result);
            setpages();

        },
        error: function () {
            console.log("Error updating variable");
        }
    });
}

//filters by themes
const addthemes = (name, type) => {
    var selected = $('#sort').find(':selected').text();
    var theme;
    if (type == 'mobile') {
        theme = document.getElementsByClassName(name)[0]
    }
    else {
        theme = document.getElementById(name)
    }
    if (theme.checked) {
        if (!themes.includes(name)) {
            themes.push(name)
            var badge = "<div class='border rounded-pill mt-3'>"
                + "<span class='p-2'>" + themes[theme_count] + "</span>"
                + "<img src='images/cancel.png' class='p-2' alt='not found' />"
                + "</div>";
            var $mybadge = $(badge)
            $mybadge.attr('id', `badge-${name.replace(/\s/g, '')}`)
            $mybadge.find('img').attr('onclick', `remove_badges("badge-${name}","theme")`)
            badge = $mybadge
            $('.all-choices').append(badge)
            if ($('.all-choices').find('div').length > 1 && $('.clear-all').length == 0) {
                $myclear = $(clearall)
                $myclear.attr('onclick', 'clear_all()')
                clearall = $myclear
                $('#clear-all').append(clearall)
            }
            theme_count++;
        }
    }
    else {
        if (themes.includes(name)) {
            themes.splice(themes.indexOf(name), 1)
            $('.all-choices').find(`#badge-${name.replace(/\s/g, '')}`).remove()
            if ($('.all-choices').find("span").length == 0) {
                page = 0
                $(".clear-all").remove()
            }
            theme_count--;
        }
    }
    $.ajax({
        url: '/home',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
        success: function (result) {
            loadmissions(result.mission.result, result.length)
            setpages();

        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}

//filters by skills
const addskills = (name, type) => {
    var selected = $('#sort').find(':selected').text();
    var skill;
    if (type == 'mobile') {
        skill = document.getElementsByClassName(name)[0]
    }
    else {
        skill = document.getElementById(name)
    }
    if (skill.checked) {
        if (!skills.includes(name)) {
            skills.push(name)
            var badge = "<div class='border rounded-pill mt-3'>"
                + "<span class='p-2'>" + skills[skill_count] + "</span>"
                + "<img src='images/cancel.png' class='p-2' alt='not found' />"
                + "</div>";
            var $mybadge = $(badge)
            $mybadge.attr('id', `badge-${name.replace(/\s/g, '')}`)
            $mybadge.find('img').attr('onclick', `remove_badges("badge-${name}","skill")`)
            badge = $mybadge
            $('.all-choices').append(badge)
            if ($('.all-choices').find('div').length > 1 && $('.clear-all').length == 0) {
                $myclear = $(clearall)
                $myclear.attr('onclick', 'clear_all()')
                clearall = $myclear
                $('#clear-all').append(clearall)
            }
            skill_count++;
        }
    }
    else {
        if (skills.includes(name)) {
            skills.splice(skills.indexOf(name), 1)
            $('.all-choices').find(`#badge-${name.replace(/\s/g, '')}`).remove()
            if ($('.all-choices').find("span").length == 0) {
                page = 0
                $(".clear-all").remove()
            }
            skill_count--;
        }
    }
    $.ajax({
        url: '/home',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
        success: function (result) {
            loadmissions(result.mission.result, result.length)
            setpages();

        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}


//cities according to coutry
const loadcities = (upadted_cities) => {
    $('.city').empty().append(upadted_cities)
    for (i = 0; i < cities.length; i++) {
        $(`#${cities[i]}`).attr("checked", "checked")
    }
}


//load filtered missions
const loadmissions = (missions, length) => {
    if (length === 0) {
        $('.no-mission-found').removeClass("d-none").addClass("d-flex flex-column");
        $('.missions').empty();
        $('.explore').addClass("d-none").removeClass("d-block");
        $('.sort').addClass("d-none").removeClass("d-flex")
        $('#pages').addClass("d-none")
    }
    else {
        $('.explore').find('b').empty().append(`${length} Missions`)
        $('.no-mission-found').addClass("d-none").removeClass("d-flex flex-column");
        $('.missions').empty().append(missions)
        $('.sort').addClass("d-flex").removeClass("d-none")
        $('.explore').addClass("d-block").removeClass("d-none");
        $('#pages').removeClass("d-none")
        if (view == "list") {
            listview()
            setpages();

        }
        else {
            gridview()
            setpages();

        }
    }
}

//load search missions
const search_missions = () => {
    var selected = $('#sort').find(':selected').text();
    var key = document.getElementById('floatingSearch').value
    if (key.length > 3) {
        key = key.toLowerCase();
        $.ajax({
            url: '/home',
            type: 'POST',

            data: { key: key, sort_by: selected.toLowerCase() },
            success: function (result) {
                loadmissions(result.mission.result, result.length)
                setpages();

            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
    else {
        $.ajax({
            url: '/home',
            type: 'POST',
            data: { countries: countries, cities: cities, themes: themes, skills: skills },
            success: function (result) {
                loadmissions(result.mission.result, result.length)
                setpages();

            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}
key.addEventListener("keydown", function (e) {
    if (e.code === "Enter") {
        search_missions()
    }
})

//sort missions
const sort_by = (user_id) => {

    var selected = $('#sort').find(':selected').text();
    if (selected != "Sort By") {
        $.ajax({
            url: '/home',
            type: 'POST',
            data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase(), user_id: user_id },
            success: function (result) {
                loadmissions(result.mission.result, result.length)
                setpages();

            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}

const explore_by = (user_id) => {
   
    var selected = $('#explore').find(':selected').text();
    if (selected != "Sort By") {
        $.ajax({
            url: '/home',
            type: 'POST',
            data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase(), user_id: user_id },
            success: function (result) {
                loadmissions(result.mission.result, result.length)
                setpages();

            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}


const clear_all = () => {
    var selected = $('#sort').find(':selected').text();
    $(".filters").find(".form-check-input").prop("checked",false)
    countries = []
    cities = []
    themes = []
    skills=[]
    $.ajax({
        url: '/home',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
        success: function (result) {
            loadmissions(result.mission.result, result.length)
            loadcities(result.city.result)
            $("#pages").empty().append(result.pages.result)
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
    $('.all-choices').empty()
    $('#clear-all').empty()
    city_count = 0
    theme_count = 0
    skill_count = 0
    country_count=0
}







//add to favourite
const add_to_favourite = (user_id, mission_id) => {

    $.ajax({
        url: `/home`,
        type: 'POST',
        data: { mission_id: mission_id, user_id: user_id },
        success: function (result) {
            console.log(result)
            if (result.success) {
                $(`.heart-${mission_id}`).removeAttr('src').attr('src', '/images/red-heart.png')
                $(`.heart-${mission_id}`).css('height', 24)
                toastr.success("add to favourite");
            }
            else {
                $(`.heart-${mission_id}`).removeAttr('src').attr('src', '/images/heart.png')
                $(`.heart-${mission_id}`).css('height', 20)
                toastr.error("Remove from favourite");
            }
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}


//remove badges
const remove_badges = (id, badge_type) => {
    
    var selected = $('#sort').find(':selected').text();
    $(`#${id.slice(6)}`).prop('checked', false);
    $('.all-choices').find(`#${id.replace(/\s/g, '')}`).remove()
    if ($('.all-choices').find("span").length == 0) {
        page = 0
        $(".clear-all").remove()
    }
    if (badge_type == "city") {
        if (cities.includes(id.slice(6))) {
            cities.splice(cities.indexOf(id.slice(6)), 1)
            city_count--;
        }
    }
    else if (badge_type == "country") {
        if (countries.includes(id.slice(6))) {
            countries.splice(countries.indexOf(id.slice(6)), 1)
            country_count--;
        }
    }
    else if (badge_type == "theme") {
        if (themes.includes(id.slice(6))) {
            themes.splice(themes.indexOf(id.slice(6)), 1)
            theme_count--;
        }
    }
    else if (badge_type == "skill") {
        if (skills.includes(id.slice(6))) {
            skills.splice(skills.indexOf(id.slice(6)), 1)
            skill_count--;
        }
    }
    $.ajax({
        url: '/home',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
        success: function (result) {
            loadmissions(result.mission.result, result.length)
            toastr.error(`Removed ${badge_type} badge`);

            setpages();

        },
        error: function () {
            console.log("Error updating variable");
        }
    })


}



//recommand


function recommendmodal() {
    $('#recommendd').modal('show');
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


function recommend(user_id, mission_id) {
    debugger
    if (co_workers.length > 0) {
        $.ajax({
            type: 'POST',
            url: `/User/Home/volunteering_mission`,
            data: { co_workers: co_workers, user_id: user_id, mission_id: mission_id, request_for: "recommend" },
            successworker: function (result) {
                debugger
                console.log(result)
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}







//pagination

var perpagecard = 6;
var currentindex;
var startpageindex;
var endpageindex;

function setpages() {
    let page_add = $("#pagination");
    var cards = $(".card1");
    var pages = Math.ceil(cards.length / perpagecard);
    if (pages < 2) {
        $(page_add).parent().hide();
    }
    else {
        $(page_add).parent().show();
        $(page_add).empty();
        for (let i = 1; i <= pages; i++) {
            $(page_add).append(`<li class="pagination_box" onclick="pagination(${i},${pages})">${i}</li>`);
        }
        $("ul#pagination li:first").addClass("active");
    }
    pagination(1, pages);
}



function pagination(page, totalpages) {
    currentindex = page;

    var pages = totalpages;
    var cards = $(".card1");
    var startpage = (currentindex - 1) * perpagecard;
    var endpage = startpage + perpagecard;
    for (let i = 0; i < cards.length; i++) {
        if (i >= startpage && i < endpage) {
            $(cards[i]).removeClass("d-none");
        }
        else {
            $(cards[i]).addClass("d-none");
        }
    }
    $("ul#pagination li").removeClass("active");
    $("ul#pagination li").eq(currentindex - 1).addClass("active");
    if (pages <= 3) {
        $("ul#pagination li").removeClass("d-none");
        $("li#prevpage-btn").hide();
        $("li#nextpage-btn").hide();
    }
    else {
        startpageindex = currentindex - 2;
        endpageindex = currentindex;
        for (let i = 0; i < pages; i++) {
            if (i >= startpageindex && i <= endpageindex) {
                $("ul#pagination li").eq(i).removeClass("d-none");
            }
            else {
                $("ul#pagination li").eq(i).addClass("d-none");
            }
        }
    }
}


function nextpage() {
    let cards = $(".card1");
    var pages = Math.ceil(cards.length / perpagecard);
    startpageindex++;
    endpageindex++;
    if (startpageindex > pages - 3) {
        startpageindex = pages - 3;
        endpageindex = pages - 1;
    }
    if (startpageindex < 0) {
        startpageindex = 0;
        endpageindex = 2;
    }
    for (let i = 0; i < pages; i++) {
        if (i >= startpageindex && i <= endpageindex) {
            $("ul#pagination li").eq(i).removeClass("d-none");
        }
        else {
            $("ul#pagination li").eq(i).addClass("d-none");
        }
    }
}



function prevpage() {
    let cards = $(".card1");
    var pages = Math.ceil(cards.length / perpagecard);
    startpageindex--;
    endpageindex--;
    if (startpageindex > pages - 3) {
        startpageindex = pages - 3;
        endpageindex = pages - 1;
    }
    if (startpageindex < 0) {
        startpageindex = 0;
        endpageindex = 2;
    }
    for (let i = 0; i < pages; i++) {
        if (i >= startpageindex && i <= endpageindex) {
            $("ul#pagination li").eq(i).removeClass("d-none");
        }
        else {
            $("ul#pagination li").eq(i).addClass("d-none");
        }
    }
}


$(document).ready(function () {
    var pages;
    $("li#firstpage").click(function () {
        let cards = $(".card1");
        pages = Math.ceil(cards.length / perpagecard);
        pagination(1, pages);
    });
    $("li#lastpage").click(function () {
        let cards = $(".card1");
        pages = Math.ceil(cards.length / perpagecard);
        pagination(pages, pages);
    });

});








