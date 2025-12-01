using UnityEngine;
using System.Collections.Generic;

// ================================
// File: Scripts/User/UserHelpers.cs
// Purpose: Provides helper methods to manipulate UserData JSON strings
// Notes: Works only with UserData, no sessions
// ================================

// ================================
// File: UserHelpers.cs
// Purpose: Simple helper to update text fields in UserData
// ================================
public static class UserHelpers
{
    // Update free-text metadata
    public static void UpdateUserMeta(UserData user, string text)
    {
        user.UserMetaData = text;
    }

    // Update free-text performance info
    public static void UpdatePerformanceData(UserData user, string text)
    {
        user.UserPerformanceData = text;
    }

    // Update free-text feedback
    public static void AddFeedback(UserData user, string text)
    {
        user.FeedbackData = text;
    }

    // ------------------------
    // NEW: Tags Helpers
    // ------------------------
    public static void AddTag(UserData user, string tag)
    {
        if (!user.Tags.Contains(tag))
            user.Tags.Add(tag);
    }

    public static void RemoveTag(UserData user, string tag)
    {
        if (user.Tags.Contains(tag))
            user.Tags.Remove(tag);
    }

    public static void ClearTags(UserData user)
    {
        user.Tags.Clear();
    }
}
