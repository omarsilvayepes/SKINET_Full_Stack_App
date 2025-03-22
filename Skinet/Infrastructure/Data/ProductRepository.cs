using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    //Primary constructor approach: injected directly in class
    public class ProductRepository(StoreContext storeContext) : IProductRepository
    {
        public void AddProduct(Product product)
        {
            storeContext.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            storeContext.Products.Remove(product);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
        {
            var query=storeContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand))
                query=query.Where(b => b.Brand == brand);

            if (!string.IsNullOrWhiteSpace(type))
                query=query.Where(t => t.Type == type);

            query = sort switch
            {
                "priceAsc"  => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                            _=> query.OrderBy(p => p.Name)
            };

            return await query.ToListAsync();
        }
            
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await storeContext.Products.FindAsync(id);
        }
        
        public bool ProductExist(int id)
        {
            return storeContext.Products.Any(x => x.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await storeContext.SaveChangesAsync()>0;
        }

        public void UpdateProduct(Product product)
        {
            storeContext.Entry(product).State = EntityState.Modified;
        }

        public async Task<IReadOnlyList<string>> GetBransAsync()
        {
          return await storeContext.Products
                .Select(x => x.Brand)
                .Distinct() 
                .ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
          return await storeContext.Products
              .Select(x => x.Type)
              .Distinct()
              .ToListAsync();
        }
    }
}
