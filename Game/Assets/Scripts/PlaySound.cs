using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource audioSource;
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }
            else
            {
                Debug.LogWarning("AudioSource or AudioClip not assigned to PlayAudioOnTouch script on " + gameObject.name);
            }
        }
    }
}
