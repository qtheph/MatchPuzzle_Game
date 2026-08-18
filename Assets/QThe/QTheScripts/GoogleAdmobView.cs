using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class GoogleAdmobView : MonoBehaviour
{
    [SerializeField] private string idBanner_Android;
    [SerializeField] private string idInterstitial_Android;
    [SerializeField] private string idRewarded_Android;
    private BannerView bannerView;
    private RewardedAd rewardedAd;
    private InterstitialAd interstitialAd;

    #region Banner
    private void ShowBanner()
    {
        DestroyBanner();
        bannerView = new BannerView(idBanner_Android, AdSize.Banner, AdPosition.Bottom);
        BannerFunc();
    }
    public void LoadBanner()
    {
        ShowBanner();
        bannerView.LoadAd(new AdRequest());
    }
    private void DestroyBanner()
    {
        if (bannerView != null)
        {
            // Always destroy the banner view when no longer needed.
            bannerView.Destroy();
            bannerView = null;
        }
    }
    private void BannerFunc()
    {
        bannerView.OnBannerAdLoaded += () =>
    {
        // Raised when an ad is loaded into the banner view.
    };
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            // Raised when an ad fails to load into the banner view.
        };
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            // Raised when the ad is estimated to have earned money.
        };
        bannerView.OnAdImpressionRecorded += () =>
        {
            // Raised when an impression is recorded for an ad.
        };
        bannerView.OnAdClicked += () =>
        {
            // Raised when a click is recorded for an ad.
        };
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            // Raised when an ad opened full screen content.
        };
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            // Raised when the ad closed full screen content.
        };
    }
    #endregion
    #region  Rewarded
    public void LoadRewarded()
    {
        DestroyRewarded();
        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        RewardedAd.Load(idRewarded_Android, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                // The ad failed to load.
                Debug.LogError("Rewarded load failed: " + error);
                return;
            }
            Debug.Log("Ad Load successfully");
            // The ad loaded successfully.
            rewardedAd = ad;
            RewardedFunc();
        });
    }
    public void ShowRewarded(Action onSuccess)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                onSuccess?.Invoke();
            });
        }
        else
        {
            Debug.LogWarning("Quảng cáo Rewarded chưa sẵn sàng hoặc đang tải!");
            LoadRewarded();
        }
    }
    public void DestroyRewarded()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
    }
    public void RewardedFunc()
    {
        rewardedAd.OnAdPaid += (AdValue adValue) =>
    {
        // Raised when the ad is estimated to have earned money.
    };
        rewardedAd.OnAdImpressionRecorded += () =>
        {
            // Raised when an impression is recorded for an ad.
        };
        rewardedAd.OnAdClicked += () =>
        {
            // Raised when a click is recorded for an ad.
        };
        rewardedAd.OnAdFullScreenContentOpened += () =>
        {
            // Raised when the ad opened full screen content.
        };
        rewardedAd.OnAdFullScreenContentClosed += () =>
        {
            // Raised when the ad closed full screen content.
            Debug.Log("Người dùng đóng quảng cáo -> Tải sẵn bài tiếp theo");
            // Tải sẵn quảng cáo mới ngay sau khi người dùng tắt quảng cáo cũ để chuẩn bị cho lần bấm tiếp theo
            LoadRewarded();
        };
        rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            // Raised when the ad failed to open full screen content.
            Debug.LogError("Quảng cáo bị lỗi khi đang hiển thị: " + error);
            // Lỗi hiển thị thì cũng nên load lại cái mới
            LoadRewarded();
        };
    }
    #endregion

    #region Interstitial
    public void LoadInterstitial()
    {
        DestroyInterstitial();
        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        InterstitialAd.Load(idInterstitial_Android, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                // The ad failed to load.
                Debug.LogError("Interstitial load failed: " + error);
                return;
            }
            // The ad loaded successfully.
            interstitialAd = ad;
            InterstitialFunc();

        });
    }
    public void ShowInterstitial()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.LogWarning("Interstitial ad chưa sẵn sàng, tiến hành tải lại!");
            LoadInterstitial();
        }
    }
    private void DestroyInterstitial()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
    }
    private void InterstitialFunc()
    {
        interstitialAd.OnAdPaid += (AdValue adValue) =>
    {
        // Raised when the ad is estimated to have earned money.
    };
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            // Raised when an impression is recorded for an ad.
        };
        interstitialAd.OnAdClicked += () =>
        {
            // Raised when a click is recorded for an ad.
        };
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            // Raised when the ad opened full screen content.
        };
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            // Raised when the ad closed full screen content.
            Debug.Log("Người dùng đóng Interstitial -> Load bài mới");
            LoadInterstitial();
        };
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            // Raised when the ad failed to open full screen content.
            Debug.LogError("Interstitial lỗi hiển thị: " + error);
            LoadInterstitial();
        };
    }
    #endregion
}

