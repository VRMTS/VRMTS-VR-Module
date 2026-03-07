using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class LabManager : MonoBehaviour
{
    public static LabManager Instance; // Singleton for easy access

    public enum LabMode { Explore, Learning, Interactive }

    [Header("--- Configuration ---")]
    public LabMode currentMode = LabMode.Explore;
    public float typeWriterSpeed = 0.03f;

    [Header("--- Roots (The 3 Styles) ---")]
    public GameObject rootExplore;     // The Rotating Pivot Model
    public GameObject rootLearning;    // The Guided Steps Objects
    public GameObject rootInteractive; // The Physics/Sandbox Objects

    [Header("--- UI References ---")]
    public GameObject panelModeSelect; // Buttons to choose mode
    public GameObject panelExplore;    // Sliders/Dropdowns
    public GameObject panelLearning;   // Text/Next Button
    public TMP_Dropdown partSelectorDropdown; // For Explore Mode
    
    [Header("--- Learning Mode Data ---")]
    public TextMeshProUGUI instructionText;
    public Button btnNextStep;
    public AudioSource labAudioSource;
    public List<LabStep> learningSteps;

    [Header("--- Explore Mode Data ---")]
    public Transform explorePivot;
    public List<GameObject> exploreParts; // For the dropdown isolation
    public Slider sliderRotX, sliderRotY, sliderZoom;
    private Vector3 initialScale;
    private Quaternion initialRotation;

    // Internal State
    private int currentStepIndex = -1;
    private Coroutine currentRoutine;
    private bool stepInteractionDone = false; // Has user grabbed the object?

    [System.Serializable]
    public class LabStep
    {
        public string name;
        [TextArea] public string instruction;
        public GameObject targetObject; // The object inside RootLearning
        public AudioClip voiceClip;
        public bool requireInteraction = true; // Does user need to grab it to proceed?
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (explorePivot)
        {
            initialScale = explorePivot.localScale;
            initialRotation = explorePivot.localRotation;
        }

        InitializeUI();
        // Start at Mode Selection
        SwitchMode(LabMode.Explore); 
    }

    // =========================================================
    // MODE SWITCHING LOGIC
    // =========================================================
    public void SwitchToExplore() => SwitchMode(LabMode.Explore);
    public void SwitchToLearning() => SwitchMode(LabMode.Learning);
    public void SwitchToInteractive() => SwitchMode(LabMode.Interactive);

    public void SwitchMode(LabMode newMode)
    {
        currentMode = newMode;

        // 1. Reset Roots
        if(rootExplore) rootExplore.SetActive(false);
        if(rootLearning) rootLearning.SetActive(false);
        if(rootInteractive) rootInteractive.SetActive(false);

        // 2. Reset UI
        if(panelExplore) panelExplore.SetActive(false);
        if(panelLearning) panelLearning.SetActive(false);
        if(panelModeSelect) panelModeSelect.SetActive(true); // Keep header visible?

        // 3. Activate Specifics
        switch (currentMode)
        {
            case LabMode.Explore:
                if(rootExplore) rootExplore.SetActive(true);
                if(panelExplore) panelExplore.SetActive(true);
                InitializeExploreDropdown();
                break;

            case LabMode.Learning:
                if(rootLearning) rootLearning.SetActive(true);
                if(panelLearning) panelLearning.SetActive(true);
                StartLearningSession();
                break;

            case LabMode.Interactive:
                if(rootInteractive) rootInteractive.SetActive(true);
                // Interactive has no HUD, just free roam
                break;
        }
    }

    // =========================================================
    // LEARNING MODE LOGIC (The Steps)
    // =========================================================
    void StartLearningSession()
    {
        // Hide all learning parts initially
        foreach (var step in learningSteps)
        {
            if (step.targetObject) step.targetObject.SetActive(false);
        }

        currentStepIndex = -1;
        AdvanceStep();
    }

    public void AdvanceStep()
    {
        // Cleanup previous step
        if (currentStepIndex >= 0 && currentStepIndex < learningSteps.Count)
        {
            if (learningSteps[currentStepIndex].targetObject)
                learningSteps[currentStepIndex].targetObject.SetActive(false);
        }

        currentStepIndex++;

        if (currentStepIndex >= learningSteps.Count)
        {
            instructionText.text = "Lab Completed!";
            btnNextStep.interactable = false;
            return;
        }

        StartCoroutine(PlayStepSequence(learningSteps[currentStepIndex]));
    }

    IEnumerator PlayStepSequence(LabStep step)
    {
        stepInteractionDone = false;
        btnNextStep.interactable = false; // Disable until interaction (if required)

        // 1. Audio
        if (labAudioSource && step.voiceClip)
        {
            labAudioSource.clip = step.voiceClip;
            labAudioSource.Play();
        }

        // 2. Text Typewriter
        instructionText.text = "";
        foreach (char c in step.instruction)
        {
            instructionText.text += c;
            yield return new WaitForSeconds(typeWriterSpeed);
        }

        // 3. Show Object
        if (step.targetObject)
        {
            step.targetObject.SetActive(true);
        }

        // 4. If no interaction needed, enable Next immediately
        if (!step.requireInteraction)
        {
            btnNextStep.interactable = true;
        }
        else
        {
            instructionText.text += "\n<size=80%>(Grab the object to continue)</size>";
        }
    }

    // CALLED BY LabPartTrigger.cs
    public void CheckStepCompletion(GameObject interactedObj)
    {
        if (currentMode != LabMode.Learning) return;
        if (currentStepIndex < 0 || currentStepIndex >= learningSteps.Count) return;

        // Check if the object grabbed is the target for this step
        if (interactedObj == learningSteps[currentStepIndex].targetObject)
        {
            if (!stepInteractionDone)
            {
                stepInteractionDone = true;
                btnNextStep.interactable = true;
                instructionText.text = learningSteps[currentStepIndex].instruction + "\n<color=green>Done! Press Next.</color>";
            }
        }
    }

    // =========================================================
    // EXPLORE MODE LOGIC (Standard View)
    // =========================================================
    void InitializeExploreDropdown()
    {
        partSelectorDropdown.ClearOptions();
        List<string> options = new List<string> { "Full Body" };
        foreach (var p in exploreParts) options.Add(p.name);
        partSelectorDropdown.AddOptions(options);
        
        partSelectorDropdown.onValueChanged.RemoveAllListeners();
        partSelectorDropdown.onValueChanged.AddListener((idx) => {
            // Index 0 = Full Body, Index 1 = Part 0
            if(idx == 0) 
            {
                foreach(var p in exploreParts) p.SetActive(true);
            }
            else 
            {
                for(int i=0; i<exploreParts.Count; i++) exploreParts[i].SetActive(i == idx-1);
            }
        });
    }

    // Link these to Sliders in Inspector
    public void UpdateRotation() 
    {
        if(!explorePivot) return;
        explorePivot.localRotation = Quaternion.Euler(sliderRotX.value, sliderRotY.value, 0) * initialRotation;
        explorePivot.localScale = initialScale * sliderZoom.value;
    }

    void InitializeUI()
    {
        if(btnNextStep) btnNextStep.onClick.AddListener(AdvanceStep);
    }
}