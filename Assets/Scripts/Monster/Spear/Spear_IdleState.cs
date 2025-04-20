
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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Vector3.Distance(enemy.transform.position, player.transform.position) <= enemy.throwDistance+2f)
        {
            if (player.position.x < enemy.transform.position.x)
            {
                enemy.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (player.position.x > enemy.transform.position.x)
            {
                enemy.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        

        if (stateTimer < 0)
        {
            

            if (Vector3.Distance(enemy.transform.position, player.transform.position) <= enemy.throwDistance)
            {
                stateMachine.ChangeState(enemy.throwState);
            }

        }

    }
}
