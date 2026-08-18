using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFoodView : MonoBehaviour
{
    [SerializeField] private int row;
    [SerializeField] private int col;
    public int GetRow => row;
    public int GetCol => col;
    public void SetRow(int row) => this.row = row;
    public void SetCol(int col) => this.col = col;
    public void SetNewRowCol(int r, int c)
    {
        SetRow(r);
        SetCol(c);
    }
}
