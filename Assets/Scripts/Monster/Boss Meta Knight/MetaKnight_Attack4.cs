using Photon.Pun;
using UnityEngine;

public class MetaKnight_Attack4 : BossState
{
    public bool bossDash;
    public MetaKnight_Attack4(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
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
        bossDash = false;
    }

    public override void Update()
    {
        base.Update();

        if (boss.isJump)
        {
            boss.SetVelocity(6 * boss.facingDir, 0);
        }

        if (bossDash)
        {
            boss.SetVelocity(8 * boss.facingDir, 0);
        }

        if (!PhotonNetwork.IsMasterClient)
            return;

        if (triggerCalled)
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }

    }
}
