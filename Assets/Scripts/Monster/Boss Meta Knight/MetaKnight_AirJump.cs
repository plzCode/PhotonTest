using Photon.Pun;
using UnityEngine;

public class MetaKnight_AirJump : BossState
{
    public MetaKnight_AirJump(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (boss.IsGroundDetected() && PhotonNetwork.IsMasterClient)
        {
            boss.SetVelocity(5f * boss.facingDir, 5f);
        }
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

        if (boss.IsGroundDetected())
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }
    }
}
