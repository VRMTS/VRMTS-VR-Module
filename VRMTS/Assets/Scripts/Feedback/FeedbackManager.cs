using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;

    void Awake()
    {
        Instance ??= this;
    }

    public string GenerateAdaptiveFeedback(TestResult result)
    {
        return "AI feedback module is under development.\n(Your performance will be analyzed here.)";
    }
}
