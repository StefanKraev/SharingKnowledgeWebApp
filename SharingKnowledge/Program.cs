using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Data;
using SharingKnowledge.Models;

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

            builder.Services.AddDefaultIdentity<Student>(options => { //User may have many roles other then student
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
            options.SignIn.RequireConfirmedEmail = configurationManager.GetValue<bool>("IdentityOptions:SignIn:RequiredConformedEmail");

            options.Lockout.MaxFailedAccessAttempts = configurationManager.GetValue<int>("IdentityOptions:Lockout:MaxFailedAttempts");
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(configurationManager.GetValue<int>("IdentityOptions:Lockout:LockoutDuration"));

            options.Password.RequireDigit = configurationManager.GetValue<bool>("IdentityOptions:Password:RequireDigit");
            options.Password.RequireUppercase = configurationManager.GetValue<bool>("IdentityOptions:Password:RequireUpper");
            options.Password.RequireLowercase = configurationManager.GetValue<bool>("IdentityOptions:Password:RequireLower");
            options.Password.RequireNonAlphanumeric = configurationManager.GetValue<bool>("IdentityOptions:Password:RequireNonAlphanumeric");
            


        }
    }
}
