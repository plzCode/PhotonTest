using Photon.Pun;
using UnityEngine;

public class MetaKnight_Dash : BossState
{
    private float AttackTime;

    public MetaKnight_Dash(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AttackTime = 0f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        AttackTime += Time.deltaTime;



        boss.SetVelocity(7f * boss.facingDir, rb.linearVelocityY);
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (Mathf.Abs(closestPlayer.position.x - boss.transform.position.x) <= 3.5f || AttackTime > 4f) //3.5f �̳��� ������ ���� or 4���̻� ������ ����
        {
            randAttackCount = Random.Range(1, 1);

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
            if (boss.IsGroundDetected())
            {
                boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
            }
            else
            {
                boss.photonView.RPC("ChangeState", RpcTarget.All, "Jump");
            }
        }
    }
}
