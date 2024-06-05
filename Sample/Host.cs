using System;
using System.Collections.Generic;
using UnityEngine;

public class Host : MonoBehaviour
{
    public StateManager stateManager;
    public GameObject hostRig;
    public AudioSource auSource;
    public AudioClip intro1, introEnvironment, selectionMode, excelentChoice;
    public AudioClip studentFeedbackLine, generalFeedbackLine, endLine, conflictLine, toiletLine;
    public AudioClip needImprovLine;
    public GameObject areYouReadyToBegin;
    public Animator anim;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        stateManager = new(new List<State>() { new Host_Intro(this), new Host_IntroToTasks(this), new Host_AskingForMode(this), new Host_ExcellentChoice(this),
        new Host_StudentFeedback(this),new Host_GeneralFeedbackGood(this),new Host_End(this),new Host_Conflict(this),new Host_Toilet(this),new Host_GeneralFeedbackBad(this),new Host_Idle(this)});
    }
    //private void OnEnable()
    //{
    //}
    private void Start()
    {
        SampleManager._instance.onStateChange += _ => AssignStateAcc(_);

        AssignStateAcc(SampleManager._instance.gameState);

    }
    //private void OnEnable()
    //{
    //}
    private void Update()
    {
        stateManager.currentState?.Update();
    }
    public void AssignStateAcc(GameState state)
    {

        switch (state)
        {
            case GameState.None:
                hostRig.SetActive(false);
                break;
            case GameState.Intro:
                stateManager.ChangeState(0);
                break;
            case GameState.IntroEnvironment:
                stateManager.ChangeState(1);
                break;
            case GameState.IntroTask:
                stateManager.ChangeState(2);
                break;
            case GameState.Gather:
                stateManager.ChangeState(3);
                break;

            case GameState.StudentFeedback:
                stateManager.ChangeState(4);
                break;
            case GameState.GeneralFeedbackGood:
                stateManager.ChangeState(5);
                break;
            case GameState.GeneralFeedbackBad:
                stateManager.ChangeState(9);
                break;
            case GameState.Story:
                stateManager.ChangeState(10);
                break;
            case GameState.Conflict:
                stateManager.ChangeState(7);
                break;
            case GameState.Toilet:
                stateManager.ChangeState(8);
                break;
            case GameState.BrokenAeroplane:
                stateManager.ChangeState(8);
                break;
            case GameState.End:
                stateManager.ChangeState(6);
                break;
            case GameState.Crafting:
                stateManager.ChangeState(10);
                break;
            case GameState.Learning:
                stateManager.ChangeState(10);
                break;
            default:
                break;
        }
    }
    public void PlayAudio(AudioClip clip)
    {
        auSource.clip = clip;
        auSource.Play();
    }

}
[Serializable]
public class Host_BaseState : State
{
    internal Host host;
    public Host_BaseState(object obj) : base(obj)
    {
        host = obj as Host;
    }

    public override void Enter()
    {
        int index = host.stateManager.GetCurrentStateIndex();
        host.anim.SetBool("talk", index != 10);
        if (!host.hostRig.activeInHierarchy) host.hostRig.SetActive(true);

    }

    public override void Exit()
    {
        host.auSource.Stop();
        host.auSource.clip = null;

    }

    public override void Update()
    {
        LookAtPlayer();
    }
    public void LookAtPlayer()
    {
        var temp = host.transform.eulerAngles;
        host.hostRig.transform.LookAt(SampleManager._instance.transform);
        host.hostRig.transform.eulerAngles = new Vector3(temp.x, host.hostRig.transform.eulerAngles.y, temp.z);
    }


}

[Serializable]

public class Host_Intro : Host_BaseState
{

    public Host_Intro(object obj) : base(obj)
    {


    }

    public override void Exit()
    {

        base.Exit();
        //host.areYouReadyToBegin.SetActive(false);
    }

    public override void Enter()
    {
        base.Enter();
        host.PlayAudio(host.intro1);
        LeanTween.delayedCall(host.intro1.length, () =>
        {
            if (SampleManager._instance.gameState == GameState.Intro)
            {
                host.areYouReadyToBegin.SetActive(true); host.stateManager.ChangeState(10);
            }

        });

    }

    public override void Update()
    {
        base.Update();
    }
}
[Serializable]

public class Host_IntroToTasks : Host_BaseState
{

    /// <summary>
    /// Run Till Mode Selection
    /// </summary>
    /// <param name="obj"></param>
    public Host_IntroToTasks(object obj) : base(obj)
    {


    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Enter()
    {
        base.Enter();
        // run sound clip
        host.PlayAudio(host.introEnvironment);
        LeanTween.delayedCall(host.introEnvironment.length, () => { if (SampleManager._instance.gameState == GameState.IntroEnvironment) SampleManager._instance.ChangeGameMode(); });

    }


    public override void Update()
    {

        base.Update();
    }
}
[Serializable]

public class Host_AskingForMode : Host_BaseState
{

