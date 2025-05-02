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
        player.GetComponent<PhotonView>().RPC("SyncFormNum", RpcTarget.AllBuffered);

        if (player.KirbyFormNum > 0) //0이상이면 먹은 적 커비로 변신
        {
            //player.KirbyFrom(); //변신
            player.Call_RPC("KirbyForm", RpcTarget.All); //변신
            player.stateMachine.ChangeState(player.changeFormState);
        }
        else
        {
            player.curAbility.OnAbilityDestroyed(player); //0이면 기본 커비로 변환
            Destroy(player.curAbility);
            player.stateMachine.ChangeState(player.idleState);
            Debug.Log("Change Form");
        }
    }

    public void AttackTrigger()
    {
        if(player.curAbility == null) return; //어빌리티가 없으면 리턴
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.curAbility.attackCheckRadius);
        Debug.Log(player.curAbility.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Debug.Log("적에게" + player.curAbility.attackPower+ "만큼 데미지를 줌");
                hit.GetComponent<Enemy>().photonView.RPC("TakeDamage", RpcTarget.All,player.curAbility.attackPower); // 데미지 처리
                //hit.gameObject.SetActive(false);  //임시로
            }
        }
    }


    [PunRPC]
    public void EatKirbyStarAttack()
    {
        RangedAttack(Attack, "Player_Effect/Kirby Eat Attack 60x60_0");
    }

    [PunRPC]
    public void CutterKirbyAttack()
    {
        RangedAttack(Attack, "Player_Effect/Cutter");
    }


    private GameObject Attack;

    private void RangedAttack(GameObject rangeAttack, string rangeAttackName)
    {
        if (rangeAttack == null)
        {
            rangeAttack = Resources.Load<GameObject>(rangeAttackName); //원거리 공격을 가져옴
        }

        Vector2 Pos = new Vector2 (player.transform.position.x, player.transform.position.y + 5);

        player.EffectAdd(player.LastMove, rangeAttack, player.AirJumpOutEffectPos); //플레이어 한태서 공격 발사
    }
}
