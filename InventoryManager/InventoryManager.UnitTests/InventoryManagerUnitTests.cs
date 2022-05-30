using AutoMapper;
using InventoryManager.BusinessLayer;
using InventoryManager.DatabaseLayer;
using InventoryManager.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Shouldly;

namespace InventoryManager.UnitTests
{
    [TestClass]
    public class InventoryManagerUnitTests
    {
        private const string TITLE_NEWHOPE = "Star Wars 4";
        private const string TITLE_EMPIRE = "Star Wars 5";
        private const string TITLE_RETURN = "Star Wars 6";
        private const string DESC_NEWHOPE = "Luke's Friends";
        private const string DESC_EMPIRE = "Luke's Dad";
        private const string DESC_RETURN = "Luke's Sister";


        private static MapperConfiguration _mapperConfig;
        private static IMapper _mapper;
        private static IServiceProvider _serviceProvider;

        private IItemsService _itemsService;
        private Mock<IItemsRepo> _itemsRepo;

        public TestContext TestContext { get; set; }



        [TestInitialize]
        public void InitializeTests()
        {
            InitializeItemsRepoMock();
            _itemsService = new ItemsService(_itemsRepo.Object, _mapper); ;
        }

        private void InitializeItemsRepoMock()
        {
            _itemsRepo = new Mock<IItemsRepo>();
            var items = GetItemsTestData();

            _itemsRepo.Setup(x => x.GetItems()).Returns(items);
        }

        private List<Item> GetItemsTestData()
        {
            return new List<Item>
            {
                new Item
                {
                    Id = 1,
                    Name = TITLE_NEWHOPE,
                    Description = DESC_NEWHOPE,
                    CategoryId = 2
                },
                new Item
                {
                    Id = 2,
                    Name = TITLE_EMPIRE,
                    Description = DESC_EMPIRE,
                    CategoryId = 2
                },
                new Item
                {
                    Id = 3,
                    Name = TITLE_RETURN,
                    Description = DESC_RETURN,
                    CategoryId = 2
                }
            };
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

            res.ShouldNotBeNull();
            res.Count.ShouldBe(3);
            var expected = GetItemsTestData();

            var item1 = res[0];
            item1.Name.ShouldBe(TITLE_NEWHOPE);
            item1.Description.ShouldBe(DESC_NEWHOPE);

            var item2 = res[1];
            item2.Name.ShouldBe(expected[1].Name);
            item2.Description.ShouldBe(expected[1].Description);
        }
    }
}