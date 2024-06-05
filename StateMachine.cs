
using System;
using System.Collections.Generic;

[System.Serializable]
public class State
{
    internal object obj;
    public State(object obj)
    {
        this.obj = obj;
    }
    public virtual void Enter()
    {

    }
    public virtual void Update()
    {

    }
    public virtual void Exit()
    {

    }
}
[Serializable]
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

        currentState?.Exit();
        currentState = state;
        currentState.Enter();
        index = states.IndexOf(state);
    }

    public void ChangeState(int index)
    {
        currentState?.Exit();
        if (index >= states.Count) return;
        currentState = states[index];
        currentState.Enter();
    }
    public int GetCurrentStateIndex()
    {
        return currentState == null ? -1 : states.IndexOf(currentState);
    }
}