    /// <summary>
    /// Run Till Mode Selection
    /// </summary>
    /// <param name="obj"></param>
    public Host_AskingForMode(object obj) : base(obj)
    {


    }
    public override void Enter()
    {
        base.Enter();
        // run sound clip
        host.PlayAudio(host.selectionMode);
        LeanTween.delayedCall(host.selectionMode.length, () => { if (SampleManager._instance.gameState == GameState.IntroTask) host.stateManager.ChangeState(10); });
    }


    public override void Update()
    {
        base.Update();
    }
}
[Serializable]

public class Host_ExcellentChoice : Host_BaseState
{

    /// <summary>
    /// Run Till Mode Selection
    /// </summary>
    /// <param name="obj"></param>
    public Host_ExcellentChoice(object obj) : base(obj)
    {


    }


    public override void Enter()
    {
        base.Enter();
        // run sound clip
        host.PlayAudio(host.excelentChoice);
        LeanTween.delayedCall(host.excelentChoice.length, () => { if (SampleManager._instance.gameState == GameState.Gather) host.stateManager.ChangeState(10); });
    }


    public override void Update()
    {
        base.Update();
    }
}
public class Host_StudentFeedback : Host_BaseState
{

    /// <summary>
    /// Run Till Mode Selection
    /// </summary>
    /// <param name="obj"></param>
    public Host_StudentFeedback(object obj) : base(obj)
    {


    }

    public override void Enter()
    {
        base.Enter();
        // run sound clip
        host.PlayAudio(host.studentFeedbackLine);
        LeanTween.delayedCall(host.studentFeedbackLine.length, () => { if (SampleManager._instance.gameState == GameState.StudentFeedback) host.stateManager.ChangeState(10); });
    }


}
public class Host_GeneralFeedbackGood : Host_BaseState
{

    /// <summary>
    /// Run Till Mode Selection
    /// </summary>
    /// <param name="obj"></param>
    public Host_GeneralFeedbackGood(object obj) : base(obj)
    {


    }


    public override void Enter()
    {
        base.Enter();
        // run sound clip
        host.PlayAudio(host.generalFeedbackLine);
        LeanTween.delayedCall(host.generalFeedbackLine.length, () => { if (SampleManager._instance.gameState == GameState.GeneralFeedbackGood) host.stateManager.ChangeState(10); });
    }


    public override void Update()
    {
        base.Update();
    }
}
public class Host_GeneralFeedbackBad : Host_BaseState
{

    /// <summary>
    /// Run Till Mode Selection
    /// </summary>
    /// <param name="obj"></param>
    public Host_GeneralFeedbackBad(object obj) : base(obj)
    {


    }


    public override void Enter()
    {
        base.Enter();
        // run sound clip
        host.PlayAudio(host.needImprovLine);
        LeanTween.delayedCall(host.needImprovLine.length, () => { if (SampleManager._instance.gameState == GameState.GeneralFeedbackBad) host.stateManager.ChangeState(10); });
    }


    public override void Update()
    {
        base.Update();
    }
}
public class Host_End : Host_BaseState
{

    /// <summary>
    /// Run Till Mode Selection
    /// </summary>
    /// <param name="obj"></param>
    public Host_End(object obj) : base(obj)
    {


    }

    public override void Enter()
    {
        base.Enter();
        // run sound clip
        host.PlayAudio(host.endLine);
        LeanTween.delayedCall(host.endLine.length, () => { if (SampleManager._instance.gameState == GameState.End || SampleManager._instance.gameState == GameState.Re) { SampleManager._instance.ChangeGameMode(); host.stateManager.ChangeState(10); } });
    }


    public override void Update()
    {
        base.Update();
    }
}
public class Host_Toilet : Host_BaseState
{

    /// <summary>
    /// Run Till Mode Selection
    /// </summary>
    /// <param name="obj"></param>
    public Host_Toilet(object obj) : base(obj)
    {


    }


    public override void Enter()
    {
        base.Enter();
        // run sound clip
        host.PlayAudio(host.toiletLine);
        LeanTween.delayedCall(host.toiletLine.length, () => { if (SampleManager._instance.gameState == GameState.Toilet) host.stateManager.ChangeState(10); });
    }


}
public class Host_Conflict : Host_BaseState
{

    /// <summary>
    /// Run Till Mode Selection
    /// </summary>
    /// <param name="obj"></param>
    public Host_Conflict(object obj) : base(obj)
    {


    }


    public override void Enter()
    {
        base.Enter();
        // run sound clip
        host.PlayAudio(host.conflictLine);
        LeanTween.delayedCall(host.conflictLine.length, () => { if (SampleManager._instance.gameState == GameState.Conflict) host.stateManager.ChangeState(10); });
    }

}
public class Host_Idle : Host_BaseState
{

    public Host_Idle(object obj) : base(obj)
    {


    }


    public override void Enter()
    {
        base.Enter();
    }

}
