using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Data;
using SharingKnowledge.Data.Configurations;
using SharingKnowledge.Data.Models;
using SharingKnowledge.Data.Repository;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Admin;
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

            builder.Services.AddScoped<IAdminService, AdminService>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                builder.Configuration.GetSection("IdentityOptions").Bind(options);
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                DataBaseSeeder.SeedRoles(services);
                DataBaseSeeder.AssignAdminRole(services);
            }

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

            app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("Content-Security-Policy",
                    "default-src 'self'; " +
                    "script-src 'self'; " + 
                    "style-src 'self' https://cdn.jsdelivr.net; " + 
                    "img-src 'self' data:; " +
                    "font-src 'self'; " +
                    "frame-ancestors 'none';");

                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

                if (context.User.Identity?.IsAuthenticated == true && context.Request.Path == "/")
                {
                    if (context.User.IsInRole("Admin"))
                    {
                        context.Response.Redirect("/Admin/Home/Index");
                        return;
                    }
                }

                await next();
            });

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
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
            options.SignIn.RequireConfirmedAccount = configurationManager.GetValue<bool>("IdentityOptions:SignIn:RequiredConfirmedAccount");
            options.SignIn.RequireConfirmedEmail = configurationManager.GetValue<bool>("IdentityOptions:SignIn:RequiredConfirmedEmail");

            options.Lockout.MaxFailedAccessAttempts = configurationManager.GetValue<int>("IdentityOptions:Lockout:MaxFailedAttempts");
            var lockoutValue = configurationManager.GetValue<string>("IdentityOptions:Lockout:DefaultLockoutTimeSpan");
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.Parse(lockoutValue ?? "00:05:00");

            options.Password.RequireDigit = configurationManager.GetValue<bool>("IdentityOptions:Password:RequireDigit");
            options.Password.RequireUppercase = configurationManager.GetValue<bool>("IdentityOptions:Password:RequireUppercase");
            options.Password.RequireLowercase = configurationManager.GetValue<bool>("IdentityOptions:Password:RequireLowercase");
            options.Password.RequireNonAlphanumeric = configurationManager.GetValue<bool>("IdentityOptions:Password:RequireNonAlphanumeric");

            options.Password.RequiredLength = configurationManager.GetValue<int>("IdentityOptions:Password:RequiredLength");
            options.Password.RequiredUniqueChars = configurationManager.GetValue<int>("IdentityOptions:Password:RequiredUniqueChars");



        }
    }
}
