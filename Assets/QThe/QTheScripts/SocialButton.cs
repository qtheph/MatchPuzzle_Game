using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocialButton : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private string url;
    void Awake()
    {
        if (btn == null) btn = GetComponent<Button>();
        btn.onClick.AddListener(() => OpenPage());
    }
    private void OpenPage()
    {
        if (string.IsNullOrWhiteSpace(url)) return;
        Application.OpenURL(url);
    }
}
