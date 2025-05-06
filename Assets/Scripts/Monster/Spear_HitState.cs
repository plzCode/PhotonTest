using Photon.Pun;
using UnityEngine;

public class Spear_HitState : EnemyState
{

    protected Enemy enemy;
    private bool isGrounded;

    public Spear_HitState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemyBase;
    }

    public override void Enter()
    {
        base.Enter();

        isGrounded = false;
        stateTimer = enemy.hitTime;
        // 시간 동기화
        if (Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            stateTimer = enemy.idleTime;
            enemy.photonView.RPC("SyncStateTimer", Photon.Pun.RpcTarget.Others, (float)stateTimer);
        }

        
        

        //enemy.fx.InvokeRepeating("RedColorBlink", 0, 0.1f);             
               

        rb.linearVelocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }
    public override void Update()
    {
        base.Update();
        if(enemy.IsGroundDetected()&&!isGrounded&&stateTimer<enemy.hitTime-0.15f)
        {
            Debug.Log("팅김");
            isGrounded = true;
            rb.linearVelocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x * 0.6f, enemy.stunDirection.y * 0.6f);
        }
        if (stateTimer < 0)
        {
            if(enemy.currentHp<=0)
            {
                enemy.photonView.RPC("Die", RpcTarget.All);
            }
            else
            {
                enemy.photonView.RPC("ChangeState", Photon.Pun.RpcTarget.All, "Idle");
            }
                
        }

    }

    public override void Exit()
    {
        //enemy.fx.Invoke("CancelRedBlink", 0);
        base.Exit();

        
    }
}
