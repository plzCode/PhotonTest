using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class M03_GroundedState : EnemyState
{
    protected Monster_03 enemy;

    protected Transform player;

    public M03_GroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_03 _enemy) : base(_enemy, _stateMachine, _animBoolName)
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
