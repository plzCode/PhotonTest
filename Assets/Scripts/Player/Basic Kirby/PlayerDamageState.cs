using UnityEngine;

public class PlayerDamageState : PlayerState
{
    public PlayerDamageState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.lineVelocity(player.EnemyAttackPos * player.MoveSpeed * 2f, 0);
        player.Flip();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
