using System;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

public static class JsonDemo
{
    private static BaseData[] GetData()
    {
        return new BaseData[]
        {
            new AData {_aDouble = 12.17},
            new AData {_aDouble = 2.3333},
            new BData {_bString = "你好"},
            new BData {_bString = "hello"},
            new CData {_cBool = false},
            new CData {_cBool = true},
        };
    }
    
    [MenuItem("JsonDemo/Serialize by LitJson", false, 0)]
    private static void SerializeByLitJson()
    {
        JsonUtility.WriteLitJson(GetData(), Application.streamingAssetsPath, "litJson.json");
        AssetDatabase.Refresh();
        Debug.Log("Serialize by LitJson success !");
    }
    
    [MenuItem("JsonDemo/Serialize by Newtonsoft.Json", false, 2)]
    private static void SerializeByNewtonsoftJson()
    {
        JsonUtility.WriteNewtonsoftJson(GetData(), Application.streamingAssetsPath, "newtonsoftJson.json");
        AssetDatabase.Refresh();
        Debug.Log("Serialize by Newtonsoft.Json success !");
    }

    [MenuItem("JsonDemo/DeSerialize by Newtonsoft.Json", false, 3)]
    private static void DeSerializeByNewtonsoftJson()
    {
        BaseData[] data = JsonUtility.ReadNewtonsoftJson<BaseData[]>(Application.streamingAssetsPath, "newtonsoftJson.json", new DataConverter());
        for (int i = 0; i < data.Length; i++)
        {
            Debug.Log(data[i].ToString());
        }
    }
    
    private class DataConverter : JsonUtility.NewtonsoftJsonConverter<BaseData>
    {
        protected override BaseData Create(Type objectType, JObject jsonObject)
        {
            DataType dataType = (DataType) Enum.Parse(typeof(DataType), jsonObject["Type"].ToString());
            switch (dataType)
            {
                case DataType.A:
                    return new AData();
                case DataType.B:
                    return new BData();
                case DataType.C:
                    return new CData();
            }

            return null;
        }
    }
}