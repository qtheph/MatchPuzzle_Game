using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct BoosterItemReward
{
    public BoosterType boosterType;
    public Sprite booterSprite;
}

public class WinPanelView : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI winTxt;
    [SerializeField] private TextMeshProUGUI coinTxt;

    [SerializeField] private GameObject itemReward;
    [SerializeField] private Image itemRewardImg;
    [SerializeField] private TextMeshProUGUI itemTxt;
    public Button btnSkip;

    [SerializeField] private List<BoosterItemReward> boosterItemRewardList = new List<BoosterItemReward>();

    private List<string> messageTxtList = new List<string>()
    {
      "Sweet!",
      "Yummy!",
      "Tasty!",
      "Sugar Rush!",
      "Delicious!",
      "Divine!",
      "Sugarlicious!",
    };

    private List<string> notificationList = new List<string>();


    void Awake()
    {
        btnSkip.interactable = false;
        WinPanelShow(false);
    }

    public void WinPanelShow(bool isActive) => winPanel.SetActive(isActive);
    public void SetInteractBtn(bool isActive) => btnSkip.interactable = isActive;
    public void UpdateWinTxt()
    {

        if (notificationList.Count == 0)
        {
            notificationList.AddRange(messageTxtList);
        }

        int randomIdx = Random.Range(0, notificationList.Count);
        winTxt.text = notificationList[randomIdx];
        notificationList.RemoveAt(randomIdx);
    }

    public void UpdateCoinTxt(int amount)
    {
        coinTxt.text = $"+{amount}";
    }

    public void UpdateItem_Txt_Img(BoosterType targetType, int amount)
    {
        if (amount <= 0)
        {
            itemReward.SetActive(false);
            return;
        }

        itemTxt.text = $"+{amount}";

        foreach (var type in boosterItemRewardList)
        {
            if (type.booterSprite != null && type.boosterType == targetType)
            {
                itemRewardImg.sprite = type.booterSprite;
                break;
            }
        }
        itemReward.SetActive(true);
    }
}
