using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class TestingManager : MonoBehaviour
{
    public string selectedLab = "lab1";  //for testing in editor

    private List<TestItem> questionBank;
    private List<TestItem> currentTest;

    public UI_TestController uiTest;
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
            UserManager.Instance.AddTag(q.tag);
        else
            UserManager.Instance.AddTag(q.tag);

        UserManager.Instance.SaveUser();

        // Tell UI which option was correct
        uiTest.MarkOptions(q.correctIndex);
    }

    // --------------------------------------------------------------
    void OnNextQuestion()
    {
        currentIndex++;

        if (currentIndex >= currentTest.Count)
        {
            uiTest.ShowEndPanel();
            return;
        }

        ShowQuestion(currentIndex);
    }
}
