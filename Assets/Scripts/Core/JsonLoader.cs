using UnityEngine;
using System.IO;

public static class JsonLoader
{
    // Load JSON into a generic object of type T
    public static T Load<T>(string fileName) where T : class, new()
    {
        string path = Path.Combine(AppConfig.DataPath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"JsonLoader: File not found at {path}, creating new object");
            return new T(); // return a fresh object if missing
        }

        string json = File.ReadAllText(path);

        // Deserialize
        T obj = JsonUtility.FromJson<T>(json);

        // Optional: sanitize UserData plain-text fields if loading UserData
        if (obj is UserData user)
        {
            // Remove any leftover JSON-wrapper style content
            user.UserMetaData = SanitizeText(user.UserMetaData);
            user.UserPerformanceData = SanitizeText(user.UserPerformanceData);
            user.FeedbackData = SanitizeText(user.FeedbackData);
        }

        return obj;
    }

    // Save an object to JSON
    public static void Save<T>(T obj, string fileName)
    {
        // Ensure folder exists
        if (!Directory.Exists(AppConfig.DataPath))
            Directory.CreateDirectory(AppConfig.DataPath);

        string path = Path.Combine(AppConfig.DataPath, fileName);

        string json = JsonUtility.ToJson(obj, true); // pretty print
        File.WriteAllText(path, json);

        Debug.Log($"JsonLoader: Saved {fileName} at {path}");
    }

    // ---------------------------
    // Helper to remove old wrapper artifacts
    // ---------------------------
    private static string SanitizeText(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        
        // Remove old {"items":[...]} wrapper if it exists
        if (text.TrimStart().StartsWith("{\"items\":"))
        {
            // Try to extract the inner text (quick hack)
            int start = text.IndexOf("Value") + 8; // skip "Value":" 
            int end = text.LastIndexOf("\"");      // last quote
            if (start >= 0 && end > start && end < text.Length)
                return text.Substring(start, end - start);
            else
                return ""; // fallback
        }

        return text; // assume plain text
    }
}
