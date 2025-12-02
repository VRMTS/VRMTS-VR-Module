using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UI_TestController : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text questionText;
    public TMP_Text progressText;
    public TMP_Text performanceText;

    public Button[] optionButtons;   // 4 buttons
    public Button nextButton;
    public Button ExitButton;

    private int currentIndex = 0;
    private bool answered = false;

    // EVENTS → TestingManager listens
    public Action<int, int> onAnswerSelected;  // (questionIndex, choice)
    public Action onNextQuestion;              // next question pressed

    public void InitUI()
    {
        ExitButton.gameObject.SetActive(false);
        performanceText.gameObject.SetActive(false);
        nextButton.interactable = false;
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() => {
            if (answered) onNextQuestion?.Invoke();
        });
    }

    // --------------------------------------------------------------
    // DISPLAY A QUESTION – TestingManager CALLS this
    // --------------------------------------------------------------
    public void DisplayQuestion(TestItem q, int index, int totalCount)
    {
        currentIndex = index;

        questionText.text = q.question;
        progressText.text = $"Question {index + 1} / {totalCount}";

        for (int b = 0; b < optionButtons.Length; b++)
        {
            int choice = b;

            optionButtons[b].interactable = true;
            optionButtons[b].image.color = Color.white;

            optionButtons[b].GetComponentInChildren<TMP_Text>().text = q.options[b];
            optionButtons[b].onClick.RemoveAllListeners();
            optionButtons[b].onClick.AddListener(() => Answer(choice));
        }

        answered = false;
        nextButton.interactable = false;
    }

    // --------------------------------------------------------------
    // USER SELECTS AN ANSWER → notify TestingManager
    // --------------------------------------------------------------
    private void Answer(int chosenIndex)
    {
        answered = true;
        nextButton.interactable = true;

        // Send chosen answer to manager
        onAnswerSelected?.Invoke(currentIndex, chosenIndex);
    }

    // --------------------------------------------------------------
    // TestingManager calls this after evaluating answer
    // --------------------------------------------------------------
    public void MarkOptions(int correctIndex)
    {
        Color correct = new Color(0.2f, 0.8f, 0.2f);
        Color wrong = new Color(0.9f, 0.2f, 0.2f);

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].image.color = (i == correctIndex) ? correct : wrong;
            optionButtons[i].interactable = false;
        }
    }

    // --------------------------------------------------------------
    // END SCREEN
    // --------------------------------------------------------------
    public void ShowEndPanel(string performanceSummary)
    {
        questionText.text = "Test Completed!";
        progressText.text = "all questions answered...";
        performanceText.text = performanceSummary;
        performanceText.gameObject.SetActive(true);


        ExitButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);

        foreach (var b in optionButtons)
            b.gameObject.SetActive(false);
    }
}
