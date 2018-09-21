using System;
using System.Collections.Generic;
using System.Linq;

namespace Web
{
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public static class WebSocketExtensions
    {
        public static async Task<byte[]> ReceiveMessage(this WebSocket webSocket, CancellationToken cancellationToken = default(CancellationToken))
        {
            WebSocketReceiveResult receive;
            var result = new List<byte>();
            var buffer = new byte[1024];
            do
            {
                receive = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                result.AddRange(new ArraySegment<byte>(buffer, 0, receive.Count));
            } while (!receive.EndOfMessage);

            return result.ToArray();
        }

        public static async Task<string> ReceiveString(this WebSocket webSocket, Encoding encoding = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var msg = await webSocket.ReceiveMessage(cancellationToken);
            var str = encoding.GetString(msg);
            return str;
        }

        public static async Task SendString(this WebSocket webSocket, string value, Encoding encoding = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var preamble = encoding.GetPreamble();
            var msg = encoding.GetBytes(value);
            var bytes = preamble.Concat(msg).ToArray();

            await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancellationToken);
        }
    }
}