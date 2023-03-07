function validateEmail(email) {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
}

//// Usage example:
//const email = "john.doe@example.com";
if (validateEmail(email)) {
    // Email is valid
} else {
    alert = "mail is unvalid";
    // Email is invalid
}