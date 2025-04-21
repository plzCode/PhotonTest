using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class M_Sword_AttackState : M_Sword_GroundedState
{
    public M_Sword_AttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Sword _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
    }


    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.moveState);
        }

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.SetZeroVelocity();
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }

    
}
