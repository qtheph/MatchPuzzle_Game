using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPresent
{
    public CloudView cloudView;
    private float rightBound;
    private float leftBound;

    public CloudPresent(CloudView cloudView)
    {
        this.cloudView = cloudView;
        HandleCloud();
    }

    private void HandleCloud()
    {
        float canvasWidth = cloudView.GetCanvasWidth();
        float cloudWidth = cloudView.GetCloudWidth();

        rightBound = (canvasWidth / 2f) + (cloudWidth / 2f);
        leftBound = -(canvasWidth / 2f) - (cloudWidth / 2f);
    }

    public void OnUpdate()
    {
        if (cloudView == null) return;
        cloudView.MoveRight(cloudView.Speed);
        if (cloudView.GetPositionX() > rightBound + cloudView.OffSet)
        {
            cloudView.SetPositionX(leftBound - cloudView.OffSet);
        }
    }
}
