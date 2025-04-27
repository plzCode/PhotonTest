using NUnit.Framework.Internal;
using Photon.Pun;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Ability_Animal : PlayerAbility
{
    public RuntimeAnimatorController animalKirby;

    public Animal_Kirby_Attack_State attackState;

    

    public override void OnAbilityCopied(Player owner) //������ �����մϴ�.
    {
        base.OnAbilityCopied(owner);
        
        PhotonView pView = owner.GetComponent<PhotonView>();
        animalKirby = Resources.Load<RuntimeAnimatorController>("Test/Animal_Kirby"); //�ٲ� �ִϸ����� ������ ã�� �����մϴ�.

        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = animalKirby; //�÷��̾�� �ִϸ����� ������ �ٲ�ֽ��ϴ�.

        pView.RPC("Change_Animator_Controller", RpcTarget.AllBuffered, pView.ViewID);
        AddState(owner);
        Debug.Log("Animal ability copied");
        
    }

    public override void OnAbilityDestroyed(Player owner) //���� �ʱ�ȭ �Դϴ�.
    {
        base.OnAbilityDestroyed(owner); //Ŀ�� ���� �ʱ�ȭ
        RemoveState(owner); 
        Debug.Log("Animal ability destroyed");
    }
    public override void AttackHandle()
    {
        if (owner == null) return;
        Debug.Log("Animal ability attack");
        owner.stateMachine.ChangeState(attackState);

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            AttackHandle();
        }
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
    }
    public void RemoveState(Player owner)
    {
        attackState = null;
    }

}
