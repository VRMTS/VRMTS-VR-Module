//"/Scripts/User/TestUserScripts/_TestOutputs/";
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
        // Print current user info
        // ---------------------------
        Debug.Log("Current User ID: " + userManager.CurrentUser.UserId);
        Debug.Log("Current User Name: " + userManager.CurrentUser.UserName);
        Debug.Log("UserMetaData: " + userManager.CurrentUser.UserMetaData);
        Debug.Log("UserPerformanceData: " + userManager.CurrentUser.UserPerformanceData);
        Debug.Log("FeedbackData: " + userManager.CurrentUser.FeedbackData);

        // ---------------------------
        // Save to TXT log
        // ---------------------------
        string logText = $"User ID: {userManager.CurrentUser.UserId}\n" +
                         $"User Name: {userManager.CurrentUser.UserName}\n" +
                         $"UserMetaData: {userManager.CurrentUser.UserMetaData}\n" +
                         $"UserPerformanceData: {userManager.CurrentUser.UserPerformanceData}\n" +
                         $"FeedbackData: {userManager.CurrentUser.FeedbackData}\n" +
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

        // ---------------------------
        // Optional: update some text fields for testing
        // ---------------------------
        userManager.UpdateUserMeta("This guy likes motorcycles");
        userManager.UpdatePerformanceData("User lacks locking in");
        userManager.AddFeedback("Please lock tf in! You suck at anatomy");

        userManager.SaveUser();
    }
}
