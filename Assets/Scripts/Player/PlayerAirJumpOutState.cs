using UnityEngine;

public class PlayerAirJumpOutState : PlayerState
{
    public float EffectTime;
    public bool Effect;

    public PlayerAirJumpOutState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Effect = true;
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

        if (EffectTime > 0.05 && Effect)
        {
            Debug.Log("Ω««‡¡ﬂ");
            player.EffectAdd(player.LastxInput, player.AirJumpOutEffect, player.AirJumpOutEffectPos);
            Effect = false;
            EffectTime = 0;
        }

        if (player.IsGroundCheck())
            stateMachine.ChangeState(player.idleState);
    }
}
