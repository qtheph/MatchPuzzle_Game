using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFitBoardView : MonoBehaviour
{
    [SerializeField] private float offSet;
    [SerializeField] private Camera mainCam;
    public float Offset => offSet;
    public void SetCamera(float targetOrtho)
    {
        if (mainCam != null)
        {
            mainCam.orthographicSize = targetOrtho;
        }
    }
}
