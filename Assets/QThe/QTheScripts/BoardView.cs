using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.UI;
public class BoardView : MonoBehaviour
{
    private LevelData levelData;
    private Dictionary<int, FoodData> map;
    private TileFoodView[,] tileFoodViews;

    private int[,] boardModel;
    private int row;
    private int col;
    private float spacing;
    private float bigCellOffset;

    [SerializeField] private float moveDuration = 0.15f;
    [SerializeField] private float dropDuration = 0.25f;

    [SerializeField] private Transform bigCellParent;
    [SerializeField] private Transform cellParent;
    [SerializeField] private Transform foodParent;

    [SerializeField] private GameObject flyToTargetVFX;

    [SerializeField] private RectTransform parentTarget;
    [SerializeField] private GameObject destroyFxPrefab;
    [SerializeField] private RectTransform parent;
    [SerializeField] TextMeshProUGUI moveTxt;

    public RectTransform GetParent => parent;
    public TextMeshProUGUI GetMoveTxt => moveTxt;
    public void InitBoardView(int[,] boardModel, LevelData levelData)
    {
        this.levelData = levelData;
        this.boardModel = boardModel;
        spacing = levelData.spacing;
        bigCellOffset = levelData.bigCellOffset;
        row = boardModel.GetLength(0);
        col = boardModel.GetLength(1);
        tileFoodViews = new TileFoodView[row, col];


        map = new Dictionary<int, FoodData>();
        foreach (FoodData food in levelData.allowedFoods)
        {
            map[food.GetId] = food;
        }

        CleanData();
        FillCellView();
        FillFoodView();
        SpawBigCellView();
    }

