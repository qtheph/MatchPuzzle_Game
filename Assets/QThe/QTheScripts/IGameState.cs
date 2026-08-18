using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState
{
    event Action<GameState> OnStateChanged;
    GameState CurrState { get; }
    void ChangeState(GameState newState);
}
