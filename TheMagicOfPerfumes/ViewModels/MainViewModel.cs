using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TheMagicOfPerfumes.Services.Interfaces;
using TheMagicOfPerfumes.ViewModels.Base;

namespace TheMagicOfPerfumes.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;
    private readonly ICustomerService _customerService;
    private readonly ISaleService _saleService;

    [ObservableProperty]
    private ViewModelBase _currentViewModel = null!;

    public MainViewModel(
        ICategoryService categoryService,
        IProductService productService,
        ICustomerService customerService,
        ISaleService saleService)
    {
        _categoryService = categoryService;
        _productService = productService;
        _customerService = customerService;
        _saleService = saleService;

        // Default screen on startup
        NavigateToDashboard();
    }

    [RelayCommand]
    private void NavigateToDashboard()
    {
        CurrentViewModel = new DashboardViewModel();
    }

    [RelayCommand]
    private void NavigateToCategories()
    {
        CurrentViewModel = new CategoryViewModel(_categoryService);
    }

    [RelayCommand]
    private void NavigateToProducts()
    {
        CurrentViewModel = new ProductViewModel(_productService, _categoryService);
    }

    [RelayCommand]
    private void NavigateToCustomers()
    {
        CurrentViewModel = new CustomerViewModel(_customerService);
    }

    [RelayCommand]
    private void NavigateToSales()
    {
        CurrentViewModel = new SaleViewModel(_saleService, _productService, _customerService);
    }
}