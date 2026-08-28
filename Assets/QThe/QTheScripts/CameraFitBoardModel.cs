using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFitBoardModel
{
    private readonly int r;
    private readonly int c;
    private readonly float spacing;

    public CameraFitBoardModel(int r, int c, float spacing)
    {
        this.r = r;
        this.c = c;
        this.spacing = spacing;
    }

    public float TargetOrtho(float minZoom)
    {
        float width = c + (c - 1) * spacing;
        float height = r + (r - 1) * spacing;

        //Tính tỉ lệ khung hình
        float screenAspect = (float)Screen.width / Screen.height;
        float targetOrtho;

        //Tính toán ortho camera phù hợp
        if (screenAspect >= (width / height))
        {
            targetOrtho = height / 2f;
        }
        else
        {
            targetOrtho = (width / 2f) / screenAspect;
        }

        float boardSize = Mathf.Max(r, c);

        float zoomFactor = Mathf.Lerp(
              minZoom,
                1f,
             Mathf.InverseLerp(8f, 5f, boardSize)
         );
        return targetOrtho * zoomFactor;
    }
}
