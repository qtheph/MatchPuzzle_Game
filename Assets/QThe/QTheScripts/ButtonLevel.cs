using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevel : MonoBehaviour
{

    public event Action<int> OnSetIndexLevelClicked;
    [SerializeField] private Button btnLevel;
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private GameObject lockedObj;
    [SerializeField] private GameObject unlockedObj;
    [SerializeField] private GameObject completedObj;
    [SerializeField] private int levelIndex;
    void Awake()
    {
        if (btnLevel == null) btnLevel = GetComponent<Button>();
        if (txtLevel == null) txtLevel = GetComponentInChildren<TextMeshProUGUI>();
        btnLevel.onClick.AddListener(() => OnBtnClick());

    }
    public void SetUp(int levelIndex)
    {
        this.levelIndex = levelIndex;
        SetTxt(levelIndex);
    }
    private void SetTxt(int levelIndex)
    {
        if (txtLevel == null) return;
        txtLevel.text = (levelIndex + 1).ToString();
    }
    private void OnBtnClick()
    {
        OnSetIndexLevelClicked?.Invoke(levelIndex);
    }
    public void UpdateUI(LevelState levelState)
    {
        lockedObj.SetActive(levelState == LevelState.Locked);
        unlockedObj.SetActive(levelState == LevelState.Unlocked);
        completedObj.SetActive(levelState == LevelState.Completed);
        btnLevel.interactable = levelState != LevelState.Locked;
    }
}
