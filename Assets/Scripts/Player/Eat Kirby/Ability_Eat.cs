using Photon.Pun;
using UnityEngine;

public class Ability_Eat : PlayerAbility
{
    private Player player => GetComponentInParent<Player>();

    public RuntimeAnimatorController EatKirby;
    public Eat_Kirby_Attack_State attackState;

    public override void OnAbilityCopied(Player owner) //������ �����մϴ�.
    {
        base.OnAbilityCopied(owner);
        owner.KirbyFormNum = 1; //Ŀ�� ����
        PhotonView pView = owner.GetComponent<PhotonView>();
        EatKirby = Resources.Load<RuntimeAnimatorController>("Test/Eat_Kirby"); //�ٲ� �ִϸ����� ������ ã�� �����մϴ�.
        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = EatKirby; //�÷��̾�� �ִϸ����� ������ �ٲ�ֽ��ϴ�.

        //pView.RPC("Change_Animator_Controller", RpcTarget.AllBuffered, pView.ViewID);
        owner.Change_Animator_Controller(pView.ViewID);
        Debug.Log("Eat ability copied");
        AddState(owner);
    }

    public override void OnAbilityDestroyed(Player owner) //���� �ʱ�ȭ �Դϴ�.
    {
        base.OnAbilityDestroyed(owner);
        owner.KirbyFormNum = 0; //Ŀ�� ���� �ʱ�ȭ
        Debug.Log("Eat ability Destroyed");
    }


    public override void AttackHandle()
    {
        if (owner == null) return;
        owner.stateMachine.ChangeState(attackState);

    }

    public void AddState(Player owner)
    {
        attackState = new Eat_Kirby_Attack_State(owner, owner.stateMachine, "Attack_1");
    }

    public void RemoveState(Player owner)
    {
        attackState = null;
    }
}
