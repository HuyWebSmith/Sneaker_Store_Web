﻿using Microsoft.EntityFrameworkCore;
using Sneaker_Store_Web.Models;

namespace Sneaker_Store_Web.Repositories
{
    public class EFBrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _context;
        public EFBrandRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            // return await _context.Products.ToListAsync(); 
            return await _context.Brands
            .Include(p => p.Products) // Include thông tin về category 
            .ToListAsync();

        }

        public async Task<Brand> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id); 
            // lấy thông tin kèm theo category 
            return await _context.Brands.Include(p =>
   p.Products).FirstOrDefaultAsync(p => p.BrandId == id);
        }

        public async Task AddAsync(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Brand brand)
        {
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
        }
    }
}
