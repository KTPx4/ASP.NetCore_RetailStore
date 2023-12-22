using Final.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Final
{
    public class Program
    {
       
        public static void Main(string[] args)
        {
            var SECRET_KEY_LOGIN = "Auth-Login";

            var keyBytes = Encoding.UTF8.GetBytes(SECRET_KEY_LOGIN.PadRight(32));
            var SecurityKey = new SymmetricSecurityKey(keyBytes);


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Auth 
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/Forbidden";
                    options.Cookie.IsEssential = true;
                });


            var connectionString = builder.Configuration.GetConnectionString("MongoDB");
            Console.WriteLine(connectionString);
            builder.Services.AddDbContext<MyDataContext>(options => options.UseMongoDB(connectionString, "FinalNodejs")); // connectrionString, db name

            // add authentication - jwt
          


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStatusCodePagesWithReExecute("/Error/{0}"); // điều hướng lỗi 404

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Use Authentication
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


            app.MapRazorPages();

            app.Run();
        }
    }
}