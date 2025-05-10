using Photon.Pun;
using UnityEngine;

public class DDD_AirJumpingState : BossState
{
    private float JumpTime;
    private float JumpOutTime;
    private Vector2 LastPlayer;

    public DDD_AirJumpingState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        JumpTime = 0;
        JumpOutTime = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (PhotonNetwork.IsMasterClient)
        {
            JumpTime += Time.deltaTime;
            JumpOutTime += Time.deltaTime;

            boss.SetVelocity(2f * boss.facingDir, rb.linearVelocityY);

            if (JumpTime >= 0.7f)
            {
                JumpTime = 0f;
                boss.SetVelocity(rb.linearVelocityX, 7f);
            }

            if (Mathf.Abs(closestPlayer.position.x - boss.transform.position.x) <= 1f ||
                (closestPlayer.position.x < boss.transform.position.x && boss.facingDir == 1) ||
                (closestPlayer.position.x > boss.transform.position.x && boss.facingDir == -1) ||
                JumpOutTime > 4f)
            {
                boss.photonView.RPC("ChangeState", RpcTarget.All, "Jump");
            }
        }
    }

}
