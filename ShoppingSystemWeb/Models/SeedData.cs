using Microsoft.EntityFrameworkCore;
using ShoppingSystemWeb.Data;

namespace ShoppingSystemWeb.Models
{
	public class SeedData
	{
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ShoppingSystemWebContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ShoppingSystemWebContext>>()))
            {
                if (context.Product.Any())
                {
                    return;  
                }

                context.Product.AddRange(
                    new Product
                    {
                        Title = "Oat cakes",
                        ExpiredDate = DateTime.Parse("2022-07-01"),
                        Category = "Grosary",
                        Price = 48.60M
                    },

                    new Product
                    {
                        Title = "Milk",
                        ExpiredDate = DateTime.Parse("2022-06-01"),
                        Category = "Milk food",
                        Price = 25.20M
                    },

                    new Product
                    {
                        Title = "Butter",
                        ExpiredDate = DateTime.Parse("2022-04-01"),
                        Category = "Milk food",
                        Price = 30.80M
                    },

                    new Product
                    {
                        Title = "Cheese",
                        ExpiredDate = DateTime.Parse("2022-05-01"),
                        Category = "Milk food",
                        Price = 40.50M
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
