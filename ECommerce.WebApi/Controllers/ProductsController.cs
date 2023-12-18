using ECommerce.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerce.WebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : ApiController
    {
        private readonly ECommerceDbContext db;


        public ProductsController(ECommerceDbContext dbContext)
        {
            this.db = dbContext;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            if (db.Products.Any() == false)
            {
                return this.NotFound();
            }

            return await db.Products.Select(x => new Product
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                ImageUrl = x.ImageUrl,
                CategoryId = x.CategoryId
            }).ToListAsync();
        }


        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return this.NotFound();
            }
            return this.Ok(product);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ProductInputModel productInput)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var product = new Product
            {
                Name = productInput.Name,
                Description = productInput.Description,
                Price = productInput.Price,
                ImageUrl = productInput.ImageUrl,
                CategoryId = productInput.CategoryId
            };
            await db.Products.AddAsync(product);
            await db.SaveChangesAsync();
            return this.CreatedAtAction(nameof(GetById), new { id = product.Id }, product);

        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] ProductInputModel productInput)
        {
            // Code logic for editing the product
            if (!ModelState.IsValid)
            {
                this.BadRequest();
            }
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return this.NotFound();
            }
            product.Name = productInput.Name;
            product.Description = productInput.Description;
            product.Price = productInput.Price;
            product.ImageUrl = productInput.ImageUrl;
            product.CategoryId = productInput.CategoryId;
            product.ModifiedOn = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return this.Ok(product);

        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            // Code logic for deleting the product
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return this.NotFound();
            }
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return this.Ok();

        }
    }
}
