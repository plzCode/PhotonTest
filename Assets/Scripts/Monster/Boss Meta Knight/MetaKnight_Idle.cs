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

        if (stateTimer < 0)
        {
            // 조건:  플레이어 존재 && 거리 8 이하
            if (closestPlayer != null && Vector2.Distance(closestPlayer.position, boss.transform.position) < 7f)
            {


                if ((boss.transform.position.x < closestPlayer.position.x) && boss.facingDir == -1)
                {
                    boss.photonView.RPC("FlipRPC", RpcTarget.All);
                }
                if ((boss.transform.position.x > closestPlayer.position.x) && boss.facingDir == 1)
                {
                    boss.photonView.RPC("FlipRPC", RpcTarget.All);
                }
                else
                {
                    randomJumpCount = Random.Range(1,6); // 2~3
                    switch (randomJumpCount)
                    {
                        case 1:
                            randAttackCount = Random.Range(1, 5);
                            boss.photonView.RPC("ChangeAnimInteger", RpcTarget.All, "Attack_Count", randAttackCount);
                            boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack1");
                            break;
                        case 2:
                            randAttackCount = Random.Range(1, 4);
                            boss.photonView.RPC("ChangeAnimInteger", RpcTarget.All, "Attack_Count", randAttackCount);
                            boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack2");
                            break;
                        case 3:
                            randAttackCount = Random.Range(1, 5);
                            boss.photonView.RPC("ChangeAnimInteger", RpcTarget.All, "Attack_Count", randAttackCount);
                            boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack3");
                            break;
                        case 4:
                            randAttackCount = Random.Range(1, 4);
                            boss.photonView.RPC("ChangeAnimInteger", RpcTarget.All, "Attack_Count", randAttackCount);
                            boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack4");
                            break;
                        case 5:
                            randAttackCount = Random.Range(1, 100);
                            if (randAttackCount < 70)
                            {
                                return;
                            }
                            else boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack5");
                            break;
                    }
                }
            }



            else if (closestPlayer != null && Vector2.Distance(closestPlayer.position, boss.transform.position) > 7f) //너무멀면 쫒아오도록
            {
                if ((boss.transform.position.x < closestPlayer.position.x) && boss.facingDir == -1)
                {
                    boss.photonView.RPC("FlipRPC", RpcTarget.All);
                }
                if ((boss.transform.position.x > closestPlayer.position.x) && boss.facingDir == 1)
                {
                    boss.photonView.RPC("FlipRPC", RpcTarget.All);
                }
                else
                {
                    randomJumpCount = Random.Range(1, 5);

                    switch (randomJumpCount)
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
}
