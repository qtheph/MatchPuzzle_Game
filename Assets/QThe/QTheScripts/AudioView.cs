using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BgMusic
{
    Map,
    Ingame,
}
public class AudioView : MonoBehaviour
{
    [SerializeField] private AudioSource audioInGame;
    [SerializeField] private AudioSource audioMap;
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
    public void PlayMusic(BgMusic type)
    {
        GetBGMusic(type)?.Play();
    }

    public void StopMusic(BgMusic type)
    {
        GetBGMusic(type)?.Stop();
    }

    public void MuteMusic(bool isMute)
    {
        audioInGame.mute = isMute;
        audioMap.mute = isMute;
    }

    public void MuteSound(bool isMute)
    {
        audioSFX.mute = isMute;
    }

    private AudioSource GetBGMusic(BgMusic type)
    {
        switch (type)
        {
            case BgMusic.Map:
                return audioMap;
            case BgMusic.Ingame:
                return audioInGame;
            default:
                return null;
        }
    }
}
