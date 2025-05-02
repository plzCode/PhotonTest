using Photon.Pun;
using UnityEngine;

public class Ability_Cutter : PlayerAbility
{
    public RuntimeAnimatorController cutterKirby;

    public Cutter_Kirby_Attack_State attackState;
    public Cutter_Kirby_Attack_End_State attackEnd;

    public float CoolTime = 1f; //��Ÿ��

    public override void OnAbilityCopied(Player owner) //������ �����մϴ�.
    {
        base.OnAbilityCopied(owner);
        owner.KirbyFormNum = 3; //Ŀ�� ���� �ʱ�ȭ
        PhotonView pView = owner.GetComponent<PhotonView>();
        cutterKirby = Resources.Load<RuntimeAnimatorController>("Test/Cutter_Kirby"); //�ٲ� �ִϸ����� ������ ã�� �����մϴ�.

        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = cutterKirby; //�÷��̾�� �ִϸ����� ������ �ٲ�ֽ��ϴ�.


        owner.Change_Animator_Controller(pView.ViewID);
        AddState(owner);
        Debug.Log("Cutter ability copied");

    }

    public override void OnAbilityDestroyed(Player owner) //���� �ʱ�ȭ �Դϴ�.
    {
        base.OnAbilityDestroyed(owner); //Ŀ�� ���� �ʱ�ȭ

        RemoveState(owner);
        Debug.Log("Cutter ability destroyed");
    }

    public override void AttackHandle()
    {
        if (owner == null) return;

        if (CoolTime > 0) return; //��Ÿ���� �����ִٸ� ������ ���� �ʽ��ϴ�.
        attackPower = 10; //���ݷ� ����
        owner.stateMachine.ChangeState(attackState);
        CoolTime = 0.7f;

    }

    public void Update()
    {
        CoolTime -= Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Cutter ability hit enemy");
        }
    }

    public void AddState(Player owner)
    {
        attackEnd = new Cutter_Kirby_Attack_End_State(owner, owner.stateMachine, "Attack_End");
        attackState = new Cutter_Kirby_Attack_State(owner, owner.stateMachine, "Attack", attackEnd);
    }

    public void RemoveState(Player owner)
    {
        attackState = null;
        attackEnd = null;
    }
}
