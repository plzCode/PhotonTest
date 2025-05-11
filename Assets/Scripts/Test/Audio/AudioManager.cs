using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public Dictionary<string, AudioClip> sfxClips = new();
    public Dictionary<string, AudioClip> bgmClips = new();

    public PhotonView audioView;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
        audioView = GetComponent<PhotonView>();
        LoadAllClips();
    }

    void LoadAllClips()
    {
        AudioClip[] sfx = Resources.LoadAll<AudioClip>("Audio/SFX");
        foreach (var clip in sfx) sfxClips[clip.name] = clip;

        AudioClip[] bgm = Resources.LoadAll<AudioClip>("Audio/BGM");
        foreach (var clip in bgm) bgmClips[clip.name] = clip;
    }
    [PunRPC]
    public void PlaySFX(string clipName)
    {
        if (!PhotonNetwork.LocalPlayer.IsLocal) return;
        if (sfxClips.TryGetValue(clipName, out var clip))
            sfxSource.PlayOneShot(clip);
        else
            Debug.LogWarning($"SFX {clipName} not found.");
    }

    public void RPC_PlaySFX(string clipName)
    {
        audioView.RPC("PlaySFX", RpcTarget.All, clipName);
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
    [PunRPC]
    public void StopSFX()
    {
        if (sfxSource.isPlaying)
        {
            sfxSource.Stop();
        }
    }
    public void RPC_StopSFX()
    {
        audioView.RPC("StopSFX", RpcTarget.All);
    }

    // 특정 구간만 재생하는 메서드 추가
    public void PlaySFXLoop(string clipName)
    {
        AudioSource audioSource = AudioManager.Instance.sfxSource;
        AudioClip sfxClip = AudioManager.Instance.sfxClips[clipName]; // SFX 클립 이름

        audioSource.clip = sfxClip;
        audioSource.loop = true; // 루프 설정
        audioSource.Play();
    }
    [PunRPC]
    public void RPC_PlaySFXLoop(string clipName)
    {
        audioView.RPC("PlaySFXLoop", RpcTarget.All, clipName);
    }

    private IEnumerator PlaySegmentCoroutine(AudioClip clip, float startTime, float endTime)
    {
        sfxSource.clip = clip;
        sfxSource.time = startTime;
        sfxSource.Play();

        yield return new WaitForSeconds(endTime - startTime);

        sfxSource.Stop();
    }
}