using Photon.Pun;
using UnityEngine;
using WebSocketSharp;

public class PlayerEating12State : PlayerState
{
    public PlayerEating12State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //player.AttackAdd(player.LastMove, player.EatEffect1, player.EatEffectPos, player.transform);
        pView.RPC("AttackAdd", RpcTarget.All, player.LastMove, player.EatEffect1.name, player.EatEffectPos.position, pView.ViewID);
        AudioManager.Instance.RPC_PlaySFX("Inhale_Sound");
    }

    public override void Exit()
    {
        base.Exit();
        //player.AttackDestroy();
        pView.RPC("AttackDestroy", RpcTarget.All);
        AudioManager.Instance.RPC_StopSFX();

    }

    public override void Update()
    {
        base.Update();
        if(pView.IsMine == false) return;
        if (Input.GetKeyUp(KeyCode.Mouse0))
            stateMachine.ChangeState(player.eatingEndState);
    }
}
