namespace CoreXPlatform.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CoreXPlatform.API.Model;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Controller returning orders
    /// </summary>
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;

        /// <summary>
        ///     OrderController()
        /// </summary>
        /// <param name="logger"></param>
        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        ///     Gets all orders
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var result = new List<Order>
            {
                new Order{ Id = 1, Reference = Guid.NewGuid().ToString() }
            };

            return Ok(result);
        }
    }
}
