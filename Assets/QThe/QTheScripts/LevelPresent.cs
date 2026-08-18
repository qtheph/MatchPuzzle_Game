using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelPresent
{
    public event Action OnSkipOrContinueLevel;
    public event Action<LevelData> OnGenerateLevel;
    private readonly List<LevelData> levelDataList;


    private readonly LevelModel levelModel;
    private readonly LevelView levelView;
    private readonly TargetGroupView targetGroupView;
    private readonly MoveView moveView;
    private readonly WinPanelView winPanelView;
    private readonly LosePanelView losePanelView;
    private readonly BucketManagerView bucketManagerView;
    private readonly AudioView audioView;
    private readonly SettingView settingView;
    private readonly IGameStorage storage;
    private bool isTutorialMode = false;


    public LevelPresent(List<LevelData> levelDataList,
       LevelView levelView,
       TargetGroupView targetGroupView,
       MoveView moveView,
       WinPanelView winPanelView,
       LosePanelView losePanelView,
       BucketManagerView bucketManagerView,
       AudioView audioView,
        SettingView settingView,

    IGameStorage storage)
    {
        this.levelDataList = levelDataList;
        this.levelView = levelView;
        this.targetGroupView = targetGroupView;
        this.moveView = moveView;
        this.winPanelView = winPanelView;
        this.losePanelView = losePanelView;
        this.bucketManagerView = bucketManagerView;
        this.audioView = audioView;
        this.settingView = settingView;

        this.storage = storage;
        levelModel = new LevelModel(storage);
    }

    public void Init()
    {
        if (!storage.HasKey("FirstGameInit"))
        {
            levelModel.SetPrefLevelState(0, LevelState.Unlocked);

            List<ButtonLevel> btnLevelList = levelView.GetButtonLevelList;
            float targetY = levelView.GetButtonPos(btnLevelList[0].GetComponent<RectTransform>());
            levelModel.SetPrefScrollBGPosY(targetY);

            storage.SetInt("FirstGameInit", 1);
            storage.Save();
        }

        levelView.Init();

        SetLevelIndex();

        UpdateLevelButtonsState();

        levelView.OnPlayBoardTargetShow += ShowPlayBoardTarget;
        levelView.OnPlayBtnClicked += TakeLevelData;
        levelView.OnClickClosePlayTargetBoard += ClosePlayTargetBoard;

    }

    private void SetScrollBackgroundPosition(float targetY)
    {
        levelView.ViewButton(targetY);
    }

    public void EnableScrollVerticle(bool isVerticle)
    {
        levelView.GetMapUI.GetComponent<ScrollRect>().vertical = isVerticle;
    }

    private void SetLevelIndex()
    {
        Debug.Log("Chạy vào setlevelindex");
        List<ButtonLevel> btnLevelList = levelView.GetButtonLevelList;
        for (int i = 0; i < btnLevelList.Count; i++)
        {

            if (i < levelDataList.Count)
            {
                levelDataList[i].levelIndex = i;
                btnLevelList[i].SetUp(i);
            }
        }
    }

    public void UpdateLevelButtonsState()
    {
        List<ButtonLevel> btnLevelList = levelView.GetButtonLevelList;
        for (int i = 0; i < btnLevelList.Count; i++)
        {
            if (i < levelDataList.Count)
            {
                LevelState levelState = levelModel.GetPrefLevelState(i);
                btnLevelList[i].UpdateUI(levelState);
            }
            else
            {
                btnLevelList[i].UpdateUI(LevelState.Locked);
            }
        }
    }
    public void CompletedLevel(int levelIndex)
    {
        List<ButtonLevel> btnLevelList = levelView.GetButtonLevelList;
        levelModel.SetPrefLevelState(levelIndex, LevelState.Completed);
        if (levelIndex + 1 < levelDataList.Count)
        {
            if (levelModel.GetPrefLevelState(levelIndex + 1) == LevelState.Locked)
            {
                levelModel.SetPrefLevelState(levelIndex + 1, LevelState.Unlocked);
                float targetY = levelView.GetButtonPos(btnLevelList[levelIndex + 1].GetComponent<RectTransform>());
                SetScrollBackgroundPosition(targetY);
                levelModel.SetPrefScrollBGPosY(targetY);
            }
        }

        UpdateLevelButtonsState();
    }

    private void TakeLevelData(int index)
    {
        if (index < 0 || index >= levelDataList.Count) return;
        LevelData levelData = levelDataList[index];
        audioView.StopMusic(BgMusic.Map);
        levelView.ShowPlayBoard(false);
        OnGenerateLevel?.Invoke(levelData);
    }

    private void ShowPlayBoardTarget(int index)
    {
        LevelData levelData = levelDataList[index];
        if (levelData != null)
        {
            targetGroupView.InitTargetItem(levelData.targets, levelView.GetParent);
            moveView.Init(levelView.GetMoveTxt, levelData);
            levelView.ShowPlayBoard(true);
        }
    }

    private void ClosePlayTargetBoard()
    {
        if (isTutorialMode) return;
        levelView.ShowPlayBoard(false);
    }

    public void SetTutorialMode(bool isTutorialMode)
    {
        this.isTutorialMode = isTutorialMode;
    }

    public void ShowUIGame()
    {
        levelView.InGameUIShow(true);
    }

    public void HandleSkipGameLose()
    {
        losePanelView.LosePanelShow(false);
        bucketManagerView.UpdateCoinUI(bucketManagerView.GetBucket());
        levelView.MapUIShow(true);
        OnSkipOrContinueLevel?.Invoke();
    }
    public void HandleContinueGameWin()
    {
        winPanelView.WinPanelShow(false);
        bucketManagerView.UpdateCoinUI(bucketManagerView.GetBucket());
        levelView.MapUIShow(true);
        OnSkipOrContinueLevel?.Invoke();
    }
}
