using Photon.Pun;
using UnityEngine;

public class MetaKnight_Attack3 : BossState
{
    public MetaKnight_Attack3(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        boss.isJump = false;
    }

    public override void Update()
    {
        base.Update();
        if (boss.isJump)
        {
            boss.SetVelocity(0, -15f);

            if (boss.IsGroundDetected())
            {
                boss.SetVelocity(0, 0);
            }
        }

        if (!PhotonNetwork.IsMasterClient)
            return;

        if (triggerCalled && boss.IsGroundDetected())
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }
    }
}
