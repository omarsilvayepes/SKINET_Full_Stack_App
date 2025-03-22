using Core.Entities;
using System.Text.Json;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.Products.Any()) {
                string path = "../Infrastructure/Data/SeedData/products.json";
                var productsData = await File.ReadAllTextAsync(path);

                var products=JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products == null) return;

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
