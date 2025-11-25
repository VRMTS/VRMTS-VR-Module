using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    public static string UpdateJsonKey(string json, string key, string value)
    {
        Dictionary<string, string> dict;

        if (string.IsNullOrEmpty(json))
            dict = new Dictionary<string, string>();
        else
            dict = JsonUtility.FromJson<SerializationWrapper>(json).ToDictionary();

        dict[key] = value;

        return JsonUtility.ToJson(new SerializationWrapper(dict), true);
    }
}

[System.Serializable]
public class SerializationWrapper
{
    public KeyValue[] items;

    public SerializationWrapper(Dictionary<string, string> dict)
    {
        items = new KeyValue[dict.Count];
        int i = 0;
        foreach (var kv in dict)
        {
            items[i] = new KeyValue { Key = kv.Key, Value = kv.Value };
            i++;
        }
    }

    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();
        foreach (var kv in items)
            dict[kv.Key] = kv.Value;
        return dict;
    }
}

[System.Serializable]
public class KeyValue
{
    public string Key;
    public string Value;
}
