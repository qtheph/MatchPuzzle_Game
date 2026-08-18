using System;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialType
{
    None,
    MapLevel1Btn,
    PlayTargetBoardLevel1Btn,
    InGameSwap,
    UnlockHammer,
    UnlockRocket,
    UnlockSprinkle
}

[Serializable]
public class TutorialData
{
    public TutorialType tutorialType;
    public GameObject tutorial;
    public Button targetBtn;
}
