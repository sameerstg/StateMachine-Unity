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
        storyMode.onClick.AddListener(() => { SampleManager._instance.stateQueue = SampleManager._instance.storyQueue; SampleManager._instance.mode = GameState.Story; SampleManager._instance.ChangeGameMode(); });
        craftingMode.onClick.AddListener(() => { SampleManager._instance.stateQueue = SampleManager._instance.craftingQueue; SampleManager._instance.mode = GameState.Crafting; SampleManager._instance.ChangeGameMode(); });
        learningMode.onClick.AddListener(() => { SampleManager._instance.stateQueue = SampleManager._instance.learningQueue; SampleManager._instance.mode = GameState.Learning; SampleManager._instance.ChangeGameMode(); });
        SampleManager._instance.onStateChange += _ => OnStateChanged(_);
        OnStateChanged(SampleManager._instance.gameState);
    }

    private void OnStateChanged(GameState mode)
    {
        gameObject.SetActive(mode == GameState.IntroTask);
    }
}
