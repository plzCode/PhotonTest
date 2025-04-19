using UnityEngine;

public class Waddle_MoveState : Waddle_GroundedState
{
    public Waddle_MoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Waddle _enemy) : base(_enemy, _stateMachine, _animBoolName,_enemy)
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

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);

        if (enemy.IsWallDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }


    }
}
