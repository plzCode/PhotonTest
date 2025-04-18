using UnityEngine;

public class PlayerAirJump : PlayerState
{
    public PlayerAirJump(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (rb.linearVelocityY < -0.1)
            stateMachine.ChangeState(player.airJumpingState);

        if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.airJumpUpState);

        if (xInput != 0)
            player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
    }
}
