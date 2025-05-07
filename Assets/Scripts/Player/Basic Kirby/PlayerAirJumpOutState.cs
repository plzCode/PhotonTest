using Photon.Pun;
using UnityEngine;

public class PlayerAirJumpOutState : PlayerState
{
    public float EffectTime;
    public bool Effect;
    public float BackTime;
    public bool Back;

    public PlayerAirJumpOutState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Effect = true;
        Back = true;
        AudioManager.Instance.RPC_PlaySFX("Air_Out_Sound");
        player.commandInput.ClearInputBuffer();  
    }

    public override void Exit()
    {
        base.Exit();
        player.TimeBool = false;
    }

    public override void Update()
    {
        base.Update();

        if (Effect)
        {
            EffectTime += Time.deltaTime;
        }

        if (EffectTime > 0.05f && Effect)
        {
            //player.EffectAdd(player.LastMove, player.AirJumpOutEffect, player.AirJumpOutEffectPos);
            pView.RPC("EffectAdd", RpcTarget.All, player.LastMove, player.AirJumpOutEffect.name, player.AirJumpOutEffectPos.position);
            Effect = false;
            EffectTime = 0;
        }

        if (Back)
        {
            BackTime += Time.deltaTime;
        }

        if (BackTime > 0.1f)
        {
            stateMachine.ChangeState(player.airState);
            Back = false;
            BackTime = 0;
        }
    }
}
