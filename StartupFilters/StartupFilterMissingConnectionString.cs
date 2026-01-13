using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Mini_Task_Manager_Application
{
    public class StartupFilterMissingConnectionString : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                var logger = app.ApplicationServices.GetService(typeof(Microsoft.Extensions.Logging.ILogger<StartupFilterMissingConnectionString>)) as Microsoft.Extensions.Logging.ILogger;
                logger?.LogError("No DefaultConnection found. The app requires a PostgreSQL connection string in production. Set ConnectionStrings__DefaultConnection in appsettings or Azure App Service Configuration.");
                next(app);
            };
        }
    }
}
