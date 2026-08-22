using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BoardModel
{

    private LevelData levelData;
    private CheckMatch checkMatch;

    private TargetManager targetManager;
    private MoveManager moveManager;

    private int[,] board;
    private int row;
    private int col;
    private const int Empty = -1;
    public BoardModel(LevelData levelData, int row, int col)
    {
        this.row = row;
        this.col = col;
        board = new int[row, col];
        this.levelData = levelData;
        checkMatch = new CheckMatch(row, col);
        targetManager = new TargetManager();
        moveManager = new MoveManager();
    }
    public int GetRow => row;
    public int GetCol => col;
    public int[,] GetBoard => board;
    public LevelData GetLevelData => levelData;
    public TargetManager GetTargetManager => targetManager;
    public MoveManager GetMoveManager => moveManager;
    public List<Vector2Int> MatchedList => checkMatch.FindMatch(board);


    public void InitBoardModel()
    {
        targetManager.InitTarget(levelData);
        moveManager.InitMove(levelData);
        MakeEmptyIdOnBoard();
        FillIdOnBoard();
    }

    public void InitBoardTutorial()
    {
        targetManager.InitTarget(levelData);
        moveManager.InitMove(levelData);
        MakeEmptyIdOnBoard();
        FillIdTurtorial();
    }

    private void FillIdTurtorial()
    {
        int[,] tutorialBoard = new int[,]
        {
{0,1,2,1,2},
{2,1,0,1,1},
{2,0,2,0,2},
{1,1,2,1,0},
{2,0,1,0,2}
    };

        int tutorialRow = tutorialBoard.GetLength(0);
        int tutorialCol = tutorialBoard.GetLength(1);

        for (int r = 0; r < tutorialRow; r++)
        {
            for (int c = 0; c < tutorialCol; c++)
            {
                SetTileId(r, c, tutorialBoard[r, c]);
            }
        }
    }
    public bool IsValidPosition(int r, int c)
    {
        return r >= 0 && r < row && c >= 0 && c < col;
    }
    public void RemoveIdMatched(List<Vector2Int> matchedList)
    {
        if (matchedList == null || matchedList.Count == 0) return;
        foreach (var pos in matchedList)
        {
            // Kiểm tra tọa độ hợp lệ trước khi gán
            if (IsValidPosition(pos.x, pos.y))
            {
                //int foodId = board[pos.x, pos.y];
                // if (foodId != Empty)
                // {
                //     targetManager.RemoveTargetAmount(foodId);
                // }
                board[pos.x, pos.y] = Empty;
            }
        }
        PrintBoard();
    }
    private void MakeEmptyIdOnBoard()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                board[i, j] = Empty;
            }
        }
        PrintBoard();
    }
    private void FillIdOnBoard()
    {
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                int id = GetIdNotMatch(r, c);
                SetTileId(r, c, id);
            }
        }
        PrintBoard();
    }
    public int GetFoodId(int r, int c)
    {
        if (IsValidPosition(r, c)) return board[r, c];
        return -1;
    }
    public List<ReFillData> ReFillIdOnBoard()
    {
        List<ReFillData> reFillDataList = new List<ReFillData>();
        for (int c = 0; c < col; c++)
        {
            int spawnRow = -1;
            for (int r = row - 1; r >= 0; r--)
            {
                if (board[r, c] != Empty) continue;
                int randomId = GetRandomId();
                SetTileId(r, c, randomId);
                reFillDataList.Add(new ReFillData(randomId, spawnRow, c, r, c));
                spawnRow--;
            }
        }
        PrintBoard();
        return reFillDataList;
    }
    private void SetTileId(int r, int c, int id)
    {
        board[r, c] = id;
    }
    private int GetRandomId()
    {
        return levelData.allowedFoods[Random.Range(0, levelData.allowedFoods.Count)].GetId;
    }
    public List<GravityData> ApplyGravityId()
    {
        List<GravityData> gravityDatas = new List<GravityData>();
        for (int c = 0; c < col; c++)
        {
            int toRow = row - 1;
            for (int r = row - 1; r >= 0; r--)
            {
                //Check empty id
                if (board[r, c] == Empty) continue;
                if (r != toRow)
                {
                    board[toRow, c] = board[r, c];
                    board[r, c] = Empty;

                    gravityDatas.Add(new GravityData(r, c, toRow, c));
                }
                toRow--;
            }
        }
        return gravityDatas;
    }

    private HashSet<int> HaveIdMatchOnFill(int row, int col)
    {
        HashSet<int> set = new HashSet<int>();
        //col
        if (col >= 2 && board[row, col - 1] != Empty && board[row, col - 1] == board[row, col - 2])
        {
            set.Add(board[row, col - 1]);
        }
        //row
        if (row >= 2 && board[row - 1, col] != Empty && board[row - 1, col] == board[row - 2, col])
        {
            set.Add(board[row - 1, col]);
        }
        return set;
    }
    private int GetIdNotMatch(int row, int col)
    {
        HashSet<int> set = HaveIdMatchOnFill(row, col);

        List<int> idNotMatch = new List<int>();
        foreach (FoodData food in levelData.allowedFoods)
        {
            if (set.Contains(food.GetId)) continue;
            idNotMatch.Add(food.GetId);
        }
        return idNotMatch[Random.Range(0, idNotMatch.Count)];
    }
    public void SwapId(int r1, int c1, int r2, int c2)
    {
        int temp = board[r1, c1];
        board[r1, c1] = board[r2, c2];
        board[r2, c2] = temp;
        PrintBoard();
    }
    private void PrintBoard()
    {
        StringBuilder s = new StringBuilder();
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                s.Append(board[i, j]).Append(" ");
            }
            s.AppendLine();
        }
        //Debug.Log(s.ToString());
    }

    public List<Vector2Int> GetHint()
    {
        List<Vector2Int> hintList = new List<Vector2Int>();
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                int hintId = board[r, c];
                Vector2Int originTile = new Vector2Int(r, c);


                //Check col left - right
                if (c < col - 1)
                {

                    SwapId(r, c, r, c + 1);
                    List<Vector2Int> currMatchedList = MatchedList;
                    if (currMatchedList != null && currMatchedList.Count > 0)
                    {
                        hintList.Add(originTile);
                        foreach (var pos in currMatchedList)
                        {
                            if (!hintList.Contains(pos) && board[pos.x, pos.y] == hintId)
                            {
                                hintList.Add(pos);
                            }
                        }
                        SwapId(r, c, r, c + 1);
                        if (hintList.Count > 1)
                        {
                            return hintList;
                        }
                        hintList.Clear();
                    }
                    else
                    {
                        SwapId(r, c, r, c + 1);
                    }
                }
                //check row up - down
                if (r < row - 1)
                {

                    SwapId(r, c, r + 1, c);
                    List<Vector2Int> currMatchedList = MatchedList;
                    if (currMatchedList != null && currMatchedList.Count > 0)
                    {
                        hintList.Add(originTile);
                        foreach (var pos in currMatchedList)
                        {
                            if (!hintList.Contains(pos) && board[pos.x, pos.y] == hintId)
                            {
                                hintList.Add(pos);
                            }
                        }
                        SwapId(r, c, r + 1, c); // Hoàn tác hoán đổi

                        if (hintList.Count > 1)
                        {
                            return hintList;
                        }

                        hintList.Clear();
                    }
                    else
                    {
                        SwapId(r, c, r + 1, c); // Hoàn tác hoán đổi
                    }
                }
            }
        }
        return hintList;
    }

}
