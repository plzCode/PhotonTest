using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class Waddle_GroundedState : EnemyState
{
    protected Monster_Waddle enemy;

    protected Transform player;

    public Waddle_GroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Waddle _enemy) : base(_enemy, _stateMachine, _animBoolName)
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
