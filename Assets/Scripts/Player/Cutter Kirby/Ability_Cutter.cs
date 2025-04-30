using Photon.Pun;
using UnityEngine;

public class Ability_Cutter : PlayerAbility
{
    public RuntimeAnimatorController cutterKirby;

    public Cutter_Kirby_Attack1_State attack1State;


    public override void OnAbilityCopied(Player owner) //������ �����մϴ�.
    {
        base.OnAbilityCopied(owner);
        owner.KirbyFormNum = 2; //Ŀ�� ���� �ʱ�ȭ
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

        owner.stateMachine.ChangeState(attack1State);

    }

    public void Update()
    {

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
        attack1State = new Cutter_Kirby_Attack1_State(owner, owner.stateMachine, "Attack_1");
    }

    public void RemoveState(Player owner)
    {
        attack1State = null;
    }
}
