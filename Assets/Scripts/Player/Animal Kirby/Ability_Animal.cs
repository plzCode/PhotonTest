using NUnit.Framework.Internal;
using Photon.Pun;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Ability_Animal : PlayerAbility
{
    public RuntimeAnimatorController animalKirby;

    public Animal_Kirby_Attack_State attackState;
    public Animal_Kirby_Dash_Attack_State dash_attackState;
    public Animal_Kirby_Down_Attack_State down_attackState;


    public override void OnAbilityCopied(Player owner) //변신을 적용합니다.
    {
        base.OnAbilityCopied(owner);
        owner.KirbyFormNum = 2; //커비 변신 초기화
        PhotonView pView = owner.GetComponent<PhotonView>();
        animalKirby = Resources.Load<RuntimeAnimatorController>("Test/Animal_Kirby"); //바꿀 애니메이터 파일을 찾아 저장합니다.

        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = animalKirby; //플레이어에게 애니메이터 파일을 바꿔넣습니다.

        //pView.RPC("Change_Animator_Controller", RpcTarget.AllBuffered, pView.ViewID);
        owner.Change_Animator_Controller(pView.ViewID);
        AddState(owner);
        Debug.Log("Animal ability copied");
        
    }

    public override void OnAbilityDestroyed(Player owner) //변신 초기화 입니다.
    {
        base.OnAbilityDestroyed(owner); //커비 변신 초기화

        RemoveState(owner); 
        Debug.Log("Animal ability destroyed");
        Destroy(owner.gameObject.GetComponent<Ability_Animal>());
    }

    public override void AttackHandle()
    {
        if (owner == null) return;
        SFX_Name = "Player_Animal_Attack";
        attackCheckRadius = 1.1f; //공격 범위 설정
        attackPower = 10; //공격력 설정

        owner.stateMachine.ChangeState(attackState);        

    }

    public override void DashAttackHandle()
    {
        if(owner == null) return;
        //SFX_Name = "Player_Animal_Dash_Attack";
        attackCheckRadius = 0.8f; //공격 범위 설정
        attackPower = 10; //공격력 설정

        owner.stateMachine.ChangeState(dash_attackState);
    }

    public override void DownAttackHandle()
    {
        PlayerState prevState = owner.stateMachine.state;
        Debug.Log(prevState);
        if (owner == null) return;
        if (owner.stateMachine.state is PlayerGroundState) return;

        //SFX_Name = "Player_Animal_Dash_Attack";
        attackCheckRadius = 0.8f; //공격 범위 설정
        attackPower = 10; //공격력 설정

        owner.stateMachine.ChangeState(down_attackState);
    }

    public void Update()
    {
        
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
        dash_attackState = new Animal_Kirby_Dash_Attack_State(owner, owner.stateMachine, "Attack_Dash");
        down_attackState = new Animal_Kirby_Down_Attack_State(owner, owner.stateMachine, "Attack_Down");
    }
    public void RemoveState(Player owner)
    {
        attackState = null;
    }

}
