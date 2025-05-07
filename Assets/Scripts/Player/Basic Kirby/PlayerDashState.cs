using Photon.Pun;
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
        AudioManager.Instance.RPC_PlaySFX("Dash_Sound");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine) return;

        player.lineVelocity(xInput * player.DashSpeed, rb.linearVelocityY);

        if(Effect)
        {
            EffectTime += Time.deltaTime;
        }

        if (EffectTime > 0.05f && Effect2) //����Ʈ �ٷμ�ȯ�ϸ� �뽬 �� �Ҷ� ����Ʈ �����Ǽ� 0.05�ʷ� ��
        {
            //player.EffectAdd(xInput, player.dashEffect, player.dashEffectPos);
            pView.RPC("EffectAdd", RpcTarget.All, xInput, player.dashEffect.name, player.dashEffectPos.position);
            Effect2 = false;
            EffectTime = 0;
        }

        if (EffectTime > 0.2f && Effect)
        {
            //player.EffectAdd(xInput, player.dashEffect, player.dashEffectPos);
            pView.RPC("EffectAdd", RpcTarget.All, xInput, player.dashEffect.name, player.dashEffectPos.position);
            Effect = false;
            EffectTime = 0;
        }

        if (xInput > 0 && player.turn) //�뽬 �� �Ҷ� ����Ʈ�� �޸����� �Ȱ��� ������ �ϱ�
        {
            stateMachine.ChangeState(player.dashTurnState);
            //player.EffectAdd(-1f, player.dashEffect, player.dashEffectPos);
            pView.RPC("EffectAdd", RpcTarget.All, -1f, player.dashEffect.name, player.dashEffectPos.position);
            return;
        }
        else if (xInput < 0 && !player.turn)
        {
            stateMachine.ChangeState(player.dashTurnState);
            //player.EffectAdd(1, player.dashEffect, player.dashEffectPos);
            pView.RPC("EffectAdd", RpcTarget.All, 1f, player.dashEffect.name, player.dashEffectPos.position);
            return;
        }

        if (xInput == 0)
        {
            player.dash = true;
            
            if (player.dashTime > 0.05f)
            {
                player.dash = false;
                stateMachine.ChangeState(player.idleState);
            }
        }

        if (!player.IsGroundCheck() && !player.isSlope)
            stateMachine.ChangeState(player.airState);
    }
}
