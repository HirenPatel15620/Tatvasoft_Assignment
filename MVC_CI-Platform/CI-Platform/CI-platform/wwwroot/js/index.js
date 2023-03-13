let items = document.getElementsByClassName("item")
let cards = document.getElementsByClassName("thumbnail")
let img_event = document.getElementsByClassName('img-event')
let list = document.getElementById("list")
let grid = document.getElementById("grid")
let cities = []
let countries = []
let themes = []
let skills = []
let country_count = 0
let city_count = 0;
let theme_count = 0;
let skill_count = 0;
var clearall = "<span class='ms-1 clear-all'>" + "Clear All" + "</span>";
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
            city_count--;
        }
    }
    $.ajax({
        url: '/home',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
        success: function (result) {
            loadmissions(result.missions)
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
            $mybadge.find('img').attr('onclick', `remove_badges("badge-${name}","country")`)
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
            country_count--;
            
        }
    }
    $.ajax({
        url: '/home',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
        success: async function(result) {
            if (result.missions.length>0) {
                await loadcities(result.missions[0].cities);
                await loadmissions(result.missions)
            }
        },
        error: function () {
            console.log("Error updating variable");
        }
    });
}

//filters by themes
const addthemes = (name,type) => {
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
            theme_count--;
        }
    }
    $.ajax({
        url: '/home',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
        success: async function (result) {
            await loadmissions(result.missions)
        },
        error: function () {
            console.log("Error updating variable");
        }
        })
}

