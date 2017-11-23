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
        [Theory]
        [MemberData(nameof(ConstructorFailureData))]
        public void Constructor_Fails_With_Null_Arguments(ILogger<ValuesController> logger)
        {
            Assert.Throws<ArgumentNullException>(() => new ValuesController(logger));
        }

        [Fact]
        public void ValuesController_Get_Ok()
        {
            var controller = new ValuesController(new Mock<ILogger<ValuesController>>().Object);

            var result = controller.Get();

            Assert.NotNull(result);

            Assert.Equal(1, result.Count());
        }

        public static TheoryData<ILogger<ValuesController>> ConstructorFailureData => 
            new TheoryData<ILogger<ValuesController>> 
        {
            { null }
        };
    }
}
