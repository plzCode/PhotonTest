using Photon.Realtime;
using System;
using UnityEngine;

public class PlayerDashState : PlayerGroundState
{
    public float EffectTime;
    public bool Effect;
    public bool Effect2;

    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.dash = false;
        player.dashTime = 0;
        Effect = true;
        Effect2 = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.lineVelocity(xInput * player.DashSpeed, rb.linearVelocityY);

        if(Effect)
        {
            EffectTime += Time.deltaTime;
        }

        if (EffectTime > 0.05 && Effect2)
        {
            player.EffectAdd(xInput, player.dashEffect, player.dashEffectPos);
            Effect2 = false;
            EffectTime = 0;
        }


        if (EffectTime > 0.2 && Effect)
        {
            player.EffectAdd(xInput, player.dashEffect, player.dashEffectPos);
            Effect = false;
            EffectTime = 0;
        }

        if (xInput > 0 && player.turn)
        {
            stateMachine.ChangeState(player.dashTurnState);
            player.EffectAdd(-1, player.dashEffect, player.dashEffectPos);
            return;
        }
        else if (xInput < 0 && !player.turn)
        {
            stateMachine.ChangeState(player.dashTurnState);
            player.EffectAdd(1, player.dashEffect, player.dashEffectPos);
            return;
        }

        if (xInput == 0)
        {
            player.dash = true;
            
            if (player.dashTime > 0.05)
            {
                player.dash = false;
                stateMachine.ChangeState(player.idleState);
            }
        }

        if (!player.IsGroundCheck())
            stateMachine.ChangeState(player.airState);
    }
}
