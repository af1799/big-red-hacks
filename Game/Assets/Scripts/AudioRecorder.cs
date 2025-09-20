using System.Collections.Generic;
using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    public static AudioRecorder Instance { get; private set; }
    public bool isReplaying;
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
        
    }


}
