using NUnit.Framework.Internal;
using Photon.Pun;
using UnityEngine;

public class Ability_Animal : PlayerAbility
{
    public RuntimeAnimatorController animalKirby;
    
    public override void OnAbilityCopied(Player owner) //변신을 적용합니다.
    {
        base.OnAbilityCopied(owner);
        animalKirby = Resources.Load<RuntimeAnimatorController>("Test/Animal_Kirby"); //바꿀 애니메이터 파일을 찾아 저장합니다.
        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = animalKirby; //플레이어에게 애니메이터 파일을 바꿔넣습니다.
        for (int i = 0; i < animalKirby.animationClips.Length; i++)
        {
            owner.GetComponentInChildren<PhotonAnimatorView>().SetParameterSynchronized(animalKirby.animationClips[i].name, PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
        }
        
        Debug.Log("Animal ability copied");
    }

    public override void OnAbilityDestroyed(Player owner) //변신 초기화 입니다.
    {
        base.OnAbilityDestroyed(owner); //커비 변신 초기화
        Debug.Log("Animal ability destroyed");
    }
    public override void AttackHandle()
    {
        Debug.Log("Animal ability attack");
    }
}
