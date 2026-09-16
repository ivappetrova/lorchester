using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip defaultClip;

        private void Awake()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }

        // Bind every UnityEvent to THIS method, everywhere, always.
        public void PlaySound()
        {
            if (audioSource != null && defaultClip != null)
                audioSource.PlayOneShot(defaultClip);
        }
    }
}