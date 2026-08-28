using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerformView : MonoBehaviour
{
    public event Action OnYesBtnClicked;
    public event Action OnNoBtnClicked;
    [SerializeField] private GameObject performBoard;
    [SerializeField] private Button noBtn;
    [SerializeField] private Button yesBtn;

    void Awake()
    {
        noBtn.onClick.AddListener(() => OnNoBtnClicked?.Invoke());
        yesBtn.onClick.AddListener(() => OnYesBtnClicked?.Invoke());
    }
    public void ShowPerformBoard(bool isActive)
    {
        performBoard.SetActive(isActive);
    }
}
