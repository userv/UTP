
using ECommerce.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerce.WebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class CategoriesController : ApiController
    {
        private readonly ECommerceDbContext db;

        public CategoriesController(ECommerceDbContext dbContext)
        {
            this.db = dbContext;
        }
        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            if (!db.Categories.Any())
            {
                return this.NotFound();
            }
            return await db.Categories
                .Include(c => c.Products)
                .Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Products = c.Products.Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId
                }).ToList(),
                CreatedOn = c.CreatedOn,
                ModifiedOn = c.ModifiedOn,
            }).ToListAsync();

        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
