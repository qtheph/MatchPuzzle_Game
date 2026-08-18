using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct GravityData
{
    public int dropFromRow;

    public int dropFromCol;
    public int toRow;
    public int toCol;
    public GravityData(int dropFromRow, int dropFromCol, int toRow, int toCol)
    {
        this.dropFromRow = dropFromRow;
        this.dropFromCol = dropFromCol;
        this.toRow = toRow;
        this.toCol = toCol;
    }
}
