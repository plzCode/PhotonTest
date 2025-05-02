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
        //���ϸ����Ϳ��� idle�� ������
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

        player.KirbyFormNum = player.EatKirbyFormNum; //����Ǿ��ִ� �� �ѹ��� �����ϴ� �ѹ��� �ѱ��
        player.GetComponent<PhotonView>().RPC("SyncFormNum", RpcTarget.AllBuffered);

        if (player.KirbyFormNum > 0) //0�̻��̸� ���� �� Ŀ��� ����
        {
            //player.KirbyFrom(); //����
            player.Call_RPC("KirbyForm", RpcTarget.All); //����
            player.stateMachine.ChangeState(player.changeFormState);
        }
        else
        {
            player.curAbility.OnAbilityDestroyed(player); //0�̸� �⺻ Ŀ��� ��ȯ
            Destroy(player.curAbility);
            player.stateMachine.ChangeState(player.idleState);
            Debug.Log("Change Form");
        }
    }

    public void AttackTrigger()
    {
        if(player.curAbility == null) return; //�����Ƽ�� ������ ����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.curAbility.attackCheckRadius);
        Debug.Log(player.curAbility.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Debug.Log("������" + player.curAbility.attackPower+ "��ŭ �������� ��");
                hit.GetComponent<Enemy>().photonView.RPC("TakeDamage", RpcTarget.All,player.curAbility.attackPower); // ������ ó��
                //hit.gameObject.SetActive(false);  //�ӽ÷�
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
            rangeAttack = Resources.Load<GameObject>(rangeAttackName); //���Ÿ� ������ ������
        }

        Vector2 Pos = new Vector2 (player.transform.position.x, player.transform.position.y + 5);

        player.EffectAdd(player.LastMove, rangeAttack, player.AirJumpOutEffectPos); //�÷��̾� ���¼� ���� �߻�
    }
}
