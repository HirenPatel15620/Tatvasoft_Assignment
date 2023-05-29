
// (1) -------------------------------------   image slider of left side    -------------------------------------

var left = 1;
var right = 4;

function show() {
    var div = document.getElementById("img-list");
    var list = document.getElementsByClassName("myimg");
    if (list.length > 1) {
        for (let i = 0; i < list.length; i++) {
            list[i].classList.add("d-none");
        }
        for (let i = left; i <= right; i++) {
            $(".myimg").eq(i - 1).removeClass("d-none");
        }
    }
}

function next() {
    left++;
    right++;
    var length = $(".myimg").length;
    console.log(length);
    if (left > length - 3) {
        left = length - 3;
        right = length;
    }
    show();
}

function prev() {
    left--;
    right--;
    if (left < 1) {
        left = 1;
        right = 4;
    }
    show();
}

function select(n) {
    var a = document.getElementsByClassName("mydiv");
    for (let i = 0; i < a.length; i++) {
        a[i].classList.add("d-none");
    }
    $("#" + n).removeClass("d-none");
}







// (2) -------------------------------------   JavaScript code to handle toggling between list and grid views   -------------------------------------   



function list() {
    var grid = document.getElementById("grid-view");
    var list = document.getElementById("list-view");
    list.classList.remove("d-none");
    grid.classList.add("d-none");
}

function grid() {
    var grid = document.getElementById("grid-view");
    var list = document.getElementById("list-view");
    grid.classList.remove("d-none");
    list.classList.add("d-none");
}

   



// (3) -------------------------------------   filtering the missions   -------------------------------------   


$(document).ready(function () {
    var clearAllButton = $('.selected-options .clear-all');
    clearAllButton.hide();

    // Get the dropdown menu element
    const dropdownMenu = $('.dropdown-menu');
    // Prevent the dropdown menu from closing when an option is clicked
    dropdownMenu.on('click', function (event) {
        event.stopPropagation();
    });
});

$(function () {
    var dropdownMenu = $('.dropdown-menu');
    var selectedOptionsList = $('.selected-options ul');
    var clearAllButton = $('.selected-options .clear-all');

    // Handle checkbox click
    dropdownMenu.on('click', 'input[type="checkbox"]', function () {
        var option = $(this).parent();
        var optionValue = $(this).val();

        if ($(this).prop('checked')) {
            // Add the selected option to the displayed list
            selectedOptionsList.append(`<li data-value="${optionValue}" >${option.text()}<span class="remove-option" ">&#x2715;</span></li>`);
        } else {
            // Remove the selected option from the displayed list
            selectedOptionsList.find(`li[data-value="${optionValue}"]`).remove();
        }

        // Show or hide the Clear All button
        if (selectedOptionsList.children().length > 0) {
            clearAllButton.show();
        } else {
            clearAllButton.hide();
        }
    });

    // Handle remove button click
    selectedOptionsList.on('click', 'span.remove-option', function () {
        // Get the value of the option that was removed
        var optionValue = $(this).parent().data('value');

        // Uncheck the corresponding checkbox in the dropdown menu
        dropdownMenu.find(`input[value="${optionValue}"]`).prop('checked', false);

        // Remove the selected option from the displayed list
        $(this).parent().remove();

        // Show or hide the Clear All button
        if (selectedOptionsList.children().length > 0) {
            clearAllButton.show();
        } else {
            clearAllButton.hide();
        }
        temp();
        GetCityByCountry();
    });

     //Handle Clear All button click

    clearAllButton.on('click', function () {
        // Uncheck all the checkboxes in the dropdown menu
        dropdownMenu.find('input[type="checkbox"]').prop('checked', false);

        // Remove all the selected options from the displayed list
        selectedOptionsList.empty();

        // Hide the Clear All button
        clearAllButton.hide();
        temp();
    });
});



// (5) -------------------------------------   tabs of missions    -------------------------------------   

