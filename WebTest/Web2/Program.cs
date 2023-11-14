using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Security.Claims;
using Web2.Data;
using Web2.Models;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages(options =>
        {
            options.Conventions.AuthorizeFolder("/Database");
			options.Conventions.AuthorizeFolder("/Admin");
			options.Conventions.AuthorizeFolder("/Processing");
		});

        builder.Services.AddDbContext<AppDbContext>(
                        options => options.UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext")));

        builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

		builder.Services.Configure<IdentityOptions>(options =>
		{
			// Default Lockout settings.
			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
			options.Lockout.MaxFailedAccessAttempts = 5;
			options.Lockout.AllowedForNewUsers = true;
			// Default Password settings.
			options.Password.RequireDigit = false;
			options.Password.RequireLowercase = false;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;
			options.Password.RequiredLength = 8;
			options.Password.RequiredUniqueChars = 1;
			// Default SignIn settings.
			options.SignIn.RequireConfirmedEmail = false;
			options.SignIn.RequireConfirmedPhoneNumber = false;
			// Default User settings.
			options.User.AllowedUserNameCharacters =
					"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
			options.User.RequireUniqueEmail = true;
		});

		// Cookie
		builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Log");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Index");
				});

		//builder.Services.AddAntiforgery(options => options.Cookie.Name = "X-CSRF-TOKEN-COOKIENAME");

		var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

		app.MapRazorPages();

		using (var scope = app.Services.CreateScope())
        {
            var roleP = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "Doctor" };

            foreach (var role in roles)
            {
                if (!await roleP.RoleExistsAsync(role))
                    await roleP.CreateAsync(new IdentityRole(role));
            }
        }

        using (var scope = app.Services.CreateScope())
        {
            var userP = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            string Login = "Kenroy";
            string Password = "14040205Dd!";
            string Surname = "Корней";
            string Name = "Дмитрий";
            string Phone = "89039192952";
            string Email = "dkorney@inbox.ru";

            if (await userP.FindByEmailAsync(Email) == null)
            {
                var user = new User();
                user.NormalizedEmail = Email;
                user.UserName = Email;
                user.Login = Login;
                user.Phone = Phone;
                user.Name = Name;
                user.Surname = Surname;
                user.Email = Email;

                await userP.CreateAsync(user, Password);

                await userP.AddToRoleAsync(user, "Admin");
            }
        }

        app.Run();

    }
}
