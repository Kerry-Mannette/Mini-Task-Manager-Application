using Microsoft.EntityFrameworkCore;
using Mini_Task_Manager_Application.Data;
using Mini_Task_Manager_Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure EF Core with PostgreSQL/SQLite. Set `DefaultConnection` in configuration or environment.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrWhiteSpace(connectionString))
{
    // Detect SQLite by connection string hints, otherwise assume Postgres.
    if (connectionString.Contains("Data Source=", StringComparison.OrdinalIgnoreCase)
        || connectionString.Contains("Filename=", StringComparison.OrdinalIgnoreCase)
        || connectionString.EndsWith(".db", StringComparison.OrdinalIgnoreCase))
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
    }
    else
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
else
{
    // No connection string found — for Development, fall back to a local SQLite file database.
    if (builder.Environment.IsDevelopment())
    {
        var dataPath = Path.Combine(builder.Environment.ContentRootPath, "MiniTaskManager.db");
        var sqliteConn = $"Data Source={dataPath}";
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(sqliteConn));
        builder.Logging.AddDebug();
        builder.Logging.AddConsole();
    }
    else
    {
        // In production without a connection string, log a clear error at startup.
        builder.Services.AddSingleton<IStartupFilter>(new StartupFilterMissingConnectionString());
    }
}

var app = builder.Build();

// Ensure database is created / migrations are applied where appropriate.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
    if (db is not null)
    {
        try
        {
            db.Database.Migrate();
        }
        catch (Exception)
        {
            try { db.Database.EnsureCreated(); } catch { }
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
