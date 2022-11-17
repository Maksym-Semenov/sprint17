using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ShoppingSystemWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using ShoppingSystemWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingSystemWeb.Controllers;
using System.Net.Sockets;
using ShoppingTests;

namespace NUnit_ShoppingTests
{
    public class TestTest
    {
        public static DbContextOptions<ShoppingSystemWebContext> dbContextOption = new DbContextOptionsBuilder<ShoppingSystemWebContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        private ShoppingSystemWebContext _context;
        private ProductsController _controller;

        //[SetUp]
        public void Setup()
        {
            _context = new ShoppingSystemWebContext(dbContextOption);
            _context.Database.EnsureCreated();
            _controller = new ProductsController(_context);

            var productsMock = DBContextMock.GetSeedData();
            _context.Product.AddRange(DBContextMock.GetQueryableMockDBSet(productsMock));
            _context.SaveChanges();
        }

        //[TearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }

        //[Test]
        public void ProductController_Edit_Get_IdIsNull_NotFound()
        {
            var result = _controller.Edit(null);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        //[Test]
        public void ProductController_Edit_Get_ProductIsNotExists_NotFound()
        {
            var result = _controller.Edit(100);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        //[Test]
        public void ProductController_Edit_Get_ProductIsExists_ViewResult()
        {
            var result = _controller.Edit(1);
            Assert.IsInstanceOf<ViewResult>(result.Result);
        }

        //[Test]
        //[TestCase(1, "Bread", "2022.11.15", "Food", 0)]
        public void ProductController_Edit_Post_IdsAreDifferent_NotFound(int id, string title, DateTime expiredDate, string category, decimal price)
        {
            var product = new Product()
            {
                Id = id,
                Title = title,
                ExpiredDate = expiredDate,
                Category = category,
                Price = price
            };

            var result = _controller.Edit(id + 1, product);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        //[Test]
        //[TestCase(1, "Bread", "2022.11.15", "Food", 0)]
        public void ProductController_Edit_Post_NotValidModel_ViewResult(int id, string title, DateTime expiredDate, string category, decimal price)
        {
            var product = new Product()
            {
                Id = id,
                Title = title,
                ExpiredDate = expiredDate,
                Category = category,
                Price = price
            };

            _controller.ModelState.AddModelError("Title", "Required");
            var result = _controller.Edit(id, product);

            Assert.IsInstanceOf<ViewResult>(result.Result);
        }

        //[Test]
        //[TestCase(1, "Bread", "2022.11.15", "Food", 0)]
        public void ProductController_Edit_Post_ProductUpdated_RedirectToAction(int id, string title, DateTime expiredDate, string category, decimal price)
        {
            var result = _controller.Edit(id);

            Assert.IsInstanceOf<ViewResult>(result.Result);

            var p = (Product)((ViewResult)result.Result).Model;

            p.Title = title;
            p.ExpiredDate = expiredDate;
            p.Category = category;
            p.Price = price;

            _controller.ModelState.Clear();
            var result2 = _controller.Edit(id, p);
            Assert.IsInstanceOf<RedirectToActionResult>(result2.Result);
        }

        //[Test]
        public void Edit_Get_Product_Exists()
        {
            var result = _controller.Edit(1);

            Assert.IsInstanceOf<ViewResult>(result.Result);

            var p = ((ViewResult)result.Result).Model;
            var product = p as Product;
            Assert.IsNotNull(product);
            Assert.That(product.Id, Is.EqualTo(1));
            Assert.That(product.Title, Is.EqualTo("Bread"));
            Assert.That(product.Category, Is.EqualTo("Food"));
            Assert.That(product.Price, Is.EqualTo(0));

            //var ppp = pp.Model as Product;

            //Assert.IsNotNull(ppp);


            //Assert.IsInstanceOf<ViewResult>(result.Result);
            //Product pr = Assert.IsInstanceOf<Product>(result.Result);
            //Product pr = (Product)result.Result;

            //Assert.That(pr.Id, Is.EqualTo(product.Id));
            //Assert.That(pr.Title, Is.EqualTo(product.Title));
            //Assert.That(pr.ExpiredDate, Is.EqualTo(product.ExpiredDate));
            //Assert.That(pr.Category, Is.EqualTo(product.Category));
            //Assert.That(pr.Price, Is.EqualTo(product.Price));
        }

        //[Test]
        public void Edit_Post_Product_Exists()
        {
            var newProduct = new Product()
            {
                //Id = 1,
                Title = "New Title",
                ExpiredDate = new DateTime(2022, 11, 16),
                Category = "Not to eat",
                Price = 999
            };
            var result = _controller.Edit(1, newProduct);
            Assert.IsInstanceOf<RedirectToActionResult>(result.Result);

            var result2 = _controller.Edit(1);

            Assert.IsInstanceOf<ViewResult>(result2.Result);

            var p = ((ViewResult)result2.Result).Model;
            var product = p as Product;
            Assert.IsNotNull(product);
            //Assert.That(product.Id, Is.EqualTo(newProduct.Id));
            Assert.That(product.Title, Is.EqualTo(newProduct.Title));
            Assert.That(product.ExpiredDate, Is.EqualTo(newProduct.ExpiredDate));
            Assert.That(product.Category, Is.EqualTo(newProduct.Category));
            Assert.That(product.Price, Is.EqualTo(newProduct.Price));
        }
    }
}
