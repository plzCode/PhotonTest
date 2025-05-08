using Photon.Pun;
using UnityEngine;

public class Wheel_TurnState : EnemyState
{
    private Enemy enemy;
    public Wheel_TurnState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase;
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.Flip();
    }

    public override void Update()
    {
        base.Update();
    }
}
