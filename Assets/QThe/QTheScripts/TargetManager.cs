using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager
{
    public event Action<int, int> OnTargetUpdated;
    private Dictionary<int, int> map = new Dictionary<int, int>();
    public void InitTarget(LevelData levelData)
    {
        map.Clear();
        if (levelData == null || levelData.targets == null)
        {
            Debug.LogWarning("LevelData hoặc targets bị NULL!");
            return;
        }
        foreach (var target in levelData.targets)
        {
            if (!map.ContainsKey(target.food.GetId))
            {
                map[target.food.GetId] = target.count;
            }
            else
            {
                map[target.food.GetId] += target.count;
            }
        }
    }
    public void RemoveTargetAmount(int foodID)
    {
        if (map.ContainsKey(foodID))
        {
            if (map[foodID] > 0) map[foodID]--;
            OnTargetUpdated?.Invoke(foodID, map[foodID]);
        }
    }
    public bool IsAllTargetFinished()
    {
        foreach (int count in map.Values)
        {
            if (count > 0) return false;
        }
        return true;
    }
}
