using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotnetFlix.ProductService.Tests
{
    public class JsonObjectHelper
    {
        public static async Task<T> Deserialize<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }
}
