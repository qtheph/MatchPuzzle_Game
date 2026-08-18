using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenPresent
{
    private readonly LoadingStartScreenView loadingStartScreenView;
    private readonly LoadingInGameScreenView loadingInGameScreenView;
    private readonly ICoroutineRunner coroutineRunner;

    public LoadingScreenPresent(LoadingStartScreenView loadingScreenView, LoadingInGameScreenView loadingInGameScreenView, ICoroutineRunner coroutineRunner)
    {
        this.loadingStartScreenView = loadingScreenView;
        this.loadingInGameScreenView = loadingInGameScreenView;
        this.coroutineRunner = coroutineRunner;
    }

    public void HandleLoadingStart(Action onComplete)
    {
        coroutineRunner.StartCoroutine(LoadingStartScreenRoutine(onComplete));
    }
    public void HandleLoadingInGame(Action onComplete)
    {
        loadingInGameScreenView.Show();
        // Action hideComplete = null;
        // hideComplete = () =>
        // {
        //     loadingInGameScreenView.OnComplete -= hideComplete;
        //     onComplete?.Invoke();
        // };
        onComplete?.Invoke();
    }
    IEnumerator LoadingStartScreenRoutine(Action onComplete)
    {
        loadingStartScreenView.ShowLoadingScreen(true);

        loadingStartScreenView.Fill(0f);
        loadingStartScreenView.SetLoadingTxt(0f);
        yield return new WaitForSeconds(0.25f);

        loadingStartScreenView.Fill(0.1f);
        loadingStartScreenView.SetLoadingTxt(10f);
        yield return new WaitForSeconds(0.5f);

        loadingStartScreenView.Fill(0.3f);
        loadingStartScreenView.SetLoadingTxt(30f);
        yield return new WaitForSeconds(0.5f);

        loadingStartScreenView.Fill(0.7f);
        loadingStartScreenView.SetLoadingTxt(70f);
        yield return new WaitForSeconds(0.5f);

        loadingStartScreenView.Fill(1f);
        loadingStartScreenView.SetLoadingTxt(100f);
        yield return new WaitForSeconds(0.25f);

        loadingStartScreenView.ShowLoadingScreen(false);
        loadingStartScreenView.Fill(0f);
        onComplete?.Invoke();
    }
}
