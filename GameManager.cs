using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    internal static GameManager _instance;
    public UiManager uiManager;
    public GameState gameState;
    public LearningGame learningGame;
    public int index;

    public Host host;
    public PlayerCaretaker playerCaretaker;
    public StudentSittingManager sittingManager;
    public List<Student> playingStudents;
    public List<Student> instantiatedStudents;
    public List<GameObject> studentAircraft;
    public Action<GameState> onStateChange;
    public List<StoryObject> storyObjects;
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
        learningQueue = QueueExtension.AddRange(new List<GameState>() { GameState.Gather, GameState.Learning,GameState.Conflict,GameState.Toilet, GameState.StudentFeedback, GameState.GeneralFeedbackGood, GameState.GeneralFeedbackBad, GameState.End, GameState.Re });

        ChangeGameMode(GameState.None);
        cheat = new() {new CheatCode("skip", () => { ChangeGameMode(); }),new CheatCode("one",()=>{ learningGame.selectedStudent = sittingManager.instantiatedStudents[0]; }) };
        
    }

    
    [ContextMenu("Game Mode")]
    public void ChangeGameMode()
    {
        if (gameState == GameState.Re) return;
        //try
        //{
        //    Debug.LogError(stateQueue);
        //    Debug.LogError(stateQueue.Count);
        //}
        //catch (Exception)
        //{

        //    Debug.LogError(stateQueue);
        //}

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
            //case GameState.None:
            //    break;
            //case GameState.Intro:
            //    break;
            //case GameState.IntroEnvironment:
            //    break;
            //case GameState.IntroTask:
            //    break;
            //case GameState.Story:
            //    break;
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
                sittingManager.instantiatedStudents[conflictStudentIndex[0]].isConflictStudent = true;
                sittingManager.instantiatedStudents[conflictStudentIndex[1]].isConflictStudent = true;
                break;
            case GameState.Toilet:
                sittingManager.instantiatedStudents[toiletStudentIndex].isToiletStudent = true;
                break;
            case GameState.BrokenAeroplane:
                sittingManager.instantiatedStudents[toiletStudentIndex].isToiletStudent = true;
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

            //case GameState.Re:

            //    //SceneManager.LoadScene(0);
            //    break;
            default:
                break;
        }
        //Debug.LogError("change");
        onStateChange?.Invoke(gameState);
    }
    //public IEnumerator WaitTillConflictResolve()
    //{
    //    yield return new WaitUntil(() => !sittingManager.instantiatedStudents.Exists(x => x.isConflictStudent));
    //    ChangeGameMode();
    //}
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
