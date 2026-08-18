using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ReFillData
{
    public int id;
    public int spawnRow;
    public int spawnCol;
    public int targetRow;
    public int targetCol;
    public ReFillData(int id, int spawnRow, int spawnCol, int targetRow, int targetCol)
    {
        this.id = id;
        this.spawnRow = spawnRow;
        this.spawnCol = spawnCol;
        this.targetRow = targetRow;
        this.targetCol = targetCol;
    }
}
