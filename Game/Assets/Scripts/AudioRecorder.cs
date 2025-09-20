using System.Collections.Generic;
using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    public static AudioRecorder Instance { get; private set; }
    List<AudioClip> audioClips = new List<AudioClip>();
    //bool isReplaying

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



}
