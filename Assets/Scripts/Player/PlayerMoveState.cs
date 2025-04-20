using Photon.Pun;
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
    }

    public override void Update()
    {
        base.Update();
        player.LastInput(xInput);

        if (!playerView.IsMine) return;
        
        player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
        
        if (xInput == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (!player.IsGroundCheck())
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.S))
            stateMachine.ChangeState(player.downState);
    }
}
