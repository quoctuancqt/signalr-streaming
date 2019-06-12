using Microsoft.AspNetCore.SignalR;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SignalStreaming
{
    public class StreamHub : Hub
    {
        public async Task SendStream(StreamData stream)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(stream.Data);

            using (var fileStream = new FileStream("D:\\video.webm", FileMode.Append))
            {
                // Write the data to the file, byte by byte.
                for (int i = 0; i < byteArray.Length; i++)
                {
                    fileStream.WriteByte(byteArray[i]);
                }

                await fileStream.FlushAsync();
            }

            await Task.CompletedTask;
        }
    }

    public class StreamData
    {
        public string Data { get; set; }

        public string Type { get; set; }
    }
}
