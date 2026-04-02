using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Data;
using SharingKnowledge.Data.Models;
using SharingKnowledge.Data.Repository;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core;
using SharingKnowledge.Services.Core.Interfaces;

namespace SharingKnowledge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddScoped<ICourseRepository, OpenCourseRepository>();

            builder.Services.AddScoped<IStudentRepository, StudentRepository>();

            builder.Services.AddScoped<IOpenCoursesService, OpenCoursesService>();

            builder.Services.AddScoped<IMyCoursesService, MyCoursesService>();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => { //User may have many roles other then student
                                                                      //but for now student is the default choice
                Identity(options, builder.Configuration);
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }

        private static void Identity(IdentityOptions options, ConfigurationManager configurationManager)
        {
            //options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = configurationManager.GetValue<bool>("IdentityOptions:SignIn:RequiredConfirmedEmail");

            options.Lockout.MaxFailedAccessAttempts = configurationManager.GetValue<int>("IdentityOptions:Lockout:MaxFailedAttempts");
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(configurationManager.GetValue<int>("IdentityOptions:Lockout:LockoutDuration"));

            options.Password.RequireDigit = configurationManager.GetValue<bool>("IdentityOptions:Password:RequireDigit");
            options.Password.RequireUppercase = configurationManager.GetValue<bool>("IdentityOptions:Password:RequireUppercase");
            options.Password.RequireLowercase = configurationManager.GetValue<bool>("IdentityOptions:Password:RequireLowercase");
            options.Password.RequireNonAlphanumeric = configurationManager.GetValue<bool>("IdentityOptions:Password:RequireNonAlphanumeric");
            


        }
    }
}
