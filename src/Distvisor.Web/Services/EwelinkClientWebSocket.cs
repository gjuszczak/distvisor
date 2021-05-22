using Distvisor.Web.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public class EwelinkClientWebSocket : IDisposable
    {
        private readonly EwelinkConfiguration _config;
        private readonly ClientWebSocket _webSocket;
        private readonly CancellationTokenSource _cts;
        private readonly Task _webSocketListener;

        public EwelinkClientWebSocket(IOptions<EwelinkConfiguration> config)
        {
            _config = config.Value;
            _webSocket = new ClientWebSocket();
            _cts = new CancellationTokenSource();
            _webSocketListener = new Task(async () => await Listen(_webSocket, _cts.Token), TaskCreationOptions.LongRunning);
        }

        public async Task Open(string accessToken, string apiKey)
        {
            await _webSocket.ConnectAsync(new Uri(_config.WebSocketUrl), _cts.Token);
            _webSocketListener.Start();

            var seq = EwelinkHelper.GenerateSequence();
            var credentialsMsg = JsonSerializer.Serialize(new
            {
                action = "userOnline",
                at = accessToken,
                apikey = apiKey,
                appid = EwelinkHelper.Constants.APP_ID,
                nonce = EwelinkHelper.GenerateNonce(),
                ts = seq.timestamp,
                userAgent = EwelinkHelper.Constants.USER_AGENT,
                sequence = seq.sequence,
                version = EwelinkHelper.Constants.VERSION,
            });

            await SendTextAsync(credentialsMsg);
        }

        private async Task Listen(ClientWebSocket webSocket, CancellationToken token)
        {
            var chunkSize = 1024;
            var buffer = new byte[chunkSize * 50];
            var messageSegment = new ArraySegment<byte>(buffer, 0, 0);
            while (!token.IsCancellationRequested && webSocket.State == WebSocketState.Open)
            {
                var chunkSegment = new ArraySegment<byte>(buffer, messageSegment.Count, chunkSize);
                var result = await _webSocket.ReceiveAsync(chunkSegment, _cts.Token);

                if (result.EndOfMessage)
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(messageSegment.Array);
                        OnMessage(message);
                    }
                    
                    messageSegment = new ArraySegment<byte>(buffer, 0, 0);
                }
                else
                {
                    messageSegment = new ArraySegment<byte>(buffer, 0, messageSegment.Count + result.Count);
                }
            }
        }

        private void OnMessage(string data)
        {

        }

        private async Task SendTextAsync(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, _cts.Token);
        }

        public void Dispose()
        {
            _cts.Cancel();
            _webSocketListener.Wait();
            _webSocket.Abort();
            _webSocket.Dispose();
        }
    }
}
