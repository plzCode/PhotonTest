using Photon.Pun;
using UnityEngine;

public class Ability_Whell : PlayerAbility
{
    public RuntimeAnimatorController whellKirby;

    public Whell_Kirby_Attack_Ready_State attackReady;
    public Whell_Kirby_Attack_State attackState;
    public Whell_Kirby_Attack_Turn_State attackTurn;
    public Whell_Kirby_Attack_End attackEnd;

    public float CoolTime = 1f; //��Ÿ��

    public override void OnAbilityCopied(Player owner) //������ �����մϴ�.
    {
        base.OnAbilityCopied(owner);
        owner.KirbyFormNum = 6; //Ŀ�� ���� �ʱ�ȭ
        PhotonView pView = owner.GetComponent<PhotonView>();
        whellKirby = Resources.Load<RuntimeAnimatorController>("Test/Whell_Kirby"); //�ٲ� �ִϸ����� ������ ã�� �����մϴ�.

        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = whellKirby; //�÷��̾�� �ִϸ����� ������ �ٲ�ֽ��ϴ�.


        owner.Change_Animator_Controller(pView.ViewID);
        AddState(owner);
        owner.stateMachine.ChangeState(attackReady);
        Debug.Log("Whell ability copied");

    }

    public override void OnAbilityDestroyed(Player owner) //���� �ʱ�ȭ �Դϴ�.
    {
        base.OnAbilityDestroyed(owner); //Ŀ�� ���� �ʱ�ȭ

        RemoveState(owner);
        Debug.Log("Whell ability destroyed");
    }

    public override void AttackHandle()
    {
        if (owner == null) return;

        if (CoolTime > 0) return; //��Ÿ���� �����ִٸ� ������ ���� �ʽ��ϴ�.
        attackCheckRadius = 0.3f;
        attackPower = 5; //���ݷ� ����
        owner.stateMachine.ChangeState(attackReady);
    }

    public void Update()
    {
        CoolTime -= Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Whell ability hit enemy");
        }
    }

    public void AddState(Player owner)
    {
        attackState = new Whell_Kirby_Attack_State(owner, owner.stateMachine, "Attack", attackEnd); // �ϴ� null�� ����
        attackTurn = new Whell_Kirby_Attack_Turn_State(owner, owner.stateMachine, "Attack_Turn", attackState);
        attackReady = new Whell_Kirby_Attack_Ready_State(owner, owner.stateMachine, "Attack_Ready", attackState);
        attackEnd = new Whell_Kirby_Attack_End(owner, owner.stateMachine, "Attack_End");


        attackState.SetNextState(attackTurn);
        attackState.attackEnd = attackEnd;
    }

    public void RemoveState(Player owner)
    {
        attackTurn = null;
        attackState = null;
        attackReady = null;
        attackEnd =null;
    }
}
