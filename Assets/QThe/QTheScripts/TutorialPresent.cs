using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPresent
{
    private readonly TutorialModel tutorialModel;
    private readonly TutorialView tutorialView;

    public TutorialPresent(TutorialModel tutorialModel, TutorialView tutorialView)
    {
        this.tutorialModel = tutorialModel;
        this.tutorialView = tutorialView;
    }

    public event Action<bool> OnTutorialShowMap
    {
        add => tutorialView.OnTutorialShow += value;
        remove => tutorialView.OnTutorialShow -= value;
    }

    public event Action<bool> OnTutorialShowInGame
    {
        add => tutorialView.OnTutorialShow += value;
        remove => tutorialView.OnTutorialShow -= value;
    }
    public void HandleTutorial(TutorialType tutorialType)
    {
        if (tutorialModel.IsShowed(tutorialType)) return;
        tutorialView.ShowTutorial(tutorialType);
        tutorialModel.MarkShowed(tutorialType);
    }

    public void SwapCloseTutorial()
    {
        tutorialView.CompleteTutorialWithoutTargetBtn();
    }

}
