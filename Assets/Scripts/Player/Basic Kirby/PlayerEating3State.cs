using UnityEngine;
using Photon.Pun;
using WebSocketSharp;

public class  PlayerEating3State : PlayerState
{
    public PlayerEating3State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Eating3 : " + player);
        pView.RPC("AttackAdd", RpcTarget.All, player.LastMove, player.EatEffect2.name, player.EatEffectPos.position, pView.ViewID);
        AudioManager.Instance.RPC_PlaySFX("Inhale_Sound_Cut");
    }

    public override void Exit()
    {
        base.Exit();
        pView.RPC("AttackDestroy", RpcTarget.All);
        AudioManager.Instance.RPC_StopSFX();
    }

    public override void Update()
    {
        base.Update();
        if (pView.IsMine == false) return;
        if (Input.GetKeyUp(KeyCode.Mouse0))
            stateMachine.ChangeState(player.eatingEndState);
    }
}
