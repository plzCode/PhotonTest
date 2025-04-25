
using System.Collections;
using UnityEngine;


public class Spear_IdleState : Spear_GroundedState
{
    public Spear_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Spear _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {

    }
    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;

        enemy.startCoru();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Transform closestPlayer = GetClosestPlayer();
        if (closestPlayer == null) return;

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, closestPlayer.position);

        if (distanceToPlayer <= enemy.throwDistance + 2f)
        {
            if (closestPlayer.position.x < enemy.transform.position.x)
            {
                enemy.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (closestPlayer.position.x > enemy.transform.position.x)
            {
                enemy.transform.rotation = Quaternion.Euler(0, 0, 0);
            }


            if (stateTimer < 0)
            {
                if (distanceToPlayer <= enemy.throwDistance)
                {
                    stateMachine.ChangeState(enemy.throwState);
                }
            }

        }

        

        

    }

    
}
