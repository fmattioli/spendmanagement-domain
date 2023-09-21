using KafkaFlow;
using Newtonsoft.Json;
using System.Text;

namespace Crosscutting.Extensions
{
    public static class MessageMiddlewareExtensions
    {
        public static string GetPartitionKey(this IMessageContext context)
        {
            if (context.Message.Key is string keyString)
            {
                return keyString;
            }

            if (context.Message.Key is byte[] keyBytes)
            {
                try
                {
                    return Encoding.UTF8.GetString(keyBytes);
                }
                catch (DecoderFallbackException)
                {
                    return Convert.ToBase64String(keyBytes);
                }
            }

            return "Invalid message key";
        }

        public static string ToJsonString(this IMessageHeaders headers)
        {
            var stringifiedHeaders = headers
                  .GroupBy(g => g.Key)
                  .ToDictionary(
                      kv => kv.Key,
                      kv => Encoding.UTF8.GetString(kv.FirstOrDefault().Value));

            return JsonConvert.SerializeObject(stringifiedHeaders);
        }
    }
}
