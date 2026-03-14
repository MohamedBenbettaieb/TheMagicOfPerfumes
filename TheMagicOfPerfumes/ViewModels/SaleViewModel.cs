using TheMagicOfPerfumes.Services.Interfaces;
using TheMagicOfPerfumes.ViewModels.Base;

namespace TheMagicOfPerfumes.ViewModels;

public partial class SaleViewModel : ViewModelBase
{
    private readonly ISaleService _saleService;
    private readonly IProductService _productService;
    private readonly ICustomerService _customerService;
    public SaleViewModel(ISaleService saleService, IProductService productService, ICustomerService customerService)
    {
        _saleService = saleService;
        _productService = productService;
        _customerService = customerService;
    }
}