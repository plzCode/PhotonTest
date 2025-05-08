using Photon.Pun;
using UnityEngine;

public class DDD_Attack1State : BossState
{

    public DDD_Attack1State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if (!PhotonNetwork.IsMasterClient)
            return;

        boss.SetVelocity(7f * boss.facingDir, 0f);

        if (Mathf.Abs(closestPlayer.position.x - boss.transform.position.x) <= 4f)
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack");
        }

        if ((closestPlayer.position.x < boss.transform.position.x && boss.facingDir == 1) || (closestPlayer.position.x > boss.transform.position.x && boss.facingDir == -1))
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack");
        }
    }
}
