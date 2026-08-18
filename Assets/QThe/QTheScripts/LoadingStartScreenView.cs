using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingStartScreenView : MonoBehaviour
{
    [SerializeField] private Image fillImg;
    [SerializeField] private TextMeshProUGUI loadingTxt;
    [SerializeField] private GameObject loadingScreen;
    public void ShowLoadingScreen(bool isActive) => loadingScreen.SetActive(isActive);

    public void Fill(float value)
    {
        fillImg.fillAmount = value;
    }
    public void SetLoadingTxt(float value)
    {
        loadingTxt.text = $"{value}%";
    }
}
