using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBUpdater : MonoBehaviour
{
    public static DBUpdater Instance;

    void Awake()
    {
        Instance ??= this;
    }

    public void UploadTestResult(TestResult result)
    {
        Debug.Log("DB Upload Placeholder → This will sync with server later.");
    }
}
