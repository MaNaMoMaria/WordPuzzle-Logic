using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * ✨ ButtonEffects: Adds visual feedback (scaling) and audio feedback (SFX)
 * to UI buttons using Unity's EventSystems.
 */

namespace WordPuzzle.Prototype
{
    public class ButtonEffects : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("🎮 Feedback Settings")]
        [Tooltip("The scale multiplier when the button is pressed (e.g., 0.9 for shrinking)")]
        public float scaleFactor = 0.9f;

        [Header("🔊 Audio Settings")]
        [Tooltip("Drag the click sound effect clip here")]
        public AudioClip clickSound;

        private AudioSource audioSource;
        private Vector3 originalScale;

        void Start ()
        {
            // Store the initial scale to ensure accurate reset
            originalScale = transform.localScale;

            // Automatically setup an AudioSource component if one doesn't exist
            audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.playOnAwake = false;
        }

        /**
         * 👇 Triggered when the pointer/finger is pressed down on the button.
         */
        public void OnPointerDown (PointerEventData eventData)
        {
            // Apply the scale effect
            transform.localScale = originalScale * scaleFactor;

            // Play the click sound effect if assigned
            if (clickSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(clickSound);
            }
        }

        /**
         * 👆 Triggered when the pointer/finger is released.
         */
        public void OnPointerUp (PointerEventData eventData)
        {
            // Reset the scale to the original state
            transform.localScale = originalScale;
        }
    }
}