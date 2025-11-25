// AppConfig.cs
public static class AppConfig
{
    public static string AppVersion = "1.0";
    public static bool VRModeEnabled = true;
    public static string DefaultLanguage = "en";
    public static string DataPath => UnityEngine.Application.persistentDataPath + "/";

    // Add more global settings here if needed
}

// ================================
// File: Scripts/Core/AppConfig.cs
// Purpose: Global app settings
// ================================