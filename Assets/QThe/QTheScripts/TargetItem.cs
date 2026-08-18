using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TargetItem : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI countTxt;
    [SerializeField] private GameObject done;

    public int GetId => id;

    public void Init(Target target)
    {
        done.SetActive(false);
        countTxt.gameObject.SetActive(true)
        ;
        id = target.food.GetId;
        img.sprite = target.food.GetSprite;

        UpdateCount(target.count);
    }

    public void UpdateCount(int count)
    {
        if (count <= 0)
        {
            countTxt.gameObject.SetActive(false);
            done.SetActive(true);
        }
        else
        {
            countTxt.text = $"x{count}";
        }
    }
}