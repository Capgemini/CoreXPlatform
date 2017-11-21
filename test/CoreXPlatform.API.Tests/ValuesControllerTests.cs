namespace CoreXPlatform.API.Tests
{
    using System;
    using System.Linq;
    using CoreXPlatform.API.Controllers;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class ValuesControllerTests
    {
        [Fact]
        public void ValuesController_Get_Ok()
        {
            var controller = new ValuesController(new Mock<ILogger<ValuesController>>().Object);

            var result = controller.Get();

            Assert.NotNull(result);

            Assert.Equal(2, result.Count());
        }
    }
}
