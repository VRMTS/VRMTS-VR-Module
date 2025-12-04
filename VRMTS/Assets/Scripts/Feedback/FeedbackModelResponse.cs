using UnityEngine;

public class FeedbackModelResponse : MonoBehaviour
{
    // ---------------------------
    // Attributes
    // ---------------------------
    public bool success;
    public string rawResponse;
    public string processedFeedback;
    public float confidenceScore;

    void Start()
    {
        // Placeholder for initialization
    }

    void Update()
    {
        // Placeholder for updates (if needed)
    }

    // ---------------------------
    // Methods (DUMMY)
    // ---------------------------

    /// <summary>
    /// Parses a raw JSON string from an AI model.
    /// </summary>
    public void ParseJson(string json)
    {
        // TODO: Add JSON parsing logic
    }

    /// <summary>
    /// Converts the response into user-display text.
    /// </summary>
    public string GetReadableFeedback()
    {
        return processedFeedback ?? "Feedback unavailable.";
    }

    /// <summary>
    /// Resets internal data.
    /// </summary>
    public void ResetResponse()
    {
        success = false;
        rawResponse = "";
        processedFeedback = "";
        confidenceScore = 0f;
    }
}
