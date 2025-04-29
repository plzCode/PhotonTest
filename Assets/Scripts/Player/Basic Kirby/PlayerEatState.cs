using Photon.Pun;
using UnityEngine;

public class PlayerEatState : PlayerState
{
    public PlayerEatState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        //pView.RPC("KirbyForm", RpcTarget.All); //먹는중인 커비로 변신
        Debug.Log("EatState.cs");
        player.KirbyForm();
            //player.KirbyFrom(); //먹는중인 커비로 변신
    }

    public override void Update()
    {
        base.Update();
    }

}
