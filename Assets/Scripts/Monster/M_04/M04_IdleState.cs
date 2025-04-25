using UnityEngine;

public class M04_IdleState : EnemyState
{
    private Monster_Mon04 enemy;
    private Transform player;
    private float idleTimer;

    public M04_IdleState(Enemy _enemyBase, EnemyStateMachine _sm, string _animBool)
        : base(_enemyBase, _sm, _animBool)
    {
        enemy = (Monster_Mon04)_enemyBase;
    }

    public override void Enter()
    {
        base.Enter();                    
        enemy.SetZeroVelocity();
        player = GameObject.FindWithTag("Player")?.transform;
        idleTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();

       
        if (enemy.IsPlayerDetected().collider != null)
        {
            stateMachine.ChangeState(enemy.walkState);
            return;
        }

      
        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f)
        {
            stateMachine.ChangeState(enemy.walkState);
        }
    }


    public override void Exit()
    {
        base.Exit();                     
    }
}
