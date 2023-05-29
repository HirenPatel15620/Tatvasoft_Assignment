
// --------------------------------------   countrylist & citylist for add user     ----------------------------------------------------------

function CountryList() {


    $.ajax({
        type: "POST",
        url: '/Admin/CountryListForAddUserbyAdmin',
        success: function (data) {
            // Update the HTML page with the response
            //$('#Country').empty();
            $('#Country option:not(:first)').remove();
            console.log(data);

            // Iterate through each country object in the result array
            data.forEach(function (item) {
                // Append a new option element with the value and text of the country 
                // to the #CountryList select dropdown
                $('#Country').append($('<option>', {
                    value: item.value,
                    text: item.text
                }));
            });
        },
        error: function (e) {
            alert('error');
        }
    });
}

function CitybyCountryForAdmin() {
    var country = $("#Country").val();
    console.log(country);
    $.ajax({
        type: "POST",
        url: '/User/GetCityListbyCountry',
        data: {
            'countryid': country
        },
        success: function (data) {
            // Update the HTML page with the response
            $('#CityId option:not(:first)').remove();
            //$('#CityId').empty();
            // Iterate through each city object in the result array
            data.cityList.forEach(function (item) {
                // Append a new option element with the forCity value to the #cityId select dropdown
                $('#CityId').append($('<option>', {
                    value: item.value,
                    text: item.text
                }));
            });
        },
        error: function (e) {
            alert('Error');
        },
    });
}

function CitybyCountryForAdminMission() {
    var country = $("#missionCountry").val();
    console.log(country);
    $.ajax({
        type: "POST",
        url: '/User/GetCityListbyCountry',
        data: {
            'countryid': country
        },
        success: function (data) {
            // Update the HTML page with the response
            $('#missionCity option:not(:first)').remove();
            
            // Iterate through each city object in the result array
            data.cityList.forEach(function (item) {
                // Append a new option element with the forCity value to the #cityId select dropdown
                $('#missionCity').append($('<option>', {
                    value: item.value,
                    text: item.text
                }));
            });
        },
        error: function (e) {
            alert('Error');
        },
    });
}



// --------------------------------------    add user by admin    ----------------------------------------------------------



function AddUserbyAdmin() {
    var fname = $("#Fname").val();
    var lname = $("#Lname").val();
    var email = $("#EmailId").val();
    var password = $("#PassWord").val();
    var Eid = $("#EmpId").val();
    var dept = $("#Dept").val();
    var profile = $("#ProfileTxt").val();
    var city = $("#CityId").val();
    var country = $("#Country").val();
    var status = $("#Status").val();

    let isValid = true;

    if (fname.trim() == "") {
        $("#fname_error").css('color', 'red').html("Name Is Required");
        isValid = false;
    }
    else { $("#fname_error").html(""); }
    if (lname.trim() == "") {
        $("#lname_error").css('color', 'red').html("Surname Is Required");
        isValid = false;
    }
    else { $("#lname_error").html(""); }
    if (email.trim() == "") {
        $("#email_error").css('color', 'red').html("Email Is Required");
        isValid = false;
    }
    else { $("#email_error").html(""); }
    if (password.trim() == "") {
        $("#pass_error").css('color', 'red').html("Password Is Required");
        isValid = false;
    }
    else { $("#pass_error").html(""); }
    if (isValid == false) {
        return;
    }
    var formData = new FormData();
    formData.append('UserId', userAddEditid);
    formData.append('FirstName', fname);
    formData.append('LastName', lname);
    formData.append('EmployeeId', Eid);
    formData.append('Department', dept);
    formData.append('Email', email);
    formData.append('Password', password);
    formData.append('Status', status);
    formData.append('CityId', city);
    formData.append('CountryId', country);
    formData.append('ProfileText', profile);
    formData.append('Avatar', avtar);
    $.ajax({
        type: "POST",
        url: '/Admin/AddUserbyAdmin',
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data == false) {
                console.log("already");
                $("#Message").html('User Already Exists');
                return;
            }
            else {
                $("#Message").html('');
                toastr.success('User Added Successfully', '', {
                });
                userAddEditid = 0;
                avtar = null;
                $('#alluserdiv').load(location.href + ' #alluserdiv >*', function () { usertable(); });
                
            }
        },
        error: function (e) {
            alert('Error');
        },
    });
}

// --------------------------------------    edit user by admin    ----------------------------------------------------------

function EditUserbyAdmin(userid) {
    $.ajax({
        url: "/Admin/FetchUser",
        type: "POST",
        data: {
            'userid': userid,
        },
        success: function (result) {
            // Update the HTML page with the response
            console.log(result);
            //$("#adduser").find("input, select, textarea").val("");
            $('#Fname').val(result.firstName);
            $('#Lname').val(result.lastName);
            $('#EmpId').val(result.employeeId);
            $('#EmailId').val(result.email);
            $("#EmailId").prop("readonly", true);
            $("#PassWord").prop("readonly", true);
            $('#PassWord').val(result.password);
            $('#Dept').val(result.department);
            $('#ProfileTxt').val(result.profileText);
            $('#Status').val(result.status);
            $("#Country").val("");
            $("#Country").val(result.countryId);
            CitybyCountryForAdmin();
            $('#CityId option:not(:first)').remove();

            setTimeout(function () {
                $("#CityId").val(result.cityId);
            }, 50);

            $('#userName').text(result.firstName + result.lastName);
            $('#userAvtar').attr('src', '');
            $('#userAvtar').attr('src', '/media/' + result.avatar);

            userAddEditid = result.userId;

        }
    });
}



// --------------------------------------    delete user by admin    ----------------------------------------------------------
function Delete(id,identity) {
    $(".yes").click(function () {
        if (identity == 1)
            DeleteUser(id);
        else if (identity == 2)
            DeleteStory(id);
        else if (identity == 3)
            DeleteTheme(id);
        else if (identity == 4)
            DeleteSkill(id);
        else if (identity == 5)
            DeleteMission(id);
        else if (identity == 6)
            DeleteBanner(id);
        else
            return;
    });
}

function DeleteUser(userid) {


    $.ajax({
        url: "/Admin/DeleteUserbyAdmin",
        type: "POST",
        data: {

            'userid': userid
        },
        success: function (data) {
            location.reload();
        },
        error: function () {
            alert("hi error");
        }
    });

}


//--------------------------------------      user Search   --------------------------------------

$("userSearch").keyup(function () {
    let str = "userSearch"
    let search = $(this).val()
    console.log(search)
    $.ajax({
        type: "POST",
        url: '/Admin/Search',
        data: {
            'Search': search,
            'Identity': str,
        },
        dataType: "html", // return datatype like JSON and HTML
        success: function (data) {
            $("#alluserdiv").empty()
            $("#alluserdiv").html(data)
        },
        error: function (e) {
            toastr.error('Please Try again', 'Error', {
                "closeButton": true,
                positionClass: 'toast-top-center',
                "preventDuplicates": true,
                "timeOut": "2000",
            });
        },

    });
})


// --------------------------------------   user profile picture   ----------------------------------------------------------

