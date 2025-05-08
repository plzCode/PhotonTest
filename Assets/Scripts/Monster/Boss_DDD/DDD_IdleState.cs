using Photon.Pun;
using UnityEngine;

public class DDD_IdleState : BossState
{
    private int moveNumber;

    public DDD_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        isJumpTurn = true;
        stateTimer = boss.idleTime;
    }

    public override void Update()
    {
        base.Update();

        if (!PhotonNetwork.IsMasterClient)
            return;


        if (stateTimer < 0)
        {
            if (isJumpTurn) //�÷��̾ �ָ��ִٸ� ������ �� �� ���ݽ���
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
                if (closestPlayer != null && Vector2.Distance(closestPlayer.position, boss.transform.position) >= 7f) //�ʹ��ָ� �i�ƿ�����
                {
                    moveNumber = Random.Range(1, 4);

                    switch (moveNumber)
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
                    }
                }
                else
                {
                    randAttackCount = Random.Range(2, 5);
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
