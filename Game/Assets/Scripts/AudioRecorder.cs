using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class AudioRecorder : MonoBehaviour
{
    public static AudioRecorder Instance { get; private set; }
    public bool isReplaying;
    public AudioSource audioSource;
    private List<AudioClip> audioClips = new List<AudioClip>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddAudio(AudioClip clip)
    {
        audioClips.Add(clip);
    }

    public void PlayAudio()
    {
        StartCoroutine(PlayClipsInSequence());
    }

    private IEnumerator PlayClipsInSequence()
    {
        for (int i = 0; i < audioClips.Count; i++)
        {
            audioSource.clip = audioClips[i];
            audioSource.Play();
            yield return new WaitForSeconds(audioClips[i].length);
        }
    }
}
