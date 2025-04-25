using NUnit.Framework.Internal;
using Photon.Pun;
using UnityEngine;

public class Ability_Animal : PlayerAbility
{
    public RuntimeAnimatorController animalKirby;
    
    public override void OnAbilityCopied(Player owner) //������ �����մϴ�.
    {
        base.OnAbilityCopied(owner);
        animalKirby = Resources.Load<RuntimeAnimatorController>("Test/Animal_Kirby"); //�ٲ� �ִϸ����� ������ ã�� �����մϴ�.
        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = animalKirby; //�÷��̾�� �ִϸ����� ������ �ٲ�ֽ��ϴ�.
        for (int i = 0; i < animalKirby.animationClips.Length; i++)
        {
            owner.GetComponentInChildren<PhotonAnimatorView>().SetParameterSynchronized(animalKirby.animationClips[i].name, PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
        }
        
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
}
