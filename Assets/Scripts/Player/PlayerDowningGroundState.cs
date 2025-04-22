using Photon.Pun;
using UnityEngine;

public class PlayerDowningGroundState : PlayerState
{
    public float Lastmove;
    public int ground;

    public PlayerDowningGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(player.flipbool)
        {
            Lastmove = 1;
        }
        else
        {
            Lastmove = -1;
        }

        //player.lineVelocity(Lastmove * player.MoveSpeed, 2);
        pView.RPC("lineVelocity", RpcTarget.All, Lastmove * player.MoveSpeed, 2f); //수평 이동
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
        pView.RPC("lineVelocity", RpcTarget.All, xInput * player.MoveSpeed, rb.linearVelocityY); //수평 이동

        if (player.IsGroundCheck())
        {
            ground += 1;

            if (player.IsGroundCheck() && ground >= 15)
            {
                ground = 0;
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
}
