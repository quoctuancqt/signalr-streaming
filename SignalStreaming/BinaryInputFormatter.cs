using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;

namespace SignalStreaming
{
    public class BinaryInputFormatter : InputFormatter
    {
        const string binaryContentType = "application/octet-stream";
        const int bufferLength = 16384;

        public BinaryInputFormatter()
        {
            var header = new MediaTypeHeaderValue(binaryContentType);

            SupportedMediaTypes.Add(header);
        }

        public async override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            using (MemoryStream ms = new MemoryStream(bufferLength))
            {
                await context.HttpContext.Request.Body.CopyToAsync(ms);
                object result = ms.ToArray();
                return await InputFormatterResult.SuccessAsync(result);
            }
        }

        protected override bool CanReadType(Type type)
        {
            if (type == typeof(byte[]))
                return true;
            else
                return false;
        }
    }
}
