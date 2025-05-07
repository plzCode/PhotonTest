using Unity.VisualScripting;
using UnityEngine;

public class Whell_Kirby_Attack_End : PlayerState
{
    public Whell_Kirby_Attack_End(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.Instance.RPC_PlaySFX("kirby_WHEEL_END");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
}