function addUserProfilePicture() {
    $("#userProfile").click();
    var pic = $("#userProfile").val();
    console.log(pic);
}
var avtar = null;
function validUserImage() {

    var files = event.target.files;
    if (files.length != 0) {
        const fileInput = document.getElementById('userProfile');
        const file = fileInput.files[0];
        console.log(file);
        // Check if file is a PNG or JPG image
        if (!file.name.endsWith('.png') && !file.name.endsWith('.jpg')) {
            $("#img_error").css('color', 'red').html("*File should be PNG or JPG image");
            return; // Exit the function
        }
        console.log("valid");
        var reader = new FileReader();
        reader.onload = function (event) {
            $("#userProfileDiv").html(`<img src="${event.target.result}" class="img-fluid rounded-circle userPicture" onclick="addUserProfilePicture()" alt="">`);
        }
        reader.readAsDataURL(files[0]);
        avtar = file;
    }
    else {
        $("#userProfileDiv").html(`<img src="/media/user1.png" class="img-fluid rounded-circle userPicture" onclick="addUserProfilePicture()" alt="">`);
    }

}

// -------------------------------------   add media images on dropzone for user interface   ----------------------------------------------------------------//
// Get the drop zone element
const dropzone = document.getElementById('dropzone');
let dropZoneFlag = true;
// Handle drag over event
dropzone.addEventListener('dragover', (event) => {
    event.preventDefault();
    dropzone.classList.add('dragging');
});

// Handle drag leave event
dropzone.addEventListener('dragleave', () => {
    dropzone.classList.remove('dragging');
});

// Handle drop event
dropzone.addEventListener('drop', (event) => {
    event.preventDefault();
    dropzone.classList.remove('dragging');
    if (dropZoneFlag == true) {

        // Get the dropped files
        const files = event.dataTransfer.files;

        // Process the dropped files
        handleFiles(files);
    }
});

// Handle file input change event
const fileInput = document.getElementById('file-input');
fileInput.addEventListener('change', () => {
    // Get the selected files
    const files = fileInput.files;

    // Process the selected files
    handleFiles(files);
});

var fileNames = [];


// Function to handle the dropped or selected files
function handleFiles(files) {
    // Get the preview container element
    const previewContainer = document.getElementById('preview-container');

    // Iterate over the files and create a preview for each file
    for (const file of files) {
        const reader = new FileReader();
        reader.onload = () => {
            // Create a preview element for the file
            const preview = document.createElement('div');
            preview.classList.add('preview');
            if (file.type.startsWith('image/')) {
                // If the file is an image, create an image element
                const image = document.createElement('img');
                image.src = reader.result;
                preview.appendChild(image);
            } else if (file.type.startsWith('video/')) {
                // If the file is a video, create a video element
                const video = document.createElement('video');
                video.src = reader.result;
                video.controls = true;
                preview.appendChild(video);
            }

            // Create a close button for the preview

            const closeButton = document.createElement('button');
            closeButton.classList.add('cancel-btn');
            closeButton.innerHTML = '&#x2716;';
            closeButton.type = 'button';
            closeButton.addEventListener('click', () => {
                // Remove the preview element from the preview container
                previewContainer.removeChild(preview);

                // Remove the file name from the array
                const index = fileNames.indexOf(file);
                if (index > -1) {
                    fileNames.splice(index, 1);
                }
            });
            preview.appendChild(closeButton);

            // Append the preview element to the preview container
            previewContainer.appendChild(preview);

            // Add the file name to the array
            fileNames.push(file);
        };
        reader.readAsDataURL(file);
    }
}

// -------------------------------------   push the drag & drop images on input    ----------------------------------------------------------------//

$(".file-upload img").on('click', function () {
    //console.log('aayo');
    $("#file-input").click();
});


// ------------------------------------   remove  media image when mission is in edit mode   ----------------------------------------------------------------//
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
   
}

var delDocList = [];
var saveDocList = [];
function removeDoc(btn) {
    const index = $('.removeFile').index(btn);
    console.log(index);
    if (index < saveDocList.length) {
        delDocList.push(saveDocList[index]);
        saveDocList.splice(index, 1);
        $(btn).closest('.removeMissionDoc').remove();

    }
    console.log(delDocList);
}
// -------------------------------------   common toastr design    ----------------------------------------------------------------//

toastr.options = {
    "closeButton": true,
    "positionClass": "toast-top-center",
    "preventDuplicates": true,
    "showDuration": "1000",
    "hideDuration": "300",
    "timeOut": "1500",
    "extendedTimeOut": "0"
};


// -------------------------------------   add cms     ----------------------------------------------------------------//
function AddCMS() {
    var title = $("#title").val();
    var data = editor.getData();
    var slug = $("#slug").val();
    var status = $("#status").val();

    console.log(title);
    console.log(data);
    console.log(slug);
    console.log(status);

    let isValid = true;

    if (title.trim() == "") {
        $("#title_error").css('color', 'red').html("Title Is Required");
        isValid = false;
    }
    else { $("#title_error").html(""); }
    if (data.trim() == "") {
        $("#desc_error").css('color', 'red').html("Discription Is Required");
        isValid = false;
    }
    else { $("#desc_error").html(""); }
    if (isValid == false) {
        return;
    }
    var formData = new FormData();

    formData.append('CmsPageId', CMSId);
    formData.append('Title', title);
    formData.append('Description', data);
    formData.append('Slug', slug);
    formData.append('Status', status);

    $.ajax({
        type: "POST",
        url: '/Admin/AddCMSbyAdmin',
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            $('#add').addClass('d-none');
            //$('#cms').load(location.href + ' #cms  >*');
            $('#cmsDiv').load(location.href + ' #cmsDiv >*', function () { cmstable(); });

            $('#cms').removeClass('d-none');
            CMSId = 0;
        },
        error: function (e) {
            alert('error');
        }
    });


}

// --------------------------------------    edit cms by admin    ----------------------------------------------------------

function EditCMSbyAdmin(cmsid) {
    $.ajax({
        url: "/Admin/FetchCMS",
        type: "POST",
        data: {
            'cmsId': cmsid,
        },
        success: function (result) {
            // Update the HTML page with the response
            console.log(result);

            $('#title').val(result.title);
            $('#slug').val(result.slug);
            $('#status').val(result.status);
            CKEDITOR.instances['editor'].setData(result.description) || '';
            CMSId = result.cmsPageId;

        }
    });
}


// --------------------------------------    delete cms by admin    ----------------------------------------------------------
function deletecms(cmsid) {
    $("#yescms").click(function () {
        DeleteCMS(cmsid);
    });
}

function DeleteCMS(cmsid) {

    $.ajax({
        url: "/Admin/DeleteCMSbyAdmin",
        type: "POST",
        data: {

            'cmsid': cmsid
        },
        success: function (data) {
            //location.reload();
            $('#cmsDiv').load(location.href + ' #cmsDiv >*', function () { cmstable(); });

        },
        error: function () {
            alert("hi error");
        }
    });

}


