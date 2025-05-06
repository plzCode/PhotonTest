using UnityEngine;

public class Whell_Kirby_Attack_Ready_State : PlayerState
{
    private PlayerState nextState;

    public Whell_Kirby_Attack_Ready_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, PlayerState _nextState)
        : base(_player, _stateMachine, _animBoolName)
    {
        nextState = _nextState;
    }

    public void SetNextState(PlayerState _nextState)
    {
        nextState = _nextState;
    }


    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine)
            return;

        if(triggerCalled)
        {
            player.stateMachine.ChangeState(nextState);
        }
    }
}
