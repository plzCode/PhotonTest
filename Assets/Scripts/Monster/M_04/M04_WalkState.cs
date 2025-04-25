using System.Diagnostics;
using UnityEngine;

public class M04_WalkState : EnemyState
{
    private Monster_Mon04 enemy;
    private Transform player;

    public M04_WalkState(Enemy _enemyBase, EnemyStateMachine _sm, string _animBoolName)
        : base(_enemyBase, _sm, _animBoolName)
    {
        enemy = (Monster_Mon04)_enemyBase;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool("Walk", true); 
        player = GameObject.FindWithTag("Player")?.transform;
    }


    public override void Update()
    {
        base.Update();

        if (player == null)
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        float dist = Vector3.Distance(enemy.transform.position, player.position);


        if (dist <= enemy.attackDistance && Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {


            if (Random.value < 0.5f)
                stateMachine.ChangeState(enemy.attack1State);
            else
                stateMachine.ChangeState(enemy.attack2State);

            return;
        }



        float direction = Mathf.Sign(player.position.x - enemy.transform.position.x);
        enemy.SetVelocity(enemy.moveSpeed * direction, 0);

    
        if (dist > enemy.playerDetectDistance * 1.5f)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.anim.SetBool("Walk", false);
    }
}
