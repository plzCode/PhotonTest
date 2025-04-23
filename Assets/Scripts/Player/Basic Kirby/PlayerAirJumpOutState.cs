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
    }

    public override void Exit()
    {
        base.Exit();
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
            player.EffectAdd(Lastmove, player.AirJumpOutEffect, player.AirJumpOutEffectPos);
            //pView.RPC("EffectAdd", RpcTarget.All, Lastmove, player.AirJumpOutEffect, player.AirJumpOutEffectPos);
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
