using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGroupView : MonoBehaviour
{
    [SerializeField] private TargetItem targetItemPrefab;
    private List<TargetItem> targetItemList = new List<TargetItem>();

    public void InitTargetItem(Target[] targets, Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
        targetItemList.Clear();
        if (targets == null) return;

        foreach (var target in targets)
        {
            if (target.food == null) continue;

            TargetItem targetItemObj = Instantiate(targetItemPrefab, parent);
            targetItemObj.Init(target);
            targetItemList.Add(targetItemObj);
        }
    }
    public void UpdateTargetCount(int foodId, int count)
    {
        foreach (var targetItemObj in targetItemList)
        {
            if (targetItemObj.GetId == foodId)
            {
                targetItemObj.UpdateCount(count);
            }
        }
    }
    public RectTransform GetTargetRect(int foodId)
    {
        foreach (var targetItemObj in targetItemList)
        {
            if (targetItemObj.GetId == foodId)
            {
                return targetItemObj.GetComponent<RectTransform>();
            }
        }
        return null;
    }
}
