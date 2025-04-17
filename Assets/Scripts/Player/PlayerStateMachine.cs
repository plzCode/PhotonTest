using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState state;

    public void StartState(PlayerState _startState)
    {
        state = _startState;
        state.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        state.Exit();
        state = _newState;
        state.Enter();
    }
}
