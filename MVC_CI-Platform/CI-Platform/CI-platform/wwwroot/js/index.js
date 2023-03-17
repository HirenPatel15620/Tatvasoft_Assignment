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
let pageindex = 0;
var view="grid"
var clearall = "<span class='ms-1 clear-all'>" + "Clear All" + "</span>";
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
    view="list"
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
    view="grid"
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
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase(), page_index: pageindex },
        success: function (result) {
            loadmissions(result.mission.result, result.length)
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
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase(), page_index: pageindex },
        success: function (result) {
            loadmissions(result.mission.result, result.length)
            loadcities(result.city.result);
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
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase(), page_index: pageindex },
        success:  function (result) {
            loadmissions(result.mission.result, result.length)
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
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase(), page_index: pageindex },
        success:  function (result) {
            loadmissions(result.mission.result, result.length)
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}


//cities according to coutry
const loadcities = (cities) => {
    $('.city').empty().append(cities)
}


//load filtered missions
const loadmissions = (missions, length) => {
    if (length === 0) {
        $('.no-mission-found').removeClass("d-none").addClass("d-flex flex-column");
        $('.missions').empty();
    }
    else {
        $('.explore').find('b').empty().append(`${length} Missions`)
        $('.no-mission-found').addClass("d-none").removeClass("d-flex flex-column");
        $('.missions').empty().append(missions)
        if (view == "list") {
            listview()
        }
        else {
            gridview()
        }
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
            data: { key: key, page_index: pageindex },
            success: function (result) {
                loadmissions(result.mission.result, result.length)
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
            data: { countries: countries, cities: cities, themes: themes, skills: skills, page_index: pageindex },
            success: function (result) {
                loadmissions(result.mission.result,result.length)
            },
            error: function () {
                console.log("Error updating variable");
            }
        })
    }
}


//sort missions
const sort_by = (user_id) => {
    var selected = $('#sort').find(':selected').text();
    if (selected != "Sort By") {
        $.ajax({
            url: '/home',
            type: 'POST',
            data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase(), page_index: pageindex, user_id: user_id },
            success: function (result) {
                loadmissions(result.mission.result,result.length)
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
    skills = []

    $.ajax({
        url: '/home',
        type: 'POST',
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase(), page_index: pageindex },
        success: function (result) {
            loadmissions(result.mission.result, result.length)
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
    $(`#${id.slice(6)}`).prop('checked', false);
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
        data: { countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase(), page_index: pageindex },
        success: function (result) {
            loadmissions(result.mission.result,result.length)
        },
        error: function () {
            console.log("Error updating variable");
        }
    })
}

const pagination = (page_index) => {
    var selected = $('#sort').find(':selected').text();
    pageindex = page_index - 1;
    $('.pagination li span').each(function (i, item) {
            item.classList.remove('page-active')
    })
    $(`#page-${page_index}`).addClass('page-active')
    $.ajax({
        url: '/home',
        type: 'POST',
        data: { page_index: page_index - 1, countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
        success: function (result) {
            loadmissions(result.mission.result, result.length)
        },
        error: function (e) {
            console.log(e);
        }
    })
}

const prev = () => {
    var current_page;
    var selected = $('#sort').find(':selected').text();
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
        url: '/home',
            type: 'POST',
            data: { page_index: current_page - 2, countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase()},
        success: function (result) {
            loadmissions(result.mission.result, result.length)
        },
        error: function (e) {
            console.log(e);
        }
    })
    }
}

const next = (max_page) => {
    var current_page;
    var selected = $('#sort').find(':selected').text();
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
            url: '/home',
            type: 'POST',
            data: { page_index: current_page, countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
            success: function (result) {
                loadmissions(result.mission.result, result.length)
            },
            error: function (e) {
                console.log(e);
            }
        })
    }
}

const first_page = () => {
    var current_page;
    var selected = $('#sort').find(':selected').text();
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
            url: '/home',
            type: 'POST',
            data: { page_index: 0, countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase() },
            success: function (result) {
                loadmissions(result.mission.result, result.length)
            },
            error: function (e) {
                console.log(e);
            }
        })
    }
}

const last_page = (max_page) => {
    var current_page;
    var selected = $('#sort').find(':selected').text();
    $('.pagination li span').each(function (i, item) {
        if (item.classList.contains('page-active')) {
            current_page = i - 1;
            if (current_page !== max_page) {
                $('.pagination li span').eq(max_page+1).addClass('page-active')
                item.classList.remove('page-active')
                return false
            }
        }
    })
    if (current_page !== max_page) {
        pageindex = max_page-1
        $.ajax({
            url: '/home',
            type: 'POST',
            data: { page_index: max_page - 1, countries: countries, cities: cities, themes: themes, skills: skills, sort_by: selected.toLowerCase()},
            success: function (result) {
                loadmissions(result.mission.result, result.length)
            },
            error: function (e) {
                console.log(e);
            }
        })
    }
}
   
