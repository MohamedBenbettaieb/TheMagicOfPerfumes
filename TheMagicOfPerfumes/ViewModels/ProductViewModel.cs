using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using TheMagicOfPerfumes.Models;
using TheMagicOfPerfumes.Services.Interfaces;
using TheMagicOfPerfumes.ViewModels.Base;

namespace TheMagicOfPerfumes.ViewModels;

public interface IProductDialogService
{
    MessageBoxResult ShowConfirmation(string message, string caption, MessageBoxButton buttons, MessageBoxImage icon);
}

internal sealed class MessageBoxProductDialogService : IProductDialogService
{
    public MessageBoxResult ShowConfirmation(string message, string caption, MessageBoxButton buttons, MessageBoxImage icon)
    {
        return MessageBox.Show(message, caption, buttons, icon);
    }
}

public partial class ProductViewModel : ViewModelBase
{
    private readonly IProductService _productService;
    private IProductDialogService _dialogService = new MessageBoxProductDialogService();

    internal IProductDialogService DialogService
    {
        get => _dialogService;
        set => _dialogService = value ?? throw new ArgumentNullException(nameof(value));
    }
    private readonly ICategoryService _categoryService;

    // --- Backing list (full, unfiltered) ---
    private List<Product> _allProducts = new();

    // --- Collections ---
    [ObservableProperty]
    private ObservableCollection<Product> _filteredProducts = new();

    [ObservableProperty]
    private ObservableCollection<Category> _categories = new();

    // --- Filter ---
    [ObservableProperty]
    private Category? _selectedFilterCategory;

    // --- Form fields ---
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string _newProductName = string.Empty;

    [ObservableProperty]
    private string _newProductDescription = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private decimal? _newProductPrice;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private Category? _newProductCategory;

    // --- State ---
    [ObservableProperty]
    private Product? _selectedProduct;

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private string _successMessage = string.Empty;

    // --- Computed ---
    public string FormTitle => IsEditing ? "Edit Product" : "Add New Product";
    public string SaveButtonText => IsEditing ? "Update" : "Add Product";

    public ProductViewModel(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        try
        {
            var categories = await _categoryService.GetAllAsync();
            Categories = new ObservableCollection<Category>(categories);
            await LoadProductsAsync();
            ErrorMessage = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load categories: {ex.Message}";
        }
    }

    private async Task LoadProductsAsync()
    {
        var products = await _productService.GetAllAsync();
        _allProducts = products.ToList();
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        var filtered = SelectedFilterCategory == null
            ? _allProducts
            : _allProducts.Where(p => p.CategoryId == SelectedFilterCategory.Id);

        FilteredProducts = new ObservableCollection<Product>(filtered);
    }

    partial void OnSelectedFilterCategoryChanged(Category? value) => ApplyFilter();

    partial void OnNewProductNameChanged(string value) { ErrorMessage = string.Empty; SuccessMessage = string.Empty; }

    partial void OnIsEditingChanged(bool value)
    {
        OnPropertyChanged(nameof(FormTitle));
        OnPropertyChanged(nameof(SaveButtonText));
    }

    // --- Commands ---

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        var excludeId = IsEditing ? SelectedProduct?.Id : null;
        var exists = await _productService.ExistsWithNameAsync(NewProductName, excludeId);

        if (exists)
        {
            ErrorMessage = "A product with this name already exists.";
            return;
        }

        if (IsEditing && SelectedProduct != null)
        {
            SelectedProduct.Name = NewProductName;
            SelectedProduct.Description = NewProductDescription;
            SelectedProduct.Price = NewProductPrice!.Value;
            SelectedProduct.CategoryId = NewProductCategory!.Id;
            await _productService.UpdateAsync(SelectedProduct);
        }
        else
        {
            var product = new Product
            {
                Name = NewProductName,
                Description = NewProductDescription,
                Price = NewProductPrice!.Value,
                CategoryId = NewProductCategory!.Id
            };
            await _productService.AddAsync(product);
        }

        await LoadProductsAsync();
        CancelEdit();
        SuccessMessage = IsEditing ? "Product updated successfully." : "Product added successfully.";
    }

    private bool CanSave() =>
        !string.IsNullOrWhiteSpace(NewProductName) &&
        NewProductPrice > 0 &&
        NewProductCategory != null;

    [RelayCommand]
    private void EditProduct(Product product)
    {
        SelectedProduct = product;
        NewProductName = product.Name;
        NewProductDescription = product.Description;
        NewProductPrice = product.Price;

        NewProductCategory = Categories.FirstOrDefault(c => c.Id == product.CategoryId);
        IsEditing = true;
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    private async Task DeleteProductAsync(Product product)
    {
        var result = _dialogService.ShowConfirmation(
            $"Are you sure you want to delete '{product.Name}'?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        var hasSales = await _productService.HasSalesHistoryAsync(product.Id);
        if (hasSales)
        {
            ErrorMessage = $"Cannot delete '{product.Name}' — it has sales history.";
            return;
        }

        await _productService.DeleteAsync(product);
        await LoadProductsAsync();
        SuccessMessage = "Product deleted successfully.";
    }

    [RelayCommand]
    private void ClearFilter()
    {
        SelectedFilterCategory = null;
    }

    [RelayCommand]
    private void CancelEdit()
    {
        SelectedProduct = null;
        NewProductName = string.Empty;
        NewProductDescription = string.Empty;
        NewProductPrice = null;
        NewProductCategory = null;
        IsEditing = false;
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;
    }
}