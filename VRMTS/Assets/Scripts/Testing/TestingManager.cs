using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

/// ==============================================
/// TESTING MANAGER  
/// Loads question bank → picks best questions
/// Runs test → scores → updates tags → saves data
/// ==============================================
public class TestingManager : MonoBehaviour
{
    public string selectedLab;            // Set from Main Menu using static class SelectedLab
    private List<TestItem> questionBank;  // Full list loaded from JSON
    private List<TestItem> currentTest;   // Final selected test set

    private int totalQuestions = 5;       // Default for Lab1 (others upscale automatically)

    // ---------------------------------------------------------
    // ENTRY POINT
    // ---------------------------------------------------------
    void Start()
    {
        selectedLab = SelectedLab.selectedLab;     // Get selected lab from menu
        LoadLab();                                 // Load json questions
        SelectBestQuestions();                     // Pick best fit for user
        StartTest();                               // Begin test UI/flow
    }


    // =========================================================
    // LOAD QUESTION DATA FROM JSON
    // =========================================================
    void LoadLab()
    {
        string path = Application.streamingAssetsPath + "/QuestionBanks/" + selectedLab + "_questions.json";

        if (!File.Exists(path))
        {
            Debug.LogError("X >> X >> Question bank not found: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        questionBank = JsonHelper.FromJson<TestItem>(json).ToList();

        // Labs other than 1 use more questions automatically (your rule)
        if (selectedLab != "lab1") 
            totalQuestions = 10;

        Debug.Log(">> Loaded " + questionBank.Count + " questions for " + selectedLab);
    }


    // =========================================================
    // CHOOSE BEST QUESTIONS FOR USER BASED ON TAGS
    // =========================================================
    void SelectBestQuestions()
    {
        List<string> userTags = UserManager.Instance.CurrentUser.Tags; // Raw tag list from user file

        // Sort questions → tags matching user appear first
        currentTest = questionBank
            .OrderByDescending(q => userTags.Contains(q.tag)) // Priority if tag already exists
            .ThenBy(q => Random.value)                       // Add randomness so variety stays
            .Take(totalQuestions)
            .ToList();

        Debug.Log(">> Selected " + currentTest.Count + " questions based on user profile");
    }


    // =========================================================
    // START TEST (UI HOOK HERE)
    // =========================================================
    void StartTest()
    {
        Debug.Log(">> Test Started for " + selectedLab);
        // TODO: -> Display Question #1 in UI or VR Panel
    }


    // =========================================================
    // SUBMIT ANSWER & UPDATE USER PROFILE
    // =========================================================
    public void SubmitAnswer(int questionIndex, int chosenIndex)
    {
        var q = currentTest[questionIndex];

        if (chosenIndex == q.correctIndex)
        {
            Debug.Log("✔ Correct → `" + q.tag + "` stays or gets reinforced");
            UserManager.Instance.AddTag(q.tag); // You already have this function
        }
        else
        {
            Debug.Log("X Wrong → Added tag to focus `" + q.tag + "` next time");
            UserManager.Instance.AddTag(q.tag);
        }

        UserManager.Instance.SaveUser(); // Save updated user_data.json
    }


    // =========================================================
    // SAVE TEST SCORE (OPTIONAL)
    // =========================================================
    void SavePerformance()
    {
        string savePath = Application.persistentDataPath + "/userPerformance.json";
        File.WriteAllText(savePath, JsonUtility.ToJson(UserManager.Instance.CurrentUser, true));

        Debug.Log(">> Saved performance data to: " + savePath);
    }
}
