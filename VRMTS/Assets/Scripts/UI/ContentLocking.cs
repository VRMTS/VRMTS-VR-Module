using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentLocking : MonoBehaviour
{
    [SerializeField]
    private Button[] moduleButtons;  // Array to hold all module buttons

    private const string PROGRESS_KEY = "ModuleProgress";
    private const int INITIAL_UNLOCKED_MODULES = 2;

    void Start()
    {
        InitializeModules();
    }

    private void InitializeModules()
    {
        // If this is the first time running (no PlayerPrefs set)
        if (!PlayerPrefs.HasKey(PROGRESS_KEY))
        {
            PlayerPrefs.SetInt(PROGRESS_KEY, INITIAL_UNLOCKED_MODULES);
            PlayerPrefs.Save();
        }

        int unlockedModules = PlayerPrefs.GetInt(PROGRESS_KEY);

        // Configure each button based on whether it should be locked or unlocked
        for (int i = 0; i < moduleButtons.Length; i++)
        {
            if (moduleButtons[i] != null)
            {
                bool isUnlocked = i < unlockedModules;
                moduleButtons[i].interactable = isUnlocked;

                // Optional: You can also modify the button's appearance here
                // For example, adding a lock icon or changing colors
                SetButtonLockState(moduleButtons[i], isUnlocked);
            }
        }
    }

    private void SetButtonLockState(Button button, bool unlocked)
    {
        // Optional: Modify the button's appearance based on lock state
        ColorBlock colors = button.colors;
        if (unlocked)
        {
            colors.normalColor = Color.white;
        }
        else
        {
            colors.normalColor = new Color(0.5f, 0.5f, 0.5f); // Grayed out
        }
        button.colors = colors;
    }

    // Call this method when a module is completed to unlock the next one
    public void UnlockNextModule()
    {
        int currentProgress = PlayerPrefs.GetInt(PROGRESS_KEY);
        if (currentProgress < moduleButtons.Length)
        {
            currentProgress++;
            PlayerPrefs.SetInt(PROGRESS_KEY, currentProgress);
            PlayerPrefs.Save();
            InitializeModules(); // Refresh the buttons
        }
    }

    // Method to reset progress (for testing or player reset)
    public void ResetProgress()
    {
        PlayerPrefs.SetInt(PROGRESS_KEY, INITIAL_UNLOCKED_MODULES);
        PlayerPrefs.Save();
        InitializeModules();
    }
}
