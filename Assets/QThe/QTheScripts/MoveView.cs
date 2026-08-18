using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MoveView : MonoBehaviour
{
    [SerializeField] private GameObject NotificationObject;
    public void Init(TextMeshProUGUI moveTxt, LevelData levelData)
    {
        UpdateMove(moveTxt, levelData.moveCount);
    }
    public void UpdateMove(TextMeshProUGUI moveTxt, int remain)
    {
        if (remain < 0) return;
        if (remain == 5) StartCoroutine(ShowNotification());
        moveTxt.color = remain > 5 ? moveTxt.color = new Color32(154, 78, 37, 255) : moveTxt.color = Color.red;

        moveTxt.text = remain.ToString();
    }
    IEnumerator ShowNotification()
    {
        NotificationObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        NotificationObject.SetActive(false);
    }
}
