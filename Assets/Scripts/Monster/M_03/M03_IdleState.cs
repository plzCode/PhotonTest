using UnityEngine;

public class M03_IdleState : M03_GroundedState
{
    public M03_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_03 _enemy) : base(_enemy, _stateMachine, _animBoolName,_enemy)
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

        Debug.Log(stateTimer);
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }

    }
}
