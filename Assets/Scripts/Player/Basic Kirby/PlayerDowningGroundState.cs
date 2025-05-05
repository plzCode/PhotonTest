using Photon.Pun;
using UnityEngine;

public class PlayerDowningGroundState : PlayerState
{
    public float Lastmove;

    public PlayerDowningGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //player.lineVelocity(Lastmove * player.MoveSpeed, 2f);
        pView.RPC("lineVelocity", RpcTarget.All, player.LastMove * player.MoveSpeed, 3f); //수평 이동
        AudioManager.Instance.RPC_PlaySFX("Player_Air_Downing_Ground_Sound");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
        pView.RPC("lineVelocity", RpcTarget.All, xInput * player.MoveSpeed, rb.linearVelocityY); //수평 이동

        //에니메이터에서 idle로 가게함
    }
}
