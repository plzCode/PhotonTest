using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.lineVelocity(rb.linearVelocityX, player.JumpPower);
            stateMachine.ChangeState(player.airJumpState);
        }

        if (player.IsGroundCheck())
            stateMachine.ChangeState(player.idleState);

        if (rb.linearVelocityY < -8)
            stateMachine.ChangeState(player.downingState);

        if (xInput != 0)
            player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
    }
}
