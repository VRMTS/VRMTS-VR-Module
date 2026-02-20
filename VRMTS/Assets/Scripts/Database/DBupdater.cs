using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class SimpleUser
{
    public int userId;
    public int studentId;
    public string name;
    public string email;
}

public class DBUpdater : MonoBehaviour
{
    public static DBUpdater Instance;

   /*currently putting this on hold...
   
    [Header("Server Configuration")]
    // Change this IP to your local IP if running on Quest, or "localhost" if running in Editor
    public string baseURL = "http://localhost/vrmts_api/"; 

    [Header("Debug Status")]
    public bool isLoggedIn = false;
    public SimpleUser currentUser;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =========================================================
    // 1. AUTHENTICATION (Login)
    // =========================================================

    public void Login(string email, string password, System.Action<bool, string> callback)
    {
        StartCoroutine(LoginCoroutine(email, password, callback));
    }

    private IEnumerator LoginCoroutine(string email, string password, System.Action<bool, string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(baseURL + "login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Network Error: " + www.error);
                callback?.Invoke(false, "Network Error");
            }
            else
            {
                string response = www.downloadHandler.text;
                Debug.Log("Server Response: " + response);

                // Assuming PHP returns JSON like: {"status":"success", "userId":1, "studentId":3, "name":"John"}
                if (response.Contains("success"))
                {
                    currentUser = JsonUtility.FromJson<SimpleUser>(response); // You might need a wrapper for strict JSON
                    isLoggedIn = true;
                    callback?.Invoke(true, "Login Successful");
                }
                else
                {
                    callback?.Invoke(false, "Invalid Credentials");
                }
            }
        }
    }

    // =========================================================
    // 2. REGISTRATION (Create User + Student)
    // =========================================================

    public void Register(string email, string password, string name, System.Action<bool, string> callback)
    {
        StartCoroutine(RegisterCoroutine(email, password, name, callback));
    }

    private IEnumerator RegisterCoroutine(string email, string password, string name, System.Action<bool, string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("name", name);

        using (UnityWebRequest www = UnityWebRequest.Post(baseURL + "register.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                callback?.Invoke(false, www.error);
            }
            else
            {
                string response = www.downloadHandler.text;
                if (response.Contains("success"))
                    callback?.Invoke(true, "Registration Complete");
                else
                    callback?.Invoke(false, "Registration Failed: " + response);
            }
        }
    }

    // =========================================================
    // 3. UPLOAD TEST RESULTS
    // =========================================================

    public void UploadTestResult(TestResult result)
    {
        if (!isLoggedIn)
        {
            Debug.LogError("Cannot upload result: User not logged in.");
            return;
        }

        StartCoroutine(UploadResultCoroutine(result));
    }

    private IEnumerator UploadResultCoroutine(TestResult result)
    {
        WWWForm form = new WWWForm();
        
        // Match these fields with the PHP script
        form.AddField("studentId", currentUser.studentId);
        form.AddField("labName", result.labName); // We will map LabName to a ModuleID in PHP
        form.AddField("score", result.correct);
        form.AddField("total", result.totalQuestions);
        form.AddField("gainedTags", string.Join(",", result.gainedTags)); // Send tags as comma string

        using (UnityWebRequest www = UnityWebRequest.Post(baseURL + "upload_result.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Upload Success: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Upload Failed: " + www.error);
            }
        }
    }




    currently putting this on hold...*/
}