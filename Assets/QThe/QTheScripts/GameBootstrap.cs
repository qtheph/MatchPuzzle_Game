using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;

public class GameBootstrap : MonoBehaviour, ICoroutineRunner
{
    // Start is called before the first frame update
    [SerializeField] private GameManager gameManager;
    [SerializeField] private BoardView boardView;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private MoveView moveView;
    [SerializeField] private TargetGroupView targetGroupView;

    [Header("GoogleAds")]
    [SerializeField] private GoogleAdmobView googleAdmobView;
    private GoogleAdmobPresenter googleAdmobPresenter;

    [Header("Loading Screen")]
    [SerializeField] private LoadingStartScreenView loadingScreenView;
    [SerializeField] private LoadingInGameScreenView loadingInGameScreenView;
    private LoadingScreenPresent loadingScreenPresent;

    [Header("Tutorial")]
    [SerializeField] private TutorialView tutorialView;
    TutorialModel tutorialModel;
    private TutorialPresent tutorialPresent;

    [Header("Board")]
    private BoardPresent boardPresent;

    [Header("Level")]
    private LevelPresent levelPresent;
    [SerializeField] private List<LevelData> levelDataList;
    [SerializeField] private LevelView levelView;

    [Header("Enviroment")]
    [SerializeField] private EnviromentView enviromentView;

    [Header("Win Lose UI")]
    [SerializeField] private WinPanelView winPanelView;
    [SerializeField] private LosePanelView losePanelView;

    [Header("Bucket")]
    [SerializeField] private BucketManagerView bucketManager;
    [SerializeField] private int priceBuyChance;

    [Header("Audio")]
    [SerializeField] private AudioView audioView;

    [Header("Setting")]
    [SerializeField] private SettingView settingView;
    SettingPresent settingPresent;

    [Header("Booster UI")]
    [SerializeField] private ItemBoosterView itemBoosterView;

    [Header("Storage")]
    private IGameStorage storage = new PlayerPrefsStorage();

    private ItemBoosterData itemBoosterData;
    private ItemBoosterPresent itemBoosterPresent;
    private int levelIndex;

    [Header("Cloud")]
    [SerializeField] private List<CloudView> cloudViewList;
    private List<CloudPresent> cloudPresentList = new List<CloudPresent>();

    void Awake()
    {
        gameManager.ChangeState(GameState.Stop);

        googleAdmobPresenter = new GoogleAdmobPresenter(googleAdmobView);
        googleAdmobPresenter.Init();

        SettingModel settingModel = new SettingModel();
        settingPresent = new SettingPresent(settingModel, settingView, audioView, levelView);
        settingPresent.Init();

        tutorialModel = new TutorialModel(storage);
        tutorialPresent = new TutorialPresent(tutorialModel, tutorialView);
        tutorialPresent.OnTutorialShowMap += HandleTutorialShowMap;

        loadingScreenPresent = new LoadingScreenPresent(loadingScreenView, loadingInGameScreenView, this);
        loadingScreenPresent.HandleLoadingStart(() =>
        {
            HandleLoadingStartScreenCompleted();
        });

        bucketManager.UpdateCoinUI(bucketManager.GetBucket());

        itemBoosterData = new ItemBoosterData(storage);
        itemBoosterData.Init();

        ItemBoosterModel itemBoosterModel = new ItemBoosterModel();

        itemBoosterPresent = new ItemBoosterPresent(audioView, itemBoosterData, itemBoosterModel, itemBoosterView, googleAdmobView);
        itemBoosterPresent.Init();

        itemBoosterPresent.OnApplyBoosterEffect += HandleApplyBoosterEffect;
        itemBoosterPresent.OnBoosterAnimationComplete += HandleBoosterAnimationComplete;

        levelPresent = new LevelPresent(levelDataList, levelView, targetGroupView, moveView, winPanelView, losePanelView, bucketManager, audioView, settingView, storage);
        levelPresent.Init();
        levelPresent.OnGenerateLevel += HandleGenerateLevel;
        levelPresent.OnSkipOrContinueLevel += HandleShowLoadingInGameScreen;

        audioView.Init();

        InitCloud();
    }

