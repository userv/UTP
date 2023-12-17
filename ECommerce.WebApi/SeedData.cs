using ECommerce.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.WebApi
{
    public class SeedData
    {
        private readonly ECommerceDbContext db;
        public SeedData(ECommerceDbContext db)
        {
            this.db = db;
        }
        public static void Seed(ECommerceDbContext db)
        {


            db.Database.Migrate();
            if (db.Categories.Any() == false)
            {
                foreach (var category in GetCategories())
                {
                    db.Categories.Add(category);
                }
               // db.Categories.AddRange(GetCategories());
                db.SaveChanges();
            }



        }
        public static IEnumerable<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category
                {

                    Name = "Electronics",
                    Description = "Electronic products"
                },
                new Category
                {

                    Name = "Clothes",
                    Description = "Clothes products"
                },
                new Category
                {

                    Name = "Shoes",
                    Description = "Shoes products"
                },
                new Category
                {

                    Name = "Furniture",
                    Description = "Furniture products"
                },
                new Category
                {

                    Name = "Books",
                    Description = "Books products"
                },
                new Category
                {

                    Name = "Food",
                    Description = "Food products"
                },
                new Category
                {

                    Name = "Toys",
                    Description = "Toys products"
                },
                new Category
                {

                    Name = "Tools",
                    Description = "Tools products"
                },
                new Category
                {

                    Name = "Sports",
                    Description = "Sports products"
                },
                new Category
                {

                    Name = "Health",
                    Description = "Health products"
                },
                new Category
                {

                    Name = "Beauty",
                    Description = "Beauty products"
                },
                new Category
                {

                    Name = "Jewelry",
                    Description = "Jewelry products"
                },
                new Category
                {

                    Name = "Games",
                    Description = "Games products"
                },
                new Category
                {

                    Name = "Movies",
                    Description = "Movies products"
                },
                new Category
                {

                    Name = "Music",
                    Description = "Music products"
                },


            };


        }
    }
}
