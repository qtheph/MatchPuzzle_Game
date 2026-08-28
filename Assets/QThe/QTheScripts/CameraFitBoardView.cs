using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFitBoardView : MonoBehaviour
{
    [SerializeField] private float minZoom;
    [SerializeField] private Camera mainCam;
    public float MinZoom => minZoom;
    public void SetCamera(float targetOrtho)
    {
        if (mainCam != null)
        {
            mainCam.orthographicSize = targetOrtho;
        }
    }
}
