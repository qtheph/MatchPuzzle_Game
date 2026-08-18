using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public enum BoosterType
{
    None,
    Hammer,
    Rocket,
    Sprinkle
}
public class ItemBoosterData
{
    private readonly Dictionary<BoosterType, int> itemBoosterCountMap = new Dictionary<BoosterType, int>();
    private readonly Dictionary<BoosterType, bool> itemBoosterUnlockMap = new Dictionary<BoosterType, bool>();
    private readonly IGameStorage storage;

    public ItemBoosterData(IGameStorage storage)
    {
        this.storage = storage;
    }

    public void Init()
    {
        itemBoosterCountMap[BoosterType.Hammer] = storage.GetInt("Hammer", 0);
        itemBoosterCountMap[BoosterType.Rocket] = storage.GetInt("Rocket", 0);
        itemBoosterCountMap[BoosterType.Sprinkle] = storage.GetInt("Sprinkle", 0);

        itemBoosterUnlockMap[BoosterType.Hammer] = storage.GetInt("Hammer_Unlocked", 0) == 1;
        itemBoosterUnlockMap[BoosterType.Rocket] = storage.GetInt("Rocket_Unlocked", 0) == 1;
        itemBoosterUnlockMap[BoosterType.Sprinkle] = storage.GetInt("Sprinkle_Unlocked", 0) == 1;
    }

    public bool IsUnlocked(BoosterType boosterType)
    {
        if (boosterType == BoosterType.None) return false;

        if (itemBoosterUnlockMap.TryGetValue(boosterType, out bool isUnlocked))
        {
            return isUnlocked;
        }
        return false;
    }

    public void UnlockBooster(BoosterType boosterType)
    {
        if (boosterType == BoosterType.None) return;
        itemBoosterUnlockMap[boosterType] = true;
        storage.SetInt(boosterType.ToString() + "_Unlocked", 1);
        storage.Save();
    }

    public int GetItemBooster(BoosterType boosterType)
    {
        return itemBoosterCountMap[boosterType];
    }

    public void AddItem(BoosterType boosterType, int amount)
    {
        if (boosterType == BoosterType.None) return;
        if (itemBoosterCountMap.ContainsKey(boosterType))
        {
            itemBoosterCountMap[boosterType] += amount;
            SaveItem(boosterType);
        }
        else
        {
            itemBoosterCountMap[boosterType] = 0;
            SaveItem(boosterType);
        }
    }

    public bool SendItem(BoosterType boosterType, int amount = 1)
    {
        if (boosterType == BoosterType.None) return false;
        if (itemBoosterCountMap.ContainsKey(boosterType) && itemBoosterCountMap[boosterType] >= amount)
        {
            itemBoosterCountMap[boosterType] -= amount;
            SaveItem(boosterType);
            return true;
        }
        return false;
    }

    public void SaveItem(BoosterType boosterType)
    {
        storage.SetInt(boosterType.ToString(), GetItemBooster(boosterType));
        storage.Save();
    }
}
