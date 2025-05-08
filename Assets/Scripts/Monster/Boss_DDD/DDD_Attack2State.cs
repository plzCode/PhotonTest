using Photon.Pun;
using UnityEngine;

public class DDD_Attack2State : BossState
{
    private bool isJumping;

    public DDD_Attack2State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (boss.IsGroundDetected() && PhotonNetwork.IsMasterClient)
        {
            boss.SetVelocity(5f * boss.facingDir, 5f);
            isJumping = true;
        }
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


        if (isJumping)
        {
            Vector2 start = boss.transform.position;
            Vector2 end = closestPlayer.transform.position;
            Vector2 distance = end - start;

            float vx =  2 * distance.x / 1.5f;
            float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
            float vy = (distance.y + 0.5f * gravity * Mathf.Pow(1.5f, 2)) / 1.5f;

            Vector2 velocity = new Vector2(vx, vy);
            rb.linearVelocity = velocity;

            isJumping = false;
        }

        if (Mathf.Abs(closestPlayer.position.x - boss.transform.position.x) <= 3f)
        {
            boss.SetVelocity(boss.facingDir, -5f);
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Attack");
        }

        if (boss.IsGroundDetected())
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }
    }
}