function toggleTab(tabId) {
    // Get all tab content elements
    const tabContents = document.querySelectorAll('.tab-content');

    // Get all tab menu items
    const menuItems = document.querySelectorAll('.tabs li');

    // Hide all tab content
    tabContents.forEach(content => content.style.display = 'none');

    // Remove the 'active' class from all menu items
    menuItems.forEach(item => item.classList.remove('active'));

    // Show the content of the selected tab
    const selectedTab = document.getElementById(tabId);
    selectedTab.style.display = 'block';


    // Add the 'active' class to the clicked menu item
    const clickedMenuItem = document.querySelector(`.tabs li a[href="#${tabId}"]`).parentNode;
    clickedMenuItem.classList.add('active');
}








// (6)  -------------------------------------   volunteers profile crousel   -------------------------------------   

const abc = document.getElementById("vollist");
const prev_vol_Btn = document.querySelector('.prev-vol-btn');
const next_vol_Btn = document.querySelector('.next-vol-btn');
const items = document.querySelectorAll('.vol-item');
const itemsPerPage = 9;
let currentPage = 1;

function showPage(page) {
    const startIndex = (page - 1) * itemsPerPage;
    var endIndex = startIndex + itemsPerPage;
    for (let i = 0; i < items.length; i++) {
       
        if (i >= startIndex && i < endIndex) {
            items[i].style.display = "inline-block";
            if (endIndex > items.length) {
                endIndex = items.length;
                abc.innerHTML = (startIndex + 1) + "-" + endIndex + " of " + items.length + " Recent Volunteers";
            } else {
                abc.innerHTML = (startIndex + 1) + "-" + (endIndex) + " of " + items.length + " Recent Volunteers";
            }
        } else {
            items[i].style.display = "none";
        }
    }
}

   
    function volprevious() {
        if (currentPage > 1) {
            currentPage--;
            showPage(currentPage);
        }
    }


    function volnext() {
        if (currentPage < Math.ceil(items.length / itemsPerPage)) {
            currentPage++;
            showPage(currentPage);
        }
    }
showPage(currentPage);


// (7) -------------------------------------   Password & confirm password validation   -------------------------------------   

function verifyPassword() {
    var pw = document.getElementById("password").value;
    var cpw = document.getElementById("cpassword").value;
    

    if (pw != cpw) {
       
        document.getElementById("Message").innerHTML = "Password does not matched";
        document.getElementById('Message').style.color = 'red';
        document.getElementById('submit').disabled = true;
        document.getElementById('submit').style.opacity = (0.5);
    }
    else {
        document.getElementById('Message').style.color = '';
        document.getElementById('Message').innerHTML = '';
        document.getElementById('submit').disabled = false;
        document.getElementById('submit').style.opacity = (1);
    }

}


// (8) -------------------------------------   SearchBar   -------------------------------------   

function search_animal() {
    let input = document.getElementById('searchb').value.toLowerCase();
    let x = document.getElementsByClassName('SearchElement');

    for (i = 0; i < x.length; i++) {
        let find = x[i].getElementsByClassName('Elements')[0];
        // get the first element with 'animals' class name
        let parentDiv = x[i];
        // get the parent div of the x[i] element
        if (!find.innerHTML.toLowerCase().includes(input)) {
            parentDiv.style.display = "none";
            console.log("hii")
        }
        else {
            parentDiv.style.display = "block";
            console.log("hello")
        }
    }
}


// (9)  -------------------------------------   Validation For Form   -------------------------------------   
$(document).ready(function () {
    $("#reset").submit(function (e) {
        if (!password_verification()) {
            e.preventDefault();
        }
        if (!cpassword_verification()) {
            e.preventDefault();
        }
        
    });
});
$(document).ready(function () {
    $("#register").submit(function (e) {
        if (!email_verification()) {
            // Prevent the form from being submitted
            e.preventDefault();
        }
        if (!password_verification()) {
            e.preventDefault();
        }
        if (!cpassword_verification()) {
            e.preventDefault();
        }
        if (!fname_verification()) {
            e.preventDefault();
        }
        if (!lname_verification()) {
            e.preventDefault();
        }
        if (!phone_verification()) {
            e.preventDefault();
        }
        
    });
});

$(document).ready(function () {
    $("#login").submit(function (e) {
        if (!email_verification()) {
            // Prevent the form from being submitted
            e.preventDefault();
        }
        if (!password_verification()) {
            e.preventDefault();
        }
    });
});

