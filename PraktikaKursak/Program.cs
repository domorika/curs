using Microsoft.EntityFrameworkCore;
using практы_курсак.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Настройка PostgreSQL
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

// Проверка подключения к базе данных (без миграций)
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        // Убираем await и делаем синхронный вызов или добавляем async
        var canConnect = dbContext.Database.CanConnect();

        if (canConnect)
        {
            Console.WriteLine("✅ Успешное подключение к базе данных PostgreSQL!");

            // Проверка, что таблицы существуют
            var servicesCount = dbContext.Services.Count();
            Console.WriteLine($"📊 В таблице Services: {servicesCount} записей");
        }
        else
        {
            Console.WriteLine("❌ Не удалось подключиться к базе данных");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Ошибка подключения к БД: {ex.Message}");
}

app.Run();