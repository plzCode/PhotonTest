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

        if (specificTime < 0)
        {
            //Ư�� �ൿ������ �߻�
        }

        if(stateTimer<0)
        {
            // ����: ������ Ŭ���̾�Ʈ && �÷��̾� ���� && �Ÿ� 8 ����
            if (PhotonNetwork.IsMasterClient && closestPlayer != null && Vector2.Distance(closestPlayer.position, boss.transform.position) <= 8f)
            {
                

                if ((boss.transform.position.x < closestPlayer.position.x) && boss.facingDir == -1)
                {
                    boss.photonView.RPC("FlipRPC", RpcTarget.All);
                }
                if ((boss.transform.position.x > closestPlayer.position.x) && boss.facingDir == 1)
                {
                    boss.photonView.RPC("FlipRPC", RpcTarget.All);
                }

                if(Vector2.Distance(closestPlayer.position, boss.transform.position) >= 5f)
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



            if (isJumpTurn)
            {

            }
            else
            {

            }
        }
        


    }

    public override void Exit()
    {
        base.Exit();

        

    }

    
}
