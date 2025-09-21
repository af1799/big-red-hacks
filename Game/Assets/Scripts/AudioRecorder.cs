using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.UIElements;

public class AudioRecorder : MonoBehaviour
{
    public static AudioRecorder Instance { get; private set; }
    public bool isReplaying;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public float clipStartTime = 0f;
    public float maxClipDuration = 2.0f;
    public UI ui;
    public UI2 ui2;
    public UI3 ui3;
    public bool isRecording = true;
    private List<AudioClip> keyboardClips = new List<AudioClip>();
    private List<AudioClip> percussionClips = new List<AudioClip>();
    private List<AudioClip> guitarClips = new List<AudioClip>();
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

    public void AddAudio(AudioClip clip, float type)
    {
        if (type == 1)
        {
            if (keyboardClips.Count < listMaxLength && isRecording)
            {
                keyboardClips.Add(clip);
                ui.AddOneIcon();
            }
        }
        else if (type == 2)
        {
            if (percussionClips.Count < listMaxLength && isRecording)
            {
                percussionClips.Add(clip);
                ui2.AddOneIcon();
            }
        }
        else if (type == 3)
        {
            if (guitarClips.Count < listMaxLength && isRecording)
            {
                guitarClips.Add(clip);
                ui3.AddOneIcon();
            }
        }
    }

    public void RemoveLastAudio()
    {
        List<List<AudioClip>> equalLists = GetEqualLengthLists();
        if (equalLists != null)
        {
            if (keyboardClips.Count != 0)
            {
                keyboardClips.RemoveAt(keyboardClips.Count - 1);
                percussionClips.RemoveAt(percussionClips.Count - 1);
                guitarClips.RemoveAt(guitarClips.Count - 1);
            }
        }
        else
        {
            List<AudioClip> list = GetMaxLength();
            if (list.Count != 0)
            {
                if (list.Equals(percussionClips))
                {
                    ui2.RemoveOneIcon();
                } else if (list.Equals(keyboardClips))
                {
                    ui.RemoveOneIcon();
                } else if (list.Equals(guitarClips)) {
                    ui3.RemoveOneIcon();   
                }
               list.RemoveAt(list.Count - 1); 
            }
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
            if (i < guitarClips.Count)
            {
                AudioClip clip = guitarClips[i];
                audioSource3.clip = clip;
                audioSource3.time = clipStartTime;
                audioSource3.Play();
            }

            yield return new WaitForSeconds(maxClipDuration);

            audioSource1.Stop();
            audioSource2.Stop();
            audioSource3.Stop();
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
        List<List<AudioClip>> allLists = new List<List<AudioClip>> { keyboardClips, percussionClips, guitarClips };
        List<AudioClip> longestList = allLists.OrderByDescending(l => l.Count).First();
        return longestList;
    }
    
   public List<List<AudioClip>> GetEqualLengthLists()
    {
        int kCount = keyboardClips.Count;
        int pCount = percussionClips.Count;
        int gCount = guitarClips.Count;

        if (kCount == pCount && pCount == gCount && kCount > 0)
        {
            return new List<List<AudioClip>> { keyboardClips, percussionClips, guitarClips };
        }

        return null;
    }



}
