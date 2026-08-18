using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoosterModel
{
    private const int Empty = -1;
    public List<Vector2Int> GetItemBoosterAffectedCell(int[,] board, int r, int c, int row, int col, BoosterType boosterType)
    {
        List<Vector2Int> affectedCell = new List<Vector2Int>();
        if (!IsValidPosition(r, c, row, col)) return affectedCell;
        switch (boosterType)
        {
            case BoosterType.Hammer:
                affectedCell.Add(new Vector2Int(r, c));
                if (IsValidPosition(r, c - 1, row, col)) affectedCell.Add(new Vector2Int(r, c - 1));
                if (IsValidPosition(r, c + 1, row, col)) affectedCell.Add(new Vector2Int(r, c + 1));
                if (IsValidPosition(r - 1, c, row, col)) affectedCell.Add(new Vector2Int(r - 1, c));
                if (IsValidPosition(r + 1, c, row, col)) affectedCell.Add(new Vector2Int(r + 1, c));
                break;

            case BoosterType.Rocket:
                for (int j = 0; j < col; j++) affectedCell.Add(new Vector2Int(r, j));
                for (int i = 0; i < row; i++) affectedCell.Add(new Vector2Int(i, c));
                break;

            case BoosterType.Sprinkle:
                int targetId = board[r, c];
                if (targetId == Empty) break;
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        if (board[i, j] == targetId)
                        {
                            affectedCell.Add(new Vector2Int(i, j));
                        }
                    }
                }
                break;
        }
        return affectedCell;
    }
    public bool IsValidPosition(int r, int c, int row, int col)
    {
        return r >= 0 && r < row && c >= 0 && c < col;
    }
}
