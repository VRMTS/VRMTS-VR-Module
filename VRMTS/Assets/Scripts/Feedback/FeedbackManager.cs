using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;

    // ---------------------------
    // Attributes
    // ---------------------------
    [Header("AI Model Config")]
    public string modelName = "AdaptiveFeedbackV1";
    public float feedbackDelay = 1.5f;

    [Header("Feedback Cache")]
    private Dictionary<string, string> cachedFeedback = new Dictionary<string, string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ---------------------------
    // Public Methods (DUMMY)
    // ---------------------------

    /// <summary>
    /// Main entry point to generate feedback.
    /// </summary>
    public string GenerateAdaptiveFeedback(TestResult result)
    {
        // Placeholder return
        return "AI adaptive feedback is under development.";
    }

    /// <summary>
    /// Sends a request to the external/local AI model.
    /// </summary>
    public void RequestModelFeedback(TestResult result)
    {
        // TODO: Implement model communication
    }

    /// <summary>
    /// Stores feedback in memory for later usage.
    /// </summary>
    public void CacheFeedback(string key, string feedback)
    {
        if (!cachedFeedback.ContainsKey(key))
            cachedFeedback.Add(key, feedback);
    }

    /// <summary>
    /// Retrieves cached feedback if available.
    /// </summary>
    public string GetCachedFeedback(string key)
    {
        return cachedFeedback.ContainsKey(key) ? cachedFeedback[key] : null;
    }

    /// <summary>
    /// Logs feedback for analytics.
    /// </summary>
    public void LogFeedback(TestResult result, string feedback)
    {
        // TODO: write to DB or persistent storage
    }

    /// <summary>
    /// Clears cached responses.
    /// </summary>
    public void ClearCache()
    {
        cachedFeedback.Clear();
    }
}
