using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.MaxJumpPower = -1;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.airJumpState);
        }
        else if (Input.GetKey(KeyCode.Space) && player.JumpPower >= player.MaxJumpPower)
        {
            player.lineVelocity(rb.linearVelocityX, player.MinJumpPower + player.JumpPower);
            player.MaxJumpPower += 0.1f;
        }

        if (rb.linearVelocityY < 0)
            stateMachine.ChangeState(player.airState);;

        if (xInput != 0)
            player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
    }
}
