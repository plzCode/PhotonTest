using Photon.Pun;
using UnityEngine;

public class Ability_Eat : PlayerAbility
{
    private Player player => GetComponentInParent<Player>();

    public RuntimeAnimatorController EatKirby;
    public Eat_Kirby_Attack_State attackState;

    public override void OnAbilityCopied(Player owner) //변신을 적용합니다.
    {
        base.OnAbilityCopied(owner);
        owner.KirbyFormNum = 1; //커비 변신
        PhotonView pView = owner.GetComponent<PhotonView>();
        EatKirby = Resources.Load<RuntimeAnimatorController>("Test/Eat_Kirby"); //바꿀 애니메이터 파일을 찾아 저장합니다.
        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = EatKirby; //플레이어에게 애니메이터 파일을 바꿔넣습니다.

        //pView.RPC("Change_Animator_Controller", RpcTarget.AllBuffered, pView.ViewID);
        owner.Change_Animator_Controller(pView.ViewID);
        Debug.Log("Eat ability copied");
        AddState(owner);
    }

    public override void OnAbilityDestroyed(Player owner) //변신 초기화 입니다.
    {
        base.OnAbilityDestroyed(owner);
        owner.KirbyFormNum = 0; //커비 변신 초기화
        Debug.Log("Eat ability Destroyed");
    }


    public override void AttackHandle()
    {
        if (owner == null) return;
        owner.stateMachine.ChangeState(attackState);

    }

    public void AddState(Player owner)
    {
        attackState = new Eat_Kirby_Attack_State(owner, owner.stateMachine, "Attack_1");
    }

    public void RemoveState(Player owner)
    {
        attackState = null;
    }
}
