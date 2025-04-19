using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class M02_IdleState : M02_GroundedState
{
    public M02_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_02 _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
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
        if (stateTimer < 0 )
        {
            if (Vector3.Distance(enemy.transform.position, player.transform.position) <= enemy.throwDistance)
            {
                stateMachine.ChangeState(enemy.throwState);
            }

            
        }

    }
}