$(document).ready(function () {
    $("#forgot").submit(function (e) {
        if (!email_verification()) {
            // Prevent the form from being submitted
            e.preventDefault();
        }
    });
});
// Email Validation
function email_verification() {
    let email = $("#email");
    if (email.val().trim() == "") {
        $("#email_error").html("Email Is Required");
        return false;
    }
    else if (!isValidEmail(email.val().trim())) {
        $("#email_error").html("Please Enter Valid Email");
        return false;
    }
    else {
        $("#email_error").html("");
        return true;
    }

}
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}



// Password Validation
function password_verification() {
    let password = $("#password").val().trim();
    if (password == "") {
        $("#password_error").html("Password Is Required");
        return false;
    }
    else if (password.length < 8) {
        $("#password_error").html("Password should be Minimum 8 char");
        return false;
    }
    else if (!isValidPassword(password)) {
        $("#password_error").html("Password should be in Form of : Abcd1@ef");
        return false;
    }
    else {
        $("#password_error").html("");
        return true;
    }

}
function isValidPassword(password) {
    const passwordRegex = /^(?=.*[A-Z])(?=.*[\W_])(?=.{8,})/;
        return passwordRegex.test(password);
}

// Confirm Password Validation
function cpassword_verification() {
    let password = $("#cpassword");
    if (password.val().trim() == "") {
        $("#cpassword_error").html("Confirm Password Is Required");
        return false;
    }
    else {
        $("#cpassword_error").html("");
        return true;
    }

}
// First Name Validation
function fname_verification() {
    let fname = $("#fname");
    if (fname.val().trim() == "") {
        $("#fname_error").html("First Name Is Required");
        return false;
    }
    else {
        $("#fname_error").html("");
        return true;
    }

}
// Last Name Validation
function lname_verification() {
    let lname = $("#lname");
    if (lname.val().trim() == "") {
        $("#lname_error").html("Last Name Is Required");
        return false;
    }
    else {
        $("#lname_error").html("");
        return true;
    }

}
// Phone Number Validation

function phone_verification() {
    let phone = $("#phone");
    if (phone.val().trim() == "") {
        $("#phone_error").html("Phone Number is Required");
        return false;
    } else if (phone.val().trim().length !== 10 || isNaN(phone.val().trim())) {
        $("#phone_error").html("Enter a Valid 10-digit Phone Number");
        return false;
    } else {
        $("#phone_error").html("");
        return true;
    }
}




// (10) -------------------------------------   Add & Remove Favourite in Platform Landing Page    -------------------------------------   

function favmission(missionid, data) {
    console.log("dshdsa");
    $.ajax({
        url: "/Platform/FavouriteMission",
        type: "POST",
        data: {
            'mId': missionid,
            'favcheck': data
        },
        success: function (result) {
            // Update the only two div tag with the response
            $('#' + missionid).load(location.href + ' #' + missionid + ' >*');
            $('#' + missionid + "list").load(location.href + ' #' + missionid + 'list' + ' >*');
            //$('#missionReload').load(location.href + ' #missionReload >*');
            //$('#listMissonReload').load(location.href + ' #listMissonReload >*');
            temp();
            Pagination();
        },
        error: function () {
            console.log("error");
        }
    });
}

// (11) -------------------------------------   Add & Remove Favourite in volunteering Page   -------------------------------------   

function favmissionvol(missionid, data) {
    console.log("dshdsa");
    $.ajax({
        url: "/Platform/FavouriteMission",
        type: "POST",
        data: {
            'mId': missionid,
            'favcheck': data
        },
        success: function (result) {
            // Update the only div tag having id=favmission  with the response
            $('#favmission').load(location.href + ' #favmission  >*');
        },
        error: function () {
            console.log("error");
        }
    });
}


// (12) -------------------------------------   Send mail for Recommanded to co-workers in mission   -------------------------------------   

