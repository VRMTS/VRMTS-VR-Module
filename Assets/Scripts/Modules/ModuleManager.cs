using UnityEngine;

public class ModuleManager : MonoBehaviour
{
    public static ModuleManager Instance; // for the singleton pattern

    public ModuleContext[] modules; // each segment of the lab

    private int currentModuleIndex = -1;

    void Awake()
    {
        Instance ??= this;
    }

    void Start()
    {
        LoadAllModules();
    }

    public void LoadAllModules()
    {
        foreach (var m in modules)
            m.gameObject.SetActive(false);
    }

    public void StartModule(int index)
    {
        if (index < 0 || index >= modules.Length) return;

        // disable previous
        if (currentModuleIndex >= 0)
            modules[currentModuleIndex].gameObject.SetActive(false);

        currentModuleIndex = index;

        modules[index].gameObject.SetActive(true);
        modules[index].InitModule();
        modules[index].ShowStep(0);

        Debug.Log("Started Module: " + modules[index].moduleTitle);
    }

    public void NextStep()
    {
        if (currentModuleIndex < 0) return;
        modules[currentModuleIndex].NextStep();
    }
}
