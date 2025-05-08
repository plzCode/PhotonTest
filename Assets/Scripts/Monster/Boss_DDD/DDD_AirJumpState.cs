using Photon.Pun;
using UnityEngine;

public class DDD_AirJumpState : BossState
{
    private float JumpTime;

    public DDD_AirJumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (!PhotonNetwork.IsMasterClient)
            return;

        boss.SetVelocity(2f * boss.facingDir, 15f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        JumpTime += Time.deltaTime;

        if (!PhotonNetwork.IsMasterClient)
            return;

        if (JumpTime >= 0.5f)
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Air_Jumping");
            JumpTime = 0f;
        }
    }
}
