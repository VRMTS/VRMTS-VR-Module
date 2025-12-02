using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class TestingManager : MonoBehaviour
{
    public string selectedLab = "lab1";  //for testing in editor

    private List<TestItem> questionBank;
    private List<TestItem> currentTest;

    public UI_TestController uiTest; //compostion
    
    private int correctCount = 0;
    private int wrongCount = 0;
    private TestResult sessionResult; //composition
    private int totalQuestions = 5;
    private int currentIndex = 0;


    void Start()
    {
        LoadLab();
        SelectBestQuestions();
        BindUI();
        StartTest();
    }

    // --------------------------------------------------------------
    void BindUI()
    {
        uiTest.onAnswerSelected = OnAnswerSelected;
        uiTest.onNextQuestion = OnNextQuestion;
    }

    // --------------------------------------------------------------
    void LoadLab()
    {
        selectedLab = SelectedLabStatic.selectedLab; // from static class's var
        string path = Application.streamingAssetsPath + "/QuestionBanks/" + selectedLab + "_questions.json";

        if (!File.Exists(path))
        {
            Debug.LogError("Question bank not found: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        questionBank = JsonHelper.FromJson<TestItem>(json).ToList();

        if (selectedLab != "lab1")
            totalQuestions = 10;

        Debug.Log("Loaded " + questionBank.Count + " questions");
    }

    // --------------------------------------------------------------
    void SelectBestQuestions()
    {
        if (UserManager.Instance == null || UserManager.Instance.CurrentUser == null)
        {
            Debug.LogError("User data missing!");
            currentTest = questionBank.Take(totalQuestions).ToList();
            return;
        }

        List<string> tags = UserManager.Instance.CurrentUser.Tags ?? new List<string>();

        currentTest = questionBank
            .OrderByDescending(q => tags.Contains(q.tag))
            .ThenBy(q => Random.value)
            .Take(totalQuestions)
            .ToList();
    }

    // --------------------------------------------------------------
    void StartTest()
    {
        uiTest.InitUI();

        // Initialize session result
        sessionResult = new TestResult
        {
            userId = UserManager.Instance.CurrentUser.UserId,
            labName = selectedLab,
            date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            totalQuestions = totalQuestions
        };


        ShowQuestion(0);
    }

    // --------------------------------------------------------------
    void ShowQuestion(int index)
    {
        currentIndex = index;
        var q = currentTest[index];
        uiTest.DisplayQuestion(q, index, currentTest.Count);
    }

    // --------------------------------------------------------------
    // When the UI reports user's answer:
    // --------------------------------------------------------------
    void OnAnswerSelected(int questionIndex, int chosenIndex)
    {
        var q = currentTest[questionIndex];

        bool isCorrect = (chosenIndex == q.correctIndex);

        if (isCorrect)
        {
            correctCount++;
            sessionResult.gainedTags.Add(q.tag);
            UserManager.Instance.AddTag(q.tag);
        }
        else
        {
            wrongCount++;
            sessionResult.lostTags.Add(q.tag);
            UserManager.Instance.RemoveTag(q.tag);
        }

        UserManager.Instance.SaveUser();

        // Tell UI which option was correct
        uiTest.MarkOptions(q.correctIndex);
    }

    // --------------------------------------------------------------
    void OnNextQuestion()
    {
        currentIndex++;

        if (currentIndex >= currentTest.Count) // test is over
        {

            string performanceSummary = GetPerformanceSummary();
            FinishTest();
            uiTest.ShowEndPanel(performanceSummary);
            return;
        }

        ShowQuestion(currentIndex);
    }


    void FinishTest()
    {
        sessionResult.correct = correctCount;
        sessionResult.incorrect = wrongCount;

        // ------------------------------------------------------------
        // SAVE NORMAL TEST RESULT (JSON)
        // ------------------------------------------------------------
        string dir = Application.persistentDataPath + "/TestResults/";
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string jsonPath = dir + sessionResult.userId + "_" + sessionResult.labName + "_" +
                        System.DateTime.Now.Ticks + ".json";

        File.WriteAllText(jsonPath, JsonUtility.ToJson(sessionResult, true));
        Debug.Log("Saved test result to: " + jsonPath);

        // ------------------------------------------------------------
        // DEBUG / TESTING OUTPUT (Readable Summary)
        // ------------------------------------------------------------

        // Create a folder in project root for visible files
        string debugDir = Application.dataPath + "/..Scripts/Testing/Testing_output_logs/";
        if (!Directory.Exists(debugDir))
            Directory.CreateDirectory(debugDir);

        string summaryTextPath = debugDir + "last_test_summary.txt";

        string readableSummary =
            "=== TEST RESULT SUMMARY ===\n" +
            $"User ID: {sessionResult.userId}\n" +
            $"Lab: {sessionResult.labName}\n" +
            $"Correct: {sessionResult.correct}/{sessionResult.totalQuestions}\n" +
            $"Incorrect: {sessionResult.incorrect}/{sessionResult.totalQuestions}\n\n" +
            "Correct Topics:\n" +
            string.Join(", ", sessionResult.gainedTags.Distinct()) + "\n\n" +
            "Wrong / Weak Topics:\n" +
            string.Join(", ", sessionResult.lostTags.Distinct()) + "\n\n" +
            "Raw JSON Path:\n" + jsonPath + "\n";

        File.WriteAllText(summaryTextPath, readableSummary);

        Debug.Log("Debug summary written to: " + summaryTextPath);
    }


    //performance summary - generator
    public string GetPerformanceSummary()
    {
        // Unique topics
        var goodTopics = sessionResult.gainedTags.Distinct().ToList();
        var badTopics  = sessionResult.lostTags.Distinct().ToList();

        string summary = "";

        summary += "Test Performance Summary\n";
        summary += $"Correct: {sessionResult.correct}/{sessionResult.totalQuestions}\n";
        summary += $"Incorrect: {sessionResult.incorrect}/{sessionResult.totalQuestions}\n";

        summary += "\nTopics You Performed Well In:  ";
        summary += goodTopics.Count > 0 
            ? " • " + string.Join(" • ", goodTopics) 
            : " • None";

        summary += "\nTopics That Need Improvement:";
        summary += badTopics.Count > 0
            ? " • " + string.Join(" • ", badTopics) 
            : " • None";

        summary += "\n--------------------------------------------\n";
        summary += "* AI Feedback (Coming Soon):\n";
        summary += "(An AI-generated explanation + personalized guidance will appear here.)\n";
        summary += "--------------------------------------------\n";

        return summary;
}


}
