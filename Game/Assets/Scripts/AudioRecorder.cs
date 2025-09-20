using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.InputSystem;
using System.Linq;

public class AudioRecorder : MonoBehaviour
{
    public static AudioRecorder Instance { get; private set; }
    public bool isReplaying;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public float clipStartTime = 0f;
    public float maxClipDuration = 2.1f;
    public bool isRecording = true;
    private List<AudioClip> keyboardClips = new List<AudioClip>();
    private List<AudioClip> percussionClips = new List<AudioClip>();
    private int listMaxLength = 20;
    private float length1;
    private float length2;

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

    public void AddAudio(AudioClip clip, float type)
    {
        if (type == 1)
        {
            if (keyboardClips.Count < listMaxLength && isRecording)
            {
                keyboardClips.Add(clip);
            }
        }
        else if (type == 2)
        {
            if (percussionClips.Count < listMaxLength && isRecording)
            {
                percussionClips.Add(clip);
            }
        }
    }

    public void RemoveLastAudio()
    {
        if (length1 > length2)
        {
            keyboardClips.RemoveAt(keyboardClips.Count - 1);
        }
        else if (length1 == length2 && keyboardClips.Count != 0)
        {
            keyboardClips.RemoveAt(keyboardClips.Count - 1);
            percussionClips.RemoveAt(percussionClips.Count - 1);
        }
        else
        {
            percussionClips.RemoveAt(percussionClips.Count - 1);
        }
    }

    public void PlayAudio()
    {
        StartCoroutine(PlayClipsInSequence());
    }

    private IEnumerator PlayClipsInSequence()
    {
        isReplaying = true;
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < GetMaxLength().Count; i++)
        {
            if (i < keyboardClips.Count)
            {
                AudioClip clip = keyboardClips[i];
                audioSource1.clip = clip;
                audioSource1.time = clipStartTime;
                audioSource1.Play();
            }
            if (i < percussionClips.Count)
            {
                AudioClip clip = percussionClips[i];
                audioSource2.clip = clip;
                audioSource2.time = clipStartTime;
                audioSource2.Play();
            }

            yield return new WaitForSeconds(maxClipDuration);

            audioSource1.Stop();
            audioSource2.Stop();
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

    public List<AudioClip> GetMaxLength()
    {
        List<List<AudioClip>> allLists = new List<List<AudioClip>> { keyboardClips, percussionClips };
        List<AudioClip> longestList = allLists.OrderByDescending(l => l.Count).First();
        return longestList;
    }

}
