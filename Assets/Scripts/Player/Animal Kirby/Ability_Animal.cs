using NUnit.Framework.Internal;
using Photon.Pun;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Ability_Animal : PlayerAbility
{
    public RuntimeAnimatorController animalKirby;

    public Animal_Kirby_Attack_State attackState;
    public Animal_Kirby_Dash_Attack_State dash_attackState;
    public Animal_Kirby_Down_Attack_State down_attackState;


    public override void OnAbilityCopied(Player owner) //������ �����մϴ�.
    {
        base.OnAbilityCopied(owner);
        owner.KirbyFormNum = 2; //Ŀ�� ���� �ʱ�ȭ
        PhotonView pView = owner.GetComponent<PhotonView>();
        animalKirby = Resources.Load<RuntimeAnimatorController>("Test/Animal_Kirby"); //�ٲ� �ִϸ����� ������ ã�� �����մϴ�.

        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = animalKirby; //�÷��̾�� �ִϸ����� ������ �ٲ�ֽ��ϴ�.

        //pView.RPC("Change_Animator_Controller", RpcTarget.AllBuffered, pView.ViewID);
        owner.Change_Animator_Controller(pView.ViewID);
        AddState(owner);
        Debug.Log("Animal ability copied");
        
    }

    public override void OnAbilityDestroyed(Player owner) //���� �ʱ�ȭ �Դϴ�.
    {
        base.OnAbilityDestroyed(owner); //Ŀ�� ���� �ʱ�ȭ

        RemoveState(owner); 
        Debug.Log("Animal ability destroyed");
        Destroy(owner.gameObject.GetComponent<Ability_Animal>());
    }

    public override void AttackHandle()
    {
        if (owner == null) return;
        SFX_Name = "Player_Animal_Attack";
        attackCheckRadius = 1.1f; //���� ���� ����
        attackPower = 10; //���ݷ� ����

        owner.stateMachine.ChangeState(attackState);        

    }

    public override void DashAttackHandle()
    {
        if(owner == null) return;
        //SFX_Name = "Player_Animal_Dash_Attack";
        attackCheckRadius = 0.8f; //���� ���� ����
        attackPower = 10; //���ݷ� ����

        owner.stateMachine.ChangeState(dash_attackState);
    }

    public override void DownAttackHandle()
    {
        PlayerState prevState = owner.stateMachine.state;
        Debug.Log(prevState);
        if (owner == null) return;
        if (owner.stateMachine.state is PlayerGroundState) return;

        //SFX_Name = "Player_Animal_Dash_Attack";
        attackCheckRadius = 0.8f; //���� ���� ����
        attackPower = 10; //���ݷ� ����

        owner.stateMachine.ChangeState(down_attackState);
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
        attackState = new Animal_Kirby_Attack_State(owner, owner.stateMachine, "Attack_1");
        dash_attackState = new Animal_Kirby_Dash_Attack_State(owner, owner.stateMachine, "Attack_Dash");
        down_attackState = new Animal_Kirby_Down_Attack_State(owner, owner.stateMachine, "Attack_Down");
    }
    public void RemoveState(Player owner)
    {
        attackState = null;
    }

}
