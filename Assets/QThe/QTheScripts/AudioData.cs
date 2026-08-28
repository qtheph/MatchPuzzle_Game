using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX
{
    Match,
    Win,
    Lose,
    Hammer,
    Rocket,
    Sprinkle,
    Swipe,
    Undo,
    Click
}
[System.Serializable]
public class AudioData
{
    public SFX id;
    public AudioClip audioClip;


    [Range(0f, 1f)] public float volume = 1f;
}


