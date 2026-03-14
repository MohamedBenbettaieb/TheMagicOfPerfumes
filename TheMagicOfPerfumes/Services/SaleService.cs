using Microsoft.EntityFrameworkCore;
using TheMagicOfPerfumes.Data;
using TheMagicOfPerfumes.Models;
using TheMagicOfPerfumes.Services.Interfaces;

namespace TheMagicOfPerfumes.Services;

public class SaleService : Repository<Sale>, ISaleService
{
    public SaleService(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Sale>> GetAllAsync()
        => await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.SaleItems)
            .OrderByDescending(s => s.Date)
            .ToListAsync();

    public async Task<Sale?> GetByIdWithItemsAsync(int id)
        => await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime from, DateTime to)
        => await _context.Sales
            .Where(s => s.Date >= from && s.Date <= to)
            .Include(s => s.Customer)
            .Include(s => s.SaleItems)
            .OrderByDescending(s => s.Date)
            .ToListAsync();

    public async Task CreateSaleAsync(Sale sale, IEnumerable<SaleItem> items)
    {
        sale.SaleItems = items.ToList();
        await AddAsync(sale);
    }
}