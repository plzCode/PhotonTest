using NUnit.Framework.Internal;
using Photon.Pun;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Ability_Animal : PlayerAbility
{
    public RuntimeAnimatorController animalKirby;

    public Animal_Kirby_Attack_State attackState;

    

    public override void OnAbilityCopied(Player owner) //변신을 적용합니다.
    {
        base.OnAbilityCopied(owner);
        
        PhotonView pView = owner.GetComponent<PhotonView>();
        animalKirby = Resources.Load<RuntimeAnimatorController>("Test/Animal_Kirby"); //바꿀 애니메이터 파일을 찾아 저장합니다.

        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = animalKirby; //플레이어에게 애니메이터 파일을 바꿔넣습니다.

        pView.RPC("Change_Animator_Controller", RpcTarget.AllBuffered, pView.ViewID);
        AddState(owner);
        Debug.Log("Animal ability copied");
        
    }

    public override void OnAbilityDestroyed(Player owner) //변신 초기화 입니다.
    {
        base.OnAbilityDestroyed(owner); //커비 변신 초기화
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
