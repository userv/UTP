using ECommerce.WebApi;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace ECommerce.Demo.API
{
    public class Services
    {
        public static void AddServices(IServiceCollection services, WebApplicationBuilder appBuilder)
        {
            services.AddDbContext<ECommerceDbContext>(options =>
            {
                options.UseSqlServer(appBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }
}
