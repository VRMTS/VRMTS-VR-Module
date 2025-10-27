using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Canvas References")]
    public CanvasGroup loginCanvas;
    public CanvasGroup moduleCanvas;
    public CanvasGroup userCanvas;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip welcomeClip;
    public AudioClip introClip;

    [Header("Settings")]
    public float fadeDuration = 1.5f;
    public float rotationSpeed = 1.0f;

    private bool hasPlayedIntro = false;

    void Start()
    {
        // Initialize states
        moduleCanvas.alpha = 0f;
        moduleCanvas.gameObject.SetActive(false);
        userCanvas.alpha = 0f;
        userCanvas.gameObject.SetActive(false);

        // Start with login canvas visible
        loginCanvas.alpha = 0f;
        loginCanvas.gameObject.SetActive(true);
        if (welcomeClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(welcomeClip);
            //yield return new WaitForSeconds(welcomeClip.length + 0.5f);
        }
        StartCoroutine(FadeCanvas(loginCanvas, 0f, 1f, fadeDuration));
    }

    void Update()
    {
        // Rotate skybox
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }

    public void OnLoginButtonClicked()
    {
        if (!hasPlayedIntro)
        {
            hasPlayedIntro = true;
            StartCoroutine(TransitionToMain());
        }
    }

    private IEnumerator TransitionToMain()
    {
        // Fade out login
        yield return StartCoroutine(FadeCanvas(loginCanvas, 1f, 0f, fadeDuration));
        loginCanvas.gameObject.SetActive(false);

        // Fade in module + user
        moduleCanvas.gameObject.SetActive(true);
        userCanvas.gameObject.SetActive(true);

        yield return StartCoroutine(FadeCanvas(moduleCanvas, 0f, 1f, fadeDuration));
        yield return StartCoroutine(FadeCanvas(userCanvas, 0f, 1f, fadeDuration));

        // Play audios once
        //if (welcomeClip != null && audioSource != null)
        //{
            //audioSource.PlayOneShot(welcomeClip);
           // yield return new WaitForSeconds(welcomeClip.length + 0.5f);
        //}

        if (introClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(introClip);
        }
    }

    private IEnumerator FadeCanvas(CanvasGroup canvas, float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvas.alpha = Mathf.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvas.alpha = end;
    }
}
