using UnityEngine;

public class Spear_GroundedState : EnemyState
{
    protected Monster_Spear enemy;

    protected Transform player;

    public Spear_GroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Spear _enemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
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
