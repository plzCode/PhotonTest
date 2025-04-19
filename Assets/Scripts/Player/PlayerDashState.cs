using Photon.Realtime;
using System;
using UnityEngine;

public class PlayerDashState : PlayerGroundState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public bool last;

    public override void Enter()
    {
        base.Enter();
        player.dashtime = 0;
        player.dash = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.lineVelocity(xInput * player.DashSpeed, rb.linearVelocityY);


        if (xInput == 0)
        {
            player.dash = true;
            
            if (player.dashtime > 0.05)
            {
                player.dash = false;
                stateMachine.ChangeState(player.idleState);
            }
        }

        if (!player.IsGroundCheck())
            stateMachine.ChangeState(player.airState);
    }
}
