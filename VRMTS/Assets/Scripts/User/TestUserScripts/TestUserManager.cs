using UnityEngine;
using System.IO;

// ============================
// Test script to inspect UserManager data
// Saves logs to .txt and .json in a custom folder
// ============================
public class TestUserManager : MonoBehaviour
{
    private string testDir;

    private void Start()
    {
        // Set up custom folder for testing
        testDir = Application.dataPath + "/Scripts/User/TestUserScripts/_TestOutputs/";
        if (!Directory.Exists(testDir))
            Directory.CreateDirectory(testDir);

        var userManager = UserManager.Instance;

        // ---------------------------
        // Add some test tags
        // ---------------------------
        //userManager.AddTag("Anatomy");
        //userManager.AddTag("Muscles");
        //userManager.AddTag("Bones");

        // ---------------------------
        // Update some metadata / performance / feedback
        // ---------------------------
        //userManager.UpdateUserMeta("This guy likes motorcycles");
        //userManager.UpdatePerformanceData("User lacks locking in");
        //userManager.AddFeedback("Please lock tf in! You suck at anatomy");

        userManager.SaveUser();

        // ---------------------------
        // Log current user info in console
        // ---------------------------
        Debug.Log("Current User ID: " + userManager.CurrentUser.UserId);
        Debug.Log("Current User Name: " + userManager.CurrentUser.UserName);
        Debug.Log("UserMetaData: " + userManager.CurrentUser.UserMetaData);
        Debug.Log("UserPerformanceData: " + userManager.CurrentUser.UserPerformanceData);
        Debug.Log("FeedbackData: " + userManager.CurrentUser.FeedbackData);
        Debug.Log("Tags: " + string.Join(", ", userManager.CurrentUser.Tags));

        // ---------------------------
        // Save to TXT log
        // ---------------------------
        string logText = $"User ID: {userManager.CurrentUser.UserId}\n" +
                         $"User Name: {userManager.CurrentUser.UserName}\n" +
                         $"UserMetaData: {userManager.CurrentUser.UserMetaData}\n" +
                         $"UserPerformanceData: {userManager.CurrentUser.UserPerformanceData}\n" +
                         $"FeedbackData: {userManager.CurrentUser.FeedbackData}\n" +
                         $"Tags: {string.Join(", ", userManager.CurrentUser.Tags)}\n" +
                         $"Time: {System.DateTime.Now}\n" +
                         "---------------------------\n";

        string logFile = Path.Combine(testDir, "UserManager_Log.txt");
        File.AppendAllText(logFile, logText);

        // ---------------------------
        // Save a snapshot JSON for this test run
        // ---------------------------
        string jsonFile = Path.Combine(testDir, $"UserData_{System.DateTime.Now:yyyyMMdd_HHmmss}.json");
        string jsonData = JsonUtility.ToJson(userManager.CurrentUser, true);
        File.WriteAllText(jsonFile, jsonData);

        Debug.Log($"User data JSON saved: {jsonFile}");
        Debug.Log($"User log TXT updated: {logFile}");
    }
}
