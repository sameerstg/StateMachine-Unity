using System.Collections.Generic;
using UnityEngine;

public class ActiveAtState : MonoBehaviour
{
    public List<GameState> statesAtWhichActive;
    public List<GameState> gameModeForInactive;
    private void Start()
    {
        GameManager._instance.onStateChange += _ => OnStateChanged(_);
        OnStateChanged(GameManager._instance.gameState);
    }
    private void OnStateChanged(GameState mode)
    {
        gameObject.SetActive(statesAtWhichActive.Exists(x => x == mode) && !gameModeForInactive.Exists(x => x == GameManager._instance.mode));
    }
}

