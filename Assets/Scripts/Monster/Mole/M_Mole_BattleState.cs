using Photon.Pun;
using UnityEngine;

public class M_Mole_BattleState : EnemyState
{
    private Transform player;
    private Enemy enemy;
    private float facingPlayer;
    private bool isJumping;

    public M_Mole_BattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameManager.Instance.GetClosestPlayer(enemy.transform.position).GetComponent<Transform>();



        if (player.position.x > enemy.transform.position.x )
        {
            facingPlayer = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            facingPlayer = -1;
        }

        enemy.FlipController(facingPlayer);
        if(PhotonNetwork.IsMasterClient)
        {
            enemy.canAttacking = true;
        }
       
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
            enemy.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.canAttacking = false;
    }



    
}
