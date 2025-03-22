using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Skinet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //Primary constructor approach:injected services directly into class
    public class ProductsController(IGenericRepository<Product> repository) : ControllerBase
    {

        [HttpGet]   
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
            string? brand, 
            string? type,
            string? sort)
        {
            var specification=new ProductSpecification(brand, type,sort);
            var products=await repository.ListAsync(specification);

            // await /async help to free the thread(do other task if requiere) for doing other tasks meanwile the list product are get
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repository.GetByIdAsync(id);
            if (product == null) { return NotFound(); }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
           repository.Add(product);

            if(await repository.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetProduct), new {id=product.Id}, product);
            }
            return BadRequest("Problem creating the product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if(product.Id!=id || !IsValidProduct(id)){
                return BadRequest("Cannot update this product");
            } 

            repository.Update(product);
            if (await repository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Problem updating the product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repository.GetByIdAsync(id);
            if (product == null) { return NotFound(); }

            repository.Remove(product);
            if (await repository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Problem deleting the product");
        }

        private bool IsValidProduct(int id)
        {
            return repository.Exists(id);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var specification=new TypeListSpecification();
            return Ok(await repository.ListAsync(specification));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var specification = new BrandListSpecification();
            return Ok(await repository.ListAsync(specification));
        }
    }
}
