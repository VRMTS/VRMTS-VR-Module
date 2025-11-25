// ================================
// File: Scripts/User/UserData.cs
// Purpose: Stores user info, LLM/feedback ready
// ================================
using System;

[Serializable]
public class UserData
{
    public string UserId;
    public string UserName;

    public string UserMetaData;       // JSON string for dynamic metadata
    public string UserPerformanceData;// JSON string for performance stats
    public string FeedbackData;       // Feedback from AI/LLM

    public UserData()
    {
        UserId = Guid.NewGuid().ToString();
        UserName = "Player";
        UserMetaData = "{}";
        UserPerformanceData = "{}";
        FeedbackData = "";
    }
}
