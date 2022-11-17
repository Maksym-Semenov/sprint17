using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShoppingSystemWeb.Controllers;
using ShoppingSystemWeb.Data;
using ShoppingSystemWeb.Models;
using ShoppingTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit_ShoppingTests
{
    public class ProductsControllerTests
    {
        private DbContextOptions<ShoppingSystemWebContext> _dbContextOption = new DbContextOptionsBuilder<ShoppingSystemWebContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        private ShoppingSystemWebContext _shoppingSystemWebContextMock;
        private ProductsController _productsControllerMock;

        [SetUp]
        public void SetUp()
        {
            _shoppingSystemWebContextMock = new ShoppingSystemWebContext(_dbContextOption);
            _shoppingSystemWebContextMock.Database.EnsureCreated();

            _productsControllerMock = new ProductsController(_shoppingSystemWebContextMock);
            _shoppingSystemWebContextMock.Product.AddRange(DBContextMock.GetQueryableMockDBSet(DBContextMock.GetSeedData()));
            _shoppingSystemWebContextMock.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _shoppingSystemWebContextMock.Database.EnsureDeleted();
        }

        #region testing controller actions
        [Test]
        public void Index_Test_With_Null_Params()
        {
            var result = _productsControllerMock.Index(null, null);
            Assert.IsInstanceOf<ViewResult>(result.Result);
        }

        [Test]
        [TestCase(1, "Oat")]
        public void Index_Test_With_Params(int page, string filter)
        {
            var result = _productsControllerMock.Index(page, filter);
            Assert.IsInstanceOf<ViewResult>(result.Result);
        }
        #endregion
        
        #region testing CRUD
        [Test]
        [TestCase(5, "Milk", "2022-06-01", "Milk food", "25.20")]
        public void Test_Add_Product_To_Database(int id, string title, DateTime expiredDate, string category, decimal price)
        {
            var product = new Product()
            {
                Id = id,
                Title = title,
                ExpiredDate = expiredDate,
                Category = category,
                Price = price
            };

            _shoppingSystemWebContextMock.Add(product);
            _shoppingSystemWebContextMock.SaveChanges();
            Assert.That(product, Is.EqualTo(_shoppingSystemWebContextMock.Product.Where(product => product.Id == id).First()));
        }
        #endregion
    }
}
