using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public Dictionary<string, AudioClip> sfxClips = new();
    public Dictionary<string, AudioClip> bgmClips = new();

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        DontDestroyOnLoad(gameObject);
        LoadAllClips();
    }

    void LoadAllClips()
    {
        AudioClip[] sfx = Resources.LoadAll<AudioClip>("Audio/SFX");
        foreach (var clip in sfx) sfxClips[clip.name] = clip;

        AudioClip[] bgm = Resources.LoadAll<AudioClip>("Audio/BGM");
        foreach (var clip in bgm) bgmClips[clip.name] = clip;
    }

    public void PlaySFX(string clipName)
    {
        if (sfxClips.TryGetValue(clipName, out var clip))
            sfxSource.PlayOneShot(clip);
        else
            Debug.LogWarning($"SFX {clipName} not found.");
    }

    public void PlayBGM(string clipName, bool loop = true)
    {
        if (bgmClips.TryGetValue(clipName, out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.Play();
        }
        else
            Debug.LogWarning($"BGM {clipName} not found.");
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}