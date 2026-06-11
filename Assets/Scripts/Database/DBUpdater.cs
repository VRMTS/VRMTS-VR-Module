using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// 1. Simple User Data Container (No namespace complication)[System.Serializable]
public class SimpleUser
{
    public int userId;
    public int studentId;
    public string name;
    public string email;
}

// 2. Main Class (Notice the lowercase 'u' in DBupdater to match your file name)
public class DBUpdater : MonoBehaviour
{
    public static DBUpdater Instance;

    [Header("Server Config")]
    public string baseURL = "http://localhost/vrmts_api/";

    [Header("Runtime Data")]
    public SimpleUser currentUser;
    public bool isLoggedIn = false;

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

    public void Login(string email, string password, System.Action<bool, string> callback)
    {
        StartCoroutine(LoginRoutine(email, password, callback));
    }

    private IEnumerator LoginRoutine(string email, string password, System.Action<bool, string> callback)
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
                callback?.Invoke(false, "Server offline.");
            }
            else
            {
                string json = www.downloadHandler.text;
                Debug.Log("Server says: " + json);

                if (json.Contains("success"))
                {
                    currentUser = JsonUtility.FromJson<SimpleUser>(json);
                    isLoggedIn = true;
                    callback?.Invoke(true, "Login Success!");
                }
                else
                {
                    callback?.Invoke(false, "Invalid Email or Password.");
                }
            }
        }
    }
}