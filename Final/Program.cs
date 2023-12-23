using Final.Middleware;
using Final.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Final
{
    public class Program
    {
       
        public static void Main(string[] args)
        {
           
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

            app.UseMiddleware<FirstLoginMiddleware>();

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