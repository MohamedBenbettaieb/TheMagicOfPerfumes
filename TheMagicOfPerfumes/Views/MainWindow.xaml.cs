using System.Windows;
using TheMagicOfPerfumes.ViewModels;

namespace TheMagicOfPerfumes.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; // ViewModel injected by DI
        }
    }
}