using Photon.Pun;
using UnityEngine;

public class MetaKnight_Ready : BossState
{
    public int Count;

    public MetaKnight_Ready(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if (Count > 0)
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "UnReady");
        }
    }
}
