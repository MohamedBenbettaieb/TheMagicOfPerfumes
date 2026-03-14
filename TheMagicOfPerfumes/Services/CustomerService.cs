using TheMagicOfPerfumes.Data;
using TheMagicOfPerfumes.Models;
using TheMagicOfPerfumes.Services.Interfaces;

namespace TheMagicOfPerfumes.Services;

public class CustomerService : Repository<Customer>, ICustomerService
{
    public CustomerService(AppDbContext context) : base(context) { }
}