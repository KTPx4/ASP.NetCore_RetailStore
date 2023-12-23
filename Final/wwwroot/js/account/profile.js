
$(document).ready(() => {
    $('#imageInput').change(function () {
        // Get the selected file
        var file = this.files[0];

        // Display the selected image in the preview
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#profileImagePreview').attr('src', e.target.result);
            }
            reader.readAsDataURL(file);
        }
    });

    setTimeout(()=>{
        $('#errAlert').removeClass("show")
        $('#errAlert').addClass("hide")
    }, 2000)

    $("#FormProfile").submit((e)=>{
        e.preventDefault()        

        let fullName = $("#i-fullName").val()
        let oldPass = $("#i-oldPass").val()
        let newPass = $("#i-newPass").val()
        let confirmPass = $("#i-confirmPass").val()

        if(fullName == "" || fullName == null)
        {
            alert("Please input Full Name")
            return;
        }
        if(oldPass)
        {
            if(!newPass)
            {
                alert("Please input new password")
                return;

            }
            if(!confirmPass)
            {
                alert("Please input confirm password")
                return;
            }
            if(newPass != confirmPass)
            {
                alert("New Password and Confirm not match")
                return;
            }
        }

        $("#loading").removeClass("d-none")
        setTimeout(()=>{
            $('#FormProfile').off('submit').submit();
        }, 600)
    })
   
});