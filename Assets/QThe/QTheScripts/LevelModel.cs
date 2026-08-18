using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelState
{
    Locked,
    Unlocked,
    Completed
}
public class LevelModel
{
    private readonly IGameStorage storage;

    public LevelModel(IGameStorage storage)
    {
        this.storage = storage;
    }

    public LevelState GetPrefLevelState(int levelIndex)
    {
        int idState = storage.GetInt("Level_" + levelIndex, 0);
        return (LevelState)idState;
    }

    public void SetPrefLevelState(int levelIndex, LevelState levelState)
    {
        storage.SetInt("Level_" + levelIndex, (int)levelState);
        storage.Save();
    }

    public void SetPrefScrollBGPosY(float posY)
    {
        storage.SetFloat("ScrollBackGroundPosY", posY);
        storage.Save();
    }

    public float GetPrefScrollBGsPosY()
    {
        return storage.GetFloat("ScrollBackGroundPosY");
    }
}
