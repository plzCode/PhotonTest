using System;
using UnityEngine;

public class PlayerSlidingState : PlayerState
{

    public PlayerSlidingState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.dash = true;
    }

    public override void Exit()
    {
        base.Exit();
        player.dash = false;
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine) return;

        player.lineVelocity(player.LastMove * player.MoveSpeed * 5, rb.linearVelocityY);

        if (player.dashTime > 0.2)
        {
            player.dashTime = 0;
            stateMachine.ChangeState(player.idleState);
        }
    }
}
