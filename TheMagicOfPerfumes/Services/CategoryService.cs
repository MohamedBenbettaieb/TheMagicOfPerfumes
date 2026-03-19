using Microsoft.EntityFrameworkCore;
using TheMagicOfPerfumes.Data;
using TheMagicOfPerfumes.Models;
using TheMagicOfPerfumes.Services.Interfaces;

namespace TheMagicOfPerfumes.Services;

public class CategoryService : Repository<Category>, ICategoryService
{
    public CategoryService(AppDbContext context) : base(context) { }
    public async Task<bool> ExistsWithNameAsync(string name, int? excludeId = null)
    {
        var normalizedName = name?.Trim();

        return await _context.Categories
            .AnyAsync(c => EF.Functions.Collate(c.Name, "NOCASE") == normalizedName
            && (!excludeId.HasValue || c.Id != excludeId.Value));
    }
}