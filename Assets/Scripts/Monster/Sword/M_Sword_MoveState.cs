using UnityEngine;

public class M_Sword_MoveState : M_Sword_GroundedState
{
    public M_Sword_MoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Sword _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);

        if (enemy.IsWallDetected()||!enemy.IsGroundDetected())
        {
            enemy.Flip();            
        }

        if (enemy.IsPlayerDetected())
        {

            //stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {

                if (CanAttack()) // 쿨다운이 아니면 공격
                {
                    // sword 몹과 boomerang을 같이 쓰는데 다른점은 combo정도
                    if(enemy.HasParameter("Combo",enemy.anim))
                    {
                        int randomCombo = Random.Range(1, 3);
                        enemy.anim.SetInteger("Combo", randomCombo);
                        stateMachine.ChangeState(enemy.attackState);
                    }
                    else
                    {
                        stateMachine.ChangeState(enemy.attackState);
                    }
                    
                }

            }
        }
        else
        {
            /*
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
            {
                stateMachine.ChangeState(enemy.moveState);
            }
            */
        }
    }


    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }
}
