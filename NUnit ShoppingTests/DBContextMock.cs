using Microsoft.EntityFrameworkCore;
using Moq;
using ShoppingSystemWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingTests
{
    public class DBContextMock
    {
        public static DbSet<T> GetQueryableMockDBSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            return dbSet.Object;
        }

        public static List<Product> GetSeedData()
        {
            List<Product> productsMock = new List<Product>()
            {
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
            };

            return productsMock;
        }
    }
}
