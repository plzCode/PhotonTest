using UnityEngine;
using Photon.Pun;

public class PlayerChageFormState : PlayerState
{
    public PlayerChageFormState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.lineVelocity(0f, 0f);
        Debug.Log("ChangeFormState.cs");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
