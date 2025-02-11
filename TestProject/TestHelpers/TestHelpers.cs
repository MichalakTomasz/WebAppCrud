﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.TestHelpers
{
    public static class TestHelpers
    {
        public static StringContent ToContent<T>(this T data)
            => new StringContent(System.Text.Json.JsonSerializer.Serialize(data), Encoding.UTF8, new MediaTypeHeaderValue(MediaTypeNames.Application.Json));

        public static async Task<T> ConvertTo<T>(this HttpResponseMessage? response)
            => response is not null ? JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync()) : default;

        public static StringContent ToGraphQlContent<T>(this T data)
            => new StringContent(JsonConvert.SerializeObject(new { query = data }), Encoding.UTF8, new MediaTypeHeaderValue(MediaTypeNames.Application.Json));

        public static async Task<T> ConvertGraphQlResponseTo<T>(this HttpResponseMessage? response)
        {
            var dynamic = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
            var result = (dynamic?.data as JObject)?.Properties().FirstOrDefault()?.Value;
            if (result == null)
                return default;

            return JsonConvert.DeserializeObject<T>(result.ToString());
        }
    }
}
