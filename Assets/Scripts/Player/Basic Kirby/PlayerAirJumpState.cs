using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAirJumpState : PlayerState
{
    public float MinJumpPower = 2f;

    public PlayerAirJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //player.lineVelocity(rb.linearVelocityX, MinJumpPower);
        pView.RPC("lineVelocity", RpcTarget.All, rb.linearVelocityX, MinJumpPower);
        AudioManager.Instance.RPC_PlaySFX("Air_Jump_up_Sound");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine) return;

        if (rb.linearVelocityY < -0.1f)
            stateMachine.ChangeState(player.airJumpingState);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.airJumpUpState);
            AudioManager.Instance.RPC_PlaySFX("Air_Jump_up_Sound");
        }



        if (xInput != 0)
            pView.RPC("lineVelocity", RpcTarget.All, xInput * player.MoveSpeed, rb.linearVelocityY);
        //player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);


        if (IsPointerOverItemElement()) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.airJumpOutState);
        }
    }
}
