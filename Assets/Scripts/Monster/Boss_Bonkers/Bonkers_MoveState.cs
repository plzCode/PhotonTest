using Photon.Pun;
using UnityEngine;

public class Bonkers_MoveState : BossState
{
    

    public Bonkers_MoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        
    }

    public override void Update()
    {
        base.Update();
        boss.SetVelocity(5 * boss.facingDir, 0);
        if (Vector2.Distance(closestPlayer.position, boss.transform.position) <= 2f)
        {
            randAttackCount = 1;
            boss.photonView.RPC("ChangeAnimInteger", RpcTarget.All, "AttackCount", randAttackCount);
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack");
            //boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }

        //플레이어를 향해 추적하고있는데 이미 지나쳐버렸을때
        if((closestPlayer.position.x<boss.transform.position.x && boss.facingDir==1)|| (closestPlayer.position.x > boss.transform.position.x && boss.facingDir == -1))
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }
       
    }

    public override void Exit()
    {
        base.Exit();
    }

    
}
