using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardPresent
{
    public event Action<int, int> OnFoodClickedForBooster;
    public event Action OnCompletedLevel;
    public event Action OnNotCompletedLevel;
    public event Action OnCloseInGameSwapTutorial;
    // public event Action OnCloseInGameBoosterTutorial;

    private readonly IGameState gameState;
    private readonly ICoroutineRunner coroutineRunner;
    private readonly BoardModel boardModel;
    private readonly BoardView boardView;
    private readonly InputManager inputManager;
    private readonly EnviromentView enviromentView;
    private readonly WinPanelView winPanelView;
    private readonly LosePanelView losePanelView;
    private readonly MoveView moveView;
    private readonly TargetGroupView targetGroupView;
    private readonly BucketManagerView bucketManager;
    private readonly ItemBoosterData itemBoosterData;
    private readonly AudioView audioView;
    private readonly LevelView levelView;
    private readonly GoogleAdmobView googleAdmobView;

    private Coroutine hintCoroutine;
    private bool CanInput => gameState != null && gameState.CurrState == GameState.Play;

    private readonly Vector2Int allowTutPos1 = new Vector2Int(1, 2);
    private readonly Vector2Int allowTutPos2 = new Vector2Int(2, 2);

    private bool isTutorialSwap = false;
    private bool isTutorialBooster = false;

    public BoardPresent(BoardModel boardModel,
    BoardView boardView,
     InputManager inputManager,
     EnviromentView enviromentView,
     MoveView moveView,
     TargetGroupView targetGroupView,
     WinPanelView winLoseView,
     LosePanelView losePanelView,
     BucketManagerView bucketManager,
    AudioView audioView,
    LevelView levelView,
    GoogleAdmobView googleAdmobView,
    ItemBoosterData itemBoosterData,

    IGameState gameState, ICoroutineRunner coroutineRunner)
    {
        this.boardModel = boardModel;
        this.boardView = boardView;
        this.inputManager = inputManager;
        this.enviromentView = enviromentView;
        this.moveView = moveView;
        this.winPanelView = winLoseView;
        this.targetGroupView = targetGroupView;
        this.losePanelView = losePanelView;
        this.bucketManager = bucketManager;
        this.audioView = audioView;
        this.levelView = levelView;
        this.googleAdmobView = googleAdmobView;
        this.itemBoosterData = itemBoosterData;

        this.gameState = gameState;
        this.coroutineRunner = coroutineRunner;

        inputManager.OnSwapDirection += HandleSwap;
        inputManager.OnFoodClicked += HandleClicked;
        boardModel.GetMoveManager.OnMoveUpdated += HandleMove;
        boardModel.GetTargetManager.OnTargetUpdated += HandleTarget;
    }

    public void InitTutorial(bool isTutorialMode)
    {
        InitBoardData(isTutorialMode);
    }

    private void InitBoardData(bool isTutorialMode)
    {

        if (isTutorialMode)
        {
            boardModel.InitBoardTutorial();
            StopHint();
        }
        else
        {
            boardModel.InitBoardModel();
            ResetCoolDownHint();
        }

        boardView.InitBoardView(boardModel.GetBoard, boardModel.GetLevelData);
        moveView.Init(boardView.GetMoveTxt, boardModel.GetLevelData);
        targetGroupView.InitTargetItem(boardModel.GetLevelData.targets, boardView.GetParent);
        StopHint();
        levelView.MapUIShow(false);
        EnviromentType currEnvi = boardModel.GetLevelData.enviromentType;
        enviromentView.SetEnviroment(currEnvi);
    }

    public void CleanOldData()
    {
        if (inputManager != null)
        {
            inputManager.OnSwapDirection -= HandleSwap;
            inputManager.OnFoodClicked -= HandleClicked;
        }
        if (boardModel != null)
        {
            boardModel.GetMoveManager.OnMoveUpdated -= HandleMove;
            boardModel.GetTargetManager.OnTargetUpdated -= HandleTarget;
        }

    }
    public int[,] GetBoard() => boardModel.GetBoard;
    public int GetRow() => boardModel.GetBoard.GetLength(0);
    public int GetCol() => boardModel.GetBoard.GetLength(1);

    public void ApplyBoosterEffect(List<Vector2Int> affectedCells)
    {
        int flyingCount = 0;
        //remove and fly to target
        List<Vector2Int> normalMatchedList = new List<Vector2Int>();
        foreach (var pos in affectedCells)
        {
            int foodId = boardModel.GetFoodId(pos.x, pos.y);
            RectTransform targetRectId = targetGroupView.GetTargetRect(foodId);

            if (targetRectId != null)
            {
                flyingCount++;
                boardView.RemoveTargetFoodAndFlyToTargetUI(pos.x, pos.y, targetRectId, () =>
                {
                    boardModel.GetTargetManager.RemoveTargetAmount(foodId);
                    flyingCount--;
                });
            }
            else
            {
                normalMatchedList.Add(pos);
            }
        }
        boardModel.RemoveIdMatched(affectedCells);
        boardView.RemoveFoodMatched(normalMatchedList);
        // boardModel.RemoveIdMatched(affectedCells);
        // boardView.RemoveFoodMatched(affectedCells);
    }
    public void RunBoosterRoutine()
    {
        gameState.ChangeState(GameState.Stop);
        coroutineRunner.StartCoroutine(BoosterRoutine());
    }
    private void HandleClicked(TileFoodView tileFoodView)
    {
        if (!CanInput) return;
        int r = tileFoodView.GetRow;
        int c = tileFoodView.GetCol;

        // if (isTutorialBooster) OnCloseInGameBoosterTutorial?.Invoke();
        OnFoodClickedForBooster?.Invoke(r, c);
    }
    private void HandleMove(int remainCount)
    {
        moveView.UpdateMove(boardView.GetMoveTxt, remainCount);
    }
    private void HandleTarget(int foodId, int remainCount)
    {
        targetGroupView.UpdateTargetCount(foodId, remainCount);
    }

    public void SetTutorialSwap(bool isTutorialMode)
    {
        isTutorialSwap = isTutorialMode;
    }

    public void SetTutorialBooster(bool isTutorialMode)
    {
        isTutorialBooster = isTutorialMode;
    }

    private void HandleSwap(TileFoodView tile, SwapDirection direction)
    {
        if (!CanInput) return;
        int r1 = tile.GetRow;
        int c1 = tile.GetCol;
        int r2 = r1;
        int c2 = c1;

        switch (direction)
        {
            case SwapDirection.Left:
                c2--;
                break;
            case SwapDirection.Right:
                c2++;
                break;
            case SwapDirection.Up:
                r2--;
                break;
            case SwapDirection.Down:
                r2++;
                break;
        }

        if (!boardModel.IsValidPosition(r2, c2)) return;


        if (isTutorialSwap)
        {
            bool valid = (r1 == allowTutPos1.x && c1 == allowTutPos1.y) && (r2 == allowTutPos2.x && c2 == allowTutPos2.y);
            if (!valid) return;
            OnCloseInGameSwapTutorial?.Invoke();
        }
        else if (isTutorialBooster)
        {
            return;
        }
        Debug.Log(isTutorialBooster);
        Swap(r1, c1, r2, c2, true);
    }

    public void BuyChanceCoin(int price)
    {
        if (bucketManager.GetBucket() < price) return;
        bucketManager.SendBucket(price);
        boardModel.GetMoveManager.AddMove(5);

        losePanelView.LosePanelShow(false);
        levelView.InGameUIShow(true);

        ResetCoolDownHint();

        audioView.PlayMusic(BgMusic.Ingame);
        gameState.ChangeState(GameState.Play);
    }
    public void BuyChanceAds()
    {
        googleAdmobView.ShowRewarded(() =>
        {
            boardModel.GetMoveManager.AddMove(5);

            losePanelView.LosePanelShow(false);
            levelView.InGameUIShow(true);

            ResetCoolDownHint();

            audioView.PlayMusic(BgMusic.Ingame);
            gameState.ChangeState(GameState.Play);
        });
    }

    private IEnumerator BoosterRoutine()
    {
        List<GravityData> gravityDataList = boardModel.ApplyGravityId();
        bool isGravityDone = false;
        boardView.AnimateGravityFood(gravityDataList, () =>
        {
            isGravityDone = true;
        });
        yield return new WaitUntil(() => isGravityDone);

        List<ReFillData> reFillDataList = boardModel.ReFillIdOnBoard();
        bool isRefillDataDone = false;
        boardView.AnimateRefillFood(reFillDataList, () =>
        {
            isRefillDataDone = true;
        });
        yield return new WaitUntil(() => isRefillDataDone);

        coroutineRunner.StartCoroutine(MatchingRoutine());
    }
    private IEnumerator MatchingRoutine()
    {
        StopHint();
        List<Vector2Int> matchedList = boardModel.MatchedList;
        if (matchedList == null || matchedList.Count == 0)
        {
            ResetCoolDownHint();
            audioView.ResetPitchSFX(1);
            if (CheckGameWin())
            {
                coroutineRunner.StartCoroutine(HandleGameWin());
                yield break;
            }
            if (CheckGameLose())
            {
                coroutineRunner.StartCoroutine(HandleGameLose());
                yield break;
            }
            gameState.ChangeState(GameState.Play);
            yield break;
        }


        int flyingCount = 0;
        //remove and fly to target
        List<Vector2Int> normalMatchedList = new List<Vector2Int>();
        foreach (var pos in matchedList)
        {
            int foodId = boardModel.GetFoodId(pos.x, pos.y);
            RectTransform targetRectId = targetGroupView.GetTargetRect(foodId);

            if (targetRectId != null)
            {
                flyingCount++;
                boardView.RemoveTargetFoodAndFlyToTargetUI(pos.x, pos.y, targetRectId, () =>
                {
                    boardModel.GetTargetManager.RemoveTargetAmount(foodId);
                    flyingCount--;
                });
            }
            else
            {
                normalMatchedList.Add(pos);
            }
        }
        boardModel.RemoveIdMatched(matchedList);
        boardView.RemoveFoodMatched(normalMatchedList);
        audioView.PlaySFX(SFX.Match);
        audioView.PitchSFX(1);

        yield return new WaitUntil(() => flyingCount <= 0);
        //apply gravity and refill
        List<GravityData> gravityDataList = boardModel.ApplyGravityId();
        bool isGravityDone = false;
        boardView.AnimateGravityFood(gravityDataList, () =>
        {
            isGravityDone = true;
        });

        yield return new WaitUntil(() => isGravityDone);

        List<ReFillData> reFillDataList = boardModel.ReFillIdOnBoard();
        bool isRefillDataDone = false;
        boardView.AnimateRefillFood(reFillDataList, () =>
        {
            isRefillDataDone = true;
        });
        yield return new WaitUntil(() => isRefillDataDone);
        coroutineRunner.StartCoroutine(MatchingRoutine());
    }

    private void Swap(int r1, int c1, int r2, int c2, bool isCheckMatch)
    {
        StopHint();
        audioView.PlaySFX(SFX.Swipe);
        gameState.ChangeState(GameState.Stop);

        boardModel.SwapId(r1, c1, r2, c2);
        boardView.AnimateSwap(r1, c1, r2, c2, () =>
        {
            if (isCheckMatch)
            {
                List<Vector2Int> matchedList = boardModel.MatchedList;
                if (matchedList.Count == 0)
                {
                    audioView.PlaySFX(SFX.Undo);

                    boardModel.SwapId(r1, c1, r2, c2);
                    boardView.AnimateSwap(r1, c1, r2, c2, () =>
                    {
                        gameState.ChangeState(GameState.Play);
                        ResetCoolDownHint();
                    });
                }
                else
                {
                    boardModel.GetMoveManager.UseMove();
                    coroutineRunner.StartCoroutine(MatchingRoutine());
                }
            }
            else
            {
                gameState.ChangeState(GameState.Play);
            }
        });
    }
    private IEnumerator HandleGameWin()
    {
        gameState.ChangeState(GameState.Stop);
        StopHint();

        int coinReward = boardModel.GetLevelData.coinAmountReward;
        bucketManager.AddBucket(coinReward);
        itemBoosterData.AddItem(boardModel.GetLevelData.boosterTypeReward, boardModel.GetLevelData.boosterAmountReward);

        yield return new WaitForSeconds(2f);
        levelView.InGameUIShow(false);
        winPanelView.UpdateWinTxt();
        winPanelView.UpdateCoinTxt(coinReward);
        winPanelView.UpdateItem_Txt_Img(boardModel.GetLevelData.boosterTypeReward, boardModel.GetLevelData.boosterAmountReward);
        winPanelView.WinPanelShow(true);

        audioView.StopMusic(BgMusic.Ingame);
        audioView.PlaySFX(SFX.Win);

        yield return new WaitForSeconds(1.25f);
        winPanelView.SetInteractBtn(true);

        OnCompletedLevel?.Invoke();
    }
    private IEnumerator HandleGameLose()
    {
        gameState.ChangeState(GameState.Stop);
        StopHint();

        yield return new WaitForSeconds(2f);
        levelView.InGameUIShow(false);
        losePanelView.UpdateLoseTxt();
        losePanelView.LosePanelShow(true);

        audioView.StopMusic(BgMusic.Ingame);
        audioView.PlaySFX(SFX.Lose);

        yield return new WaitForSeconds(1.25f);
        losePanelView.SetInteractBtn(true);

        OnNotCompletedLevel?.Invoke();
    }
    private IEnumerator HandleHint()
    {
        yield return new WaitForSeconds(5f);
        boardView.AnimateHint(boardModel.GetHint());
    }
    public void ResetCoolDownHint()
    {
        StopHint();
        hintCoroutine = coroutineRunner.StartCoroutine(HandleHint());
    }
    public void StopHint()
    {
        boardView.StopAllAnimateHint();
        if (hintCoroutine != null)
        {
            coroutineRunner.StopCoroutine(hintCoroutine);
            hintCoroutine = null;
        }
    }
    private bool CheckGameWin()
    {
        return boardModel.GetTargetManager.IsAllTargetFinished();
    }
    private bool CheckGameLose()
    {
        return boardModel.GetMoveManager.EndOfMove();
    }

}