function SendMail(missionid) {
    var inputbox = $("#checkedbox input");
    var userlist = [];

    for (var i = 0; i < inputbox.length; i++) {
        if ($(inputbox[i]).prop('checked')) {
            userlist.push($(inputbox[i]).val());
        }
    }
    if (userlist.length != 0) {
        console.log(userlist);
        $.ajax({
            url: "/Platform/SendMail",
            type: "POST",
            data: {
                'mID': missionid,
                'userId': userlist
            },
            success: function () {
                $("#email_msg").css('color', 'green').html("Email Sent Successfully");
                $("#email_msg1").css('color', 'green').html("Email Sent Successfully");
                toastr.success('Email Sent Successfully', '', {
                });
            },
            error: function () {
                console.log("hi error");
            }
        });
    }
    else {
        
        $("#email_msg").css('color', 'red').html("*Select User to Send Mail");
        $("#email_msg1").css('color', 'red').html("*Select User to Send Mail");

    }
}

// (12) -------------------------------------   Send mail for Recommanded to co-workers in story   -------------------------------------   

function SendMailForStory(storyid) {
    var inputbox = $("#checkedbox input");
    var userlist = [];

    for (var i = 0; i < inputbox.length; i++) {
        if ($(inputbox[i]).prop('checked')) {
            userlist.push($(inputbox[i]).val());
        }
    }
    if (userlist.length != 0) {
        console.log(userlist);
        $.ajax({
            url: "/Story/SendMail",
            type: "POST",
            data: {
                'sID': storyid,
                'userId': userlist
            },
            success: function () {
                $("#email_msg").css('color', 'green').html("Email Sent Successfully");
                $("#email_msg1").css('color', 'green').html("Email Sent Successfully");
                toastr.success('Email Sent Successfully', '', {
                });
            },
            error: function () {
                console.log("hi error");
            }
        });
    }
    else {

        $("#email_msg").css('color', 'red').html("*Select User to Send Mail");
        $("#email_msg1").css('color', 'red').html("*Select User to Send Mail");

    }
}


// (13) -------------------------------------   clear the checkbox on close Button    -------------------------------------   

$(".closebox").click(function () {
    $("#oldpass").val('');
    $("#password").val('');
    $("#cpassword").val('');
    $('input[type="checkbox"]').prop('checked', false);
    //$('#exampleModal').load(location.href + ' #exampleModal >*');
});



// (14) -------------------------------------   Append Send mail button to the modal of recommended   -------------------------------------   

function SendMail1(missionid) {
    $("#delete-btn").remove();
    $("#model-footer").append(`<button type="button" id="delete-btn" class="closebox orangeButton" onclick="SendMail(${missionid})">Send Mail</button>`);
}



/* (*) advanced method of giving onchange() function without html
// Get the ul element
const ulElement = document.querySelector('ul');

// Create a new MutationObserver
const observer = new MutationObserver(() => {
    // Call the filter function
    filter();
});

// Configure the observer to watch for changes to child nodes of the ul element
const config = { childList: true };

// Start observing the ul element for changes
observer.observe(ulElement, config);

// Define the filter function
function filter() {
    // Your filter logic here
}*/



// (16)-------------------------------------   push the drag & drop images on input    ----------------------------------------------------------------//


function adding() {
    console.log('hii');
    console.log(fileNames);
    var fileInput = document.getElementById('file-input');
    const newFileList = new DataTransfer();
    for (const file of fileNames) {
        newFileList.items.add(file);
    }
    fileInput.files = newFileList.files;
}

$(".file-upload img").on('click', function () {
    //console.log('aayo');
    $("#file-input").click();
});




// (17)-------------------------------------   remove  media image when draft story is in edit mode   ----------------------------------------------------------------//
var delImgList = [];
var savedImgList = [];
console.log(savedImgList);

function closeBtnDraft(btn) {
    // Remove the preview element from the preview container
    console.log('hii');
    // Remove the file name from the array
    const index = $('.cancel-btn').index(btn);
    console.log(index);
    delImgList.push(savedImgList[index]);
    savedImgList.splice(index, 1);
    $(btn).closest('.preview').remove();
    submitbuttonremove();
}

