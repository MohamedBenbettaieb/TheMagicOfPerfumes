using TheMagicOfPerfumes.Models;

namespace TheMagicOfPerfumes.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
    Task<bool> HasSalesHistoryAsync(int productId);
    Task<bool> ExistsWithNameAsync(string name, int? excludeId = null);

}