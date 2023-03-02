$(document).ready(function () {
    $("#new-pass").on('keyup', function () {
        var password = $("#new-pass").val();
        if (password.length >= 8 && password.length <= 15)
            $("#pass-limit-error").html("");
        else
            $("#pass-limit-error").html("Password must between 8 to 15 character!").css("color", "red");
        if (password == "")
            $("#pass-limit-error").html("");
    });
    $("#cnf-pass").on('keyup', function () {
        var password = $("#new-pass").val();
        var confirmPassword = $("#cnf-pass").val();
        if (password != confirmPassword) {
            $("#pass-error").html("Password does not match !").css("color", "red");
            $('#regi').prop('disabled', true);
        }
        else {
            $("#pass-error").html("");
            if (password.length >= 8 && password.length <= 15)
                $('#regi').prop('disabled', false);
        }
        if (confirmPassword == "")
            $("#pass-error").html("");
    });
});