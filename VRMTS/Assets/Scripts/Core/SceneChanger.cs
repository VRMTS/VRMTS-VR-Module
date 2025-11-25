using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Singleton instance
    public static SceneChanger Instance;

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); // avoid duplicates
            return;
        }

        Instance = this;

        // Keep this GameObject alive across scenes
        //DontDestroyOnLoad(this.gameObject);
    }

    // Load a new scene
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Reload the current scene
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

// ================================
// File: Scripts/Core/SceneChanger.cs
// Purpose: Switch scenes, singleton
// ================================