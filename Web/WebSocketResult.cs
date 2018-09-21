using System;

namespace Web
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.WebSockets;

    public class WebSocketResult : ActionResult
    {
        private Func<AspNetWebSocketContext, Task> SocketHandler { get; }
        private AspNetWebSocketOptions Options { get; }

        public WebSocketResult(Func<AspNetWebSocketContext, Task> socketHandler, AspNetWebSocketOptions options = null)
        {
            SocketHandler = socketHandler;
            Options = options;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.AcceptWebSocketRequest(SocketHandler, Options);
            new HttpStatusCodeResult(HttpStatusCode.SwitchingProtocols).ExecuteResult(context);
        }
    }
}