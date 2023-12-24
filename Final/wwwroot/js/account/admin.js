$(document).ready(()=>{
    // Lấy modal
    setTimeout(()=>{
        $('#alert-err').addClass('d-none')
    }, 2600)
    setTimeout(()=>{
        $('#alert-succ').addClass('d-none')
    }, 2600)

    var modal = document.getElementById('myModal');

    // Lấy nút mở modal
    var btn = document.querySelector('.filter-options button.bg-success');

    // Lấy nút đóng modal
    var span = document.getElementsByClassName('close')[0];

    // Khi người dùng nhấn nút, mở modal
    btn.onclick = function () {
    modal.style.display = 'block';
    };

    // Khi người dùng nhấn vào nút đóng, đóng modal
    span.onclick = function () {
    modal.style.display = 'none';
    };
   
    $('#i-name').on('click',()=> {
        $('#modal-add-err').addClass('d-none')
    })
    $('#i-email').on('click',()=> {
        $('#modal-add-err').addClass('d-none')
    })

    $("#formAdd").submit((e)=>{
        e.preventDefault()
        let name = $('#i-name').val()
        let email = $('#i-email').val()
        let err = $('#modal-add-err')
        if(!name)
        {
            err.removeClass('d-none')
            err.html('Please input Full Name!')
        }
        else if(!email)
        {
            err.removeClass('d-none')
            err.html('Please input Email!')
        }
        else if(!isValidEmail(email))
        {
            err.removeClass('d-none')
            err.html('Invalid Email!')
        }
        else
        {  
            $('#formAdd').off('submit').submit();
        }
    })

// Khi người dùng nhấn ra ngoài modal, đóng modal
// window.onclick = function (event) {
//   if (event.target == modal) {
//     modal.style.display = 'none';
//   }
// };


})

function isValidEmail(email) {
    // Biểu thức chính quy kiểm tra địa chỉ email
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}