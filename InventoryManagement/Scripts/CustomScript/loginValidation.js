$(document).ready(function () {
    console.log("Document ready");

    //show-hide password 
    $('#showPass').on('change', function () {
        console.log("Checkbox changed");
        var passwordInput = $('#password');
        passwordInput.attr('type', this.checked ? 'text' : 'password');
    });

    //validation 
    function emptyCheck() {
        var userName = $('#userName').val();
        var password = $('#password').val();


        $('#usernameError').html('');
        $('#passwordError').html('');

        var isValid = true;

        if (userName === '') {
            $('#usernameError').html("*Username is required");
            isValid = false;
        }

        if (password === '') {
            $('#passwordError').html("*Password is required");
            isValid = false;
        }
        return isValid;
    }
    $('#userName').on('input', function () {
        $('#usernameError').html(''); 
    });

    $('#password').on('input', function () {
        $('#passwordError').html(''); 
    });

    $('#loginForm').on('submit', function (e) {
        e.preventDefault();

        if (!emptyCheck()) {
            return;
        }

        Swal.fire({
            title: 'Logging in...',
            text: 'Please wait',
            allowOutsideClick: false,
            showConfirmButton: false,
            willOpen: () => {
                Swal.showLoading();
            }
        });

        // form submission
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Login Successful',
                        text: 'Please wait, Redirecting to dashboard...',
                        showConfirmButton: false,
                        allowOutsideClick: false
                    })

                    window.location.href = response.redirectUrl;
                    
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Login Failed',
                        text: 'Incorrect username or password',
                        confirmButtonText: 'Try Again'
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An error occurred. Please try again.',
                    confirmButtonText: 'OK'
                });
            }
        });
    });
  
   
});