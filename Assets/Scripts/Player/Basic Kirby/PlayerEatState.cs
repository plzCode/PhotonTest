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
        AudioManager.Instance.RPC_PlaySFX("Eat_Sound");
    }

    public override void Exit()
    {
        base.Exit();
        //pView.RPC("KirbyForm", RpcTarget.All); //�Դ����� Ŀ��� ����
        Debug.Log("EatState.cs");
        //player.KirbyForm();
        if (PhotonNetwork.IsMasterClient)
        {
            pView.RPC("KirbyForm", RpcTarget.All); //�Դ����� Ŀ��� ����
        }
        
        //player.KirbyFrom(); //�Դ����� Ŀ��� ����
    }

    public override void Update()
    {
        base.Update();
    }

}
