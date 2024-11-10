$(function () {
    $("#loginForm").validate({
        rules: {
            Email: {
                required: true,
                email: true
            },
            Password: {
                required: true,
                minlength: 3
            },
        },
        messages: {
            Email: {
                required: "Please enter email address",
                regex: "Please enter a valid email address"
            },
            Password: {
                required: "Please enter password",
                minlength: "Password must be at least 3 characters long"
            },
        },
        highlight: function (element) {
            $(element).addClass('is-invalid').removeClass('is-valid');
        },
        unhighlight: function (element) {
            $(element).addClass('is-valid').removeClass('is-invalid');
        }
    });
}); 