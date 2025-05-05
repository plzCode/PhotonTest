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
            //Ư�� �ൿ������ �߻�
        }

        if(stateTimer<0)
        {
            if(isJumpTurn) // ������
            {
                isJumpTurn = false;
                // ����:  �÷��̾� ���� && �Ÿ� 8 ����
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
            else // �������� �ƴҶ� -> ���ڸ�����(��ź ������ or �������) �Ǵ� ���ŷ�ĳ������(�� ��������) // �ָ� �����̵�
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
                if (closestPlayer != null && Vector2.Distance(closestPlayer.position, boss.transform.position) >= 7f) //�ʹ��ָ� �i�ƿ�����
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
