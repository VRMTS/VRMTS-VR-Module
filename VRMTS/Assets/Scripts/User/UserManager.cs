// ================================
// File: Scripts/User/UserManager.cs
// Purpose: Singleton manager for user data
// Notes: Handles loading, saving, and updating metadata/performance/feedback
// ================================
using UnityEngine;
using System;
using TMPro;


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


    // ---------------------------
    // NEW: Tags Helpers
    public void AddTag(string tag)
    {
        UserHelpers.AddTag(CurrentUser, tag);
    }

    public void RemoveTag(string tag)
    {
        UserHelpers.RemoveTag(CurrentUser, tag);
    }

    public void ClearTags()
    {
        UserHelpers.ClearTags(CurrentUser);
    }

    // function to fill settings data - used for dev-testing purposes
    public void FillSettingsData( TMP_Text userDataText )
    {
        // Placeholder for future settings data population
         string TextFromUserManagerInstance = $"User ID: {CurrentUser.UserId}\n" +
                         $"User Name: {CurrentUser.UserName}\n" +
                         $"UserMetaData: {CurrentUser.UserMetaData}\n" +
                         $"UserPerformanceData: {CurrentUser.UserPerformanceData}\n" +
                         $"FeedbackData: {CurrentUser.FeedbackData}\n" +
                         $"Tags: {string.Join(", ", CurrentUser.Tags)}\n" +
                         $"Time: {System.DateTime.Now}\n" +
                         "---------------------------\n";
        
        userDataText.text = TextFromUserManagerInstance;
    }

}
