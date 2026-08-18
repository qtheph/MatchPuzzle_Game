using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
public class GoogleAdmobPresenter
{
    private GoogleAdmobView googleAdmobView;
    private int levelCount = 0;

    public GoogleAdmobPresenter(GoogleAdmobView googleAdmobView)
    {
        this.googleAdmobView = googleAdmobView;
    }
    public void Init()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
                    {
                        // This callback is called once the MobileAds SDK is initialized.
                        googleAdmobView.LoadBanner();
                        googleAdmobView.LoadRewarded();
                        googleAdmobView.LoadInterstitial();
                    });
    }

    public void HandleShowInterstitial()
    {
        if (levelCount >= 3 && levelCount % 3 == 0)
        {
            googleAdmobView.ShowInterstitial();
        }
        levelCount++;
    }
}
