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
        float offset = cameraFitBoardView.Offset;
        cameraFitBoardView.SetCamera(cameraFitBoardModel.TargetOrtho(offset));
    }
}
