using UnityEngine;

public class M_Mole_BattleState : EnemyState
{
    private Transform player;
    private Monster_Mole enemy;
    private float facingPlayer;
    private bool isJumping;

    public M_Mole_BattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Mole _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;



        if (player.position.x > enemy.transform.position.x )
        {
            facingPlayer = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            facingPlayer = -1;
        }

        enemy.FlipController(facingPlayer);
    }

    public override void Update()
    {
        base.Update();

        if(!enemy.IsGroundDetected())
        {
            Debug.Log("¶¥À» ¶«");
            isJumping = true;
            
        }
        if(enemy.IsGroundDetected() && isJumping)
        {
            isJumping = false;
            stateMachine.ChangeState(enemy.idleState);            
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    
}
