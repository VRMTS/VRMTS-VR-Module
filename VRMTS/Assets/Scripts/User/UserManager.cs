// ================================
// File: Scripts/User/UserManager.cs
// Purpose: Singleton manager for user & session data
// ================================
using UnityEngine;
using System;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance;

    public UserData CurrentUser;
    public SessionData CurrentSession;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        LoadUser();
        StartSession();
    }

    // ---------------------------
    // User management
    // ---------------------------
    private void LoadUser()
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
    // Session management
    // ---------------------------
    private void StartSession()
    {
        CurrentSession = new SessionData();
    }

    public void EndSession()
    {
        CurrentSession.EndTime = DateTime.UtcNow.ToString("o");
        UpdateUserMeta("LastSessionSummary", CurrentSession.SessionSummary);
        SaveUser();
    }

    // ---------------------------
    // Metadata helpers
    // ---------------------------
    public void UpdateUserMeta(string key, string value)
    {
        CurrentUser.UserMetaData = JsonHelper.UpdateJsonKey(CurrentUser.UserMetaData, key, value);
    }

    public void UpdatePerformanceData(string key, string value)
    {
        CurrentUser.UserPerformanceData = JsonHelper.UpdateJsonKey(CurrentUser.UserPerformanceData, key, value);
    }

    public void AddFeedback(string feedback)
    {
        CurrentUser.FeedbackData = feedback;
    }
}
