using UnityEngine;

public class Waddle_IdleState : Waddle_GroundedState
{
    public Waddle_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Waddle _enemy) : base(_enemy, _stateMachine, _animBoolName,_enemy)
    {
        
    }
    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }

    }
}
