using UnityEngine;
using UnityEngine.Events;

// Purpose: Detects interaction (Grab), plays particles, and notifies the Manager.
public class LabPartTrigger : MonoBehaviour
{
    [Header("--- Settings ---")]
    [Tooltip("Particle system to play on interaction.")]
    public ParticleSystem hitParticles;
    [Tooltip("Should particles play only once per session?")]
    public bool playOnce = true;

    [Header("--- Events ---")]
    // Assign LabManager.OnPartInteracted(this) here in Editor if needed, 
    // OR the Manager will find this script automatically.
    public UnityEvent onInteracted; 

    private bool hasPlayed = false;

    // Call this function from your XR Grab Interactable -> "Select Entered" event
    public void OnGrabbed()
    {
        // 1. Play Particles
        if (hitParticles != null)
        {
            if (!playOnce || (playOnce && !hasPlayed))
            {
                // Move particles to this object's position
                hitParticles.transform.position = transform.position;
                hitParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                hitParticles.Play();
                hasPlayed = true;
            }
        }

        // 2. Add Haptics here later (e.g., controller.SendHapticImpulse)

        // 3. Notify the System
        onInteracted.Invoke();
        
        // Static Global call to Manager (Easiest way to link without dragging everywhere)
        if (LabManager.Instance != null)
        {
            LabManager.Instance.CheckStepCompletion(this.gameObject);
        }
    }
}