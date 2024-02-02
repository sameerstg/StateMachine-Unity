using System;
using System.Collections.Generic;
using UnityEngine;

public class Host : MonoBehaviour
{
    public StateManager stateManager;
    public GameObject hostRig;
    public Transform introEndPosition;
    private void Awake()
    {
        stateManager = new(new List<State>() { new Host_Intro(this), new Host_IntroToTasks(this) });
    }
    private void Start()
    {

        stateManager.ChangeState(stateManager.states[0]);
    }
    private void Update()
    {
        stateManager.currentState?.Update();
    }
}
[Serializable]

public class Host_Intro : State
{

    Host host;
    /// <summary>
    /// Run Till Are You Ready To Begin
    /// </summary>
    /// <param name="obj"></param>
    public Host_Intro(object obj) : base(obj)
    {

        host = obj as Host;

    }

    public override void Exit()
    {

    }

    public override void Enter()
    {
        Debug.LogError("done");
        LeanTween.move(host.hostRig, host.introEndPosition, 4).setOnComplete(() =>
        {

            //run sound clip
            // todo : this change will not remain here
            Change();


        });
    }
    public void Change()
    {
        host.stateManager.ChangeState();
    }

    public override void Update()
    {
    }
}
[Serializable]

public class Host_IntroToTasks : State
{

    Host host;
    /// <summary>
    /// Run Till Mode Selection
    /// </summary>
    /// <param name="obj"></param>
    public Host_IntroToTasks(object obj) : base(obj)
    {

        host = obj as Host;

    }

    public override void Exit()
    {

    }

    public override void Enter()
    {
        // run sound clip
    }

    public override void Update()
    {
    }
}
