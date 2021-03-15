using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using LitJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public static class JsonUtility
{
    /// <summary>
    /// 将对象序列化为Json并保存为文件
    /// </summary>
    /// <param name="value"></param>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    public static void WriteLitJson(object value, string filePath, string fileName)
    {
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        var stringBuilder = new StringBuilder();
        var jsonWriter = new LitJson.JsonWriter(stringBuilder) {PrettyPrint = true}; // 缩进格式输出。缩进间隔：IndentValue = 2；
        JsonMapper.ToJson(value, jsonWriter);
        var jsonData = Regex.Unescape(stringBuilder.ToString()); // 保存中文字符
        File.WriteAllText(Path.Combine(filePath, fileName), jsonData);
    }

    /// <summary>
    /// 读取Json文件并反序列化为对象
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadLitJson<T>(string filePath, string fileName) where T : class
    {
        string path = Path.Combine(filePath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogError("Json file not exist ：" + path);
            return null;
        }

        return JsonMapper.ToObject<T>(File.ReadAllText(path));
    }

    /// <summary>
    /// 反序列化Json字符串为对象
    /// </summary>
    /// <param name="json"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadLitJson<T>(string json) where T : class
    {
        return JsonMapper.ToObject<T>(json);
    }

    /// <summary>
    /// 将对象序列化为Json并保存为文件
    /// </summary>
    /// <param name="value"></param>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    public static void WriteNewtonsoftJson(object value, string filePath, string fileName)
    {
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        File.WriteAllText(Path.Combine(filePath, fileName), JsonConvert.SerializeObject(value, Formatting.Indented));
    }

    /// <summary>
    /// 读取Json文件并反序列化为对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    /// <param name="converters"></param>
    /// <returns></returns>
    public static T ReadNewtonsoftJson<T>(string filePath, string fileName, params JsonConverter[] converters) where T : class
    {
        string path = Path.Combine(filePath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogError("Json file not exist ：" + path);
            return null;
        }

        return JsonConvert.DeserializeObject<T>(File.ReadAllText(path), converters);
    }

    /// <summary>
    /// 反序列化Json字符串为对象
    /// </summary>
    /// <param name="json"></param>
    /// <param name="converters"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadNewtonsoftJson<T>(string json, params JsonConverter[] converters) where T : class
    {
        return JsonConvert.DeserializeObject<T>(json, converters);
    }

    /// <summary>
    /// 自定义转换器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NewtonsoftJsonConverter<T> : JsonConverter
    {
        protected abstract T Create(Type objectType, JObject jsonObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var target = Create(objectType, jsonObject);
            serializer.Populate(jsonObject.CreateReader(), target);
            return target;
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}