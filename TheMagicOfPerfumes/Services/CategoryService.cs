using Microsoft.EntityFrameworkCore;
using TheMagicOfPerfumes.Data;
using TheMagicOfPerfumes.Models;
using TheMagicOfPerfumes.Services.Interfaces;

namespace TheMagicOfPerfumes.Services;

public class CategoryService : Repository<Category>, ICategoryService
{
    public CategoryService(AppDbContext context) : base(context) { }
    public async Task<bool> ExistsWithNameAsync(string name, int? excludeId = null)
            => await _context.Categories
                .AnyAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim()
                            && (!excludeId.HasValue || c.Id != excludeId.Value));
}