// (18)-------------------------------------   validation while submit story   ----------------------------------------------------------------//
$(document).ready(function () {
    $("#storyform").on("submit", function (e) {
        console.log($("#preview-container").html());

        if ($("#title").val().trim() == "" ||
            $("#date").val().trim() == "" ||
            CKEDITOR.instances.editor.getData().trim() == "" ||
            $("#preview-container").html() == "") {
            e.preventDefault();
            toastr.error('All Fields are Required', '', {
            });
        }
        else {
            toastr.success('Story Submitted Successfully', '', {
            });
        }

    });
});


// (19)-------------------------------------    submit button disable on changes of form   ----------------------------------------------------------------//
function submitbuttonremove() {

    console.log("submit");
    $("#previewbtn").addClass("d-none");
    $("#submit").addClass("d-none");
}
function submitbuttonadd() {

    console.log("submit select");
    setTimeout(function () {
        $("#submit").removeClass("d-none");
        $("#previewbtn").removeClass("d-none");
    }, 50); // 0.05 second delay

}


// (20)-------------------------------------    show the draft story of user   ----------------------------------------------------------------//
function SavedDraft() {
    var missionid = parseInt($("#missionId").val(), 10);
    delImgList = [];
    savedImgList = [];
    fileNames = [];
    $.ajax({
        type: "GET",
        url: '/Story/DraftSavedStory',
        data: {
            'missionid': missionid
        },
        success: function (data) {
            if (data != null && data.draftStory.deletedAt == null) {
                console.log(data);
                $("#title").val(data.draftStory.title || '');
                var datetime = new Date(data.draftStory.createdAt) || '';
                var date = datetime.toISOString().slice(0, 10);
                $("#date").val(date);
                $("#previewBtn").attr("href", "/Story/StoryDetails?id=" + data.draftStory.storyId);
                $("#videoURL").val(data.videoURL.join('\n').replace(/embed\//g, "watch?v="));
                CKEDITOR.instances['editor'].setData(data.draftStory.description) || '';
                $('#preview-container').html("");
                
                    data.draftMedia.forEach(function (item) {
                        $('#preview-container').append(`<div class="preview">
                            <img src="/media/${item}"/>
                            <button type="button" class="cancel-btn" onclick="closeBtnDraft(this);">&#x2716;</button>
                            </div>`);
                        savedImgList.push(item);

                    });
                
                submitbuttonadd();
                if (data.draftStory.status == 2) {
                    toastr.success('Your Story for This Mission is Approved', '', {
                        "closeButton": true,
                        positionClass: 'toast-top-center',
                        "preventDuplicates": true,
                        "timeOut": "2000",
                    });
                    $(".cancel-btn").prop('disabled', true).css('opacity', 0.5);
                    $("#savebtn").prop('disabled', true).css('opacity', 0.5);
                    $("#submit").prop('disabled', true).css('opacity', 0.5);
                    $("input, textarea").prop("disabled", true);
                    CKEDITOR.instances['editor'].readOnly = true;
                    dropZoneFlag = false;

                }
                else if (data.draftStory.status == 3 ) {
                    toastr.error('Your Story is Rejected by Admin', '', {
                    });
                    $("#submit").prop('disabled', false).css('opacity', 1);
                    $("#savebtn").prop('disabled', false).css('opacity', 1);
                    $("input,textarea").prop('disabled', false);
                    editor.setReadOnly(false);
                    dropzoneEnabler = true;
                }
                else {
                    $(".cancel-btn").prop('disabled', false).css('opacity', 1);
                    $("#savebtn").prop('disabled', false).css('opacity', 1);
                    $("#submit").prop('disabled', false).css('opacity', 1);
                    $("input, textarea").prop("disabled", false);
                    CKEDITOR.instances['editor'].readOnly = false;
                    dropZoneFlag = true;
                }
            }
        },
        error: function (e) {
            alert('Error');
        },
    });
}

// (21)-------------------------------------   save story    ----------------------------------------------------------------//
function SaveStory() {
    var title = $("#title").val();
    var data = editor.getData();
    var missionId = parseInt($("#missionId").val(), 10);
    var date = $("#date").val();

    var formData = new FormData();
    var textareaValue = $("#videoURL").val().trim().replace(/.be\//g, "be.com/watch?v=");
    var video = textareaValue ? textareaValue.split(/\s+/) : [];
    console.log(video);

    if (video.length > 20) {
        toastr.error("You can add a maximum of 20 video URLs.", '', {
        });
        return false;
    }

    for (var i = 0; i < video.length; i++) {

        var videoUrl = video[i].trim();
        if (!/^https?:\/\/(www\.)?youtube\.com\/watch\?v=[a-zA-Z0-9_-]{11}$/.test(videoUrl)) {

            toastr.error("Invalid YouTube URL: " + videoUrl, '', {
            });
            return false;
        }
    }
    formData.append('title', title);
    formData.append('description', data);
    formData.append('missionid', missionId);
    formData.append('date', date);
    for (var i = 0; i < delImgList.length; i++) {
        formData.append('delImgList', delImgList[i]);
    }
    for (var i = 0; i < fileNames.length; i++) {
        formData.append('fileList', fileNames[i]);
    }
    for (var i = 0; i < video.length; i++) {
        formData.append('VideoUrl', video[i].replace("watch?v=", "embed/"));
    }

    $.ajax({
        type: "POST",
        url: '/Story/SaveYourStory',
        data: formData,
        contentType: false,
        processData: false,
        enctype: "multipart/form-data",
        success: function (data) {
            if (data == false) {

                toastr.error('All Fields are Required', '', {
                });
            } else {
                fileNames = [];
                $("#submit").removeClass("d-none");

                toastr.success('Story Saved Successfully', '', {
                });
            }
            SavedDraft();
        },
        error: function (e) {
            alert('error');
        }
    });


}

// (22)-------------------------------------   common toastr design    ----------------------------------------------------------------//
    toastr.options = {
        "closeButton": true,
        "positionClass": "toast-top-center",
        "preventDuplicates": true,
        "showDuration": "1000",
        "hideDuration": "300",
        "timeOut": "1500",
        "extendedTimeOut": "0"
    };



// (23)-------------------------------------   Pagination with backend    ----------------------------------------------------------------//


var leftPageBtn
var rightPageBtn
var totalMission
var PageSize = 3
var PageNumber = 1
var pages
function Pagination() {
    totalMission = $("#mission_count").data('count')
    if (totalMission == 0) {
        $("#pagination").parent().hide()
        return;
    }
    else
        $("#pagination").parent().show()
    pages = Math.ceil(totalMission / PageSize)
    leftPageBtn = Math.max(1, PageNumber - 1)
    rightPageBtn = Math.min(pages, leftPageBtn + 2)
    leftPageBtn = Math.max(1, rightPageBtn - 2)
    $("#pagination").empty()
    for (let i = 1; i <= pages; i++) {
        $("#pagination").append(`<li class="pagination_box">${i}</li>`)
    }
    $("#pagination li").removeClass("active")
    $("#pagination li:nth-child(" + PageNumber + ")").addClass("active")
    setPageBtn()
}
$(function () {
    $("#pagination").on('click', 'li', function () {
        PageNumber = $("#pagination li").index(this) + 1
        temp()
    })
    $("#prevpage-btn").click(() => {
        if (leftPageBtn > 1) {
            leftPageBtn--
            rightPageBtn--
            setPageBtn()
        }
    })
    $("#nextpage-btn").click(() => {
        if (rightPageBtn < pages) {
            leftPageBtn++
            rightPageBtn++
            setPageBtn()
        }
    })
    $("#firstpage").click(() => {
        PageNumber = 1
        temp()
    })
    $("#lastpage").click(() => {
        PageNumber = pages
        temp()
    })
    $("#grd-btn").click(() => {
        grid()
    })
    $("#lst-btn").click(() => {
        list()
    })
})


function setPageBtn() {
    $.each($("#pagination li"), function (index, pageBtn) {
        if (index + 1 >= leftPageBtn && index + 1 <= rightPageBtn)
            $(pageBtn).css('display', 'block')
        else
            $(pageBtn).css('display', 'none')
    })
}


// (24)-------------------------------------   Notification     ----------------------------------------------------------------//

function closeDropdown() {
    $('#notificationSettingDropdown').removeClass('show');
    $('#notificationDropdown').addClass('show');
}
