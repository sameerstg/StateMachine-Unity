using System;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectionUi : MonoBehaviour
{
    public Button storyMode;
    public Button craftingMode;
    public Button learningMode;
    private void Start()
    {
        storyMode.onClick.AddListener(() => { GameManager._instance.stateQueue = GameManager._instance.storyQueue; GameManager._instance.mode = GameState.Story; GameManager._instance.ChangeGameMode(); });
        craftingMode.onClick.AddListener(() => { GameManager._instance.stateQueue = GameManager._instance.craftingQueue; GameManager._instance.mode = GameState.Crafting; GameManager._instance.ChangeGameMode(); });
        learningMode.onClick.AddListener(() => { GameManager._instance.stateQueue = GameManager._instance.learningQueue; GameManager._instance.mode = GameState.Learning; GameManager._instance.ChangeGameMode(); });
        GameManager._instance.onStateChange += _ => OnStateChanged(_);
        OnStateChanged(GameManager._instance.gameState);
    }

    private void OnStateChanged(GameState mode)
    {
        gameObject.SetActive(mode == GameState.IntroTask);
    }
    //public void Active()
    //{
    //    gameObject.SetActive(true);
    //}
    //public void Inactive()
    //{
    //    gameObject.SetActive(false);
    //}

    //public void Referesh()
    //{
    //}
}
