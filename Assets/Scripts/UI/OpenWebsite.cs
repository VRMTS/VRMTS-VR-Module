using UnityEngine;

public class OpenWebsite : MonoBehaviour
{
    void Start()
    {
        Application.OpenURL("https://vrmts-web.vercel.app/");
    }
}