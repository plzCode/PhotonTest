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

        if (player.dashtime > 0.3)
        {
            player.dash = false;
            player.dashtime = 0;
        }

        if (xInput != 0 && !player.isBusy)
        {
            player.dash = true;
            stateMachine.ChangeState(player.moveState);
        }

        if (xInput != 0 && player.dashtime > 0.1)
        {
            stateMachine.ChangeState(player.dashState);
        }

        if (!player.IsGroundCheck())
            stateMachine.ChangeState(player.airState);

        if (Input.GetKey(KeyCode.S))
            stateMachine.ChangeState(player.downState);
    }
}
