using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoosterView : MonoBehaviour
{
    [SerializeField] private Transform boosterItemParent;
    [SerializeField] private GameObject itemOverPlay;
    [SerializeField] private GameObject hammerPrefab;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private GameObject sprinklePrefab;
    [SerializeField] private LineRenderer lineRendererPrefab;

    [Header("Booster UI")]
    [SerializeField] private TextMeshProUGUI hammerTxt;
    [SerializeField] private TextMeshProUGUI rocketTxt;
    [SerializeField] private TextMeshProUGUI sprinkleTxt;
    [SerializeField] private Button hammerButton;
    [SerializeField] private Button rocketButton;
    [SerializeField] private Button sprinkleButton;
    [SerializeField] private Button hammerButtonAds;
    [SerializeField] private Button rocketButtonAds;
    [SerializeField] private Button sprinkleButtonAds;
    [SerializeField] private RectTransform hammerParentRect;
    [SerializeField] private RectTransform rocketParentRect;
    [SerializeField] private RectTransform sprinkleParentRect;

    [Header("Lock Objects")]
    [SerializeField] private GameObject hammerLockObj;
    [SerializeField] private GameObject rocketLockObj;
    [SerializeField] private GameObject sprinkleLockObj;

    [Header("Unlock Obj")]
    [SerializeField] private GameObject hammerUnlockObj;
    [SerializeField] private GameObject rocketUnlockObj;
    [SerializeField] private GameObject sprinkleUnlockObj;

    public Button GetHammerBtn => hammerButton;
    public Button GetRocketBtn => rocketButton;
    public Button GetSprinkleBtn => sprinkleButton;
    public Button GetHammerBtnAds => hammerButtonAds;
    public Button GetRocketBtnAds => rocketButtonAds;
    public Button GetSprinkleBtnAds => sprinkleButtonAds;

    public void ShowItemOverPlay(bool isActive) => itemOverPlay.SetActive(isActive);
    private void UpdateBoosterRemainUI(BoosterType boosterType, int remain)
    {
        switch (boosterType)
        {
            case BoosterType.Hammer:
                hammerTxt.text = remain > 0 ? remain.ToString() : "+";
                break;
            case BoosterType.Rocket:
                rocketTxt.text = remain > 0 ? remain.ToString() : "+";
                break;
            case BoosterType.Sprinkle:
                sprinkleTxt.text = remain > 0 ? remain.ToString() : "+";
                break;
        }
    }
    public void HandleBoosterBtnInterac(BoosterType boosterType, int remain, bool isUnlocked)
    {
        switch (boosterType)
        {
            case BoosterType.Hammer:
                if (hammerLockObj != null && hammerUnlockObj != null)
                {
                    hammerLockObj.SetActive(!isUnlocked);
                    hammerUnlockObj.SetActive(isUnlocked);
                }
                hammerButton.interactable = remain > 0;
                hammerButtonAds.interactable = remain <= 0;
                break;
            case BoosterType.Rocket:
                if (rocketLockObj != null)
                {
                    rocketLockObj.SetActive(!isUnlocked);
                    rocketUnlockObj.SetActive(isUnlocked);
                }
                rocketButton.interactable = remain > 0;
                rocketButtonAds.interactable = remain <= 0;
                break;
            case BoosterType.Sprinkle:
                if (sprinkleLockObj != null)
                {
                    sprinkleLockObj.SetActive(!isUnlocked);
                    sprinkleUnlockObj.SetActive(isUnlocked);
                }
                sprinkleButton.interactable = remain > 0;
                sprinkleButtonAds.interactable = remain <= 0;
                break;
        }
        UpdateBoosterRemainUI(boosterType, remain);
    }
    public void AnimateBoosterBtn(BoosterType boosterType, float size)
    {
        switch (boosterType)
        {
            case BoosterType.Hammer:
                hammerParentRect.DOScale(size, 0.25f);
                break;
            case BoosterType.Rocket:
                rocketParentRect.DOScale(size, 0.25f);
                break;
            case BoosterType.Sprinkle:
                sprinkleParentRect.DOScale(size, 0.25f);
                break;
        }
    }
    public void AnimateBoosterAffect(BoosterType boosterType, Vector3 originPos, List<Vector3> targetPosList, Action onRemove, Action onComplete)
    {
        switch (boosterType)
        {
            case BoosterType.Hammer:
                AnimateHammer(originPos, onRemove, onComplete);
                break;
            case BoosterType.Rocket:
                AnimateRocket(originPos, onRemove, onComplete);
                break;
            case BoosterType.Sprinkle:
                AnimateSprinkle(originPos, targetPosList, onRemove, onComplete);
                break;
        }
    }
    private void AnimateHammer(Vector3 targetPos, Action onRemove, Action onComplete)
    {
        Vector3 spawPos = targetPos + new Vector3(0, 0.5f, 0);
        GameObject hammerObj = Instantiate(hammerPrefab, spawPos, Quaternion.identity, boosterItemParent);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(hammerObj.transform.DOMove(targetPos, 0.2f)).SetEase(Ease.Linear);
        sequence.Join(hammerObj.transform.DORotate(new Vector3(0, 0, 35f), 0.2f));
        sequence.AppendCallback(() =>
        {
            Destroy(hammerObj);
            onRemove?.Invoke();
        });
        sequence.AppendInterval(2f);
        sequence.OnComplete(() =>
        {

            onComplete?.Invoke();
        });
    }
    private void AnimateRocket(Vector3 originPos, Action onRemove, Action onComplete)
    {
        Vector3 spawPos = originPos;
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right };
        float[] angles = { 90f, -90f, 180f, 0f };
        float flyDistance = 20f;

        List<GameObject> rocketList = new List<GameObject>();
        Sequence sequence = DOTween.Sequence();

        onRemove?.Invoke();

        for (int i = 0; i < 4; i++)
        {
            GameObject rocketObj = Instantiate(rocketPrefab, spawPos, Quaternion.Euler(0f, 0f, angles[i]), boosterItemParent);
            rocketList.Add(rocketObj);
            Vector3 destination = originPos + directions[i] * flyDistance;
            Tween rocketFly = rocketObj.transform.DOMove(destination, 2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(rocketObj);
            });
            sequence.Join(rocketFly);
        }
        sequence.AppendInterval(0f);

        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
    private void AnimateSprinkle(Vector3 originPos, List<Vector3> targetWorldPositions, Action onRemove, Action onComplete)
    {
        Vector3 spawnSprinklePos = originPos;
        GameObject sprinkleObj = Instantiate(sprinklePrefab, spawnSprinklePos, Quaternion.identity, boosterItemParent);

        List<LineRenderer> lineRendererList = new List<LineRenderer>();
        Sequence sequence = DOTween.Sequence();
        float laserDuration = 0.4f;
        foreach (var targetPos in targetWorldPositions)
        {
            if (lineRendererPrefab != null)
            {
                LineRenderer lineRenderer = Instantiate(lineRendererPrefab, boosterItemParent);
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, originPos);
                lineRenderer.SetPosition(1, originPos);
                lineRendererList.Add(lineRenderer);
                sequence.Join(
            DOTween.To(() => originPos, x => lineRenderer.SetPosition(1, x), targetPos, laserDuration)
                .SetEase(Ease.Linear)
        );
            }
        }
        sequence.AppendInterval(0.35f);

        sequence.AppendCallback(() =>
        {
            foreach (var line in lineRendererList)
            {
                Destroy(line.gameObject);
            }

            Destroy(sprinkleObj);
            onRemove?.Invoke();
        });

        sequence.AppendInterval(2f);
        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

}
