namespace Final.Middleware
{
    public class FirstLoginMiddleware
    {
        private readonly RequestDelegate _next;

        public FirstLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var user = context.User;
            var firstLoginClaim = user.FindFirst("firstLogin");


            // Kiểm tra xem nếu người dùng đã đang ở trang đổi mật khẩu, thì không cần chuyển hướng lại
            if (context.Request.Path != "/Account/ChangePassword" &&
                (context.Request.Path != "/Account/Login" && context.Request.QueryString.ToString() != "?handler=Logout") && // Thêm điều kiện ở đây
                context.Request.Path != "/Account/Active" &&
                firstLoginClaim != null &&
                bool.TryParse(firstLoginClaim.Value, out var isFirstLogin) && isFirstLogin)
            {
                // Là lần đăng nhập đầu tiên, chuyển hướng đến trang đổi mật khẩu
                context.Response.Redirect("/Account/ChangePassword");
                return;
            }

            // Tiếp tục xử lý request
            await _next(context);
        }
    }

}
