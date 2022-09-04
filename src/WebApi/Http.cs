using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace transport
{
    /**
        ByteArrayContent
        EmptyContent
        FormUrlEncodedContent
        HttpContent
        MultipartContent
        MultipartFormDataContent
        ReadOnlyMemoryContent
        StreamContent
        StringContent
        DecompressedContent
        GZipDecompressedContent
        DeflateDecompressedContent
        BrotliDecompressedContent
        HttpConnectionResponseContent
     */
    public class Http
    {
        private readonly HttpClient _httpClient;

        public Http(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


    }
}
