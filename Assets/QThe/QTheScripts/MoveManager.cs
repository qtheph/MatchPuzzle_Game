using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager
{
    public event Action<int> OnMoveUpdated;
    public int MoveCount { get; private set; }

    public void InitMove(LevelData levelData)
    {
        if (levelData == null || levelData.moveCount == 0)
        {
            Debug.LogError("Chưa khởi tạo move count");
            return;
        }
        MoveCount = levelData.moveCount;
    }
    public void UseMove()
    {
        if (MoveCount <= 0) return;
        MoveCount--;
        OnMoveUpdated?.Invoke(MoveCount);
    }
    public void AddMove(int amount)
    {
        if (MoveCount > 0) return;
        MoveCount += amount;
        OnMoveUpdated?.Invoke(MoveCount);
    }
    public bool EndOfMove()
    {
        return MoveCount <= 0;
    }
}
