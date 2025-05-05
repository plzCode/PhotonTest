using UnityEngine;

public class Eat_Kirby_Attack_State : PlayerState
{
    private GameObject EatAttackEffect;

    public Eat_Kirby_Attack_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.Instance.RPC_PlaySFX("Eat_Kirby_Star_Attack_Sound");
    }

    public override void Exit()
    {
        base.Exit();
        player.curAbility.OnAbilityDestroyed(player);
    }

    public override void Update()
    {
        base.Update();
    }
}