//--------------------------------------      cms Search   --------------------------------------
$("cmsSearch").keyup(function () {
    let str = "cmsSearch"
    let search = $(this).val()
    console.log(search)
    $.ajax({
        type: "POST",
        url: '/Admin/Search',
        data: {
            'Search': search,
            'Identity': str,
        },
        dataType: "html", // return datatype like JSON and HTML
        success: function (data) {
            $("#cmsDiv").empty()
            $("#cmsDiv").html(data)
        },
        error: function (e) {
            toastr.error('Please Try again', 'Error', {
                "closeButton": true,
                positionClass: 'toast-top-center',
                "preventDuplicates": true,
                "timeOut": "2000",
            });
        },

    });
})


//  -------------------------------------   image slider of left side    -------------------------------------

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



// -------------------------------------    story view by admin    -------------------------------------------------//

function viewStory(storyid) {
    console.log(storyid);

    $.ajax({
        url: "/Admin/ViewStorybyAdmin",
        type: "POST",
        data: {
            'storyid': storyid,
        },
        success: function (data) {
            $("#viewStoryDiv").empty()
            $("#viewStoryDiv").html(data)
            $(".mydiv").eq(0).removeClass("d-none");
            show();
        },
        error: function () {
            alert("hi error");
        }
    });

}

// -------------------------------------    story approved by admin    -------------------------------------------------//

function ApprovedStory(storyid) {
    console.log(storyid);

    $.ajax({
        url: "/Admin/StoryApprovedbyAdmin",
        type: "POST",
        data: {
            'storyid': storyid,
        },
        success: function (data) {
            toastr.success('Story Approved', {
            });
            //$('#story').load(location.href + ' #story  >*');
            $('#storyDiv').load(location.href + ' #storyDiv >*', function () { storytable(); });

        },
        error: function () {
            alert("hi error");
        }
    });
}

// -------------------------------------    story rejected by admin    -------------------------------------------------//

function RejectedStory(storyid) {
    console.log(storyid);

    $.ajax({
        url: "/Admin/StoryRejectbyAdmin",
        type: "POST",
        data: {
            'storyid': storyid,
        },
        success: function (data) {
            toastr.error('Story Rejected', {
            });
            //$('#story').load(location.href + ' #story  >*');
            $('#storyDiv').load(location.href + ' #storyDiv >*', function () { storytable(); });

        },
        error: function () {
            alert("hi error");
        }
    });
}

// -------------------------------------    story deleted by admin    -------------------------------------------------//

function DeleteStory(storyid) {
    console.log(storyid);

    $.ajax({
        url: "/Admin/StoryDeletedbyAdmin",
        type: "POST",
        data: {
            'storyid': storyid,
        },
        success: function (data) {
           // $('#story').load(location.href + ' #story  >*');
            $('#storyDiv').load(location.href + ' #storyDiv >*', function () { storytable(); });

        },
        error: function () {
            alert("hi error");
        }
    });
}

//--------------------------------------      story Search   --------------------------------------
$("storySearch").keyup(function () {
    let str = "storySearch"
    let search = $(this).val()
    console.log(search)
    $.ajax({
        type: "POST",
        url: '/Admin/Search',
        data: {
            'Search': search,
            'Identity': str,
        },
        dataType: "html", // return datatype like JSON and HTML
        success: function (data) {
            $("#storyDiv").empty()
            $("#storyDiv").html(data)
        },
        error: function (e) {
            toastr.error('Please Try again', 'Error', {
            });
        },

    });
})


// -------------------------------------    mission-application approved by admin    -------------------------------------------------//

function ApprovedMissionApplication(missionid) {


    $.ajax({
        url: "/Admin/MissionApplicationApprovedbyAdmin",
        type: "POST",
        data: {
            'missionid': missionid,
        },
        success: function (data) {
            toastr.success('Mission Application Approved', {
            });
            // $('#missionapplication').load(location.href + ' #missionapplication  >*');
            $('#missionAppDiv').load(location.href + ' #missionAppDiv >*', function () { missionAppTable(); });


        },
        error: function () {
            alert("hi error");
        }
    });
}

// -------------------------------------    mission-application rejected by admin    -------------------------------------------------//

function RejectedMissionApplication(missionid) {


    $.ajax({
        url: "/Admin/MissionApplicationRejectbyAdmin",
        type: "POST",
        data: {
            'missionid': missionid,
        },
        success: function (data) {
            toastr.error('Mission Application Rejected', {
            });
            //$('#missionapplication').load(location.href + ' #missionapplication  >*');
            $('#missionAppDiv').load(location.href + ' #missionAppDiv >*', function () { missionAppTable(); });

        },
        error: function () {
            alert("hi error");
        }
    });
}

//--------------------------------------      mission-application Search   --------------------------------------
$("MissionAppSearch").keyup(function () {
    let str = "missionAppSearch"
    let search = $(this).val()

    $.ajax({
        type: "POST",
        url: '/Admin/Search',
        data: {
            'Search': search,
            'Identity': str,
        },
        dataType: "html", // return datatype like JSON and HTML
        success: function (data) {
            $("#missionAppDiv").empty()
            $("#missionAppDiv").html(data)
        },
        error: function (e) {
            toastr.error('Please Try again', 'Error', {
            });
        },

    });
})


// -------------------------------------   add mission theme     ----------------------------------------------------------------//
function AddTheme() {
    var title = $("#Themetitle").val();
    var status = $("#Themestatus").val();


    let isValid = true;
    if (title.trim() == "") {
        $("#Mtitle_error").css('color', 'red').html("Title Is Required");
        isValid = false;
    }
    else { $("#Mtitle_error").html(""); }
    if (isValid == false) {
        return;
    }

    $.ajax({
        type: "POST",
        url: '/Admin/AddThemebyAdmin',
        data: {
            'Title': title,
            'Status': status,
            'MissionThemeId': ThemeID,
        },
        success: function (data) {
            $('#addMissionTheme').addClass('d-none');
            // $('#missiontheme').load(location.href + ' #missiontheme  >*');
            $('#themeDiv').load(location.href + ' #themeDiv >*', function () { themetable(); });

            $('#missiontheme').removeClass('d-none');
            ThemeID = 0;
        },
        error: function (e) {
            alert('error');
        }
    });


}

// --------------------------------------    edit mission theme by admin    ----------------------------------------------------------

function EditThemebyAdmin(themeid) {
    $.ajax({
        url: "/Admin/FetchMissionTheme",
        type: "POST",
        data: {
            'themeid': themeid,
        },
        success: function (result) {
            // Update the HTML page with the response
            console.log(result);

            $('#Themetitle').val(result.title);
            $('#Themestatus').val(result.status);
            ThemeID = result.missionThemeId;

        }
    });
}

// --------------------------------------    delete mission theme by admin    ----------------------------------------------------------


