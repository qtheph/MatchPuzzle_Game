using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingModel
{
    public bool IsMusicOn { get; private set; } = true;
    public bool IsSoundOn { get; private set; } = true;
    public void ToogleMusic()
    {
        IsMusicOn = !IsMusicOn;
    }
    public void ToogleSound()
    {
        IsSoundOn = !IsSoundOn;
    }
}
