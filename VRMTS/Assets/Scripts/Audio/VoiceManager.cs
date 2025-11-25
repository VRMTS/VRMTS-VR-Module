using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    public AudioSource voiceAudioSource;
    public AudioClip voiceClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void PlayVoiceClip()
    {
        if (voiceAudioSource != null && voiceClip != null)
        {
            voiceAudioSource.PlayOneShot(voiceClip);
        }
    }
}
