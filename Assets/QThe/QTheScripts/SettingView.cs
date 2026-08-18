using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : MonoBehaviour
{
    public event Action OnOpenPaneSetting;
    public event Action OnClosePaneSetting;
    public event Action OnMusicToggle;
    public event Action OnSoundToggle;
    [SerializeField] private GameObject PanelSetting;

    [SerializeField] private Button openBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button musicBtn;
    [SerializeField] private Button soundBtn;

    [SerializeField] private GameObject musicOn;
    [SerializeField] private GameObject musicOff;

    [SerializeField] private GameObject soundOn;
    [SerializeField] private GameObject soundOff;

    public void ShowOpenSettingBtn(bool isActive) => openBtn.gameObject.SetActive(isActive);
    public void Init()
    {
        openBtn.onClick.AddListener(() => OnOpenPaneSetting?.Invoke());
        closeBtn.onClick.AddListener(() => OnClosePaneSetting?.Invoke());
        musicBtn.onClick.AddListener(() => OnMusicToggle?.Invoke());
        soundBtn.onClick.AddListener(() => OnSoundToggle?.Invoke());
    }

    public void ShowPanelSetting(bool isActive)
    {
        PanelSetting.SetActive(isActive);
    }

    public void MusicToggle(bool isMusicOn)
    {
        musicOff.SetActive(isMusicOn);
        musicOn.SetActive(!isMusicOn);
    }

    public void SoundToggle(bool isSoundOn)
    {
        soundOff.SetActive(isSoundOn);
        soundOn.SetActive(!isSoundOn);
    }
}
