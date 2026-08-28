using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioView : MonoBehaviour
{
    [SerializeField] private AudioSource audioMusic;
    [SerializeField] private AudioSource audioSFX;

    [SerializeField] private List<AudioData> audioDataList = new List<AudioData>();
    private Dictionary<SFX, AudioData> mapSFX = new Dictionary<SFX, AudioData>();

    public void Init()
    {
        foreach (var audio in audioDataList)
        {
            if (mapSFX.ContainsKey(audio.id)) continue;
            mapSFX[audio.id] = audio;
        }
    }

    public void PlaySFX(SFX id)
    {
        if (mapSFX.TryGetValue(id, out AudioData audio))
        {
            audioSFX.PlayOneShot(audio.audioClip, audio.volume);
        }
    }
    public void PitchSFX(int value)
    {
        audioSFX.pitch += value;
    }
    public void ResetPitchSFX(int value)
    {
        audioSFX.pitch = value;
    }

    public void MuteMusic(bool isMute)
    {
        audioMusic.mute = isMute;
    }

    public void MuteSound(bool isMute)
    {
        audioSFX.mute = isMute;
    }

    public void PlayMusic(AudioClip audioClip)
    {
        if (audioMusic == null || audioClip == null) return;
        audioMusic.clip = audioClip;
        audioMusic.Play();
    }
    public void StopMusic()
    {
        if (audioMusic == null) return;
        audioMusic.Stop();
    }
}
