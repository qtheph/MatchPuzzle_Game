using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private RectTransform cloudRect;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private float speed;
    [SerializeField] private float offSet;

    void Awake()
    {
        cloudRect = GetComponent<RectTransform>();
        Canvas canvasParent = GetComponentInParent<Canvas>();
        if (canvasParent != null) canvasRect = canvasParent.GetComponent<RectTransform>();
    }

    void Update()
    {
        HandleCloud();
    }

    private void HandleCloud()
    {
        float canvasWidth = canvasRect != null ? canvasRect.rect.width : 0f;
        float cloudWidth = cloudRect != null ? cloudRect.rect.width : 0f;

        float rightBound = (canvasWidth / 2f) + (cloudWidth / 2f);
        float leftBound = -(canvasWidth / 2f) - (cloudWidth / 2f);

        cloudRect.anchoredPosition += Vector2.right * speed * Time.deltaTime;
        if (cloudRect.anchoredPosition.x > rightBound + offSet)
        {
            cloudRect.anchoredPosition = new Vector2(leftBound - offSet, cloudRect.anchoredPosition.y);
        }
    }
}
