using Microsoft.EntityFrameworkCore;
using практы_курсак.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var canConnect = await dbContext.Database.CanConnectAsync();

        if (canConnect)
        {
            Console.WriteLine("Успешное подключение к базе данных PostgreSQL!");
            =
            var servicesCount = await dbContext.Services.CountAsync();
            Console.WriteLine($"В таблице Services: {servicesCount} записей");
        }
        else
        {
            Console.WriteLine("Не удалось подключиться к базе данных");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка подключения к БД: {ex.Message}");
}

app.Run();