function DeleteTheme(themeid) {

    $.ajax({
        url: "/Admin/DeleteMissionThemebyAdmin",
        type: "POST",
        data: {

            'themeid': themeid
        },
        success: function (data) {
            //$('#missiontheme').load(location.href + ' #missiontheme  >*');
            $('#themeDiv').load(location.href + ' #themeDiv >*', function () { themetable(); });

        },
        error: function () {
            alert("hi error");
        }
    });

}
//--------------------------------------      mission theme Search   --------------------------------------
$("themeSearch").keyup(function () {
    let str = "themeSearch"
    let search = $(this).val()
    console.log(search)
    $.ajax({
        type: "POST",
        url: '/Admin/Search',
        data: {
            'Search': search,
            'Identity': str,
        },
        dataType: "html", // return datatype like JSON and HTML
        success: function (data) {
            $("#themeDiv").empty()
            $("#themeDiv").html(data)
        },
        error: function (e) {
            toastr.error('Please Try again', 'Error', {
            });
        },

    });
})


// -------------------------------------   add mission skill     ----------------------------------------------------------------//
function AddSkill() {
    var title = $("#skillname").val();
    var status = $("#Skillstatus").val();


    let isValid = true;
    if (title.trim() == "") {
        $("#skill_error").css('color', 'red').html("Title Is Required");
        isValid = false;
    }
    else { $("#skill_error").html(""); }
    if (isValid == false) {
        return;
    }

    $.ajax({
        type: "POST",
        url: '/Admin/AddSkillbyAdmin',
        data: {
            'SkillName': title,
            'Status': status,
            'SkillId': SkillID,
        },
        success: function (data) {
            $('#addMissionSkill').addClass('d-none');
            //$('#missionskill').load(location.href + ' #missionskill  >*');
            $('#skillDiv').load(location.href + ' #skillDiv >*', function () { skilltable(); });

            $('#missionskill').removeClass('d-none');
            SkillID = 0;
        },
        error: function (e) {
            alert('error');
        }
    });


}

// --------------------------------------    edit mission skill by admin    ----------------------------------------------------------

function EditSkillbyAdmin(skillid) {
    $.ajax({
        url: "/Admin/FetchMissionSkill",
        type: "POST",
        data: {
            'skillid': skillid,
        },
        success: function (result) {
            // Update the HTML page with the response
            console.log(result);

            $('#skillname').val(result.skillName);
            $('#Skillstatus').val(result.status);
            SkillID = result.skillId;

        }
    });
}

// --------------------------------------    delete mission skill by admin    ----------------------------------------------------------


function DeleteSkill(skillid) {

    $.ajax({
        url: "/Admin/DeleteMissionSkillbyAdmin",
        type: "POST",
        data: {

            'skillid': skillid
        },
        success: function (data) {
            //$('#missionskill').load(location.href + ' #missionskill  >*');
            $('#skillDiv').load(location.href + ' #skillDiv >*', function () { skilltable(); });

        },
        error: function () {
            alert("hi error");
        }
    });

}
//--------------------------------------      mission skill Search   --------------------------------------
$("skillSearch").keyup(function () {
    let str = "skillSearch"
    let search = $(this).val()
    console.log(search)
    $.ajax({
        type: "POST",
        url: '/Admin/Search',
        data: {
            'Search': search,
            'Identity': str,
        },
        dataType: "html", // return datatype like JSON and HTML
        success: function (data) {
            $("#skillDiv").empty()
            $("#skillDiv").html(data)
        },
        error: function (e) {
            toastr.error('Please Try again', 'Error', {
            });
        },

    });
})



// --------------------------------------   Add mission skill   ----------------------------------------------------------
$('#selected ul').on('click', 'li', function () {
    if (!$(this).hasClass('selected')) {
        $(this).addClass('selected');
        $(this).css('background-color', '#bebebe');

    } else {
        $(this).removeClass('selected');
        $(this).css('background-color', 'white');
    }
});

$("#right").click(function () {
    var nameList = $('#selected ul li');

    for (let i = 0; i < nameList.length; i++) {
        if (nameList.eq(i).hasClass('selected')) {
            $('#selected-skills ul').append(`<li value="${nameList.eq(i).val()}">` + nameList.eq(i).html() + `</li>`);
            nameList.eq(i).remove();

        }
    }
});

$('#selected-skills ul').on('click', 'li', function () {
    if (!$(this).hasClass('selected')) {
        $(this).addClass('selected');
        $(this).css('background-color', '#bebebe');

    } else {
        $(this).removeClass('selected');
        $(this).css('background-color', 'white');
    }
});

$("#left").click(function () {
    var sidenameList = $('#selected-skills ul li');

    for (let i = 0; i < sidenameList.length; i++) {
        if (sidenameList.eq(i).hasClass('selected')) {
            $('#selected ul').append(`<li value="${sidenameList.eq(i).val()}">` + sidenameList.eq(i).html() + `</li>`);
            sidenameList.eq(i).remove();
        }
    }
});

var Listing = [];
$('#saveSkills').on('click', function () {
    $('#skillText').html('');
    var addedSkills = $('#selected-skills ul li');
    console.log(addedSkills)
    for (let i = 0; i < addedSkills.length; i++) {
        console.log(i)
        Listing.push(addedSkills.eq(i).html());
    }
    var TextBox = $('#skillText');
    TextBox.val(Listing.join('\n'));
    Listing.splice(0, Listing.length);
});

// -----------------------------------------------------     add documents in mission tab       ---------------------------------------------------

$("#docbtn").click(() => {
    $("#docfile").click();
})

var DocList = [];
$("#docfile").on('change', function (evt) {

    for (var i = 0; i < evt.target.files.length; i++) {
        let file = evt.target.files[i];
        DocList.push(file);

        var removeLink = "<a class=\"text-end removeFile\" >&#128473;</a>";

        //$("#doclist").append("<li><strong>" + file.name + "</strong> &nbsp; &nbsp; " + removeLink + "</li>");
        $("#doclist").append(`<button class="btn btn-outline-secondary doc me-2"><i class="fa-sharp fa-solid fa-files fa-lg" style="color: #0f58d7;"></i>${file.name}&nbsp;${removeLink}</button>`);
    };

    evt.target.value = null;
})

$("#doclist").on('click', '.removeFile', function () {
    let index = $(".removeFile").index(this);
    $(this).parent().remove();
    DocList.splice(index, 1);
})


//--------------------------------------      mission Search   --------------------------------------
$("missionSearch").keyup(function () {
    let str = "missionSearch"
    let search = $(this).val()
    $.ajax({
        type: "POST",
        url: '/Admin/Search',
        data: {
            'Search': search,
            'Identity': str,
        },
        dataType: "html", // return datatype like JSON and HTML
        success: function (data) {
            $("#missionDiv").empty()
            $("#missionDiv").html(data)
        },
        error: function (e) {
            toastr.error('Please Try again', 'Error', {
                "closeButton": true,
                positionClass: 'toast-top-center',
                "preventDuplicates": true,
                "timeOut": "2000",
            });
        },

    });
})

