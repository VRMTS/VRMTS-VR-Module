using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class LabController : MonoBehaviour
{
    public static LabController Instance;

    public enum LabMode { Explore = 0, Learning = 1, Interactive = 2 }[Header("--- Current State ---")]
    public LabMode currentMode = LabMode.Learning;[Header("--- Progression & Locking ---")]
    public string labSaveKey = "Lab1_Completed";
    public Button btnModeExplore;     
    public Button btnModeInteractive; 
    private bool isLabCompleted = false;[Header("--- 3D Roots (Parents) ---")]
    public GameObject rootExplore;     
    public GameObject rootLearning;    
    public GameObject rootInteractive; 

    [Header("--- UI: Panels ---")]
    public GameObject uiPanelExploreLeft;  
    public GameObject uiPanelExploreRight; 
    public GameObject uiPanelLearning;     
    public GameObject uiPanelInteractive;  
    public GameObject uiModeSelection;[Header("--- Explore Mode Settings ---")]
    public Transform explorePivot; 
    public TMP_Dropdown exploreDropdown; 
    public List<ExplorePart> exploreParts;

    [Header("--- Explore Sliders ---")]
    public Slider sliderRotX;
    public Slider sliderRotY;
    public Slider sliderRotZ;
    public Slider sliderZoom;
    
    private Vector3 initScale;
    private Quaternion initRot;[Header("--- Learning Mode Settings ---")]
    public TextMeshProUGUI instructionText;
    public Button btnNextStep;
    public Button btnPrevStep;   
    public Button btnRestartLab;[Header("--- Audio ---")]
    public AudioSource labAudioSource;
    public AudioClip enterExploreAudio;      // Plays when entering Explore
    public AudioClip enterInteractiveAudio;  // Plays when entering Interactive
    
    public float typeWriterSpeed = 0.04f;
    public List<LabStep> learningSteps;

    private int currentStepIndex = -1;
    private bool stepInteractionDone = false; 
    private Coroutine typingCoroutine; 

    // --- DATA STRUCTURES ---

    [System.Serializable]
    public class ExplorePart
    {
        public string name;
        public GameObject modelPart; 
    }

    [System.Serializable]
    public class LabStep
    {
        public string name;[TextArea] public string instruction;
        public GameObject visualRoot; 
        public GameObject interactionPart; 
        public bool requireInteraction; 
        public AudioClip voiceClip;
    }

    void Awake() 
    { 
        Instance = this; 
        if (explorePivot) 
        { 
            initScale = explorePivot.localScale; 
            initRot = explorePivot.localRotation; 
        }
        
        // Check Save State
        //isLabCompleted = PlayerPrefs.GetInt(labSaveKey, 0) == 1;
        isLabCompleted = true; // TEMP: Unlock everything for development testing
    }

    void Start()
    {
        InitializeExploreSliders();
        InitializeExploreDropdown();
        InitializeButtons(); 
        
        UpdateLockUI();

        SwitchMode(1); 
    }

    void InitializeButtons()
    {
        // Wire up Learning Navigation Buttons
        if (btnNextStep)
        {
            btnNextStep.onClick.RemoveAllListeners();
            btnNextStep.onClick.AddListener(() => LoadStep(currentStepIndex + 1));
        }

        if (btnPrevStep)
        {
            btnPrevStep.onClick.RemoveAllListeners();
            btnPrevStep.onClick.AddListener(() => LoadStep(currentStepIndex - 1));
        }

        if (btnRestartLab)
        {
            btnRestartLab.onClick.RemoveAllListeners();
            btnRestartLab.onClick.AddListener(() => LoadStep(0)); // Start from beginning
        }
    }

    void UpdateLockUI()
    {
        if (btnModeExplore) btnModeExplore.interactable = isLabCompleted;
        if (btnModeInteractive) btnModeInteractive.interactable = isLabCompleted;
    }

    // =========================================================
    // 1. MODE SWITCHING
    // =========================================================
    public void SwitchMode(int modeIndex)
    {
        // CONTENT LOCK GUARD: Prevent switching if not completed
        if (!isLabCompleted && modeIndex != 1)
        {
            Debug.LogWarning("Access Denied: Complete Learning Mode first!");
            return; 
        }

        currentMode = (LabMode)modeIndex;

        // Hide Everything Initially
        if(rootExplore) rootExplore.SetActive(false);
        if(rootLearning) rootLearning.SetActive(false);
        if(rootInteractive) rootInteractive.SetActive(false);

        if(uiPanelExploreLeft) uiPanelExploreLeft.SetActive(false);
        if(uiPanelExploreRight) uiPanelExploreRight.SetActive(false);
        if(uiPanelLearning) uiPanelLearning.SetActive(false);
        if(uiPanelInteractive) uiPanelInteractive.SetActive(false);

        // Stop audio if switching modes
        if (labAudioSource) labAudioSource.Stop();

        switch (currentMode)
        {
            case LabMode.Explore:
                if(rootExplore) rootExplore.SetActive(true);
                if(uiPanelExploreLeft) uiPanelExploreLeft.SetActive(true);
                if(uiPanelExploreRight) uiPanelExploreRight.SetActive(true);
                
                // Play Explore Intro Audio safely
                PlayModeAudio(enterExploreAudio);
                break;

            case LabMode.Learning:
                if(rootLearning) rootLearning.SetActive(true);
                if(uiPanelLearning) uiPanelLearning.SetActive(true);
                
                StartLearningSession();
                break;

            case LabMode.Interactive:
                if(rootInteractive) rootInteractive.SetActive(true);
                if(uiPanelInteractive) uiPanelInteractive.SetActive(true);
                
                // Play Interactive Intro Audio safely
                PlayModeAudio(enterInteractiveAudio);
                break;
        }
    }

    // --- SAFE AUDIO HELPER ---
    private void PlayModeAudio(AudioClip clip)
    {
        if (labAudioSource != null && clip != null)
        {
            // Failsafe: Check if the audio source is attached to an inactive object
            if (!labAudioSource.gameObject.activeInHierarchy)
            {
                Debug.LogError("[Audio Error] The AudioSource is attached to a GameObject that is hidden! Move the AudioSource component directly to the 'Lab_Manager' object.");
                return;
            }

            labAudioSource.Stop();
            labAudioSource.PlayOneShot(clip);
            Debug.Log($"[Audio Success] Playing mode clip: {clip.name}");
        }
        else
        {
            Debug.LogWarning("[Audio Warning] Either the AudioSource or the AudioClip is missing in the Inspector!");
        }
    }

    // =========================================================
    // 2. EXPLORE MODE LOGIC
    // =========================================================
    void InitializeExploreSliders()
    {
        SetupRotationSlider(sliderRotX);
        SetupRotationSlider(sliderRotY);
        SetupRotationSlider(sliderRotZ);

        if (sliderZoom)
        {
            sliderZoom.minValue = 0.5f;
            sliderZoom.maxValue = 2.5f;
            sliderZoom.value = 1.0f; 
            sliderZoom.onValueChanged.AddListener((v) => UpdateExploreTransform());
        }
    }

    void SetupRotationSlider(Slider s)
    {
        if(s != null)
        {
            s.minValue = 0f;
            s.maxValue = 360f; 
            s.onValueChanged.RemoveAllListeners();
            s.onValueChanged.AddListener((v) => UpdateExploreTransform());
        }
    }

    void InitializeExploreDropdown()
    {
        if (exploreDropdown == null) return;

        exploreDropdown.ClearOptions();
        List<string> options = new List<string>(); 

        foreach (var part in exploreParts)
        {
            options.Add(part.name);
        }

        exploreDropdown.AddOptions(options);
        exploreDropdown.onValueChanged.RemoveAllListeners();
        exploreDropdown.onValueChanged.AddListener(OnExploreDropdownChanged);

        if (exploreParts.Count > 0)
        {
            exploreDropdown.value = 0;
            OnExploreDropdownChanged(0);
        }
    }

    void OnExploreDropdownChanged(int index)
    {
        for (int i = 0; i < exploreParts.Count; i++)
        {
            if (exploreParts[i].modelPart != null)
            {
                bool isActive = (i == index);
                exploreParts[i].modelPart.SetActive(isActive);
            }
        }
    }

    void UpdateExploreTransform()
    {
        if(!explorePivot) return;
        
        float x = sliderRotX ? sliderRotX.value : 0;
        float y = sliderRotY ? sliderRotY.value : 0;
        float z = sliderRotZ ? sliderRotZ.value : 0;
        explorePivot.localRotation = Quaternion.Euler(x, y, z) * initRot;
        
        float scale = sliderZoom ? sliderZoom.value : 1.0f;
        explorePivot.localScale = initScale * scale;
    }

    // =========================================================
    // 3. LEARNING MODE LOGIC
    // =========================================================
    void StartLearningSession()
    {
        // Hide all parts
        foreach(var step in learningSteps) 
            if(step.visualRoot) step.visualRoot.SetActive(false);

        // Resume where they left off, or start at 0 if new/finished
        if (currentStepIndex < 0 || currentStepIndex >= learningSteps.Count)
        {
            LoadStep(0);
        }
        else
        {
            LoadStep(currentStepIndex);
        }
    }

    // Master function for navigating steps
    private void LoadStep(int targetIndex)
    {
        // Stop current typing effect if jumping around
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        // Hide current step visual
        if(currentStepIndex >= 0 && currentStepIndex < learningSteps.Count)
        {
            if(learningSteps[currentStepIndex].visualRoot)
                learningSteps[currentStepIndex].visualRoot.SetActive(false);
        }

        // Apply new index
        currentStepIndex = targetIndex;

        // Handle Completion Screen
        if (currentStepIndex >= learningSteps.Count)
        {
            isLabCompleted = true;
            PlayerPrefs.SetInt(labSaveKey, 1);
            PlayerPrefs.Save();
            UpdateLockUI(); 

            instructionText.text = "<color=green>Lab Completed!</color>\nExplore and Interactive Modes are now unlocked. You can review steps or restart the lab.";
            
            if (btnNextStep) btnNextStep.interactable = false;
            if (btnPrevStep) btnPrevStep.interactable = true; // Allow going back to last step
            if (btnRestartLab) btnRestartLab.gameObject.SetActive(true); 
            
            return;
        }

        // Play the newly selected step
        typingCoroutine = StartCoroutine(PlayStepSequence(learningSteps[currentStepIndex]));
    }

    IEnumerator PlayStepSequence(LabStep step)
    {
        stepInteractionDone = false;
        
        // UI Button States
        if (btnNextStep) btnNextStep.interactable = !step.requireInteraction; // Lock next if interaction required
        if (btnPrevStep) btnPrevStep.interactable = (currentStepIndex > 0);   // Can't go previous on step 0
        if (btnRestartLab) btnRestartLab.gameObject.SetActive(true); 

        // Play Voice
        if(labAudioSource && step.voiceClip) 
        { 
            labAudioSource.Stop(); 
            labAudioSource.clip = step.voiceClip; 
            labAudioSource.Play(); 
        }

        // Typewriter Effect
        instructionText.text = "";
        foreach(char c in step.instruction)
        {
            instructionText.text += c;
            yield return new WaitForSeconds(typeWriterSpeed);
        }

        // SHOW THE PARENT (Visuals)
        if(step.visualRoot) step.visualRoot.SetActive(true);

        if (step.requireInteraction)
        {
            instructionText.text += "\n\n<color=yellow>(Grab the highlighted object to continue)</color>";
        }
    }

    public void OnItemInteracted(LabItem item)
    {
        if (currentMode != LabMode.Learning) return;
        if (currentStepIndex < 0 || currentStepIndex >= learningSteps.Count) return;

        LabStep currentStep = learningSteps[currentStepIndex];

        if (item.gameObject == currentStep.interactionPart)
        {
            if (!stepInteractionDone)
            {
                stepInteractionDone = true;
                if (btnNextStep) btnNextStep.interactable = true; 
                instructionText.text = currentStep.instruction + "\n\n<color=green>Great! Press Next.</color>";
            }
        }
    }

    // Used for backend testing/development
    public void ResetProgress()
    {
        PlayerPrefs.SetInt(labSaveKey, 0);
        PlayerPrefs.Save();
        isLabCompleted = false;
        UpdateLockUI();
        Debug.Log("Progress Reset!");
    }
}