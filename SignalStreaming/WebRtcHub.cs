using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalStreaming
{
    using Microsoft.AspNetCore.SignalR;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class WebRtcHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task StartConection(object data)
        {
            await Clients.All.SendCoreAsync("MessagerReceived", new[] { new { Offset = 0 } });
        }



        public async Task SendStream(StreamVideo streamVideo)
        {
            var block = streamVideo.Buffer.Split(";");

            var contentType = block[0].Split(":")[1];

            var realData = block[1].Split(",")[1];

            byte[] bytes = Convert.FromBase64String(realData);

            if (!File.Exists(@"D:\1.webm"))
            {
                using (var stream = new FileStream(@"D:\1.webm", FileMode.Create))
                {
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                }
            }
            else
            {
                using (var stream = new FileStream(@"D:\1.webm", FileMode.Append))
                {
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                }
            }

            await Clients.All.SendCoreAsync("MessagerReceived", new[] { new { streamVideo.Offset } });

        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }

    public class StreamVideo
    {
        [JsonProperty("buffer")]
        public string Buffer { get; set; }
        [JsonProperty("size")]
        public long Size { get; set; }
        [JsonProperty("offset")]
        public int Offset { get; set; }
    }
}
