using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFitBoardPresent
{
    private readonly CameraFitBoardModel cameraFitBoardModel;
    private readonly CameraFitBoardView cameraFitBoardView;
    public CameraFitBoardPresent(CameraFitBoardModel cameraFitBoardModel, CameraFitBoardView cameraFitBoardView)
    {
        this.cameraFitBoardModel = cameraFitBoardModel;
        this.cameraFitBoardView = cameraFitBoardView;
    }
    public void HandleOrtho()
    {
        float minZoom = cameraFitBoardView.MinZoom;
        cameraFitBoardView.SetCamera(cameraFitBoardModel.TargetOrtho(minZoom));
    }
}
