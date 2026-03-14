using TheMagicOfPerfumes.Services.Interfaces;
using TheMagicOfPerfumes.ViewModels.Base;

namespace TheMagicOfPerfumes.ViewModels;

public partial class CategoryViewModel : ViewModelBase
{
    private readonly ICategoryService _categoryService;
    public CategoryViewModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
}