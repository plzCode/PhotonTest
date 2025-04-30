using UnityEngine;

public class Cutter_Kirby_Attack1_State : PlayerState
{
    public Cutter_Kirby_Attack1_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.ChangeState(player.idleState);
    }

    public override void Update()
    {
        base.Update();
    }
}
