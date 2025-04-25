using System;
using System.Security;
using UnityEngine;


public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player.lineVelocity(0f, 0f); //움직임 0으로 만들기
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine) return;

        player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);

        if (xInput == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
