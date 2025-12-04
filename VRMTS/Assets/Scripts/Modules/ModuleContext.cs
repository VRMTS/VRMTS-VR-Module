using UnityEngine;

public class ModuleContext : MonoBehaviour
{
    public string moduleId;
    public string moduleTitle;

    // All module steps (these are GameObjects you enable/disable)
    public GameObject[] steps;

    private int currentStep = 0;

    public void InitModule()
    {
        HideAllSteps();
        currentStep = 0;
    }

    public void ShowStep(int index)
    {
        if (index < 0 || index >= steps.Length) return;

        HideAllSteps();
        steps[index].SetActive(true);
        currentStep = index;
    }

    public void NextStep()
    {
        int next = currentStep + 1;

        if (next >= steps.Length)
        {
            Debug.Log($"{moduleTitle} completed!");
            HideAllSteps();
            return;
        }

        ShowStep(next);
    }

    private void HideAllSteps()
    {
        foreach (var s in steps)
            s.SetActive(false);
    }
}
