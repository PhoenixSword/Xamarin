using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnitTestProject1;
using Server.Models;

namespace Tests
{
    public class Tests : TestBase
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void Test1()
        {
            var context = GetDbContext();
            context.Dishes.Add(new Dish
            {
                Description = "asdasd",
                Id = Guid.NewGuid().ToString(),
                Name = "Name1",
                Sum = 123
            });

            context.SaveChanges();
            var data = context.Dishes.ToList();
            Assert.AreEqual(data.Count, 1);
        }

        [Test]
        public void Test2()
        {
            var context = GetDbContext();
            context.Ingredients.Add(new Ingredient
            {
                DishId = "1",
                Id = Guid.NewGuid().ToString(),
                Name = "Name1"
            });

            Assert.Throws<DbUpdateException>(() =>
            {
                context.SaveChanges();
            });
        }
    }
}