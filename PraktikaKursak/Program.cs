using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using практы_курсак.Data;
using практы_курсак.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Настройка PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Настройка аутентификации
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ============================================
// СОЗДАНИЕ РОЛИ ADMIN И АДМИНИСТРАТОРА
// ЭТОТ БЛОК ДОЛЖЕН БЫТЬ ПОСЛЕ app = builder.Build() !!!
// ============================================
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Создание роли Admin (если её нет)
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        var roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
        if (roleResult.Succeeded)
        {
            Console.WriteLine("Роль Admin создана");
        }
        else
        {
            foreach (var error in roleResult.Errors)
            {
                Console.WriteLine($"Ошибка создания роли: {error.Description}");
            }
        }
    }

    // Создание администратора
    var adminEmail = "admin@artkadr.ru";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            LastName = "Администратор",
            FirstName = "Системы",
            MiddleName = null,
            PhoneNumber = "+7 (999) 999-99-99",
            IsAdmin = true,
            RegisteredAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(adminUser, "Admin123!");

        if (result.Succeeded)
        {
            // Добавляем пользователя в роль Admin
            var addToRoleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
            if (addToRoleResult.Succeeded)
            {
                Console.WriteLine("Администратор создан и добавлен в роль Admin");
            }
            else
            {
                foreach (var error in addToRoleResult.Errors)
                {
                    Console.WriteLine($"Ошибка добавления в роль: {error.Description}");
                }
            }
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Ошибка создания администратора: {error.Description}");
            }
        }
    }
}

app.Run();