    public void AnimateHint(List<Vector2Int> hintPos)
    {
        if (hintPos != null && hintPos.Count > 0)
        {
            foreach (var pos in hintPos)
            {
                tileFoodViews[pos.x, pos.y].transform.DOScale(1.5f, 0.5f)
                .SetLoops(-1, loopType: LoopType.Yoyo);
            }
        }
    }
    public void StopAllAnimateHint()
    {
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                if (tileFoodViews[r, c] != null)
                {
                    tileFoodViews[r, c].transform.DOKill();
                    tileFoodViews[r, c].transform.localScale = Vector3.one;
                }
            }
        }
    }
    public void AnimateSwap(int r1, int c1, int r2, int c2, Action OnComplete)
    {
        TileFoodView foodA = tileFoodViews[r1, c1];
        TileFoodView foodB = tileFoodViews[r2, c2];
        Vector2 posA = foodA.transform.position;
        Vector2 posB = foodB.transform.position;
        foodA.transform.DOMove(posB, moveDuration).SetEase(Ease.Linear);
        foodB.transform.DOMove(posA, moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            tileFoodViews[r1, c1] = foodB;
            tileFoodViews[r2, c2] = foodA;

            foodA.SetNewRowCol(r2, c2);
            foodB.SetNewRowCol(r1, c1);
            OnComplete?.Invoke();
        });

    }
    public void AnimateGravityFood(List<GravityData> gravityDatas, Action OnComplete)
    {
        if (gravityDatas == null || gravityDatas.Count == 0)
        {
            OnComplete?.Invoke();
            return;
        }
        Sequence dropSquence = DOTween.Sequence();
        foreach (var pos in gravityDatas)
        {
            TileFoodView food = tileFoodViews[pos.dropFromRow, pos.dropFromCol];
            tileFoodViews[pos.toRow, pos.toCol] = food;
            tileFoodViews[pos.dropFromRow, pos.dropFromCol] = null;

            if (food == null) continue;
            food.SetNewRowCol(pos.toRow, pos.toCol);
            Vector3 targetPostion = GetWorldPos(pos.toRow, pos.toCol);
            dropSquence.Join(food.transform.DOMove(targetPostion, dropDuration)).SetEase(Ease.Linear);

        }
        dropSquence.OnComplete(() =>
          {
              OnComplete?.Invoke();
          });
    }

    public void AnimateRefillFood(List<ReFillData> reFillDatas, Action OnComplete)
    {
        if (reFillDatas == null || reFillDatas.Count == 0)
        {
            OnComplete?.Invoke();
            return;
        }
        Sequence refillSquence = DOTween.Sequence();
        foreach (var reFillData in reFillDatas)
        {
            Vector3 spawnPos = GetWorldPos(reFillData.spawnRow, reFillData.spawnCol);
            Vector3 targetPos = GetWorldPos(reFillData.targetRow, reFillData.targetCol);
            //Find Prefab 
            if (map.TryGetValue(reFillData.id, out FoodData foodData))
            {
                GameObject foodObj = Instantiate(foodData.GetPrefab, spawnPos, Quaternion.identity, foodParent);
                TileFoodView food = foodObj.GetComponent<TileFoodView>();
                food.SetRow(reFillData.targetRow);
                food.SetCol(reFillData.targetCol);
                tileFoodViews[reFillData.targetRow, reFillData.targetCol] = food;
                refillSquence.Join(food.transform.DOMove(targetPos, dropDuration)).SetEase(Ease.InQuad);
            }
        }
        refillSquence.OnComplete(() =>
        {
            OnComplete?.Invoke();
        });
    }

    private void CleanData()
    {
        foreach (Transform child in bigCellParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in cellParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in foodParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void FillFoodView()
    {
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                // if (boardView[r, c] != null) continue;
                int id = boardModel[r, c];
                Vector3 position = GetWorldPos(r, c);
                if (map.TryGetValue(id, out FoodData foodData))
                {
                    GameObject foodObj = Instantiate(foodData.GetPrefab, position, Quaternion.identity, foodParent);
                    TileFoodView food = foodObj.GetComponent<TileFoodView>();
                    food.SetRow(r);
                    food.SetCol(c);
                    tileFoodViews[r, c] = food;
                }
            }
        }
    }

    private void FillCellView()
    {
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                Vector3 position = GetWorldPos(r, c);
                Instantiate(levelData.cellPrefab, position, Quaternion.identity, cellParent);
            }
        }
    }

    private void SpawBigCellView()
    {
        float minX = tileFoodViews[0, 0].transform.position.x;
        float maxX = tileFoodViews[0, row - 1].transform.position.x;
        float minY = tileFoodViews[col - 1, 0].transform.position.y;
        float maxY = tileFoodViews[0, 0].transform.position.y;

        Debug.Log($"minX:{minX}, maxX:{maxX}");
        Debug.Log($"minY:{minY}, maxY:{maxY}");

        float centerX = (minX + maxX) / 2;
        float centerY = (minY + maxY) / 2;

        float cellWidth = (maxX - minX + 1) * spacing;
        float cellHeight = (maxY - minY + 1) * spacing;

        GameObject bigCell = Instantiate(levelData.bigCell, new Vector3(centerX, centerY, 0), Quaternion.identity, bigCellParent);
        bigCell.transform.localScale = new Vector3(cellWidth + bigCellOffset, cellHeight + bigCellOffset, 0);
    }

    public void RemoveFoodMatched(List<Vector2Int> matchedList)
    {
        if (matchedList == null || matchedList.Count == 0) return;
        foreach (var pos in matchedList)
        {
            if (tileFoodViews[pos.x, pos.y] == null) continue;
            Destroy(tileFoodViews[pos.x, pos.y].gameObject);
            PlayVFX(pos.x, pos.y);
            tileFoodViews[pos.x, pos.y] = null;
        }
    }
    public void RemoveTargetFoodAndFlyToTargetUI(int r, int c, RectTransform targetRect, Action onComplete)
    {
        StartCoroutine(AnimateFlyToTargetUI(r, c, targetRect, onComplete));
    }
    private IEnumerator AnimateFlyToTargetUI(int r, int c, RectTransform targetRect, Action onComplete)
    {
        TileFoodView tileFood = tileFoodViews[r, c];
        if (tileFood == null) yield break;


        GameObject flyObj = Instantiate(flyToTargetVFX, parentTarget);

        RectTransform flyRect = flyObj.GetComponent<RectTransform>();
        Image flyImg = flyObj.GetComponent<Image>();
        SpriteRenderer foodSR = tileFood.GetComponentInChildren<SpriteRenderer>();

        if (flyRect != null && foodSR != null)
        {
            flyImg.sprite = foodSR.sprite;
        }

        Vector3 spawnWorldPos = GetWorldPos(r, c);
        Vector2 screenPos = Camera.main.WorldToScreenPoint(spawnWorldPos);

        Canvas canvas = parentTarget.GetComponentInParent<Canvas>();
        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTarget, screenPos, uiCamera, out Vector2 startWorldPos))
        {
            flyRect.anchoredPosition = startWorldPos;
        }

        Debug.Log($"[Fly] spawnWorldPos(board): {spawnWorldPos}");

        Destroy(tileFood.gameObject);
        PlayVFX(r, c);
        tileFoodViews[r, c] = null;

        bool isFlyingDone = false;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(flyObj.transform.DOMove(targetRect.position, 0.5f).SetEase(Ease.InQuad));
        sequence.Join(flyObj.transform.DOScale(Vector3.one * 0.8f, 0.5f));
        sequence.OnComplete(() =>
        {
            isFlyingDone = true;
        });
        yield return new WaitUntil(() => isFlyingDone);

        targetRect.DOScale(1.2f, 0.15f).SetLoops(2, LoopType.Yoyo);
        Destroy(flyObj);
        onComplete?.Invoke();
    }

    private void PlayVFX(int r, int c)
    {
        Vector3 destroyPos = GetWorldPos(r, c);
        GameObject vfxObj = Instantiate(destroyFxPrefab, destroyPos, Quaternion.identity);
        Destroy(vfxObj, 2f);
    }

    public Vector3 GetWorldPos(int r, int c)
    {
        // Tính Offset để tâm của bàn cờ trùng với gốc tọa độ (0, 0)
        float offsetX = (col - 1) * spacing / 2f;
        float offsetY = (row - 1) * spacing / 2f;
        //Tính vị trí spawn
        float x = (c * spacing) - offsetX;
        float y = (-r * spacing) + offsetY;
        return new Vector3(x, y, 0);
    }
}
