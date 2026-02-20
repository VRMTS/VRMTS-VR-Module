/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBUpdater : MonoBehaviour
{
    public static DBUpdater Instance;

    // ------------------------------
    // Simulated server endpoint
    // ------------------------------
    public string serverURL = "https://example.com/api/upload";
    public bool isConnected = false;

    // Queue for offline uploads
    private List<TestResult> pendingUploads = new List<TestResult>();

    void Awake()
    {
        Instance ??= this;
    }

    // ------------------------------
    // Public API
    // ------------------------------

    public void UploadTestResult(TestResult result)
    {
        Debug.Log("DBUploader → Simulated upload request received.");

        if (!isConnected)
        {
            Debug.Log("DBUploader → Offline. Storing result locally.");
            pendingUploads.Add(result);
            return;
        }

        StartCoroutine(SimulateUpload(result));
    }

    public void SyncPendingData()
    {
        Debug.Log("DBUploader → Attempting to sync pending uploads...");

        if (!isConnected || pendingUploads.Count == 0)
            return;

        foreach (var r in pendingUploads)
            StartCoroutine(SimulateUpload(r));

        pendingUploads.Clear();
    }

    public void SetConnectionStatus(bool status)
    {
        isConnected = status;
        Debug.Log("DBUploader → Connection status set to: " + status);
    }

    // ------------------------------
    // Internal simulation helpers
    // ------------------------------

    private IEnumerator SimulateUpload(TestResult result)
    {
        Debug.Log("DBUploader → Uploading to: " + serverURL);

        yield return new WaitForSeconds(1.5f); // simulate delay

        Debug.Log("DBUploader → Upload successful (simulated).");
    }

    private void SavePendingLocally()
    {
        Debug.Log("DBUploader → Saving pending uploads locally (placeholder).");
    }

    private void LoadPendingOnStart()
    {
        Debug.Log("DBUploader → Loading pending uploads (placeholder).");
    }
}
*/