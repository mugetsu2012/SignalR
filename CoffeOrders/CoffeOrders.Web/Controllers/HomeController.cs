using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoffeOrders.Web.ApplicationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoffeOrders.Web.Models;
using Microsoft.AspNetCore.Http;

namespace CoffeOrders.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderChecker _orderChecker;

        public HomeController(ILogger<HomeController> logger,
            IHttpContextAccessor httpContextAccessor, IOrderChecker orderChecker)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _orderChecker = orderChecker;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult OrderCoffe(OrderDto order)
        {
            return Accepted(1);
        }

        [HttpGet("{orderNo}")]
        public async void GetUpdateForOrder(int orderNo)
        {
            var context = _httpContextAccessor.HttpContext;

            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await SendEvents(webSocket, orderNo);
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        private async Task SendEvents(WebSocket webSocket, int orderNo)
        {
            CheckResultsDto checkResultsDto;
            do
            {
                checkResultsDto = _orderChecker.GetUpdate(orderNo);

                await Task.Delay(2000);

                if(!checkResultsDto.New) continue;

                string jsonMessage = $"\"{checkResultsDto.Update}\"";

                await webSocket.SendAsync(
                    buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(jsonMessage), offset: 0,
                        count: jsonMessage.Length), messageType: WebSocketMessageType.Text, endOfMessage: true,
                    cancellationToken: CancellationToken.None);



            } while (!checkResultsDto.Finished);
        }
    }
}
