// ================================
// File: Scripts/User/UserManager.cs
// Purpose: Singleton manager for user data
// Notes: Handles loading, saving, and updating metadata/performance/feedback
// ================================
using UnityEngine;
using System;


// ================================
// File: UserManager.cs
// Purpose: Singleton manager for user data
// ================================
public class UserManager : MonoBehaviour
{
    public static UserManager Instance;

    public UserData CurrentUser;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        LoadUser();
    }

    // ---------------------------
    // Load / Save user data
    // ---------------------------
    public void LoadUser()
    {
        CurrentUser = JsonLoader.Load<UserData>("user_data.json");

        if (CurrentUser == null)
        {
            CurrentUser = new UserData();
            SaveUser();
        }
    }

    public void SaveUser()
    {
        JsonLoader.Save(CurrentUser, "user_data.json");
    }

    // ---------------------------
    // Helpers for metadata, performance, feedback
    // ---------------------------
    public void UpdateUserMeta(string text)
    {
        UserHelpers.UpdateUserMeta(CurrentUser, text);
    }

    public void UpdatePerformanceData(string text)
    {
        UserHelpers.UpdatePerformanceData(CurrentUser, text);
    }

    public void AddFeedback(string text)
    {
        UserHelpers.AddFeedback(CurrentUser, text);
    }
}
