using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudView : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private RectTransform cloudRect;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private float speed;
    [SerializeField] private float offSet;

    public float Speed => speed;
    public float OffSet => offSet;
    public float GetCanvasWidth() => canvasRect != null ? canvasRect.rect.width : 0f;
    public float GetCloudWidth() => cloudRect != null ? cloudRect.rect.width : 0f;
    public float GetPositionX() => cloudRect.anchoredPosition.x;

    void Awake()
    {
        cloudRect = GetComponent<RectTransform>();
        Canvas canvasParent = GetComponentInParent<Canvas>();
        if (canvasParent != null) canvasRect = canvasParent.GetComponent<RectTransform>();
    }

    public void MoveRight(float speed)
    {
        cloudRect.anchoredPosition += Vector2.right * speed * Time.deltaTime;
    }

    public void SetPositionX(float x)
    {
        cloudRect.anchoredPosition = new Vector2(x, cloudRect.anchoredPosition.y);
    }
}
