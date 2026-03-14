using TheMagicOfPerfumes.Services.Interfaces;
using TheMagicOfPerfumes.ViewModels.Base;

namespace TheMagicOfPerfumes.ViewModels;

public partial class CustomerViewModel : ViewModelBase
{
    private readonly ICustomerService _customerService;
    public CustomerViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
    }
}