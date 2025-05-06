using Photon.Pun;
using UnityEngine;

public class Whell_Kirby_Attack_State : PlayerState
{
    private PlayerState nextState;
    public Whell_Kirby_Attack_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public void SetNextState(PlayerState _nextState)
    {
        nextState = _nextState;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine)
            return;

        player.lineVelocity(player.LastMove * player.MoveSpeed * 3f, rb.linearVelocityY);

        if(Input.GetKeyDown(KeyCode.S))
        {
            player.stateMachine.ChangeState(player.idleState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundCheck())
        {
            pView.RPC("lineVelocity", RpcTarget.All, rb.linearVelocityX, player.JumpPower + 5f);
        }

        if (player.LastMove == 1f)
        {
            if (xInput == -1f)
            {
                player.stateMachine.ChangeState(nextState);
            }
        }

        if (player.LastMove == -1f)
        {
            if (xInput == 1f)
            {
                player.stateMachine.ChangeState(nextState);
            }
        }
    }
}
