using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LoadingInGameScreenView : MonoBehaviour
{
    [SerializeField] private GameObject imgLoading;
    public void Show()
    {
        imgLoading.SetActive(true);
    }
    public void HideInAn()
    {
        imgLoading.SetActive(false);
    }
}
