using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
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
        if (pView.IsMine == false) return;

        if (!player.IsGroundCheck() && !player.isSlope)
            stateMachine.ChangeState(player.airState);


        if(player.isBusy) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if (Input.GetKey(KeyCode.S))
        {
            stateMachine.ChangeState(player.downState);
            //if (player.KirbyFormNum != 1) 
            //{
            //    player.pView.RPC("RPC_ChangeForm", RpcTarget.AllBuffered); 
            //}
        }

        if (IsPointerOverItemElement()) return;



        if (Input.GetKeyDown(KeyCode.Mouse0) && player.curAbility == null)
        {
            
            stateMachine.ChangeState(player.eating12State);
        }

        /*if (Input.GetKeyDown(KeyCode.Mouse0) && player.curAbility != null)
        {
            if (player.dash)
            {
                player.curAbility.DashAttackHandle();
            }

            player.curAbility.AttackHandle();
        }*/
    }
}
