using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Canvas References")]
    public CanvasGroup loginCanvas;
    public CanvasGroup moduleCanvas;   // main menu pair
    public CanvasGroup userCanvas;     // main menu pair
    public CanvasGroup selectTestCanvas;
    public CanvasGroup settingsCanvas;
    public CanvasGroup analyticsCanvas;

    private CanvasGroup currentScreen;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip welcomeClip;
    public AudioClip introClip;

    [Header("Settings")]
    public float fadeDuration = 0.5f;
    public float rotationSpeed = 1.0f;

    private bool hasPlayedIntro = false;
    private static bool hasSeenLoginThisSession = false;

    void Start()
    {
        HideAllScreens();

        if (!hasSeenLoginThisSession)
            Login();
        else
            ShowMainMenu();
    }

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }

    // ============================
    // SCREEN CONTROL
    // ============================
    private void HideAllScreens()
    {
        CanvasGroup[] all = { loginCanvas, moduleCanvas, userCanvas, selectTestCanvas, settingsCanvas, analyticsCanvas };
        foreach (var cg in all)
        {
            cg.alpha = 0f;
            cg.gameObject.SetActive(false);
        }
        currentScreen = null;
    }

    private void ShowMainMenu()
    {
        moduleCanvas.gameObject.SetActive(true);
        userCanvas.gameObject.SetActive(true);
        FadeIn(moduleCanvas);
        FadeIn(userCanvas);
        currentScreen = moduleCanvas; // reference one of the main menu screens
    }

    // ============================
    // LOGIN / LOGOUT
    // ============================
    public void Login()
    {
        loginCanvas.gameObject.SetActive(true);
        FadeIn(loginCanvas);
        currentScreen = loginCanvas;

        if (!hasPlayedIntro && audioSource != null && welcomeClip != null)
            audioSource.PlayOneShot(welcomeClip);

        hasSeenLoginThisSession = true;
    }

    public void Logout()
    {
        FadeOut(currentScreen);
        FadeOut(moduleCanvas);
        FadeOut(userCanvas);
        Login();
    }

    // ============================
    // LOAD / EXIT SCREENS
    // ============================
    public void LoadScreen(CanvasGroup screen)
    {
        if (screen == moduleCanvas || screen == userCanvas)
        {
            FadeOut(currentScreen);
            ShowMainMenu();
            return;
        }

        // fade out main menu pair
        FadeOut(moduleCanvas);
        FadeOut(userCanvas);

        // fade in new screen
        screen.gameObject.SetActive(true);
        FadeIn(screen);
        currentScreen = screen;
    }

    public void ReturnToMainMenu()
    {
        // fade out current screen
        if (currentScreen != null && currentScreen != moduleCanvas && currentScreen != userCanvas)
            FadeOut(currentScreen);

        // fade in main menu pair
        ShowMainMenu();
    }

    // ============================
    // FADE HELPERS
    // ============================
    private void FadeIn(CanvasGroup canvas)
    {
        //canvas.alpha = 1f;
        //canvas.gameObject.SetActive(true);
        StartCoroutine(FadeIn_routine(canvas, fadeDuration));
    }

    private void FadeOut(CanvasGroup canvas)
    {
        //canvas.alpha = 0f;
        //canvas.gameObject.SetActive(false);
        StartCoroutine(FadeOut_routine(canvas, fadeDuration));
    }

    private IEnumerator FadeIn_routine(CanvasGroup canvas, float duration)
    {
        canvas.gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvas.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvas.alpha = 1f; // ensure fully visible at the end
        canvas.gameObject.SetActive(true);
    }

    private IEnumerator FadeOut_routine(CanvasGroup canvas, float duration)
    {
        float elapsed = 0f;
        float startAlpha = canvas.alpha;
        while (elapsed < duration)
        {
            canvas.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvas.alpha = 0f;
        canvas.gameObject.SetActive(false); // hide after fade
    }

    // ============================
    // QUIT
    // ============================
    public void ExitApplication()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
