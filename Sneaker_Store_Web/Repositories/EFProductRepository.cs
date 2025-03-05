using Microsoft.EntityFrameworkCore;
using Sneaker_Store_Web.Models;

namespace Sneaker_Store_Web.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // return await _context.Products.ToListAsync(); 
            return await _context.Products
        .Include(p => p.Brand) // Include thông tin về brand 
        .ToListAsync();

        }

        public async Task<Product> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id); 
            // lấy thông tin kèm theo brand 
            return await _context.Products.Include(p =>p.Brand)
                                            .FirstOrDefaultAsync(p => p.ProductId == id);

        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }  
        }
    }
}
