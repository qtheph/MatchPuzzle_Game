using System;
using UnityEngine;

public enum GameState
{
    Play,
    Stop
}
public class GameManager : MonoBehaviour, IGameState
{
    public event Action<GameState> OnStateChanged;
    public GameState CurrState { get; private set; }
    public void ChangeState(GameState newState)
    {
        CurrState = newState;
        OnStateChanged?.Invoke(newState);
    }
}
