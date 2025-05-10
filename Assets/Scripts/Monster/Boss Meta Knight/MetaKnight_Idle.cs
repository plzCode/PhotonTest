using Photon.Pun;
using UnityEngine;

public class MetaKnight_Idle : BossState
{
    public MetaKnight_Idle(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isJumpTurn = true;
        stateTimer = boss.idleTime;
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


        if (stateTimer < 0 && closestPlayer != null)
        {
            if (isJumpTurn) //플레이어가 멀리있다면 가까이 간 후 공격시작
            {
                isJumpTurn = false;

                if ((boss.transform.position.x < closestPlayer.position.x) && boss.facingDir == -1)
                {
                    boss.photonView.RPC("FlipRPC", RpcTarget.All);
                }
                if ((boss.transform.position.x > closestPlayer.position.x) && boss.facingDir == 1)
                {
                    boss.photonView.RPC("FlipRPC", RpcTarget.All);
                }
                randAttackCount = Random.Range(1, 4);

                switch (randAttackCount)
                {
                    case 1:
                        boss.photonView.RPC("ChangeState", RpcTarget.All, "Move");
                        break;
                    case 2:
                        boss.photonView.RPC("ChangeState", RpcTarget.All, "Dash");
                        break;
                    case 3:
                        boss.photonView.RPC("ChangeState", RpcTarget.All, "Jump");
                        break;
                    case 4:
                        boss.photonView.RPC("ChangeState", RpcTarget.All, "Air_Jump");
                        break;
                }
            }
        }
    }
}
