
using DataAccess.SqlServer;
using DataAccess.SqlServer.Repositories;
using Domain.Interfaces;
using Domain.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.UnitTests
{
    public class ProductRepositoryTests
    {
        [Fact]
        public async Task GetAsyncTetsPassed()
        {
            // arrange
            var repository = CreateRepositiory();
            Product product = CreateTestProduct();

            // act
            await repository.AddAsync(product);
            await repository.SaveChangesAsync();

            var getResult = await repository.GetAsync();
            
            // assert
            Assert.True((getResult.Count()) == 1);
        }

        [Fact]
        public async Task AddProduct()
        {
            // arrange
            var repository = CreateRepositiory();
            var product = CreateTestProduct();

            // act
            await repository.AddAsync(product);
            await repository.SaveChangesAsync();
            var getResult = await repository.GetAsync();

            var savedProduct = getResult.First();

            // assert
            product.Should().BeEquivalentTo(savedProduct,
                p => p.Including(r => r.Name)
                .Including(r => r.Code)
                .Including(r => r.UrlPicture)
                .Including(r => r.Price)
                .Including(r => r.Description));
        }

        [Fact]
        public async Task UpdateProductWithTheSameRepositoryInstanceTestPassed()
        {
            // arrange
            var repository = CreateRepositiory();
            Product product = CreateTestProduct();

            var addedProduct = await repository.AddAsync(product);
            await repository.SaveChangesAsync();

            addedProduct.Code = "Telefon";
            addedProduct.Name = "Telefon Xiaomi";
            addedProduct.Description = "Telefon Xiaomi w promocji";
            addedProduct.UrlPicture = "https://sklep.telefon.pl";
            addedProduct.Price = 555;

            // act
            await repository.UpdateAsync(addedProduct);
            await repository.SaveChangesAsync();
            var products = await repository.GetAsync();
            var updatedProduct = products.First();

            // assert
            addedProduct.Should().BeEquivalentTo(updatedProduct,
                p => p
                .Including(r => r.Id)
                .Including(r => r.Name)
                .Including(r => r.Code)
                .Including(r => r.UrlPicture)
                .Including(r => r.Price)
                .Including(r => r.Description));
        }

        [Fact]
        public async Task UpdateProductWithNewRepositoryInstanceTestPassed()
        {
            // arrange
            var repository = CreateRepositiory();
            Product product = CreateTestProduct();

            var addedProduct = await repository.AddAsync(product);
            await repository.SaveChangesAsync();

            repository = CreateRepositiory(clearDatabase: false);

            Product productToUpdate = new()
            {
                Id = 1,
                Code = "Telefon",
                Name = "Telefon Xiaomi",
                Description = "Telefon Xiaomi w promocji",
                UrlPicture = "https://sklep.telefon.pl",
                Price = 555
            };

            // act
            await repository.UpdateAsync(productToUpdate);
            await repository.SaveChangesAsync();
            var products = await repository.GetAsync();
            var updatedProduct = products.First();

            // assert
            updatedProduct.Should().BeEquivalentTo(productToUpdate,
                p => p
                .Including(r => r.Id)
                .Including(r => r.Name)
                .Including(r => r.Code)
                .Including(r => r.UrlPicture)
                .Including(r => r.Price)
                .Including(r => r.Description));
        }

        [Fact]
        public async Task DeleteProductTestPassed()
        {
            // arrange
            var repository = CreateRepositiory();
            Product product = CreateTestProduct();

            var addedProduct = await repository.AddAsync(product);
            await repository.SaveChangesAsync();
            var productsCountAfterAddNewProduct = (await repository.GetAsync()).Count;

            // act
            await repository.DeleteAsync(addedProduct.Id);
            await repository.SaveChangesAsync();
            var productsCountAfterDeleteProduct = (await repository.GetAsync()).Count;
    
            // assert
            Assert.Equal(1, productsCountAfterAddNewProduct);
            Assert.Equal(0, productsCountAfterDeleteProduct);
        }

        private GenericRepository<Product> CreateRepositiory(bool clearDatabase = true)
        {
            var options = new DbContextOptionsBuilder<SqlServerDbContext>().UseInMemoryDatabase("InMemoryDb").Options;

            var ctx = new SqlServerDbContext(options);
            if (clearDatabase)
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
            }
            
            return  new GenericRepository<Product>(ctx);
        }

        private Product CreateTestProduct()
            => new()
            {
                Code = "tv",
                Name = "Telewizor",
                Description = "Telewizor Samsung",
                UrlPicture = "http://sklep.rv.pl",
                Price = 1435
            };
    }
}
