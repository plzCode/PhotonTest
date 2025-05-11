using Photon.Pun;
using UnityEngine;

public class MetaKnight_Attack2 : BossState
{
    public MetaKnight_Attack2(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if (!boss.isJump)
        {
            boss.SetVelocity(8 * boss.facingDir, 0);
        }


        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (triggerCalled)
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }
    }
}
