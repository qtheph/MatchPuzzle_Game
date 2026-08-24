using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    public event Action<int> OnPlayBtnClicked;
    public event Action<int> OnPlayBoardTargetShow;
    public event Action OnClickClosePlayTargetBoard;

    [SerializeField] private RectTransform scrollBackground;
    [SerializeField] private List<ButtonLevel> buttonLevelList;
    [SerializeField] private GameObject playBoard;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button playBtn;
    [SerializeField] private RectTransform parent;
    [SerializeField] TextMeshProUGUI moveTxt;
    [SerializeField] private GameObject mapUI;
    [SerializeField] private GameObject inGameUI;

    private int selectedLevelIndex = -1;
    public List<ButtonLevel> GetButtonLevelList => buttonLevelList;
    public RectTransform GetBScrollBackground => scrollBackground;
    public RectTransform GetParent => parent;
    public TextMeshProUGUI GetMoveTxt => moveTxt;
    public GameObject GetMapUI => mapUI;

    public void ShowPlayBoard(bool isActive) => playBoard.SetActive(isActive);
    public void MapUIShow(bool isActive) => mapUI.SetActive(isActive);
    public void InGameUIShow(bool isActive) => inGameUI.SetActive(isActive);

    public void Init()
    {
        if (buttonLevelList == null || buttonLevelList.Count == 0)
        {
            buttonLevelList = new List<ButtonLevel>(GetComponentsInChildren<ButtonLevel>());
        }

        SignEvent();

        closeBtn.onClick.AddListener(() => OnClickClose());
        playBtn.onClick.AddListener(() => OnClickPlay());
    }
    private void SignEvent()
    {
        for (int i = 0; i < buttonLevelList.Count; i++)
        {
            buttonLevelList[i].OnSetIndexLevelClicked -= SelectedLevel;
            buttonLevelList[i].OnSetIndexLevelClicked += SelectedLevel;
        }
    }
    private void SelectedLevel(int levelIndex)
    {
        //có thể gen ra target ở đây
        selectedLevelIndex = levelIndex;
        OnPlayBoardTargetShow?.Invoke(levelIndex);
    }
    private void OnClickPlay()
    {
        if (selectedLevelIndex != -1)
        {
            OnPlayBtnClicked?.Invoke(selectedLevelIndex);
        }
    }
    private void OnClickClose()
    {
        OnClickClosePlayTargetBoard?.Invoke();
    }

    public float GetButtonPosY(RectTransform levelButton)
    {
        Vector3 btnPos = scrollBackground.InverseTransformPoint(levelButton.position);
        float targetY = -btnPos.y;
        return targetY;
    }
    public void ViewButton(float targetY)
    {
        if (scrollBackground == null) return;
        Vector2 currentAnchoredPos = scrollBackground.anchoredPosition;
        currentAnchoredPos.y = targetY;
        scrollBackground.anchoredPosition = currentAnchoredPos;
    }
}
