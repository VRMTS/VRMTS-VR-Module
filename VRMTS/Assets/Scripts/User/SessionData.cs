// ================================
// File: Scripts/User/SessionData.cs
// Purpose: Tracks current session
// ================================
using System;

[Serializable]
public class SessionData
{
    public string SessionId;
    public string StartTime;       // ISO 8601 string
    public string EndTime;         // ISO 8601 string
    public string CurrentModule;   // Active module
    public string SessionSummary;  // LLM-friendly summary

    public SessionData()
    {
        SessionId = Guid.NewGuid().ToString();
        StartTime = DateTime.UtcNow.ToString("o");
        EndTime = null;
        CurrentModule = "";
        SessionSummary = "";
    }
}
