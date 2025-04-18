using UnityEngine;

public class Spear_ThrowState : Spear_GroundedState
{
    public Spear_ThrowState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Spear _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
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