    private void HandleGenerateLevel(LevelData levelData)
    {
        StopAllRoutine();

        loadingScreenPresent.HandleLoadingInGame(() =>
        {
            audioView.PlayMusic(BgMusic.Ingame);
        });

        BoardModel boardModel = new BoardModel(levelData, levelData.row, levelData.col);
        levelIndex = levelData.levelIndex;

        if (boardPresent != null)
        {
            boardPresent.OnFoodClickedForBooster -= HandleFoodClickedForBooster;
            boardPresent.OnCompletedLevel -= HandleLevelCompleted;
            boardPresent.OnNotCompletedLevel -= HandleLevelNotCompleted;
            boardPresent.CleanOldData();
            boardPresent = null;
        }

        boardPresent = new BoardPresent(boardModel,
          boardView,
          inputManager,
          enviromentView,
          moveView,
          targetGroupView,
          winPanelView,
          losePanelView,
          bucketManager,
          audioView,
          levelView,
          googleAdmobView,
          itemBoosterData,

          gameManager,
        this);

        boardPresent.OnFoodClickedForBooster += HandleFoodClickedForBooster;
        boardPresent.OnCompletedLevel += HandleLevelCompleted;
        boardPresent.OnNotCompletedLevel += HandleLevelNotCompleted;
        boardPresent.OnCloseInGameSwapTutorial += HandleCloseInGameSwap;

        levelPresent.ShowUIGame();
        SignUIEvents();
        itemBoosterPresent.CheckUnlockedLevel(levelData.levelIndex);
        itemBoosterPresent.RefreshUI();

        tutorialPresent.OnTutorialShowInGame -= HandleTutorialShowInGameSwap;
        tutorialPresent.OnTutorialShowInGame -= HandleTutorialShowInGameBooster;
        tutorialPresent.OnTutorialShowInGame -= HandleBoosterClickedTutorial;

        TutorialType type = boardModel.GetLevelData.tutorialType;
        bool isTutorialNotShowedYet = (type != TutorialType.None) && !tutorialModel.IsShowed(type);

        if (isTutorialNotShowedYet)
        {
            if (type == TutorialType.InGameSwap)
            {
                Debug.Log(type);
                boardPresent.InitTutorial(true);
                tutorialPresent.OnTutorialShowInGame += HandleTutorialShowInGameSwap;
            }
            else
            {
                boardPresent.InitTutorial(false);
                tutorialPresent.OnTutorialShowInGame += HandleTutorialShowInGameBooster;
                tutorialPresent.OnTutorialShowInGame += HandleBoosterClickedTutorial;
            }

            HandleShowTutorial(levelData.levelIndex);
        }
        else
        {
            boardPresent.InitTutorial(false);
        }
        gameManager.ChangeState(GameState.Play);
    }
    private void HandleFoodClickedForBooster(int r, int c)
    {
        if (itemBoosterPresent.IsBoosterSelected())
        {
            gameManager.ChangeState(GameState.Stop);
            int[,] board = boardPresent.GetBoard();
            int rows = boardPresent.GetRow();
            int cols = boardPresent.GetCol();
            itemBoosterPresent.ExecuteBoosterAt(r, c, board, rows, cols, boardView.GetWorldPos);
            boardPresent.StopHint();
        }
    }
    private void HandleApplyBoosterEffect(List<Vector2Int> affectedCells)
    {
        boardPresent?.ApplyBoosterEffect(affectedCells);
    }

