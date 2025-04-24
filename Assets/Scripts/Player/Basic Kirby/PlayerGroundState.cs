using Photon.Realtime;
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
        if(pView.IsMine == false) return;
        if (!player.IsGroundCheck())
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.jumpState);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.eatingState);

        if (Input.GetKey(KeyCode.S))
            stateMachine.ChangeState(player.downState);
    }
}
