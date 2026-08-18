using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "Match3/Food")]
public class FoodData : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private Sprite sprite;
    [SerializeField] GameObject prefab;
    public int GetId => id;
    public Sprite GetSprite => sprite;
    public GameObject GetPrefab => prefab;

}
