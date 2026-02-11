using Microsoft.EntityFrameworkCore;
using Mini_Task_Manager_Application.Data;
using Mini_Task_Manager_Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure EF Core with SQL Server / PostgreSQL / SQLite. Set `DefaultConnection` in configuration or environment.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
string chosenEfProvider = "None";
if (!string.IsNullOrWhiteSpace(connectionString))
{
    // Detect common SQL Server connection string keywords first
    var isSqlServer = connectionString.Contains("Initial Catalog", StringComparison.OrdinalIgnoreCase)
        || connectionString.Contains("User ID=", StringComparison.OrdinalIgnoreCase)
        || connectionString.Contains("Trusted_Connection", StringComparison.OrdinalIgnoreCase)
        || connectionString.Contains("Integrated Security", StringComparison.OrdinalIgnoreCase)
        || (connectionString.Contains("Server=", StringComparison.OrdinalIgnoreCase)
            && (connectionString.Contains("tcp:", StringComparison.OrdinalIgnoreCase) || connectionString.Contains(",")));

    // Detect SQLite by connection string hints
    var isSqlite = connectionString.Contains("Data Source=", StringComparison.OrdinalIgnoreCase)
        || connectionString.Contains("Filename=", StringComparison.OrdinalIgnoreCase)
        || connectionString.EndsWith(".db", StringComparison.OrdinalIgnoreCase);

    if (isSqlServer)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        chosenEfProvider = "SqlServer";
    }
    else if (isSqlite)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
        chosenEfProvider = "Sqlite";
    }
    else
    {
        // Fall back to PostgreSQL by default if not recognized
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        chosenEfProvider = "Postgres";
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

// Log the chosen EF provider for easier debugging in production
try
{
    app.Logger.LogInformation("EF provider selected: {Provider}", chosenEfProvider);
}
catch { }

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
