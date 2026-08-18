using UnityEngine;

public enum SortingLayerOrder
{
    BubbleUI = 1000,
    CloudUI = 1500,
    UI = 2000,
    LoadingScreenUI = 2500,
}
public class SetupSortingOrder : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private SortingLayerOrder sortingLayerOrder;
    [SerializeField] private int offSet;

    void Awake()
    {
        if (canvas == null) canvas = GetComponent<Canvas>();
        SetupCanvas();
    }
    private void SetupCanvas()
    {
        canvas.overrideSorting = true;
        canvas.sortingOrder = (int)sortingLayerOrder + offSet;
    }
}
