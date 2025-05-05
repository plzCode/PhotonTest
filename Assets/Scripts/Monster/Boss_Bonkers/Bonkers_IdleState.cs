using Photon.Pun;
using UnityEngine;

public class Bonkers_IdleState : BossState
{
    

    public Bonkers_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = boss.idleTime;
    }

    public override void Update()
    {
        base.Update();

        if (!PhotonNetwork.IsMasterClient)
            return;

        if (specificTime < 0)
        {
            //특정 행동패턴을 발생
        }

        if(stateTimer<0)
        {
            if(isJumpTurn) // 점프턴
            {
                isJumpTurn = false;
                // 조건:  플레이어 존재 && 거리 8 이하
                if (closestPlayer != null && Vector2.Distance(closestPlayer.position, boss.transform.position) <= 8f)
                {


                    if ((boss.transform.position.x < closestPlayer.position.x) && boss.facingDir == -1)
                    {
                        boss.photonView.RPC("FlipRPC", RpcTarget.All);
                    }
                    if ((boss.transform.position.x > closestPlayer.position.x) && boss.facingDir == 1)
                    {
                        boss.photonView.RPC("FlipRPC", RpcTarget.All);
                    }

                    if (Vector2.Distance(closestPlayer.position, boss.transform.position) >= 5f)
                    {
                        randomJumpCount = 1;
                    }
                    else
                    {
                        randomJumpCount = Random.Range(2, 4); // 2~3
                    }
                    boss.photonView.RPC("ChangeAnimInteger", RpcTarget.All, "JumpCount", randomJumpCount);
                    boss.photonView.RPC("ChangeState", RpcTarget.All, "Jump");
                }

                
            }
            else // 점프턴이 아닐때 -> 제자리공격(폭탄 던지기 or 내려찍기) 또는 백워킹후내려찍기(후 추적연계) // 멀면 추적이동
            {
                isJumpTurn = true;

                if ((boss.transform.position.x < closestPlayer.position.x) && boss.facingDir == -1)
                {
                    boss.photonView.RPC("FlipRPC", RpcTarget.All);
                }
                if ((boss.transform.position.x > closestPlayer.position.x) && boss.facingDir == 1)
                {
                    boss.photonView.RPC("FlipRPC", RpcTarget.All);
                }
                if (closestPlayer != null && Vector2.Distance(closestPlayer.position, boss.transform.position) >= 7f) //너무멀면 쫒아오도록
                {
                    boss.photonView.RPC("ChangeState", RpcTarget.All, "Move");
                }
                else
                {
                    randAttackCount = Random.Range(1, 4);
                    boss.photonView.RPC("ChangeAnimInteger", RpcTarget.All, "AttackCount", randAttackCount);
                    boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack");
                }
                
            }                    



            
        }        


    }

    public override void Exit()
    {
        base.Exit();

        

    }

    
}
