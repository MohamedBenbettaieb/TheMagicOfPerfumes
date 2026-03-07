using Microsoft.Extensions.DependencyInjection;
using System.Windows;
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

            // 2. Resolve MainWindow from DI and show it
            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // --- ViewModels ---
            services.AddTransient<MainViewModel>();

            // --- Views ---
            services.AddTransient<MainWindow>();
        }
    }
}