// -------------------------------------    edit mission by admin  ----------------------------------------------------------------//
function EditMissionbyAdmin(missionid) {
    delImgList = [];
    savedImgList = [];
    fileNames = [];
    $.ajax({
        type: "POST",
        url: '/Admin/FetchMission',
        data: {
            'missionid': missionid
        },
        success: function (data) {
            console.log(data);

            $("#missionTitle").val(data.missionName);
            $("#missionType").val(data.missionType);
            $("#missionType").prop("disabled", true);
            $('#MissionGoalAction').val(data.msn_Goal_Action);
            $('#MissionGoalObjective').val(data.msn_Goal_Obj);

            if (data.missionType == 1) {
                $("#ForGoal").removeClass('d-none');
                $("#EndDate").addClass('d-none');
                $("#Deadline").addClass('d-none');
            }
            else {
                $("#ForGoal").addClass('d-none');
                $("#EndDate").removeClass('d-none');
                $("#Deadline").removeClass('d-none');
            }
            $("#totalSeats").val(data.seats);
            $("#missionTheme").val(data.missionThemeId);
            $("#missionCountry").val(data.missionCountryId);
            CitybyCountryForAdminMission();
            $("#MSdate").val(data.startDate.substr(0, 10));
            $("#MEdate").val(data.endDate.substr(0, 10));
            $("#MDdate").val(data.deadline.substr(0, 10).substr(0, 10));
            $("#missionOrgnztn").val(data.organizationName);
            $("#missionOrgnztnDetail").val(data.organizationDetails);
            setTimeout(function () {
                $("#missionCity").val(data.missionCityId);
            }, 100);
            $("#skillText").val(data.skillNames.join('\n'));
            $("#videoURL").val(data.videoUrl.join('\n').replace(/embed\//g, "watch?v="));
            Desc.setData(data.description);
            SDesc.setData(data.shortDescription);


            // show uploaded images
            $('#preview-container').html("");
            data.mimgList.forEach(function (item) {
                $('#preview-container').append(`<div class="preview">
                            <img src="/media/${item}"/>
                            <button type="button" class="cancel-btn" onclick="closeBtnDraft(this);">&#x2716;</button>
                            </div>`);
                savedImgList.push(item);
            });


            // show uploaded skills

            $("#selected-skills ul li").addClass('selected');
            $("#left").click()
            $('#selected ul li').filter(function () {
                return data.skillIds.includes(parseInt($(this).val()));
            }).addClass('selected');
            $("#right").click();
            $("#SaveSkiils").click();


            // show uploaded documents
            $("#doclist").html("");
            var removeLink = '<a class=\"removeFile\" onclick="removeDoc(this)">&#128473;</a>';
            $.each(data.missionDoc, function (index, doc) {
                $("#doclist").append(`
                        <a class="removeMissionDoc" href=/media/${doc.docPathList} style="text-decoration:none !important;color:#757575" target = "_blank" > 
                            <button type="button" class="doc btn btn-outline-secondary  me-2"><i class="fa-solid fa-file-pdf"></i>&nbsp;&nbsp;
                            ${doc.docNameList}&nbsp;&nbsp;${removeLink}</button> 
                       </a >`
                );
                saveDocList.push(doc.docPathList);
            });
            MissionID = data.missionId;
        },
        error: function (e) {
            alert('Error');
        },
    });
}



// --------------------------------------    delete mission  by admin    ----------------------------------------------------------


function DeleteMission(missionid) {

    $.ajax({
        url: "/Admin/DeleteMissionbyAdmin",
        type: "POST",
        data: {

            'missionid': missionid
        },
        success: function (data) {
            $('#mission').load(location.href + ' #mission  >*');
        },
        error: function () {
            alert("hi error");
        }
    });

}

// ---------------------------    goal value disbled in mission tab    --------------------------------------

function openAction() {
    if ($("#missionType").val() == 1) {
        $("#ForGoal").removeClass('d-none');
        $("#EndDate").addClass('d-none');
        $("#Deadline").addClass('d-none');
    }
    else {
        $("#ForGoal").addClass('d-none');
        $("#EndDate").removeClass('d-none');
        $("#Deadline").removeClass('d-none');
    }
}


IsvalidMission = false;
// -------------------------------------   add mission by admin     ----------------------------------------------------------------//
function AddMission() {



    var title = $("#missionTitle").val();
    var type = $("#missionType").val();
    var seats = $("#totalSeats").val();
    var theme = $("#missionTheme").val();
    var country = $("#missionCountry").val();
    var city = $("#missionCity").val();
    var desc = Desc.getData();
    var Shortdesc = SDesc.getData();
    var Sdate = $("#MSdate").val();
    var Edate = $("#MEdate").val();
    var Ddate = $("#MDdate").val();
    var MGoalObj = $('#MissionGoalObjective').val();
    var MGoalAction = $('#MissionGoalAction').val();
    var orgName = $("#missionOrgnztn").val();
    var orgDetail = $("#missionOrgnztnDetail").val();
    var skillIdList = $("#selected-skills ul li").map(function () {
        return parseInt($(this).val());
    }).get();


    if (title == '') {
        $("#missionTitle").prev('label').css('color', 'red').text("Mission Title*");
        IsvalidMission = false;
    } else
        $("#missionTitle").prev('label').css('color', 'black').text("Mission Title");
    if (type == "") {
        $("#missionType").prev('label').css('color', 'red').text("Mission Type*");
        IsvalidMission = false;
    } else
        $("#missionType").prev('label').css('color', 'black').text("Mission Type");
    if (seats == 0) {
        $("#totalSeats").prev('label').css('color', 'red').text("Total Seats*");
        IsvalidMission = false;
    } else
        $("#totalSeats").prev('label').css('color', 'black').text("Total Seats");
    if (theme == "") {
        $("#missionTheme").prev('label').css('color', 'red').text("Theme*");
        IsvalidMission = false;
    } else
        $("#missionTheme").prev('label').css('color', 'black').text("Theme");
    if (country == "") {
        $("#missionCountry").prev('label').css('color', 'red').text("Country*");
        IsvalidMission = false;
    } else
        $("#missionCountry").prev('label').css('color', 'black').text("Country");
    if (city == "") {
        $("#missionCity").prev('label').css('color', 'red').text("City*");
        IsvalidMission = false;
    } else
        $("#missionCity").prev('label').css('color', 'black').text("City");
    if (desc == '') {
        $("#missionDesc").prev('label').css('color', 'red').text("Description*");
        IsvalidMission = false;
    } else
        $("#missionDesc").prev('label').css('color', 'black').text("Description");
    if (Shortdesc == '') {
        $("#missionSDesc").prev('label').css('color', 'red').text("Short Description*");
        IsvalidMission = false;
    } else
        $("#missionSDesc").prev('label').css('color', 'black').text("Short Description");
    if (Sdate == "") {
        $("#MSdate").parent().prev('label').css('color', 'red').text("Start Date*");
        IsvalidMission = false;
    } else
        $("#MSdate").parent().prev('label').css('color', 'black').text("Start Date");
    if (type == 0) {
        if (Edate == "") {
            $("#MEdate").parent().prev('label').css('color', 'red').text("End Date*");
            IsvalidMission = false;
        } else
            $("#MEdate").parent().prev('label').css('color', 'black').text("End Date");
        if (Ddate == "") {
            $("#MDdate").parent().prev('label').css('color', 'red').text("Deadline*");
            IsvalidMission = false;
        } else
            $("#MDdate").parent().prev('label').css('color', 'black').text("Deadline");
    }
    if (type == 1) {
        if ( MGoalAction == 0) {
            $("#MissionGoalAction").prev('label').css('color', 'red').text("Action*");
            IsvalidMission = false;
        }
        else
            $("#MissionGoalAction").prev('label').css('color', 'black').text("Action");
        if (MGoalObj == "") {
            $("#MissionGoalObjective").prev('label').css('color', 'red').text("Goal Objective*");
            IsvalidMission = false;
        }
        else
            $("#MissionGoalObjective").prev('label').css('color', 'black').text("Goal Objective");
    }
    
    if (orgName == '') {
        $("#missionOrgnztn").prev('label').css('color', 'red').text("Organization Name*");
        IsvalidMission = false;
    } else
        $("#missionOrgnztn").prev('label').css('color', 'black').text("Organization Name");
    if (orgDetail == '') {
        $("#missionOrgnztnDetail").prev('label').css('color', 'red').text("Organization Detail*");
        IsvalidMission = false;
    } else
        $("#missionOrgnztnDetail").prev('label').css('color', 'black').text("Organization Detail");

    if ($("#preview-container").html() == '') {
        toastr.error("Please Add Atleast One Photo For Mission", '', {
        });
        IsvalidMission = false;
    }
    if (title != '' && Sdate == "" && type != null && seats != 0 && theme != null && country != null && city != null && desc != '' && Shortdesc != '' && orgName != '' && orgDetail != '') {
        if (type == 0 && ( Ddate != "" || Edate == ""))
            IsvalidMission = false;
        else if (type == 1 && (MGoalObj == 0 || MGoalAction == ""))
            IsvalidMission = false;
        else
            IsvalidMission = true;
    }


    if (IsvalidMission == false) {
        return
    }

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
    formData.append('missionId', MissionID);
    formData.append('missionName', title);
    formData.append('description', desc);
    formData.append('shortDescription', Shortdesc);
    formData.append('missionType', type);
    formData.append('seats', seats);
    formData.append('missionThemeId', theme);
    formData.append('missionCityId', city);
    formData.append('missionCountryId', country);
    formData.append('startDate', Sdate);
    formData.append('endDate', Edate);
    formData.append('deadline', Ddate);
    formData.append('Msn_Goal_Obj', MGoalObj);
    formData.append('Msn_Goal_Action', MGoalAction);
    formData.append('organizationName', orgName);
    formData.append('organizationDetails', orgDetail);


    $.each(skillIdList, function (index, skill) {
        formData.append('skillIds', skill);
    });
    for (var i = 0; i < delImgList.length; i++) {
        formData.append('delImgList', delImgList[i]);
    }
    for (var i = 0; i < delDocList.length; i++) {
        formData.append('delDocList', delDocList[i]);
    }
    for (var i = 0; i < fileNames.length; i++) {
        formData.append('fileList', fileNames[i]);
    }
    for (var i = 0; i < video.length; i++) {
        formData.append('VideoUrl', video[i].replace("watch?v=", "embed/"));
    }
    for (var i = 0; i < DocList.length; i++) {
        formData.append('docList', DocList[i]);
    }

    $.ajax({
        type: "POST",
        url: '/Admin/AddMissionbyAdmin',
        data: formData,
        contentType: false,
        processData: false,
        enctype: "multipart/form-data",
        success: function (data) {

            fileNames = [];
            $('#addmission').addClass('d-none');
            $('#missionDiv').load(location.href + ' #missionDiv >*', function () { missiontable(); });
            $("#mission").removeClass("d-none");
            toastr.success('Mission Saved Successfully', '', {
            });
            MissionID = 0;
        },
        error: function (e) {
            alert('error');
        }
    });
}


// -------------------------------------   show selected banner photo by user    ----------------------------------------------------------------//

    var bannerImage;
    const photoInput = document.querySelector('#photo-input');
    const previewContainer = document.querySelector('#preview-container-banner');
    const previewImage = document.createElement('img');

    photoInput.addEventListener('change', function() {
        $('#preview-container-banner').html("");
            const file = this.files[0];

    if (file) {
                const reader = new FileReader();

    previewImage.style.display = 'block';
    previewImage.style.width = '30vh';
    previewImage.style.height = '30vh';

    reader.addEventListener('load', function() {
        previewImage.setAttribute('src', this.result);
                });

        reader.readAsDataURL(file);
        bannerImage = file;
    previewContainer.appendChild(previewImage);
            } else {
        previewImage.style.display = null;
    previewImage.style.width = null;
    previewImage.style.height = null;
    previewImage.setAttribute('src', null);
    previewContainer.removeChild(previewImage);
            }
        });


//--------------------------------------      banner Search   --------------------------------------


$("bannerSearch").keyup(function () {
    let str = "bannerSearch"
    let search = $(this).val()
    $.ajax({
        type: "POST",
        url: '/Admin/Search',
        data: {
            'Search': search,
            'Identity': str,
        },
        dataType: "html", // return datatype like JSON and HTML
        success: function (data) {
            $("#bannerDiv").empty()
            $("#bannerDiv").html(data)
        },
        error: function (e) {
            toastr.error('Please Try again', 'Error', {
                "closeButton": true,
                positionClass: 'toast-top-center',
                "preventDuplicates": true,
                "timeOut": "2000",
            });
        },

    });
})



// --------------------------------------   Add banner    ----------------------------------------------------------

function AddBanner() {
    var Text = $('#BannerText').val();
    var sortOrder = $('#bannerSortOrder').val();
    var image = $('#preview-container-banner').html();


    let isValid = true;

    if (sortOrder.trim() == "") {
        $("#sort_error").css('color', 'red').html("Sort Order Is Required");
        isValid = false;
    }
    else { $("#sort_error").html(""); }
    if (image == "") {
        $("#image_error").css('color', 'red').html("Image Is Required");
        isValid = false;
    }
    else { $("#image_error").html(""); }
    if (isValid == false) {
        return;
    }

    var BannerData = new FormData();
    BannerData.append('BannerAddEditId', bannerAddEditId);
    BannerData.append('BannerText', Text);
    BannerData.append('BannerSortOrder', sortOrder);
    BannerData.append('BannerImage', bannerImage);
    $.ajax({
        url: "/Admin/AddBannerbyAdmin",
        type: "POST",
        data: BannerData,
        processData: false,
        contentType: false,
        success: function () {
            $('#addBanner').addClass('d-none');
            //$('#bannerDiv').load(location.href + ' #bannerDiv  >*');
            $('#bannerDiv').load(location.href + ' #bannerDiv >*', function () { bannerTable(); });
            $("#banner").removeClass("d-none");
            toastr.success("Banner Saved Successfully");
            bannerAddEditId = 0;
        },
        error: function () {
            // Handle any errors that occurred
            alert('error');
        }
    });
}


// -------------------------------------    edit banner by admin  ----------------------------------------------------------------//

function fetchBanner(bannerId) {
    $.ajax({
        url: "/Admin/FetchBanner",
        type: "POST",
        data: {
            'BannerId': bannerId,
        },
        success: function (result) {
            console.log(result);

            $('#BannerText').val(result.text);
            $('#bannerSortOrder').val(result.sortOrder);
            $('#preview-container-banner').html("");
            $('#preview-container-banner').append(`<img src="/bannerImages/${result.image}" style="display: block; width: 30vh; height: 30vh;">`)
            bannerAddEditId = result.bannerId;


        }
    });
}


// -------------------------------------    delete banner by admin  ----------------------------------------------------------------//

function DeleteBanner(bannerid) {


    $.ajax({
        url: "/Admin/DeleteBannerbyAdmin",
        type: "POST",
        data: {

            'bannerid': bannerid
        },
        success: function (data) {
            location.reload();
        },
        error: function () {
            alert("hi error");
        }
    });

}

// -----------------------------      Password Validation    -------------------------------------------------------------------
function password_verification() {
    let password = $("#PassWord").val().trim();
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



























$(document).ready(function () {
    $('a.list-group-item').click(function () {
        $('a.list-group-item.active').removeClass('active');
        $(this).addClass('active');
        setTimeout(function () {
            $('#sidebarMenu').removeClass('show');
        }, 100);

    });

    $('#alluserdiv').on('click', '.useredit', () => {
        $('#adduser').removeClass('d-none');
        $('#user').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#story').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#missionapplication').addClass('d-none');

        $('#addmission').addClass('d-none');
        $('#add').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');

        $('#viewstory').addClass('d-none');
    });

    $('#cmsDiv').on('click', '.viewCMS', () => {
        // view cms
        $('#add').removeClass('d-none');
        $('#user').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#story').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
    });

    $('#storyDiv').on('click', '.view', () => {

        $('#viewstory').removeClass('d-none');
        $('#user').addClass('d-none');
        $('#add').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#story').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
    });

    $('#themeDiv').on('click', '.viewTheme', () => {
    
            $('#addMissionTheme').removeClass('d-none');
            $('#user').addClass('d-none');
            $('#add').addClass('d-none');
            $('#banner').addClass('d-none');
            $('#addMissionSkill').addClass('d-none');
            $('#viewstory').addClass('d-none');
            $('#cms').addClass('d-none');
            $('#mission').addClass('d-none');
            $('#addmission').addClass('d-none');
            $('#missiontheme').addClass('d-none');
            $('#missionskill').addClass('d-none');
            $('#missionapplication').addClass('d-none');
            $('#story').addClass('d-none');
            $('#adduser').addClass('d-none');
            $('#addBanner').addClass('d-none');
        });

    $('#bannerDiv').on('click', '.viewBanner', () => {
    
            $('#addBanner').removeClass('d-none');
            $('#user').addClass('d-none');
            $('#add').addClass('d-none');
            $('#banner').addClass('d-none');
            $('#addMissionSkill').addClass('d-none');
            $('#viewstory').addClass('d-none');
            $('#cms').addClass('d-none');
            $('#mission').addClass('d-none');
            $('#addmission').addClass('d-none');
            $('#missiontheme').addClass('d-none');
            $('#missionskill').addClass('d-none');
            $('#missionapplication').addClass('d-none');
            $('#story').addClass('d-none');
            $('#adduser').addClass('d-none');
            $('#addMissionTheme').addClass('d-none');
        });

    $('#skillDiv').on('click', '.viewSkill', () => {
    
            $('#addMissionSkill').removeClass('d-none');
            $('#user').addClass('d-none');
            $('#add').addClass('d-none');
            $('#banner').addClass('d-none');
            $('#viewstory').addClass('d-none');
            $('#cms').addClass('d-none');
            $('#mission').addClass('d-none');
            $('#addmission').addClass('d-none');
            $('#missiontheme').addClass('d-none');
            $('#missionskill').addClass('d-none');
            $('#missionapplication').addClass('d-none');
            $('#story').addClass('d-none');
            $('#adduser').addClass('d-none');
            $('#addBanner').addClass('d-none');
            $('#addMissionTheme').addClass('d-none');
        });

    $('#missionDiv').on('click', '.viewMission', () => {
    
            $('#addmission').removeClass('d-none');
            $('#addMissionSkill').addClass('d-none');
            $('#user').addClass('d-none');
            $('#add').addClass('d-none');
            $('#banner').addClass('d-none');
            $('#viewstory').addClass('d-none');
            $('#cms').addClass('d-none');
            $('#mission').addClass('d-none');
            $('#missiontheme').addClass('d-none');
            $('#missionskill').addClass('d-none');
            $('#missionapplication').addClass('d-none');
            $('#story').addClass('d-none');
            $('#adduser').addClass('d-none');
            $('#addBanner').addClass('d-none');
            $('#addMissionTheme').addClass('d-none');
        });

});
$(document).ready(function () {
    $('#usertab').click(function () {
        $('#user').removeClass('d-none');
        $('#cms').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#story').addClass('d-none');

        $('#addMissionSkill').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#add').addClass('d-none');
        $('#addBanner').addClass('d-none');

        $('#viewstory').addClass('d-none');

    });
});
$(document).ready(function () {
    $('#banneradd').click(function () {
        $('#addBanner').removeClass('d-none');
        $("#addBanner").find("input").val("");
        $('#preview-container-banner').html("");

        $('#user').addClass('d-none');
        $('#story').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#missionapplication').addClass('d-none');

        $('#addMissionSkill').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#add').addClass('d-none');
        $('#viewstory').addClass('d-none');
    });
});
$(document).ready(function () {
    $('#useradd').click(function () {
        $('#adduser').removeClass('d-none');

        $("#EmailId").prop("readonly", false);
        $("#PassWord").prop("readonly", false);
        $("#adduser").find("input, select, textarea").val("");
        $('#userName').text("");
        $('#userAvtar').attr('src', '/media/user1.png');
        $('#CityId option:not(:first)').remove();
        $('#CityId')[0].selectedIndex = 0;
        $('#Country')[0].selectedIndex = 0;
        $('#Status')[0].selectedIndex = 0;

        $('#user').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#story').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#missiontheme').addClass('d-none');

        $('#addBanner').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#add').addClass('d-none');

        $('#viewstory').addClass('d-none');
    });
});
$(document).ready(function () {
    $('#missionskilltab').click(function () {
        $('#missionskill').removeClass('d-none');


        $('#addBanner').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#story').addClass('d-none');
        $('#add').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#user').addClass('d-none');
        $('#missiontheme').addClass('d-none');
    });
});
$(document).ready(function () {
    $('#missionThemetab').click(function () {
        $('#missiontheme').removeClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#story').addClass('d-none');
        $('#add').addClass('d-none');
        $('#user').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
    });
});
$(document).ready(function () {
    $('#missionapplicationtab').click(function () {
        $('#missionapplication').removeClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#story').addClass('d-none');
        $('#add').addClass('d-none');
        $('#user').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');

    });
});
$(document).ready(function () {
    $('#bannertab').click(function () {
        $('#banner').removeClass('d-none');
        $('#addmission').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#story').addClass('d-none');
        $('#add').addClass('d-none');
        $('#user').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
    });
});
$(document).ready(function () {
    $('#cmstab').click(function () {
        $('#cms').removeClass('d-none');
        $('#user').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#story').addClass('d-none');
        $('#add').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
    });
});
$(document).ready(function () {
    $('#missiontab').click(function () {
        $('#mission').removeClass('d-none');
        $('#cms').addClass('d-none');
        $('#user').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#story').addClass('d-none');
        $('#add').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
    });
});
$(document).ready(function () {
    $('#storytab').click(function () {
        $('#story').removeClass('d-none');
        $('#cms').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#user').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#add').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
    });
});
$(document).ready(function () {
    $('#addcms').click(function () {
        $("#add").find("input, select, textarea").val("");
        editor.setData('');
        $('#add').removeClass('d-none');
        $('#cms').addClass('d-none');
        $('#user').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#story').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
    });
});
$(document).ready(function () {
    $('#addTheme').click(function () {
        $("#addMissionTheme").find("input, select, textarea").val("");

        $('#addMissionTheme').removeClass('d-none');
        $('#cms').addClass('d-none');
        $('#user').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#story').addClass('d-none');
        $('#addBanner').addClass('d-none');
    });
});
$(document).ready(function () {
    $('#addSkill').click(function () {
        $("#addMissionSkill").find("input, select, textarea").val("");

        $('#addMissionSkill').removeClass('d-none');
        $('#cms').addClass('d-none');
        $('#user').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#story').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#add').addClass('d-none');

    });
});
$(document).ready(function () {
    $('#addSkill').click(function () {
        $("#addMissionSkill").find("input, select, textarea").val("");
        $('#addMissionSkill').removeClass('d-none');
        $('#cms').addClass('d-none');
        $('#user').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#story').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#add').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');

    });
});
$(document).ready(function () {
    $('#Cancle').click(function () {
        $('#user').removeClass('d-none');
        $('#add').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#story').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
        $('#addBanner').addClass('d-none');
    });
});
$(document).ready(function () {
    $('#cancleCMS').click(function () {
        $('#cms').removeClass('d-none');
        $('#add').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#story').addClass('d-none');
        $('#user').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');

    });
});
$(document).ready(function () {
    $('#cancleStory').click(function () {
        $('#story').removeClass('d-none');
        $('#add').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#user').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
    });
});
$(document).ready(function () {
    $('#cancleTheme').click(function () {
        $('#missiontheme').removeClass('d-none');
        $('#add').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#story').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#user').addClass('d-none');
        $('#addBanner').addClass('d-none');
    });
});
$(document).ready(function () {
    $('#cancleSkill').click(function () {
        $('#missionskill').removeClass('d-none');
        $('#add').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#story').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#user').addClass('d-none');
        $('#addBanner').addClass('d-none');
    });
});
$(document).ready(function () {
    $('#cancleMission').click(function () {
        $('#mission').removeClass('d-none');
        $('#missionskill').add('d-none');
        $('#add').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#story').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#user').addClass('d-none');
        $('#addmission').addClass('d-none');

    });
});
$(document).ready(function () {
    $('#cancleBanner').click(function () {
        $('#banner').removeClass('d-none');
        $('#missionskill').add('d-none');
        $('#add').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#story').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#user').addClass('d-none');
        $('#addmission').addClass('d-none');
        $('#addBanner').addClass('d-none');


    });
});
$(document).ready(function () {
    $('#missionadd').click(function () {
        $("#addmission").find("input, select, textarea").val("");
        Desc.setData('');
        SDesc.setData('');
        $("#selected-skills ul li").addClass('selected');
        $("#left,saveSkills").click();
        $("#preview-container").html('');
        $("#doclist").html('');
        $("#missionType").prop("disabled", false);

        $('#addmission').removeClass('d-none');
        $('#add').addClass('d-none');
        $('#adduser').addClass('d-none');
        $('#addMissionSkill').addClass('d-none');
        $('#cms').addClass('d-none');
        $('#banner').addClass('d-none');
        $('#viewstory').addClass('d-none');
        $('#missionapplication').addClass('d-none');
        $('#missiontheme').addClass('d-none');
        $('#missionskill').addClass('d-none');
        $('#mission').addClass('d-none');
        $('#story').addClass('d-none');
        $('#user').addClass('d-none');
        $('#addBanner').addClass('d-none');
        $('#addMissionTheme').addClass('d-none');
    });
});




// ------------------------ pagination for tables in admin    ---------------------------------------------------


$.extend(true, $.fn.dataTable.defaults, {
    "pagingType": "full_numbers",
    "language": {
        "paginate": {
            "first": "&laquo;",
            "last": "&raquo;",
            "previous": "&#60;",
            "next": "&#62;"
        }
    }
});

$(document).ready(function () {
    usertable()
    cmstable()
    missiontable()
    storytable()
    themetable()
    skilltable()
    missionAppTable()
    bannerTable()
})

// user datatable
function usertable() {
    var usertable = $('#usertable').dataTable({
        info: false,
        ordering: false,
        "lengthChange": false
    });
    $("#userSearch").keyup(function () {
        usertable.fnFilter(this.value);
    });
}

// cms datatable
function cmstable() {
    var cmstable = $('#cmstable').dataTable({
        info: false,
        ordering: false,
        "lengthChange": false
    });
    $("#cmsSearch").keyup(function () {
        cmstable.fnFilter(this.value);
    });
}

// mission datatable
function missiontable() {
    var missiontable = $('#missiontable').dataTable({
        info: false,
        ordering: false,
        "lengthChange": false
    });
    $("#missionSearch").keyup(function () {
        storySearch
        missiontable.fnFilter(this.value);
    });
}

// story datatable
function storytable() {
    var storytable = $('#storytable').dataTable({
        info: false,
        ordering: false,
        "lengthChange": false
    });
    $("#storySearch").keyup(function () {
        storytable.fnFilter(this.value);
    });
}

// mission theme datatable
function themetable() {
    var themetable = $('#themetable').dataTable({
        info: false,
        ordering: false,
        "lengthChange": false
    });
    $("#themeSearch").keyup(function () {
        themetable.fnFilter(this.value);
    });
}

// mission skill datatable
function skilltable() {
    var skilltable = $('#skilltable').dataTable({
        info: false,
        ordering: false,
        "lengthChange": false,
    });
    $("#skillSearch").keyup(function () {
        skilltable.fnFilter(this.value);
    });
}


// mission application datatable
function missionAppTable() {
    var missionAppTable = $('#missionAppTable').dataTable({
        info: false,
        ordering: false,
        "lengthChange": false
    });
    $("#MissionAppSearch").keyup(function () {
        missionAppTable.fnFilter(this.value);
    });
}


// Banner datatable
function bannerTable() {
    var bannerTable = $('#bannerTable').dataTable({
        info: false,
        ordering: false,
        "lengthChange": false
    });
    $("#bannerSearch").keyup(function () {
        bannerTable.fnFilter(this.value);
    });
}