using Photon.Pun;
using UnityEngine;

public class Ability_Cutter : PlayerAbility
{
    public RuntimeAnimatorController cutterKirby;

    public Cutter_Kirby_Attack_State attackState;
    public Cutter_Kirby_Attack_End_State attackEnd;

    public float CoolTime = 1f; //쿨타임

    public override void OnAbilityCopied(Player owner) //변신을 적용합니다.
    {
        base.OnAbilityCopied(owner);
        owner.KirbyFormNum = 3; //커비 변신 초기화
        PhotonView pView = owner.GetComponent<PhotonView>();
        cutterKirby = Resources.Load<RuntimeAnimatorController>("Test/Cutter_Kirby"); //바꿀 애니메이터 파일을 찾아 저장합니다.

        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = cutterKirby; //플레이어에게 애니메이터 파일을 바꿔넣습니다.


        owner.Change_Animator_Controller(pView.ViewID);
        AddState(owner);
        Debug.Log("Cutter ability copied");

    }

    public override void OnAbilityDestroyed(Player owner) //변신 초기화 입니다.
    {
        base.OnAbilityDestroyed(owner); //커비 변신 초기화

        RemoveState(owner);
        Debug.Log("Cutter ability destroyed");
    }

    public override void AttackHandle()
    {
        if (owner == null) return;

        if (CoolTime > 0) return; //쿨타임이 남아있다면 공격을 하지 않습니다.
        attackPower = 10; //공격력 설정
        owner.stateMachine.ChangeState(attackState);
        CoolTime = 0.7f;

    }

    public void Update()
    {
        CoolTime -= Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Cutter ability hit enemy");
        }
    }

    public void AddState(Player owner)
    {
        attackEnd = new Cutter_Kirby_Attack_End_State(owner, owner.stateMachine, "Attack_End");
        attackState = new Cutter_Kirby_Attack_State(owner, owner.stateMachine, "Attack", attackEnd);
    }

    public void RemoveState(Player owner)
    {
        attackState = null;
        attackEnd = null;
    }
}
