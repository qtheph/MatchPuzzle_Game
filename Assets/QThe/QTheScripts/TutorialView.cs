using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialView : MonoBehaviour
{
    [SerializeField] private List<TutorialData> tutorialDataList;

    public event Action<bool> OnTutorialShow;

    private readonly Queue<TutorialType> tutorialQueue = new Queue<TutorialType>();
    private TutorialData currentData;
    private Button registeredButton;

    public void ShowTutorial(TutorialType tutorialType)
    {
        tutorialQueue.Enqueue(tutorialType);

        if (currentData == null)
        {
            GoNextTutorial();
        }
    }

    private void GoNextTutorial()
    {
        if (tutorialQueue.Count == 0)
        {
            ClearCurrentTutorial();
            OnTutorialShow?.Invoke(false);
            return;
        }

        TutorialType nextType = tutorialQueue.Dequeue();
        TutorialData targetData = tutorialDataList.Find(t => t.tutorialType == nextType);

        if (targetData == null)
        {
            // Bỏ qua nếu không tìm thấy dữ liệu và xử lý cái tiếp theo
            GoNextTutorial();
            return;
        }

        currentData = targetData;
        SetupCurrentTutorial();
    }

    private void SetupCurrentTutorial()
    {
        if (currentData.tutorial != null)
        {
            currentData.tutorial.SetActive(true);
            OnTutorialShow?.Invoke(true);
        }

        if (currentData.targetBtn != null)
        {
            registeredButton = currentData.targetBtn;
            registeredButton.onClick.AddListener(OnTargetBtnClicked);
        }
    }

    private void OnTargetBtnClicked()
    {
        CompleteTutorial();
    }
    public void CompleteTutorialWithoutTargetBtn()
    {
        CompleteTutorial();
    }

    private void CompleteTutorial()
    {
        if (currentData == null) return;

        ClearCurrentTutorial();
        GoNextTutorial();
    }

    private void ClearCurrentTutorial()
    {
        if (registeredButton != null)
        {
            registeredButton.onClick.RemoveListener(OnTargetBtnClicked);
            registeredButton = null;
        }

        if (currentData != null && currentData.tutorial != null)
        {
            currentData.tutorial.SetActive(false);
        }

        currentData = null;
    }
}