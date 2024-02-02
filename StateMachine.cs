
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class State
{
    internal object obj;
    public State(object obj)
    {
        this.obj = obj;
    }
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
public class StateManager
{
    public StateManager(List<State> states)
    {

        this.states = states;
    }
    public List<State> states = new();
    public State currentState;
    public int index;
    public void ChangeState(State state)
    {
        Debug.LogError("done");

        currentState?.Exit();
        currentState = state;
        currentState.Enter();
        index = states.IndexOf(state);
    }
    public void ChangeState()
    {
        Debug.LogError("done");

        currentState?.Exit();
        if (index >= states.Count) return;
        currentState = states[++index];
        currentState.Enter();
    }
    public void ChangeState(int index)
    {
        Debug.LogError("done");

        currentState?.Exit();
        if (index >= states.Count) return;
        currentState = states[index];
        currentState.Enter();
    }
}
