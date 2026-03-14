using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TheMagicOfPerfumes.Data;
using TheMagicOfPerfumes.ViewModels;
using TheMagicOfPerfumes.Views;

namespace TheMagicOfPerfumes
{
    public partial class App : Application
    {
        // The DI container — accessible app-wide
        public static IServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. Build the DI container
            var services = new ServiceCollection();
            ConfigureServices(services);
            Services = services.BuildServiceProvider();

            // 2. Apply migrations automatically on every startup
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();

            // 3. Resolve MainWindow from DI and show it
            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // --- Database ---
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=TheMagicOfPerfumes.db"));

            // --- ViewModels ---
            services.AddTransient<MainViewModel>();

            // --- Views ---
            services.AddTransient<MainWindow>();
        }
    }
}