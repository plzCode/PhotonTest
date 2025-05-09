using Photon.Pun;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DDD_JumpState : BossState
{
    private bool isJumping;

    public DDD_JumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(boss.IsGroundDetected() && PhotonNetwork.IsMasterClient)
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

        


        if (isJumping)
        {
            Vector2 start = boss.transform.position;
            Vector2 end = closestPlayer.transform.position;
            Vector2 distance = end - start;

            float vx = distance.x / 1.5f;
            float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
            float vy = (distance.y + 0.5f * gravity * Mathf.Pow(1.5f, 2)) / 1.5f;

            Vector2 velocity = new Vector2(vx, vy);
            rb.linearVelocity = velocity;

            isJumping = false;
        }
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (boss.IsGroundDetected())
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }
    }
}
