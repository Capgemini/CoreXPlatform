namespace CoreXPlatform.API.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CoreXPlatform.API.Controllers;
    using CoreXPlatform.API.Model;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class OrderControllerTests
    {
        [Theory]
        [MemberData(nameof(ConstructorFailureData))]
        public void Constructor_Fails_With_Null_Arguments(ILogger<OrderController> logger)
        {
            Assert.Throws<ArgumentNullException>(() => new OrderController(logger));
        }

        [Fact]
        public void ValuesController_Get_Ok()
        {
            var controller = new OrderController(new Mock<ILogger<OrderController>>().Object);

            var result = controller.GetAllOrders();

            Assert.IsType<OkObjectResult>(result);

            OkObjectResult ok = result as OkObjectResult;

            Assert.Single(ok.Value as IEnumerable<Order>);
        }

        public static TheoryData<ILogger<OrderController>> ConstructorFailureData =>
            new TheoryData<ILogger<OrderController>>
        {
            { null }
        };
    }
}
