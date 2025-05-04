using Photon.Pun;
using UnityEngine;

public class Ability_Cupid : PlayerAbility
{
    public RuntimeAnimatorController cupidKirby;

    public Cupid_Kirby_Attack_State attackState;
    public Cupid_Kirby_Direction_State attackDirection;
    public Cupid_Kirby_Attack_End_State attackEnd;


    public override void OnAbilityCopied(Player owner)
    {
        base.OnAbilityCopied(owner);
        owner.KirbyFormNum = 4; //커비 변신 초기화
        PhotonView pView = owner.GetComponent<PhotonView>();
        cupidKirby = Resources.Load<RuntimeAnimatorController>("Test/Cupid_Kirby"); //바꿀 애니메이터 파일을 찾아 저장합니다.

        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = cupidKirby; //플레이어에게 애니메이터 파일을 바꿔넣습니다.

        owner.Change_Animator_Controller(pView.ViewID);
        AddState(owner);
        Debug.Log("Cupid ability copied");
    }

    public override void OnAbilityDestroyed(Player owner)
    {
        base.OnAbilityDestroyed(owner);

        RemoveState(owner);
        Debug.Log("Cupid ability destroyed");
    }

    public override void AttackHandle()
    {
        if (owner == null) return;

        owner.stateMachine.ChangeState(attackState);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Cupid ability hit enemy");
        }
    }

    public void AddState(Player owner)
    {
        attackDirection = new Cupid_Kirby_Direction_State(owner, owner.stateMachine, "Attack_End", attackEnd);
        attackEnd = new Cupid_Kirby_Attack_End_State(owner, owner.stateMachine, "Attack_End");
        attackState = new Cupid_Kirby_Attack_State(owner, owner.stateMachine, "Attack", attackDirection);
    }

    public void RemoveState(Player owner)
    {
        attackState = null;
        attackDirection = null;
        attackEnd = null;
    }
}