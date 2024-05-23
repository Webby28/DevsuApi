using StackExchange.Redis;
using Newtonsoft.Json;
using System;

namespace WebApi.Infrastructure.Database.Configuration
{
    public class RedisPersona
    {
        public RedisPersona() { }

        // Método para setear un valor en Redis
        public void SetValue<T>(string key, T value)
        {
            var redisDB = RedisContext.Connection.GetDatabase();
            string serializedValue = JsonConvert.SerializeObject(value);
            redisDB.StringSet(key, serializedValue);
        }

        // Método para obtener un valor de Redis
        public T GetValue<T>(string key)
        {
            var redisDB = RedisContext.Connection.GetDatabase();
            string serializedValue = redisDB.StringGet(key);

            if (serializedValue == null)
            {
                return default(T); // Devuelve el valor predeterminado para el tipo T
            }

            return JsonConvert.DeserializeObject<T>(serializedValue);
        }
    }
}
