using UnityEditor;
using UnityEngine;
using Photon.Pun;

public abstract class PlayerAbility : MonoBehaviour
{
    protected Player owner;

    enum AibilityNumber 
    {
        Normal, //0
        Eat, //1
        Animal, //2
    }


    public virtual void OnAbilityCopied(Player owner)
    {
        this.owner = owner;
        RuntimeAnimatorController prevAnim = owner.GetComponentInChildren<Animator>().runtimeAnimatorController;
        for (int i = 0; i < prevAnim.animationClips.Length; i++)
        {
            owner.GetComponentInChildren<PhotonAnimatorView>().SetParameterSynchronized(prevAnim.animationClips[i].name, PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Disabled);
        }

    }

    public virtual void OnAbilityDestroyed(Player owner)
    {
        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = 
            Resources.Load<RuntimeAnimatorController>("Test/Kirby");
        owner.KirbyFormNum = 0; //Ŀ�� ���� �ʱ�ȭ
        this.owner = null;         
       
    }

    public abstract void AttackHandle();

    
}
