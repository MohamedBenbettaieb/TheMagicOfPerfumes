using TheMagicOfPerfumes.Models;

namespace TheMagicOfPerfumes.Services.Interfaces;

public interface ISaleService
{
    Task<IEnumerable<Sale>> GetAllAsync();
    Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime from, DateTime to);
    Task<Sale?> GetByIdWithItemsAsync(int id);
    Task CreateSaleAsync(Sale sale, IEnumerable<SaleItem> items);
    Task DeleteAsync(Sale sale);
}