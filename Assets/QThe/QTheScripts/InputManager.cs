using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
public enum SwapDirection
{
    Left,
    Right,
    Up,
    Down
}
public class InputManager : MonoBehaviour
{
    public event Action<TileFoodView> OnFoodClicked;
    public event Action<TileFoodView, SwapDirection> OnSwapDirection;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private float distanceSwap = 0.5f;
    [SerializeField] LayerMask foodLayerMask;
    private Vector2 startPos;
    private TileFoodView tileFoodView;

    public void HandleInput()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject() || gameManager != null && gameManager.CurrState != GameState.Play)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(startPos);
            Collider2D hit = Physics2D.OverlapPoint(startPos, foodLayerMask);
            if (hit == null) return;
            if (hit.TryGetComponent(out TileFoodView foodView))
            {
                tileFoodView = foodView;
                Debug.Log(tileFoodView.name);
                return;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (tileFoodView == null) return;
            Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dragPos = endPos - startPos;
            if (dragPos.magnitude < distanceSwap)
            {
                OnFoodClicked?.Invoke(tileFoodView);
                tileFoodView = null;
                return;
            }

            SwapDirection direction;
            if (Math.Abs(dragPos.x) > math.abs(dragPos.y))
            {
                direction = dragPos.x > 0 ? SwapDirection.Right : SwapDirection.Left;
            }
            else
            {
                direction = dragPos.y > 0 ? SwapDirection.Up : SwapDirection.Down;
            }
            //Debug.Log(direction);
            OnSwapDirection?.Invoke(tileFoodView, direction);
            tileFoodView = null;
        }
    }
}
