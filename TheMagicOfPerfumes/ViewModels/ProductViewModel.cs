using TheMagicOfPerfumes.Services.Interfaces;
using TheMagicOfPerfumes.ViewModels.Base;

namespace TheMagicOfPerfumes.ViewModels;

public partial class ProductViewModel : ViewModelBase
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    public ProductViewModel(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }
}