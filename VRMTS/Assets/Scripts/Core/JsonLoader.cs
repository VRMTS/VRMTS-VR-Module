using UnityEngine;
using System.IO;


public static class JsonLoader
{
    // Load JSON into a generic object of type T
    public static T Load<T>(string fileName)
    {
        // Full path using AppConfig
        string path = Path.Combine(AppConfig.DataPath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"JsonLoader: File not found at {path}");
            return default; // return default value if missing
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }

    // Save an object to JSON
    public static void Save<T>(T obj, string fileName)
    {
        // Ensure folder exists
        if (!Directory.Exists(AppConfig.DataPath))
        {
            Directory.CreateDirectory(AppConfig.DataPath);
        }

        string path = Path.Combine(AppConfig.DataPath, fileName);
        string json = JsonUtility.ToJson(obj, true); // pretty print
        File.WriteAllText(path, json);

        Debug.Log($"JsonLoader: Saved {fileName} at {path}");
    }
}

// ================================
// File: Scripts/Core/JsonLoader.cs
// Purpose: Generic JSON load/save helper
// ================================
