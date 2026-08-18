using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LosePanelView : MonoBehaviour
{
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI loseTxt;
    public Button btnBuyChanceCoin;
    public Button btnBuyChanceAds;
    public Button btnSkip;
    public Button btnClose;

    private List<string> messageTxtList = new List<string>()
    {
      "Out of Moves!",
      "So Close!",
      "Oops!",
      "Oh No!",
      "Meltdown!",
      "Sugar Crash!",
      "Not Sweet!",
    };

    private List<string> notificationList = new List<string>();


    void Awake()
    {
        btnSkip.interactable = false;
        LosePanelShow(false);
    }
    public void SetInteractBtn(bool isActive) => btnSkip.interactable = isActive;
    public void UpdateLoseTxt()
    {
        if (notificationList.Count == 0)
        {
            notificationList.AddRange(messageTxtList);
        }

        int randomIdx = Random.Range(0, notificationList.Count);
        loseTxt.text = notificationList[randomIdx];
        notificationList.RemoveAt(randomIdx);
    }

    public void LosePanelShow(bool isActive) => losePanel.SetActive(isActive);

}
