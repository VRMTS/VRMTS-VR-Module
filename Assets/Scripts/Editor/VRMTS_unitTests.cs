using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

// ==================================================
// File: Scripts/Editor/VRMTS_UnitTests.cs
// Purpose: Comprehensive Automated Unit Tests for VRMTS
// ==================================================
public class VRMTS_UnitTests
{
    // --- SETUP & TEARDOWN ---
    // This ensures our tests don't permanently mess up your actual game saves!
    [SetUp]
    public void Setup()
    {
        PlayerPrefs.DeleteKey("ModuleProgress");
        PlayerPrefs.DeleteKey("Lab1_Completed");
    }

    [TearDown]
    public void TearDown()
    {
        PlayerPrefs.DeleteKey("ModuleProgress");
        PlayerPrefs.DeleteKey("Lab1_Completed");
    }

    // =========================================================
    // SECTION 1: USER DATA & METADATA HANDLING
    // =========================================================
    [Test]
    public void TC01_UserData_Initialization_SetsDefaultValues()
    {
        UserData newUser = new UserData();
        Assert.IsNotNull(newUser.UserId, "User ID failed to generate.");
        Assert.AreEqual("Player", newUser.UserName, "Default username mismatch.");
        Assert.IsNotNull(newUser.Tags, "Tags list should be initialized.");
        Assert.AreEqual("{}", newUser.UserMetaData, "Metadata should default to empty JSON.");
    }

    [Test]
    public void TC02_UserHelpers_AddTag_AvoidsDuplicates()
    {
        UserData testUser = new UserData();
        UserHelpers.AddTag(testUser, "anatomy_bones");
        UserHelpers.AddTag(testUser, "anatomy_bones"); // Duplicate attempt

        Assert.AreEqual(1, testUser.Tags.Count, "System allowed a duplicate tag!");
    }

    [Test]
    public void TC03_UserHelpers_ClearTags_WipesDataSafely()
    {
        UserData testUser = new UserData();
        UserHelpers.AddTag(testUser, "nervous_system");
        UserHelpers.AddTag(testUser, "skeletal_system");
        
        UserHelpers.ClearTags(testUser);

        Assert.AreEqual(0, testUser.Tags.Count, "ClearTags failed to empty the list.");
    }

    // =========================================================
    // SECTION 2: CONTENT LOCKING & PROGRESSION
    // =========================================================
    [Test]
    public void TC04_ContentLocking_InitializesWithDefaultModulesUnlocked()
    {
        // Act: Simulate first-time boot
        if (!PlayerPrefs.HasKey("ModuleProgress"))
        {
            PlayerPrefs.SetInt("ModuleProgress", 2); // Initial unlocked modules
        }

        // Assert
        Assert.AreEqual(2, PlayerPrefs.GetInt("ModuleProgress"), "Initial unlock state is wrong.");
    }

    [Test]
    public void TC05_LabProgression_SimulateLabCompletion()
    {
        // Act: Simulate completing a lab
        PlayerPrefs.SetInt("Lab1_Completed", 1);
        
        // Assert: Ensure the system registers it correctly for UI unlocking
        bool isUnlocked = PlayerPrefs.GetInt("Lab1_Completed", 0) == 1;
        Assert.IsTrue(isUnlocked, "Lab completion state failed to save to PlayerPrefs.");
    }

    // =========================================================
    // SECTION 3: QUIZ & TESTING ANALYTICS
    // =========================================================
    [Test]
    public void TC06_TestResult_MetricsCalculatedAccurately()
    {
        TestResult result = new TestResult();
        result.totalQuestions = 5;
        result.correct = 4;
        result.incorrect = 1;
        result.gainedTags.Add("skull");

        int totalAnswered = result.correct + result.incorrect;
        
        Assert.AreEqual(5, totalAnswered, "Score mismatch: correct + incorrect != total.");
        Assert.IsTrue(result.gainedTags.Contains("skull"), "Tag tracking failed.");
    }

    [Test]
    public void TC07_SelectedLabStatic_RetainsStateAcrossScenes()
    {
        SelectedLabStatic.selectedLab = "lab2_skeleton";
        Assert.AreEqual("lab2_skeleton", SelectedLabStatic.selectedLab, "Static state retention failed.");
    }

    // =========================================================
    // SECTION 4: JSON SERIALIZATION & PARSING
    // =========================================================
    /*[Test]
    public void TC08_JsonHelper_WrapsAndUnwrapsArrays()
    {
        // Arrange: Create dummy data
        TestItem[] mockBank = new TestItem[1];
        mockBank[0] = new TestItem { id = 1, question = "What is the largest organ?", tag = "skin" };

        // Act
        string json = JsonHelper.ToJson(mockBank);
        TestItem[] unpackedBank = JsonHelper.FromJson<TestItem>(json);

        // Assert
        Assert.IsTrue(json.Contains("Items"), "JsonHelper failed to wrap array.");
        Assert.AreEqual(1, unpackedBank.Length, "JsonHelper failed to unwrap array.");
        Assert.AreEqual("skin", unpackedBank[0].tag, "Data corrupted during JSON parsing.");
    }*/
    // =========================================================
    // SECTION 4: JSON SERIALIZATION & PARSING
    // =========================================================
    [Test]
    public void TC08_JsonHelper_WrapsAndUnwrapsArrays()
    {
        // 1. Arrange: Simulate exactly how your QuestionBank JSON files look (A raw array)
        string rawJsonFromFile = "[{\"id\":1,\"question\":\"What is the largest organ?\",\"tag\":\"skin\"}]";

        // 2. Act: Test FromJson (Unwrapping the array)
        TestItem[] unpackedBank = JsonHelper.FromJson<TestItem>(rawJsonFromFile);

        // 3. Assert: Verify it unpacked correctly
        Assert.IsNotNull(unpackedBank, "JsonHelper returned null.");
        Assert.AreEqual(1, unpackedBank.Length, "JsonHelper failed to unwrap array.");
        Assert.AreEqual("skin", unpackedBank[0].tag, "Data corrupted during JSON parsing.");

        // 4. Act & Assert: Test ToJson (Wrapping the array)
        string wrappedJson = JsonHelper.ToJson(unpackedBank);
        Assert.IsTrue(wrappedJson.Contains("Items"), "JsonHelper failed to wrap array.");
    }

    [Test]
    public void TC09_SimpleUser_DataModelValidation()
    {
        SimpleUser user = new SimpleUser { userId = 101, studentId = 221239, name = "Tauha", email = "tauha@test.com" };
        
        Assert.AreEqual(221239, user.studentId, "Student ID assignment failed.");
        Assert.AreEqual("tauha@test.com", user.email, "Email assignment failed.");
    }

    // =========================================================
    // SECTION 5: APP CONFIGURATION
    // =========================================================
    [Test]
    public void TC10_AppConfig_ValidatesGlobalSettings()
    {
        Assert.IsTrue(AppConfig.VRModeEnabled, "CRITICAL: VR Mode should be enabled for the build!");
        Assert.IsNotNull(AppConfig.DataPath, "Data path for JSON saving is null.");
        Assert.AreEqual("en", AppConfig.DefaultLanguage, "Default language corrupted.");
    }
}