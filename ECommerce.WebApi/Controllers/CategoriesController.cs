
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
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
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
        public async Task<ActionResult> GetById(int id)
        {
            var category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return this.NotFound();
            }
            return this.Ok(category);

        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CategoryInputModel categoryInput)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var category = new Category
            {
                Name = categoryInput.Name,
                Description = categoryInput.Description
            };

            await db.Categories.AddAsync(category);
            await db.SaveChangesAsync();

            return this.CreatedAtAction(nameof(this.GetById), new { id = category.Id }, category);
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] CategoryInputModel categoryInput)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return this.NotFound();
            }

            category.Name = categoryInput.Name;
            category.Description = categoryInput.Description;
            category.ModifiedOn = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return this.Ok(category);

        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return this.NotFound();
            }

            db.Categories.Remove(category);
            await db.SaveChangesAsync();

            return this.Ok();
        }
    }
}
