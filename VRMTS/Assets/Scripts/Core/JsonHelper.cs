using UnityEngine;
using System;
using System.Collections.Generic;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(FixJson(json));
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array, bool prettyPrint = false)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    private static string FixJson(string json)
    {
        return "{\"Items\":" + json + "}";
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
