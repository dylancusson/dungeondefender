using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer AudioMixer;

    public void SetMasterVolume(float level)
    {
        AudioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSoundFXVolume(float level)
    {
        AudioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        AudioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
    }
}
