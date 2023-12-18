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
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return this.NotFound();
            }
            return product;
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] ProductInputModel productInput)
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
        public void Edit(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
