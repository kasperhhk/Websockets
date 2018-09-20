namespace Web.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.WebSockets;

    public class WebsocketController : Controller
    {
        public ActionResult Index()
        {
            ControllerContext.HttpContext.AcceptWebSocketRequest(AcceptWebsocket);
            return new HttpStatusCodeResult(HttpStatusCode.SwitchingProtocols);
        }

        private async Task AcceptWebsocket(AspNetWebSocketContext context)
        {
            await Task.Delay(1000);
            await Send(context.WebSocket, "Hello");

            await Task.Delay(1000);
            await Send(context.WebSocket, "World");

            await Task.Delay(1000);
            await Send(context.WebSocket, "2018!");
        }

        private async Task Send(WebSocket websocket, string message)
        {
            var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(message)).ToArray();
            await websocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}