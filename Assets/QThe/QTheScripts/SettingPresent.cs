using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPresent
{
    private readonly SettingModel settingModel;
    private readonly SettingView settingView;
    private readonly AudioView audioView;
    private readonly LevelView levelView;
    private bool isTutorialMode = false;

    public SettingPresent(SettingModel settingModel, SettingView settingView, AudioView audioView, LevelView levelView)
    {
        this.settingModel = settingModel;
        this.settingView = settingView;
        this.audioView = audioView;
        this.levelView = levelView;
    }
    public void Init()
    {
        settingView.Init();
        audioView.Init();
        settingView.OnClosePaneSetting += ClosePanelSetting;
        settingView.OnOpenPaneSetting += OpenPanelSetting;
        settingView.OnMusicToggle += HandleMusicToggle;
        settingView.OnSoundToggle += HandleSoundToggle;
    }
    private void HandleMusicToggle()
    {
        settingView.MusicToggle(settingModel.IsMusicOn);
        audioView.MuteMusic(settingModel.IsMusicOn);
        settingModel.ToogleMusic();
    }
    private void HandleSoundToggle()
    {
        settingView.SoundToggle(settingModel.IsSoundOn);
        audioView.MuteSound(settingModel.IsSoundOn);
        settingModel.ToogleSound();
    }
    private void ClosePanelSetting()
    {
        settingView.ShowPanelSetting(false);
    }
    private void OpenPanelSetting()
    {
        if (isTutorialMode) return;
        settingView.ShowPanelSetting(true);
        levelView.ShowPlayBoard(false);
    }
    public void SetTutorialMode(bool isTutorialMode)
    {
        this.isTutorialMode = isTutorialMode;
    }
}
