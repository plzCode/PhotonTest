using Photon.Pun;
using UnityEngine;

public class MetaKnight_Attack5 : BossState
{
    public MetaKnight_Attack5(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if (triggerCalled)
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }
    }
}
