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
        //pView.RPC("AttackAdd", RpcTarget.All, player.LastMove, player.EatEffect1.name, player.EatEffectPos.position, pView.ViewID);
        player.AttackAdd(player.LastMove, "ChangeForm Effect 85x85_0", player.transform.position, pView.ViewID);
        AudioManager.Instance.RPC_PlaySFX("Change_Form_Sound");
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
