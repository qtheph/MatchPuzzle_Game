using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CheckMatch
{
    private int row;
    private int col;
    private const int Empty = -1;
    public CheckMatch(int row, int col)
    {
        this.row = row;
        this.col = col;
    }
    public List<Vector2Int> FindMatch(int[,] board)
    {
        HashSet<Vector2Int> set = new HashSet<Vector2Int>();
        //Check Col
        for (int r = 0; r < row; r++)
        {
            int c = 0;
            while (c < col)
            {
                int len = 1;
                while (c + len < col && board[r, c] != Empty && board[r, c + len] == board[r, c])
                {
                    len++;
                }
                if (len >= 3)
                {
                    for (int i = 0; i < len; i++)
                    {
                        set.Add(new Vector2Int(r, c + i));
                    }
                }
                c += len;
            }
        }
        //Check Row
        for (int c = 0; c < col; c++)
        {
            int r = 0;
            while (r < row)
            {
                int len = 1;
                while (r + len < row && board[r, c] != Empty && board[r + len, c] == board[r, c])
                {
                    len++;
                }
                if (len >= 3)
                {
                    for (int i = 0; i < len; i++)
                    {
                        set.Add(new Vector2Int(r + i, c));
                    }
                }
                r += len;
            }
        }
        return set.ToList();
    }
}
