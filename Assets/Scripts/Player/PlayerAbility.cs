using UnityEditor;
using UnityEngine;
using Photon.Pun;

public abstract class PlayerAbility : MonoBehaviour
{
    protected Player owner;

    [Header("공격 관련")]
    public float attackCheckRadius = 0;
    public float attackPower = 0;

    public string SFX_Name = "";

    enum AibilityNumber 
    {
        Normal, //0
        Eat, //1
        Animal, //2
        Cutter, //3
        Cupid, //4
        Sword, //5
        Whell, //6
    }


    public virtual void OnAbilityCopied(Player owner)
    {
        this.owner = owner;
        /*PhotonAnimatorView animatorView = owner.GetComponentInChildren<PhotonAnimatorView>();
        RuntimeAnimatorController prevAnim = owner.GetComponentInChildren<Animator>().runtimeAnimatorController;
        for (int i = 0; i < prevAnim.animationClips.Length; i++)
        {
            animatorView.SetParameterSynchronized(prevAnim.animationClips[i].name, PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Disabled);
        }
        animatorView.GetSynchronizedParameters().Clear();
        animatorView.GetSynchronizedLayers().Clear();*/

    }

    public virtual void OnAbilityDestroyed(Player owner)
    {
        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = 
        Resources.Load<RuntimeAnimatorController>("Test/Kirby");
        //owner.KirbyFormNum = 0; //커비 변신 초기화
        
        PhotonView pView = owner.GetComponent<PhotonView>();
        //pView.RPC("Change_Animator_Controller", RpcTarget.All, pView.ViewID);
        pView.GetComponent<Player>().Change_Animator_Controller(pView.ViewID);

        Destroy(owner.curAbility);
        this.owner = null;

        
    }

    public abstract void AttackHandle();

    public virtual void UpAttackHandle()
    {
    }

    public virtual void DashAttackHandle() 
    {
    }

    public virtual void DownAttackHandle()
    {
    }


}
