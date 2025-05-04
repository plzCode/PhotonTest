using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Cue")]
public class AudioCue : ScriptableObject
{
    public AudioClip clip;
    public bool loop = false;
    public float volume = 1.0f;
}