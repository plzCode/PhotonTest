using UnityEngine;

public class Whell_Kirby_Attack_Turn_State : PlayerState
{
    private PlayerState nextState;

    public Whell_Kirby_Attack_Turn_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, PlayerState _nextState)
        : base(_player, _stateMachine, _animBoolName)
    {
        nextState = _nextState;
    }

    public override void Enter()
    {
        base.Enter();
        player.lineVelocity(0f, -1f);
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

        if(xInput == 1f)
        {
            player.LastMove = 1f;
        }

        if (xInput == -1f)
        {
            player.LastMove = -1f;
        }

        if (triggerCalled)
        {
            player.stateMachine.ChangeState(nextState);
        }

    }
}
