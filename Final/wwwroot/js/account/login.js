$(document).ready(() => {
   


    $("#i-UserName").on("click", () => {      
        $("#ErrMess").hide()
    });
    $("#i-Password").on("click", () => {        
        $("#ErrMess").hide()
    });
   $("#FormLogin").submit((e)=>{
    e.preventDefault();
    $('#loading').removeClass('d-none');
    setTimeout(()=>{
        $('#FormLogin').off('submit').submit();
    }, 1200)
   })
});