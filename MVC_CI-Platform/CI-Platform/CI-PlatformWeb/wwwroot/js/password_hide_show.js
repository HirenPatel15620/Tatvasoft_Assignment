function showPass(idpass) {
    var type = document.getElementById(idpass).getAttribute("type");
    if (type == "text") {
        document.getElementById(idpass).setAttribute("type", "password");
        document.getElementById("eye_close-" + idpass).classList.add("visually-hidden");
    }
    else {
        document.getElementById(idpass).setAttribute("type", "text");
        document.getElementById("eye_close-" + idpass).classList.remove("visually-hidden");
    }
}