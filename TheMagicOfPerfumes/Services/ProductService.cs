using Microsoft.EntityFrameworkCore;
using TheMagicOfPerfumes.Data;
using TheMagicOfPerfumes.Models;
using TheMagicOfPerfumes.Services.Interfaces;

namespace TheMagicOfPerfumes.Services;

public class ProductService : Repository<Product>, IProductService
{
    public ProductService(AppDbContext context) : base(context) { }

    public new async Task<IEnumerable<Product>> GetAllAsync()
        => await _context.Products
            .Include(p => p.Category)
            .ToListAsync();

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        => await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Category)
            .ToListAsync();

    public async Task<bool> HasSalesHistoryAsync(int productId)
        => await _context.SaleItems
            .AnyAsync(si => si.ProductId == productId);

    public async Task<bool> ExistsWithNameAsync(string name, int? excludeId = null)
    => await _context.Products
        .AnyAsync(p => EF.Functions.Collate(p.Name, "NOCASE") == EF.Functions.Collate(name, "NOCASE")
            && (!excludeId.HasValue || p.Id != excludeId.Value));
}