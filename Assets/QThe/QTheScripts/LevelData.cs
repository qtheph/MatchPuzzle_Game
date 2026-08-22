using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Target
{
    public FoodData food;
    public int count;

}
public enum EnviromentType
{
    Forest,
    Candy,
    Cloudy,
    Castle
}

[CreateAssetMenu(fileName = "New Level", menuName = "Match3/LevelData")]
public class LevelData : ScriptableObject
{
    public TutorialType tutorialType = TutorialType.None;
    public int levelIndex;
    public int row;
    public int col;
    public float spacing = 0.57f;
    public float bigCellOffset = 2f;
    public List<FoodData> allowedFoods;
    public Target[] targets;
    public GameObject cellPrefab;
    public GameObject bigCell;
    public int moveCount;
    public int coinAmountReward;
    public BoosterType boosterTypeReward;
    public int boosterAmountReward;
    public EnviromentType enviromentType;
}
