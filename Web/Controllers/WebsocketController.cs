namespace Web.Controllers
{
    using System;
    using System.Net.WebSockets;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.WebSockets;
    
    public class WebsocketController : Controller
    {
        public ActionResult Index()
        {
            return new WebSocketResult(AcceptWebsocket);
        }

        private async Task AcceptWebsocket(AspNetWebSocketContext context)
        {
            try
            {
                var send = SendHelloWorld(context);
                var receive = Receive(context);

                await Task.WhenAll(send, receive);
            }
            catch (Exception ex)
            {
                var x = 10;
            }
        }

        private async Task Receive(AspNetWebSocketContext context)
        {
            while (context.WebSocket.State == WebSocketState.Open)
            {
                var str = await context.WebSocket.ReceiveString();
                if (context.WebSocket.State == WebSocketState.Open)
                {
                    await context.WebSocket.SendString("Received message: " + str);
                }
            }
        }

        private async Task SendHelloWorld(AspNetWebSocketContext context)
        {
            await Task.Delay(1000);
            await context.WebSocket.SendString("Hello");

            await Task.Delay(1000);
            await context.WebSocket.SendString("World");

            await Task.Delay(1000);
            await context.WebSocket.SendString("2018!");
        }
    }
}