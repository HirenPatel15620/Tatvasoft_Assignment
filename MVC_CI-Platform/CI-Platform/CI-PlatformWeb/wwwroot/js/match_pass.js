$(document).ready(function () {
    $("#cnf-pass").on('keyup', function () {
        var password = $("#new-pass").val();
        var confirmPassword = $("#cnf-pass").val();
        if (password != confirmPassword) {
            $("#pass-error").html("Password does not match !").css("color", "red");
            $('#regi').prop('disabled', true);
        }
        else {
            $("#pass-error").html("");
            $('#regi').prop('disabled', false);
        }

        if (confirmPassword == "")
            $("#pass-error").html("");
    });
});