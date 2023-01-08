using Microsoft.EntityFrameworkCore;
using TaskTracker_DAL;

namespace TaskTracker
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication? webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<EfDbContext>())
                {
                    try
                    {
                        //appContext.Database.EnsureCreated();
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            return webApp;
        }
    }
}
