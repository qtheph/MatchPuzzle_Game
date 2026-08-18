using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoosterPresent
{
    public event Action<List<Vector2Int>> OnApplyBoosterEffect;
    public event Action OnBoosterAnimationComplete;
    private readonly AudioView audioView;
    private readonly ItemBoosterData itemBoosterData;
    private readonly ItemBoosterView itemBoosterView;
    private readonly ItemBoosterModel itemBoosterModel;
    private readonly GoogleAdmobView googleAdmobView;
    private BoosterType selectedBooster = BoosterType.None;
    private BoosterType tutorialBooster = BoosterType.None;
    private bool isTutorialBoosterMode = false;

    public ItemBoosterPresent(
        AudioView audioView,
        ItemBoosterData itemBoosterData,
        ItemBoosterModel itemBoosterModel,
        ItemBoosterView itemBoosterView,
        GoogleAdmobView googleAdmobView
        )
    {
        this.audioView = audioView;
        this.itemBoosterData = itemBoosterData;
        this.itemBoosterModel = itemBoosterModel;
        this.itemBoosterView = itemBoosterView;
        this.googleAdmobView = googleAdmobView;
    }
    public void Init()
    {
        itemBoosterView.GetHammerBtn.onClick.AddListener(() => OnBoosterClicked(BoosterType.Hammer));
        itemBoosterView.GetRocketBtn.onClick.AddListener(() => OnBoosterClicked(BoosterType.Rocket));
        itemBoosterView.GetSprinkleBtn.onClick.AddListener(() => OnBoosterClicked(BoosterType.Sprinkle));

        itemBoosterView.GetHammerBtnAds.onClick.AddListener(() => OnAdsBoosterClicked(BoosterType.Hammer));
        itemBoosterView.GetRocketBtnAds.onClick.AddListener(() => OnAdsBoosterClicked(BoosterType.Rocket));
        itemBoosterView.GetSprinkleBtnAds.onClick.AddListener(() => OnAdsBoosterClicked(BoosterType.Sprinkle));
        RefreshUI();

    }
    public void SetBoosterCanSelectInTutorial(BoosterType boosterType)
    {
        tutorialBooster = boosterType;
    }
    public void SetTutorialMode(bool isTutorialMode)
    {
        isTutorialBoosterMode = isTutorialMode;
    }
    public void RefreshUI()
    {
        UpdateBoosterState(BoosterType.Hammer);
        UpdateBoosterState(BoosterType.Rocket);
        UpdateBoosterState(BoosterType.Sprinkle);
    }
    private void UpdateBoosterState(BoosterType type)
    {
        int remain = itemBoosterData.GetItemBooster(type);
        bool isUnlocked = itemBoosterData.IsUnlocked(type);
        itemBoosterView.HandleBoosterBtnInterac(type, remain, isUnlocked);
    }
    private readonly Dictionary<int, BoosterType> levelUnlockedBoosterMap = new Dictionary<int, BoosterType>()
    {
        {2,BoosterType.Hammer},
        {3,BoosterType.Rocket},
        {4,BoosterType.Sprinkle}
    };

    public void CheckUnlockedLevel(int currLevel)
    {
        foreach (var levelUnlockedBooster in levelUnlockedBoosterMap)
        {
            if (currLevel >= levelUnlockedBooster.Key)
                if (!itemBoosterData.IsUnlocked(levelUnlockedBooster.Value))
                {
                    itemBoosterData.UnlockBooster(levelUnlockedBooster.Value);
                    itemBoosterData.AddItem(levelUnlockedBooster.Value, 2);
                }
        }
        RefreshUI();
    }
    private void OnBoosterClicked(BoosterType type)
    {
        if (!itemBoosterData.IsUnlocked(type)) return;

        if (isTutorialBoosterMode)
        {
            if (tutorialBooster != type) return;
        }

        if (selectedBooster == type)
        {
            DeselectBooster();
            return;
        }
        SelectBooster(type);
    }
    private void OnAdsBoosterClicked(BoosterType type)
    {
        googleAdmobView.ShowRewarded(() =>
        {
            itemBoosterData.AddItem(type, 1);
            RefreshUI();
        });
    }
    private void SelectBooster(BoosterType type)
    {
        if (selectedBooster != BoosterType.None)
        {
            itemBoosterView.AnimateBoosterBtn(selectedBooster, 1.0f);
        }

        selectedBooster = type;
        itemBoosterView.AnimateBoosterBtn(selectedBooster, 1.2f);
        itemBoosterView.ShowItemOverPlay(true);
    }
    public void DeselectBooster()
    {
        if (selectedBooster != BoosterType.None)
        {
            itemBoosterView.AnimateBoosterBtn(selectedBooster, 1.0f);
            selectedBooster = BoosterType.None;
        }
        itemBoosterView.ShowItemOverPlay(false);
    }
    public bool IsBoosterSelected() => selectedBooster != BoosterType.None;
    public void ExecuteBoosterAt(int r, int c, int[,] board, int rows, int cols, Func<int, int, Vector3> getWorldPos)
    {
        if (selectedBooster == BoosterType.None) return;

        BoosterType currentBooster = selectedBooster;

        switch (currentBooster)
        {
            case BoosterType.Hammer:
                audioView.PlaySFX(SFX.Hammer);
                break;
            case BoosterType.Rocket:
                audioView.PlaySFX(SFX.Rocket);
                break;
            case BoosterType.Sprinkle:
                audioView.PlaySFX(SFX.Sprinkle);
                break;
        }

        if (!itemBoosterData.SendItem(currentBooster, 1))
        {
            DeselectBooster();
            return;
        }
        RefreshUI();

        // 1. Tính toán danh sách cell bị tác động từ Model
        List<Vector2Int> affectedCells = itemBoosterModel.GetItemBoosterAffectedCell(board, r, c, rows, cols, currentBooster);

        // 2. Chạy Animation View
        Vector3 originPos = getWorldPos(r, c); // Cần thiết lập tọa độ nếu hiển thị ở vị trí cụ thể
        List<Vector3> targetposList = new List<Vector3>();
        if (getWorldPos != null)
        {
            foreach (var cell in affectedCells)
            {
                targetposList.Add(getWorldPos(cell.x, cell.y));
            }
        }
        itemBoosterView.AnimateBoosterAffect(currentBooster, originPos, targetposList,
            onRemove: () =>
            {
                OnApplyBoosterEffect?.Invoke(affectedCells);
                audioView.PlaySFX(SFX.Match);
            },
            onComplete: () =>
            {
                DeselectBooster();
                OnBoosterAnimationComplete?.Invoke();
            });
    }
}
