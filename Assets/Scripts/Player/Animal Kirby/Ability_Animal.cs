using NUnit.Framework.Internal;
using Photon.Pun;
using UnityEngine;

public class Ability_Animal : PlayerAbility
{
    public RuntimeAnimatorController animalKirby;
    
    public override void OnAbilityCopied(Player owner) //������ �����մϴ�.
    {
        base.OnAbilityCopied(owner);
        
        PhotonView pView = owner.GetComponent<PhotonView>();
        animalKirby = Resources.Load<RuntimeAnimatorController>("Test/Animal_Kirby"); //�ٲ� �ִϸ����� ������ ã�� �����մϴ�.
        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = animalKirby; //�÷��̾�� �ִϸ����� ������ �ٲ�ֽ��ϴ�.

        pView.RPC("Change_Animator_Controller", RpcTarget.AllBuffered, pView.ViewID);
        Debug.Log("Animal ability copied");
    }

    public override void OnAbilityDestroyed(Player owner) //���� �ʱ�ȭ �Դϴ�.
    {
        base.OnAbilityDestroyed(owner); //Ŀ�� ���� �ʱ�ȭ
        Debug.Log("Animal ability destroyed");
    }
    public override void AttackHandle()
    {
        Debug.Log("Animal ability attack");    

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

}
