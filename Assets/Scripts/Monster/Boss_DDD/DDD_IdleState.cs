using Photon.Pun;
using UnityEngine;

public class DDD_IdleState : BossState
{
    public DDD_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        isJumpTurn = true;
        stateTimer = boss.idleTime;

        //if (!boss.IsGroundDetected() && PhotonNetwork.IsMasterClient)
        //{
        //    boss.photonView.RPC("ChangeState", RpcTarget.All, "Jump");
        //}
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
                    randAttackCount = Random.Range(1, 7);

                switch (randAttackCount)
                {
                    case 1:
                        boss.photonView.RPC("ChangeState", RpcTarget.All, "Move");
                        break;
                    case 2:
                        boss.photonView.RPC("ChangeState", RpcTarget.All, "Jump");
                        break;
                    case 3:
                        boss.photonView.RPC("ChangeState", RpcTarget.All, "Air_Jump");
                        break;
                    case 4:
                        boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack1");
                        break;
                    case 5:
                        boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack2");
                        break;
                    case 6:
                        boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack3");
                        break;
                }
            }
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
