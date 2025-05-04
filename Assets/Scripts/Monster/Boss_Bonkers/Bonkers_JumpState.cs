using Photon.Pun;
using UnityEngine;

public class Bonkers_JumpState : BossState
{
    
    public Bonkers_JumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {         

        base.Enter();     
       
        
        
    }

    public override void Update()
    {
        base.Update();

        if (!PhotonNetwork.IsMasterClient)
            return;

        if (triggerCalled)
        {
            boss.photonView.RPC("ChangeState",RpcTarget.All,"Idle");
        }

        if(boss.isJump==true&& boss.IsGroundDetected())
        {
            
            boss.photonView.RPC("SetJumpEnd", RpcTarget.All);
        }

        
        switch (randomJumpCount)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }


    public override void Exit()
    {
        base.Exit();

        boss.isJump = false;
        boss.anim.SetBool("JumpEnd", false);
    }

    
}
