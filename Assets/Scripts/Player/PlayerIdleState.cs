using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.ZerolineVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();


        if(xInput != 0 && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }

        if(!player.IsGroundCheck())
            stateMachine.ChangeState(player.airState); ;

        if (Input.GetKey(KeyCode.S))
            stateMachine.ChangeState(player.downState);
    }
}
