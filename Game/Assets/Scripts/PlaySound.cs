using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour
{
    public AudioSource audioSource;
    public float clipStartTime = 1f;
    public float clipDuration = 2f;
    private Coroutine playRoutine;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!audioSource.isPlaying)
            {
               if (audioSource != null && audioSource.clip != null)
                {
                    PlayClipSegment();
                }
                else
                {
                    Debug.LogWarning("AudioSource or AudioClip not assigned to PlayAudioOnTouch script on " + gameObject.name);
                } 
            }
        }
    }

    private void PlayClipSegment()
    {
        if (playRoutine != null)
            StopCoroutine(playRoutine);
        playRoutine = StartCoroutine(PlaySegment());
        AudioRecorder.Instance.AddAudio(audioSource.clip);
    }

    private IEnumerator PlaySegment()
    {
        audioSource.time = clipStartTime;
        audioSource.Play();
        yield return new WaitForSeconds(clipDuration);
        audioSource.Stop();
    }

}
