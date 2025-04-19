using UnityEngine;

public class PlayerAirJumpUp : PlayerState
{
    public PlayerAirJumpUp(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.lineVelocity(rb.linearVelocityX, player.MinJumpPower);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(rb.linearVelocityY < -0.1)
            stateMachine.ChangeState(player.airJumpingState);

        if (xInput != 0)
            player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
    }
}
