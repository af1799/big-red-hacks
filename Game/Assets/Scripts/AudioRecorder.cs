using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

public class AudioRecorder : MonoBehaviour
{
    public static AudioRecorder Instance { get; private set; }
    public bool isReplaying;
    public AudioSource audioSource;
    public float clipStartTime = 0f;
    public float clipDuration = 2f;
    public bool isRecording = true;
    private List<AudioClip> audioClips = new List<AudioClip>();
    private int listMaxLength = 20;

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
        if (audioClips.Count < listMaxLength && isRecording)
        {
            audioClips.Add(clip);
        }
    }

    public void RemoveLastAudio()
    {
        if (audioClips.Count > 0)
        {
            audioClips.RemoveAt(audioClips.Count - 1);
        }
    }

    public void PlayAudio()
    {
        StartCoroutine(PlayClipsInSequence());
    }

    private IEnumerator PlayClipsInSequence()
    {
        isReplaying = true;

        for (int i = 0; i < audioClips.Count; i++)
        {
            AudioClip clip = audioClips[i];
            audioSource.clip = clip;
            audioSource.time = clipStartTime;
            audioSource.Play();

            yield return new WaitForSeconds(clipDuration);

            audioSource.Stop();
        }

        isReplaying = false;
    }

    public void Record()
    {
        if (isRecording)
        {
            isRecording = false;
        }
        else
        {
            isRecording = true;
        }
    }
}
