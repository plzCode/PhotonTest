using Photon.Pun;
using UnityEngine;

public class Ability_Whell : PlayerAbility
{
    public RuntimeAnimatorController whellKirby;

    public Whell_Kirby_Attack_Ready_State attackReady;
    public Whell_Kirby_Attack_State attackState;
    public Whell_Kirby_Attack_Turn_State attackTurn;
    public Whell_Kirby_Attack_End attackEnd;

    public float CoolTime = 1f; //쿨타임

    public override void OnAbilityCopied(Player owner) //변신을 적용합니다.
    {
        base.OnAbilityCopied(owner);
        owner.KirbyFormNum = 6; //커비 변신 초기화
        PhotonView pView = owner.GetComponent<PhotonView>();
        whellKirby = Resources.Load<RuntimeAnimatorController>("Test/Whell_Kirby"); //바꿀 애니메이터 파일을 찾아 저장합니다.

        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = whellKirby; //플레이어에게 애니메이터 파일을 바꿔넣습니다.


        owner.Change_Animator_Controller(pView.ViewID);
        AddState(owner);
        owner.stateMachine.ChangeState(attackReady);
        Debug.Log("Whell ability copied");

    }

    public override void OnAbilityDestroyed(Player owner) //변신 초기화 입니다.
    {
        base.OnAbilityDestroyed(owner); //커비 변신 초기화

        RemoveState(owner);
        Debug.Log("Whell ability destroyed");
    }

    public override void AttackHandle()
    {
        if (owner == null) return;

        if (CoolTime > 0) return; //쿨타임이 남아있다면 공격을 하지 않습니다.
        attackCheckRadius = 0.3f;
        attackPower = 5; //공격력 설정
        owner.stateMachine.ChangeState(attackReady);
    }

    public void Update()
    {
        CoolTime -= Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Whell ability hit enemy");
        }
    }

    public void AddState(Player owner)
    {
        attackState = new Whell_Kirby_Attack_State(owner, owner.stateMachine, "Attack", attackEnd); // 일단 null로 넣음
        attackTurn = new Whell_Kirby_Attack_Turn_State(owner, owner.stateMachine, "Attack_Turn", attackState);
        attackReady = new Whell_Kirby_Attack_Ready_State(owner, owner.stateMachine, "Attack_Ready", attackState);
        attackEnd = new Whell_Kirby_Attack_End(owner, owner.stateMachine, "Attack_End");


        attackState.SetNextState(attackTurn);
        attackState.attackEnd = attackEnd;
    }

    public void RemoveState(Player owner)
    {
        attackTurn = null;
        attackState = null;
        attackReady = null;
        attackEnd =null;
    }
}
