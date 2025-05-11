using Photon.Pun;
using UnityEngine;

public class DDD_MoveState : BossState
{
    private float AttackTime;

    public DDD_MoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AttackTime = 0f;
    }

    public override void Update()
    {
        base.Update();

        AttackTime += Time.deltaTime;

        

        boss.SetVelocity(5f * boss.facingDir, rb.linearVelocityY);
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (Vector2.Distance(closestPlayer.position, boss.transform.position) <= 3.5f || AttackTime > 4f) //3.5f �̳��� ������ ���� or 4���̻� ������ ����
        {
            randAttackCount = Random.Range(1, 4);

            switch (randAttackCount)
            {
                case 1:
                    boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack1");
                    break;
                case 2:
                    boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack2");
                    break;
                case 3:
                    boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack3");
                    break;
            }
            //boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }

        //�÷��̾ ���� �����ϰ��ִµ� �̹� �����Ĺ�������
        if ((closestPlayer.position.x < boss.transform.position.x && boss.facingDir == 1) || (closestPlayer.position.x > boss.transform.position.x && boss.facingDir == -1))
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
