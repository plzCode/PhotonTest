using Photon.Pun;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public float coolTime;

    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        coolTime = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (pView.IsMine == false) return;

        coolTime += Time.deltaTime;

        if (!player.TimeBool && coolTime > 0.4f)
        {
            player.TimeBool = true;
        }


        if (player.IsGroundCheck())
        {
            //player.EffectAdd(1f, player.GroundStarEffect, player.GroundEffectPos);
            pView.RPC("EffectAdd", RpcTarget.All, 1f, player.GroundStarEffect.name, player.GroundEffectPos.position);
            stateMachine.ChangeState(player.idleState);
            return;
        }

        if(player.isBusy) return;

        if (xInput != 0)
            //player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
            pView.RPC("lineVelocity", RpcTarget.All, xInput * player.MoveSpeed, rb.linearVelocityY); //수평 이동




        if (player.KirbyFormNum == 1) //몹을 입에 담고 있는중 일때 에어점프랑 땅으로 쭉떨어지는 모션 못하게 막음
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.TimeBool)
        {
            //player.lineVelocity(rb.linearVelocityX, player.JumpPower);
            pView.RPC("lineVelocity", RpcTarget.All, rb.linearVelocityX, player.JumpPower);
            stateMachine.ChangeState(player.airJumpState);
        }

        if (rb.linearVelocityY < -8f)
            stateMachine.ChangeState(player.downingState);

        if (IsPointerOverItemElement()) return;

        if (Input.GetKeyDown(KeyCode.Mouse0) && player.curAbility == null)
        {
            stateMachine.ChangeState(player.eating12State);
        }

        /*if (Input.GetKeyDown(KeyCode.Mouse0) && player.curAbility != null)
        {
            player.curAbility.AttackHandle();
        }*/
    }
}