//filters by skills
const addskills = (name,type) => {
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
            skill_count--;
        }
    }
    $.ajax({
        url: '/home',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
        success: async function (result) {
            await loadmissions(result.missions)
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}


//cities according to coutry
const loadcities = (cities) => {
    $('.city').empty()
     $.each(cities, function (i, item) {
         var data = "<span class='dropdown-item'>"
             + "<input class='form-check-input' type='checkbox'>"
             + "<label class='form-check-label ms-1' for='flexCheckDefault'>"
             + item.name
             + "</label>"
             + "</span>";
         $('.city').append(data)
         $('.city span').each(function () {
             $('.city span').eq(i).find('input').attr('id', item.name)
             $('.city span').eq(i).find('input').attr('onchange', `addcities('${item.name}')`)
         })
     })
}


//load filtered missions
const loadmissions = (missions) => {
    if (missions.length == 0) {
        $('.missions').empty()
        var no_mission ="<p>"+
                "No Mission Found"
            + "</p>" +
            "<button class='applyButton btn'>" + "Submit New Mission" + "<img src='images/right-arrow.png' alt=''>"
                + "</button>"
        $('.no-mission-found').append(no_mission);
    }
    else {
        $('.missions').empty()
        $('.no-mission-found').empty();
        $.each(missions, function (i, item) {
            var data = "<div class='item col-md-6 col-lg-4 col-sm-6 mt-3'>"
                + "<div class='thumbnail card d-flex'>"
                + "<div class='img-event'>"
                + " <img class='group list-group-image w-100 h-100' src='' alt='' />"
                + "<div class='location-img'>"
                + "<img class='text-light' src='images/pin.png' alt=''>"
                + "<span class='text-light ms-1'>"
                + item.mission_city
                + "</span>"
                + "</div>"
                + "<button class='like-img border-0'>"
                + "<img class='text-light' src='images/heart.png' alt=''>"
                + "</button>"
                + "<button class='stop-img border-0'>"
                + "<img class='text-light' src='images/user.png' alt=''>"
                + " </button>"
                + "<button class='mission-theme border-0'>"
                + "<span class='p-2'>"
                + item.mission_theme
                + "</span > "
                + " </button>"
                + " </div>"
                + " <div class='caption card-body'>"
                + "<h4 class='group card-title inner list-group-item-heading'>"
                + item.missions.title.slice(0, 50) + "..." + "</h4>"
                + " <p class='group inner list-group-item-text'>"
                + item.missions.description.slice(0, 100) + "....." + "</p>"
                + "<div class='d-flex list-view justify-content-between'>"
                + " <span class='organization'>" + item.missions.organizationName + "</span>"
                + "<div class='rating'>"

                + "<img src='images/selected-star.png' alt=''>"
                + " <img src='images/selected-star.png' alt=''>"
                + " <img src='images/selected-star.png' alt=''>"
                + " <img src='images/star.png' alt=''>"
                + " <img src='images/star.png' alt=''>"
                + " </div>"
                + "</div>"
                + "<div class='duration-seats-info mt-4'>"
                + " <div class='duration'>"
                + " </div>"
                + " </div>"
                + "<div class='d-flex justify-content-between border-bottom mt-3'>"
                + " <div class='Seats d-flex align-items-center'>"
                + "<img src='images/Seats-left.png' alt=''>"
                + " <span>" + item.missions.avbSeat + "<p>" + "seats left" + "</p>"
                + " </span>"
                + " </div>"
                + " <div class='deadline d-flex align-items-center'>"
                + "</div>"
                + " </div>"
                + " <div class='d-flex justify-content-center mt-4'>"
                + "<button class='applyButton btn'>" + "Apply" + "<img src='images/right-arrow.png' alt=''>"
                + "</button>"
                + "</div>"
                + " </div>"
                + "</div>"
                + " </div>"

            // some operations.
            if ($('#list').hasClass('view')) {
                var $mydata = $(data);
                $mydata.removeClass('col-md-6 col-lg-4 col-sm-6').addClass('col-lg-12 col-md-12 col-sm-12 mb-5')
                $mydata.find('div').eq(0).removeClass('card')
                $mydata.find('div').eq(1).addClass('list_image')
                data = $mydata
            }
            $('.missions').append(data)
            $('.img-event').eq(i).find('img').eq(0).attr('src', item.image.mediaPath)
            if (item.missions.missionType === "GOAL") {
                var achieved = "<img src='images/achieved.png' alt=''>"
                    + "<div class='w-100'>"
                    + "<div class='progress' style='width:90%;margin-left:10px;'>"
                    + "<div class='progress-bar' role='progressbar'' aria-valuenow='75' aria-valuemin='0' aria-valuemax='100'>" + "</div>"
                    + "</div>"
                    + "<p class='text-muted'>" + item.missions.achieved + " achieved" + "</p>"
                    + " </div>"
                var goalobject = parseInt(item.missions.goalObject.match(/\d/g).join(''), 10)
                var percent = (item.missions.achieved / goalobject) * 100;
                var goal = "<p id='duration-txt' style='margin-bottom: 0;'>" + item.missions.goalObject + "</p>"
                $('.duration').eq(i).append(goal)
                $('.deadline').eq(i).append(achieved)
                $('.deadline').eq(i).find('.progress-bar').css("width", percent + "%")
            }
            else {
                var deadline = "<img src='images/deadline.png' alt=''>"
                    + "<span>" + item.missions.deadline.slice(0, 10) +
                    "<p>" + "Deadline" + "</p>" + "</span>"
                var time = "<p id='duration-txt' style='margin-bottom: 0;'>" + "From " + item.missions.startDate.slice(0, 10) + " To " + item.missions.endDate.slice(0, 10) + "</p>"
                $('.duration').eq(i).append(time)
                $('.deadline').eq(i).append(deadline)
            }
        })
    }
}

//load search missions
const search_missions = () => {
    var key = document.getElementById('floatingSearch').value
    if (key.length > 3) {
        key = key.toLowerCase();
        $.ajax({
            url: '/home',
            type: 'POST',
            data: { key },
            success: function (result) {
                loadmissions(result.missions)
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
                loadmissions(result.missions)
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}


//sort missions
const sort_by = () => {
    var selected = $('#sort').find(':selected').text();
    if (selected != "Sort By") {
        $.ajax({
            url: '/home',
            type: 'POST',
            data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
            success: function (result) {
                loadmissions(result.missions)
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}


//clear all
const clear_all = () => {
    var selected = $('#sort').find(':selected').text();
    countries = []
    cities = []
    themes = []
    skills=[]
    $.ajax({
        url: '/home',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
        success: function (result) {
            loadmissions(result.missions)
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
    $('.all-choices').empty()
    $('#clear-all').empty()
}

//remove badges
const remove_badges = (id, badge_type) => {
    var selected = $('#sort').find(':selected').text();
    $('.all-choices').find(`#${id.replace(/\s/g, '')}`).remove()
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
            loadmissions(result.missions)
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}


   
