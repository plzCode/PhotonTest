using UnityEngine;
using Photon.Pun;

public class PlayerAnimatorController : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();


    private void AnimationTrigger()
    {
        player.AnimationFinishTrigger();
    }

    public void DowningGround_Idle_State()
    {
        if (!player.IsGroundCheck() && !player.isSlope)
        {
            player.stateMachine.ChangeState(player.airState);
        }
        else
            player.stateMachine.ChangeState(player.idleState);
        //에니메이터에서 idle로 가게함
    }

    public void Idle_state()
    {
        player.stateMachine.ChangeState(player.idleState);
    }

    public void Air_State()
    {
        player.stateMachine.ChangeState(player.airState);
    }

    public void ChangeForm()
    {
        player.KirbyFormNum = player.EatKirbyFormNum; //저장되어있던 몹 넘버를 변신하는 넘버로 넘긴다

        if (player.KirbyFormNum > 0) //0이상이면 먹은 적 커비로 변신
        {
            //player.KirbyFrom(); //변신
            player.Call_RPC("KirbyFrom", RpcTarget.All); //변신
            player.stateMachine.ChangeState(player.changeFormState);
        }
        else
        {
            player.curAbility.OnAbilityDestroyed(player); //0이면 기본 커비로 변환
            Destroy(player.curAbility);
            player.stateMachine.ChangeState(player.idleState);
        }


    }
}
