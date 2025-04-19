using UnityEngine;

public class M02_GroundedState : EnemyState
{
    protected Monster_02 enemy;

    protected Transform player;

    public M02_GroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_02 _enemy) : base(_enemy, _stateMachine, _animBoolName)
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
