using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WzFrame.Entity.Configuration;

namespace WzFrame.Shared
{
    public static class AppSettings
    {
        private static Dictionary<string, IJsonConfiguration> Configs { get; set; } = new Dictionary<string, IJsonConfiguration>();

        public static T Get<T>() where T : class, IJsonConfiguration
        {
            if (Configs.ContainsKey(typeof(T).Name))
            {
                return Configs[typeof(T).Name] as T ?? throw new Exception($"Config {typeof(T).Name} not found");
            }
            throw new Exception($"Config {typeof(T).Name} not found");
        }



        public static void AddConfig<T>(JsonSerializerOptions jsonOptions) where T : class, IJsonConfiguration
        {
            string key = typeof(T).Name[0..^6];
            string path = AppDomain.CurrentDomain.BaseDirectory + $"Configuration//{key}.json";

            using var stream = new StreamReader(path);
            var json = stream.ReadToEnd();


            var options = JsonSerializer.Deserialize<T>(json, jsonOptions);
            if(options is not null)
            {
                Configs.Add(typeof(T).Name, options);
            }
        }

        public static void AddConfigs(this WebApplicationBuilder builder)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            };
            jsonOptions.Converters.Add(new JsonStringEnumConverter());
            AddConfig<DatabaseConfig>(jsonOptions);
            AddConfig<WebsiteConfig>(jsonOptions);

        }

        public static Task SaveConfigAsync<T>() where T : class, IJsonConfiguration
        {
            string key = typeof(T).Name[0..^6];
            string path = AppDomain.CurrentDomain.BaseDirectory + $"Configuration//{key}.json";
            var config = Get<T>();
            if (config is not null)
            {
                var json = JsonSerializer.Serialize(config);
                using var stream = new StreamWriter(path);
                return stream.WriteAsync(json);
            }
            return Task.CompletedTask;
        }




    }
}
