using UnityEngine;

public class M_Sword_GroundedState : EnemyState
{
    protected Monster_Sword enemy;

    protected Transform player;
    public M_Sword_GroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Sword _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
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
    }
}
