using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LabController : MonoBehaviour
{
    [Header("--- 3D Model References ---")]
    [Tooltip("The parent transform of the main anatomy model.")]
    public Transform mainModelPivot; 
    
    [Tooltip("The root object containing all interactive mode objects.")]
    public GameObject interactiveModeRoot;

    [Header("--- Data Definition ---")]
    [Tooltip("List of parts to populate the Left Panel Dropdown.")]
    public List<AnatomyPart> anatomyParts;

    [Header("--- UI: Left Panel (Selection) ---")]
    public GameObject leftPanelRoot;
    public TMP_Dropdown partSelectorDropdown;

    [Header("--- UI: Right Panel (Controls) ---")]
    public GameObject rightPanelRoot;
    public Slider sliderRotX;
    public Slider sliderRotY;
    public Slider sliderRotZ;
    public Slider sliderZoom;
    public Button btnEnterInteractive;
    public Button btnExitToMenu;

    [Header("--- UI: Interactive Mode HUD ---")]
    public GameObject interactiveHUDRoot;
    public Button btnExitInteractive;

    // Internal State
    private Vector3 initialScale;
    private Quaternion initialRotation;

    [System.Serializable]
    public class AnatomyPart
    {
        public string name;
        public GameObject partObject;
    }

    void Start()
    {
        // 1. Store initial transform data
        if (mainModelPivot != null)
        {
            initialScale = mainModelPivot.localScale;
            initialRotation = mainModelPivot.localRotation;
        }

        // 2. Initialize UI States
        InitializeDropdown();
        InitializeSliders();
        InitializeButtons();

        // 3. Set default mode (Standard Lab Mode)
        SetInteractiveMode(false);
    }

    // ---------------------------------------------------------
    // Initialization
    // ---------------------------------------------------------
    void InitializeDropdown()
    {
        if (partSelectorDropdown == null) return;

        partSelectorDropdown.ClearOptions();
        List<string> options = new List<string>();

        // REMOVED: options.Add("Full Model"); 

        // Add actual parts only
        foreach (var part in anatomyParts)
        {
            options.Add(part.name);
        }

        partSelectorDropdown.AddOptions(options);
        
        partSelectorDropdown.onValueChanged.RemoveAllListeners(); 
        partSelectorDropdown.onValueChanged.AddListener(OnPartSelected);

        // FORCE UPDATE: Select the first item immediately on startup
        if (anatomyParts.Count > 0)
        {
            partSelectorDropdown.value = 0; // Ensure UI visual is 0
            OnPartSelected(0);              // Run logic for 0
        }
    }

    void InitializeSliders()
    {
        SetupSlider(sliderRotX, 0, 360, (val) => UpdateModelTransform());
        SetupSlider(sliderRotY, 0, 360, (val) => UpdateModelTransform());
        SetupSlider(sliderRotZ, 0, 360, (val) => UpdateModelTransform());
        SetupSlider(sliderZoom, 0.5f, 2.5f, (val) => UpdateModelTransform());
        
        if(sliderZoom != null) sliderZoom.value = 1.0f;
    }

    void SetupSlider(Slider s, float min, float max, UnityEngine.Events.UnityAction<float> action)
    {
        if (s == null) return;
        s.minValue = min;
        s.maxValue = max;
        s.onValueChanged.RemoveAllListeners();
        s.onValueChanged.AddListener(action);
    }

    void InitializeButtons()
    {
        if (btnEnterInteractive != null)
            btnEnterInteractive.onClick.AddListener(() => SetInteractiveMode(true));

        if (btnExitInteractive != null)
            btnExitInteractive.onClick.AddListener(() => SetInteractiveMode(false));

        if (btnExitToMenu != null)
            btnExitToMenu.onClick.AddListener(ExitToMainMenu);
    }

    // ---------------------------------------------------------
    // Logic: Model Manipulation
    // ---------------------------------------------------------
    void UpdateModelTransform()
    {
        if (mainModelPivot == null) return;

        float x = sliderRotX ? sliderRotX.value : 0;
        float y = sliderRotY ? sliderRotY.value : 0;
        float z = sliderRotZ ? sliderRotZ.value : 0;
        mainModelPivot.localRotation = Quaternion.Euler(x, y, z) * initialRotation;

        float scaleMultiplier = sliderZoom ? sliderZoom.value : 1.0f;
        mainModelPivot.localScale = initialScale * scaleMultiplier;
    }

    // ---------------------------------------------------------
    // Logic: Part Selection (Left Panel)
    // ---------------------------------------------------------
    void OnPartSelected(int index)
    {
        // DIRECT MAPPING:
        // Index 0 in Dropdown = Index 0 in List
        
        for (int i = 0; i < anatomyParts.Count; i++)
        {
            if (anatomyParts[i].partObject != null)
            {
                // If i matches the selected index, set active (true).
                // If i does not match, set inactive (false).
                bool shouldBeActive = (i == index);
                anatomyParts[i].partObject.SetActive(shouldBeActive);
            }
        }
    }

    // ---------------------------------------------------------
    // Logic: Mode Switching
    // ---------------------------------------------------------
    public void SetInteractiveMode(bool isInteractive)
    {
        if (leftPanelRoot) leftPanelRoot.SetActive(!isInteractive);
        if (rightPanelRoot) rightPanelRoot.SetActive(!isInteractive);
        if (interactiveHUDRoot) interactiveHUDRoot.SetActive(isInteractive);

        if (mainModelPivot) mainModelPivot.gameObject.SetActive(!isInteractive);
        if (interactiveModeRoot) interactiveModeRoot.SetActive(isInteractive);
    }

    // ---------------------------------------------------------
    // Logic: Navigation
    // ---------------------------------------------------------
    void ExitToMainMenu()
    {
        if (SceneChanger.Instance != null)
        {
            SceneChanger.Instance.ChangeScene("MainMenu"); 
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}