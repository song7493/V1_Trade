using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace V1_Trade.Infrastructure.Config
{
    public static class AppConfig
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        private static Dictionary<string, object> _data;

        static AppConfig()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                _data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
            }
            else
            {
                _data = new Dictionary<string, object>();
                Save();
            }
        }

        public static T Get<T>(string key, T defaultValue)
        {
            if (TryTraverse(key, out var value))
            {
                try
                {
                    if (value is T typed)
                        return typed;
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                }
            }
            return defaultValue;
        }

        public static void Set(string key, object value)
        {
            var parts = key.Split(':');
            var current = _data;
            for (int i = 0; i < parts.Length - 1; i++)
            {
                var part = parts[i];
                if (!current.TryGetValue(part, out var next) || next is not Dictionary<string, object> dict)
                {
                    dict = new Dictionary<string, object>();
                    current[part] = dict;
                }
                current = dict;
            }
            current[parts[^1]] = value;
            Save();
        }

        private static bool TryTraverse(string key, out object value)
        {
            var parts = key.Split(':');
            object current = _data;
            foreach (var part in parts)
            {
                if (current is Dictionary<string, object> dict && dict.TryGetValue(part, out var next))
                {
                    current = next;
                }
                else
                {
                    value = null;
                    return false;
                }
            }
            value = current;
            return true;
        }

        private static void Save()
        {
            var json = JsonConvert.SerializeObject(_data, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
    }
}
