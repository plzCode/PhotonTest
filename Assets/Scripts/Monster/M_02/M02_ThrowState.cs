using UnityEngine;

public class M02_ThrowState : M02_GroundedState
{
    public M02_ThrowState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_02 _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
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

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
