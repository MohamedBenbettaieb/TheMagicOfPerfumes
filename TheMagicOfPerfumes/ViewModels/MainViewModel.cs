using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TheMagicOfPerfumes.Services.Interfaces;
using TheMagicOfPerfumes.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;

namespace TheMagicOfPerfumes.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IServiceScopeFactory _scopeFactory;
    private IServiceScope? _currentScope;

    [ObservableProperty]
    private ViewModelBase _currentViewModel = null!;

    public MainViewModel(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        // Default screen on startup
        NavigateToDashboard();
    }

    private void SetCurrentViewModel(ViewModelBase newViewModel, IServiceScope? newScope)
    {
        // Dispose the previous scope (and its scoped services) when navigating away.
        _currentScope?.Dispose();
        _currentScope = newScope;
        CurrentViewModel = newViewModel;
    }

    [RelayCommand]
    private void NavigateToDashboard()
    {
        // Dashboard does not require EF‑backed scoped services, so no scope is needed.
        SetCurrentViewModel(new DashboardViewModel(), null);
    }

    [RelayCommand]
    private void NavigateToCategories()
    {
        var scope = _scopeFactory.CreateScope();
        var viewModel = scope.ServiceProvider.GetRequiredService<CategoryViewModel>();
        SetCurrentViewModel(viewModel, scope);
    }

    [RelayCommand]
    private void NavigateToProducts()
    {
        var scope = _scopeFactory.CreateScope();
        var viewModel = scope.ServiceProvider.GetRequiredService<ProductViewModel>();
        SetCurrentViewModel(viewModel, scope);
    }

    [RelayCommand]
    private void NavigateToCustomers()
    {
        var scope = _scopeFactory.CreateScope();
        var viewModel = scope.ServiceProvider.GetRequiredService<CustomerViewModel>();
        SetCurrentViewModel(viewModel, scope);
    }

    [RelayCommand]
    private void NavigateToSales()
    {
        var scope = _scopeFactory.CreateScope();
        var viewModel = scope.ServiceProvider.GetRequiredService<SaleViewModel>();
        SetCurrentViewModel(viewModel, scope);
    }
}