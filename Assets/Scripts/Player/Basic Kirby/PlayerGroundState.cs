using Photon.Pun;
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

        if (!player.IsGroundCheck() && !player.isSlope)
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if (Input.GetKey(KeyCode.S))
        {
            stateMachine.ChangeState(player.downState);
            //if (player.KirbyFormNum != 1) // 이미 변신 중이 아니라면
            //{
            //    player.pView.RPC("RPC_ChangeForm", RpcTarget.AllBuffered);  // 다른 클라이언트들에게도 변신 상태 전달
            //}
        }

        if (player.KirbyFormNum == 1) //몹을 입에 담고 있는중 일때 볼빵빵으로 못 가게 막음
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && player.KirbyFormNum == 0)
            stateMachine.ChangeState(player.eating12State);

        //if (Input.GetKeyDown(KeyCode.Mouse0) && player.curAbility != null)
        //{
        //    player.GetComponent<PhotonView>().RPC(nameof(PerformAttack), RpcTarget.All);
        //}
    }

    //[PunRPC]
    //public void PerformAttack()
    //{
    //    switch (player.KirbyFormNum)
    //    {
    //        case 1:
    //            player.EatKirby.Attack();
    //            break;
    //        case 2:
    //            player.curAbility.AttackHandle();
    //            break;
    //    }
    //}
}
