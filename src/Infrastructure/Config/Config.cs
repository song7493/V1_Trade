using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace V1_Trade.Infrastructure.Config
{
    public class Config
    {
        private readonly string _filePath;
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        private Dictionary<string, object> _data;

        public Config()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath, Encoding.UTF8);
                _data = _serializer.Deserialize<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
            }
            else
            {
                _data = new Dictionary<string, object>();
            }
        }

        public T Get<T>(string key, T defaultValue)
        {
            if (TryTraverse(key, out var value))
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                }
            }
            return defaultValue;
        }

        public void Set(string key, object value)
        {
            var parts = key.Split(':');
            var current = _data;
            for (int i = 0; i < parts.Length - 1; i++)
            {
                var part = parts[i];
                if (!current.TryGetValue(part, out var next) || !(next is Dictionary<string, object> dict))
                {
                    dict = new Dictionary<string, object>();
                    current[part] = dict;
                }
                current = dict;
            }
            current[parts[^1]] = value;
            Save();
        }

        private bool TryTraverse(string key, out object value)
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

        private void Save()
        {
            var json = _serializer.Serialize(_data);
            File.WriteAllText(_filePath, json, Encoding.UTF8);
        }
    }
}
