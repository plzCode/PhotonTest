using UnityEngine;

public class PlayerAirJump : PlayerState
{
    public PlayerAirJump(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
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

        if (player.IsGroundCheck())
            stateMachine.ChangeState(player.idleState);

        if (Input.GetKey(KeyCode.Space))
            stateMachine.ChangeState(player.airJumpUpState);

        if (xInput != 0)
            player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
    }
}
