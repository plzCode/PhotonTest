using Photon.Pun;
using UnityEngine;

public class MetaKnight_Move : BossState
{
    private float AttackTime;

    public MetaKnight_Move(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        boss.SetVelocity(6 * boss.facingDir, 0);

        if (Vector2.Distance(closestPlayer.position, boss.transform.position) <= 3.5f)
        {
            randAttackCount = Random.Range(1, 5);
            boss.photonView.RPC("ChangeAnimInteger", RpcTarget.All, "Attack_Count", randAttackCount);
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack1");
            //boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }

        //플레이어를 향해 추적하고있는데 이미 지나쳐버렸을때
        if ((closestPlayer.position.x < boss.transform.position.x && boss.facingDir == 1) || (closestPlayer.position.x > boss.transform.position.x && boss.facingDir == -1))
        {
            if (boss.IsGroundDetected())
            {
                boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
            }
            else
            {
                boss.photonView.RPC("ChangeState", RpcTarget.All, "Jump");
            }
        }
    }
}
