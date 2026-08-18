using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialModel
{
    private readonly IGameStorage storage;

    public TutorialModel(IGameStorage storage)
    {
        this.storage = storage;
    }

    public bool IsShowed(TutorialType tutorialType)
    {
        if (tutorialType == TutorialType.None) return false;
        return storage.GetInt(tutorialType.ToString() + "_Used", 0) == 1;
    }

    public void MarkShowed(TutorialType tutorialType)
    {
        if (tutorialType == TutorialType.None) return;
        storage.SetInt(tutorialType.ToString() + "_Used", 1);
        storage.Save();
    }
}
