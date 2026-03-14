using TheMagicOfPerfumes.Data;
using TheMagicOfPerfumes.Models;
using TheMagicOfPerfumes.Services.Interfaces;

namespace TheMagicOfPerfumes.Services;

public class CategoryService : Repository<Category>, ICategoryService
{
    public CategoryService(AppDbContext context) : base(context) { }
}