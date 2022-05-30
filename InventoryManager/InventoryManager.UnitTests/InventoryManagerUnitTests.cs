using AutoMapper;
using InventoryManager.BusinessLayer;
using InventoryManager.DatabaseLayer;
using InventoryManager.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace InventoryManager.UnitTests
{
    [TestClass]
    public class InventoryManagerUnitTests
    {
        private static MapperConfiguration _mapperConfig;
        private static IMapper _mapper;
        private static IServiceProvider _serviceProvider;

        private IItemsService _itemsService;
        private Mock<IItemsRepo> _itemsRepo;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void InitializeTests()
        {
            _itemsRepo = new Mock<IItemsRepo>();

            var items = new List<Item>
            {
                new Item
                {
                    Id = 1,
                    Name = "Star Wars 4",
                    Description = "Luke's Friends",
                    CategoryId = 2
                },
                new Item
                {
                    Id = 2,
                    Name = "Star Wars 5",
                    Description = "Luke's Dad",
                    CategoryId = 2
                },
                new Item
                {
                    Id = 3,
                    Name = "Star wars 6",
                    Description = "Luke's Sister",
                    CategoryId = 2
                }
            };

            _itemsRepo.Setup(x => x.GetItems()).Returns(items);
            _itemsService = new ItemsService(_itemsRepo.Object, _mapper); ;
        }

        [ClassInitialize]
        public static void InitializeTestEnvinroment(TestContext testContext)
        {
            var services = new ServiceCollection();
            services.AddAutoMapper(typeof(InventoryMapper));
            _serviceProvider = services.BuildServiceProvider();

            _mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<InventoryMapper>();
            });
            _mapperConfig.AssertConfigurationIsValid();
            _mapper = _mapperConfig.CreateMapper();
        }

        [TestMethod]
        public void TestGetItems()
        {
            var res = _itemsService.GetItems();

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Count > 0);
        }
    }
}