using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleManager : MonoBehaviour
{
    internal static SampleManager _instance;
    public GameState gameState;
    public int index;

    public Host host;
    public List<Student> playingStudents;
    public List<Student> instantiatedStudents;
    public List<GameObject> studentAircraft;
    public Action<GameState> onStateChange;
    public int toiletStudentIndex;
    public List<int> conflictStudentIndex;
    public int classManagementScore;
    public Queue<GameState> stateQueue;
    public Queue<GameState> storyQueue, craftingQueue, learningQueue;
    public GameState mode;
    List<CheatCode> cheat;
    private void Awake()
    {
        _instance = this;
        storyQueue = QueueExtension.AddRange(new List<GameState>() { GameState.Gather, GameState.Story, GameState.Conflict, GameState.Toilet, GameState.StudentFeedback, GameState.GeneralFeedbackGood, GameState.GeneralFeedbackBad, GameState.End, GameState.Re });
        craftingQueue = QueueExtension.AddRange(new List<GameState>() { GameState.Gather, GameState.Crafting, GameState.StudentCrafting, GameState.Conflict, GameState.BrokenAeroplane, GameState.StudentFeedback, GameState.GeneralFeedbackGood, GameState.GeneralFeedbackBad, GameState.End, GameState.Re });
        learningQueue = QueueExtension.AddRange(new List<GameState>() { GameState.Gather, GameState.Learning, GameState.Conflict, GameState.Toilet, GameState.StudentFeedback, GameState.GeneralFeedbackGood, GameState.GeneralFeedbackBad, GameState.End, GameState.Re });

        ChangeGameMode(GameState.None);
        cheat = new() { new CheatCode("skip", () => { ChangeGameMode(); }) };

    }


    [ContextMenu("Game Mode")]
    public void ChangeGameMode()
    {
        if (gameState == GameState.Re) return;

        if (stateQueue != null && stateQueue.Count != 0)
        {
            ChangeGameMode(stateQueue.Dequeue());
            return;
        }
        ChangeGameMode((GameState)(++index));
    }
    public void ChangeGameMode(GameState mode)
    {
        StopAllCoroutines();
        this.gameState = mode;
        index = (int)mode;
        switch (mode)
        {
            case GameState.StudentFeedback:
                StartCoroutine(DelayStateStart(GameState.GeneralFeedbackGood, 15));
                break;
            case GameState.GeneralFeedbackGood:
                StartCoroutine(DelayStateStart(GameState.GeneralFeedbackBad, 10));
                break;
            case GameState.GeneralFeedbackBad:
                StartCoroutine(DelayStateStart(GameState.End, 10));
                break;
            case GameState.Conflict:
                Debug.LogError("Conflict Start");
                break;
            case GameState.Toilet:
                Debug.LogError("Toilet Start");
                break;
            case GameState.BrokenAeroplane:
                Debug.LogError("Broken Aeroplanece Start");
                break;
            case GameState.End:
                StartCoroutine(DelayStateStart(GameState.Re, 10));
                break;
            case GameState.StudentCrafting:
                // if less students
                for (int i = 2; i < studentAircraft.Count; i++)
                {
                    studentAircraft[i].gameObject.SetActive(false);
                }
                StartCoroutine(DelayStateStart(8));
                break;

            default:
                break;
        }
        onStateChange?.Invoke(gameState);
    }
    public IEnumerator DelayStateStart(GameState state, float time)
    {
        GameState curr = gameState;
        yield return new WaitForSeconds(time);

        if (curr == gameState)
            ChangeGameMode(state);
    }
    public IEnumerator DelayStateStart(float time)
    {
        GameState curr = gameState;
        yield return new WaitForSeconds(time);
        if (curr == gameState)
            ChangeGameMode();
    }
    private void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RThumbstick)) SceneManager.LoadScene(0);
        if (OVRInput.Get(OVRInput.RawButton.A) && OVRInput.GetDown(OVRInput.RawButton.B)) ChangeGameMode();
        CheatCode.Update(cheat);
    }


}

public enum GameState
{
    None, Intro, IntroEnvironment, IntroTask, Gather, Story, Conflict, Toilet, StudentFeedback, GeneralFeedbackGood, GeneralFeedbackBad, End, Re, Crafting, BrokenAeroplane, StudentCrafting, Learning
}
public static class QueueExtension
{
    public static Queue<T> AddRange<T>(IEnumerable<T> enu)
    {
        Queue<T> queue = new();
        foreach (T obj in enu)
            queue.Enqueue(obj);
        return queue;
    }
}
