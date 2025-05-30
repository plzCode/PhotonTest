using Photon.Pun;
using UnityEngine;

public class PlayerDanceState : PlayerState
{
    public PlayerDanceState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
        player.isBusy = false;
    }
    public override void Update()
    {
        base.Update();
    }
}
