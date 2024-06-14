using Euro_2024_Management_System.Server.Data;
using Euro_2024_Management_System.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Euro_2024_Management_System.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Inicjalizacja danych
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    SeedRoles.Initialize(userManager, roleManager).Wait();
}

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    try
    {
        string roleName = "Admin";

        // Sprawdzamy, czy rola ju¿ istnieje
        var roleExists = await roleManager.RoleExistsAsync(roleName);

        if (!roleExists)
        {
            // Tworzymy now¹ rolê
            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));

            if (roleResult.Succeeded)
            {
                Console.WriteLine($"Utworzono now¹ rolê: {roleName}");
            }
            else
            {
                Console.WriteLine($"B³¹d podczas tworzenia roli: {roleName}");
                foreach (var error in roleResult.Errors)
                {
                    Console.WriteLine($"Error: {error.Description}");
                }
            }
        }
        else
        {
            Console.WriteLine($"Rola {roleName} ju¿ istnieje.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Wyst¹pi³ b³¹d podczas tworzenia roli: {ex.Message}");
    }
}

// Inicjalizacja danych
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    try
    {
        var userId = "09ccc57d-efe2-42c8-9365-bdbc109980c8"; 
        var user = await userManager.FindByIdAsync(userId);

        if (user != null)
        {
            // Sprawdzamy, czy u¿ytkownik ma ju¿ rolê Admin
            var isAdmin = await userManager.IsInRoleAsync(user, "Admin");

            if (!isAdmin)
            {
                // Dodawanie roli Admin
                await userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                Console.WriteLine("U¿ytkownik ju¿ posiada rolê Admin.");
            }
        }
        else
        {
            Console.WriteLine($"Nie mo¿na znaleŸæ u¿ytkownika o ID: {userId}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Wyst¹pi³ b³¹d podczas przypisywania roli Admin: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication(); // Dodane, aby uwzglêdniæ uwierzytelnianie
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();

