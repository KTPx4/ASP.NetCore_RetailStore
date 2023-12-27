$(document).ready(()=>{

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













})