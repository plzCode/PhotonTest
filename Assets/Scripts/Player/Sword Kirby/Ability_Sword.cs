using Photon.Pun;
using UnityEngine;

public class Ability_Sword : PlayerAbility
{
    public RuntimeAnimatorController swordKirby;

    public Sword_Kirby_Attack1_State attack1State;
    public Sword_Kirby_Attack2_State attack2State;
    public Sword_Kirby_Attack2_End_State attack2_EndState;
    public Sword_Kirby_Attack3_State attack3State;
    public Sword_Kirby_Attack3_1State attack3_1State;
    public Sword_Kirby_Attack3_2State attack3_2State;
    public Sword_Kirby_Attack3_End_State attack3_EndState;
    public Sword_Kirby_Attack4_State attack4State;



    public override void OnAbilityCopied(Player owner) //������ �����մϴ�.
    {
        base.OnAbilityCopied(owner);
        owner.KirbyFormNum = 5; //Ŀ�� ���� �ʱ�ȭ
        PhotonView pView = owner.GetComponent<PhotonView>();
        swordKirby = Resources.Load<RuntimeAnimatorController>("Test/Sword_Kirby"); //�ٲ� �ִϸ����� ������ ã�� �����մϴ�.

        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = swordKirby; //�÷��̾�� �ִϸ����� ������ �ٲ�ֽ��ϴ�.

        //pView.RPC("Change_Animator_Controller", RpcTarget.AllBuffered, pView.ViewID);
        owner.Change_Animator_Controller(pView.ViewID);
        AddState(owner);
        Debug.Log("Sword ability copied");

    }

    public override void OnAbilityDestroyed(Player owner) //���� �ʱ�ȭ �Դϴ�.
    {
        base.OnAbilityDestroyed(owner); //Ŀ�� ���� �ʱ�ȭ

        RemoveState(owner);
        Debug.Log("Sword ability destroyed");
        Destroy(owner.gameObject.GetComponent<Ability_Sword>());
    }

    public override void AttackHandle()
    {
        if (owner == null) return;
        attackCheckRadius = 1f; //���� ���� ����
        attackPower = 10; //���ݷ� ����

        owner.stateMachine.ChangeState(attack1State);
    }

    public override void UpAttackHandle()
    {
        if (owner == null) return;
        attackCheckRadius = 0.5f; //���� ���� ����
        attackPower = 10; //���ݷ� ����

        owner.stateMachine.ChangeState(attack3State);
    }


    public void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Animal ability hit enemy");
        }
    }

    public void AddState(Player owner)
    {
        attack4State = new Sword_Kirby_Attack4_State(owner, owner.stateMachine, "Attack4");
        attack3_EndState = new Sword_Kirby_Attack3_End_State(owner, owner.stateMachine, "Attack3_End");
        attack3_2State = new Sword_Kirby_Attack3_2State(owner, owner.stateMachine, "Attack3_2", attack3_EndState);
        attack3_1State = new Sword_Kirby_Attack3_1State(owner, owner.stateMachine, "Attack3_1", attack3_2State);
        attack3State = new Sword_Kirby_Attack3_State(owner, owner.stateMachine, "Attack3", attack3_1State);
        attack2_EndState = new Sword_Kirby_Attack2_End_State(owner, owner.stateMachine, "Attack2_End");
        attack2State = new Sword_Kirby_Attack2_State(owner, owner.stateMachine, "Attack2", attack2_EndState);
        attack1State = new Sword_Kirby_Attack1_State(owner, owner.stateMachine, "Attack1", attack2State);
    }
    public void RemoveState(Player owner)
    {
        attack1State = null;
    }
}