    // Khi Animation hoàn tất -> Board chạy Gravity & Refill
    private void HandleBoosterAnimationComplete()
    {
        boardPresent?.RunBoosterRoutine();
    }
    private void SignUIEvents()
    {
        if (losePanelView.btnClose != null)
        {
            losePanelView.btnClose.onClick.RemoveAllListeners();
            losePanelView.btnClose.onClick.AddListener(() => levelPresent.HandleSkipGameLose());
        }

        if (losePanelView.btnSkip != null)
        {
            losePanelView.btnSkip.onClick.RemoveAllListeners();
            losePanelView.btnSkip.onClick.AddListener(() => levelPresent.HandleSkipGameLose());
        }

        if (winPanelView.btnSkip != null)
        {
            winPanelView.btnSkip.onClick.RemoveAllListeners();
            winPanelView.btnSkip.onClick.AddListener(() => levelPresent.HandleContinueGameWin());
        }

        if (losePanelView.btnBuyChanceCoin != null)
        {
            if (bucketManager.GetBucket() < priceBuyChance)
            {
                losePanelView.btnBuyChanceCoin.interactable = false;
            }
            else
            {
                losePanelView.btnBuyChanceCoin.interactable = true;
                losePanelView.btnBuyChanceCoin.onClick.RemoveAllListeners();
                losePanelView.btnBuyChanceCoin.onClick.AddListener(() => boardPresent.BuyChanceCoin(priceBuyChance));
            }
        }

        if (losePanelView.btnBuyChanceAds != null)
        {
            losePanelView.btnBuyChanceAds.onClick.RemoveAllListeners();
            losePanelView.btnBuyChanceAds.onClick.AddListener(() => boardPresent.BuyChanceAds());
        }
    }
    private void HandleLevelCompleted()
    {
        levelPresent.CompletedLevel(levelIndex);
        itemBoosterPresent.DeselectBooster();
    }

    private void HandleLevelNotCompleted()
    {
        itemBoosterPresent.DeselectBooster();
    }

    private void HandleCloseInGameSwap()
    {
        tutorialPresent.SwapCloseTutorial();
    }

    private void HandleTutorialShowMap(bool isActive)
    {
        levelPresent.EnableScrollVerticle(!isActive);
        levelPresent.SetTutorialMode(isActive);
        settingPresent.SetTutorialMode(isActive);
    }

    private void HandleTutorialShowInGameSwap(bool isActive)
    {
        boardPresent.SetTutorialSwap(isActive);
    }

    private void HandleTutorialShowInGameBooster(bool isActive)
    {
        boardPresent.SetTutorialBooster(isActive);
    }

    private void HandleBoosterClickedTutorial(bool isActive)
    {
        itemBoosterPresent.SetTutorialMode(isActive);
    }


    private void HandleShowLoadingInGameScreen()
    {
        loadingScreenPresent.HandleLoadingInGame(() =>
        {
            googleAdmobPresenter.HandleShowInterstitial();
            audioView.PlayMusic(BgMusic.Map);
        });
    }

    private void HandleLoadingStartScreenCompleted()
    {
        audioView.PlayMusic(BgMusic.Map);
        googleAdmobPresenter.HandleShowInterstitial();
        tutorialPresent.HandleTutorial(TutorialType.MapLevel1Btn);
        tutorialPresent.HandleTutorial(TutorialType.PlayTargetBoardLevel1Btn);
    }

    private void HandleShowTutorial(int level)
    {
        switch (level)
        {
            case 0:
                tutorialPresent.HandleTutorial(TutorialType.InGameSwap);
                break;
            case 2:
                tutorialPresent.HandleTutorial(TutorialType.UnlockHammer);
                itemBoosterPresent.SetBoosterCanSelectInTutorial(BoosterType.Hammer);
                break;
            case 3:
                tutorialPresent.HandleTutorial(TutorialType.UnlockRocket);
                itemBoosterPresent.SetBoosterCanSelectInTutorial(BoosterType.Rocket);
                break;
            case 4:
                tutorialPresent.HandleTutorial(TutorialType.UnlockSprinkle);
                itemBoosterPresent.SetBoosterCanSelectInTutorial(BoosterType.Sprinkle);
                break;
        }
        itemBoosterPresent.CheckUnlockedLevel(level);
    }

    private void StopAllRoutine()
    {
        StopAllCoroutines();
    }

    private void InitCloud()
    {
        cloudPresentList.Clear();
        foreach (var cloudView in cloudViewList)
        {
            if (cloudView == null) continue;
            cloudPresentList.Add(new CloudPresent(cloudView));
        }
    }


    void Update()
    {
        inputManager.HandleInput();

        foreach (var cloudPrensent in cloudPresentList)
        {
            if (cloudPrensent == null) continue;
            cloudPrensent.OnUpdate();
        }
    }
}
