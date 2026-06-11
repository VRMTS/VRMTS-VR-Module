using UnityEngine;
using UnityEngine.Events;

// ==================================================
// File: LabItem.cs
// Purpose: Detects XR Grabs, Plays Particles, Notifies Controller
// ==================================================
public class LabItem : MonoBehaviour
{
    [Header("--- Settings ---")]
    [Tooltip("Assign the Particle System from the scene here.")]
    public ParticleSystem interactionParticles;
    
    [Tooltip("If true, particles play only the first time it is grabbed.")]
    public bool playOnce = true;

    private bool hasPlayed = false;

    // --------------------------------------------------------
    // CONNECT THIS TO: XR Grab Interactable -> Interactable Events -> Select Entered
    // --------------------------------------------------------
    public void OnGrabbed()
    {
        
         // DEBUG LINE 1: Did the event fire?
        Debug.Log($"[LabItem] OnGrabbed called on object: {gameObject.name}");
        
        // 1. Play Particles
        if (interactionParticles != null)
        {
            if (!playOnce || (playOnce && !hasPlayed))
            {
                // Move particles to this object
                interactionParticles.transform.position = transform.position;
                interactionParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                interactionParticles.Play();
                hasPlayed = true;
            }
        }

        // 2. Notify Controller
        if (LabController.Instance != null)
        {
             // DEBUG LINE 2: Talking to controller
            Debug.Log($"[LabItem] Notifying LabController about: {gameObject.name}");
            //LabController.Instance.OnItemInteracted(this);
            LabController.Instance.OnItemInteracted(this);
        }
        else
        {
             Debug.LogError("[LabItem] LabController Instance is NULL!");
        }
    }
}