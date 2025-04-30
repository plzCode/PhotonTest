using Photon.Pun;
using UnityEngine;

public class Ability_Cutter : PlayerAbility
{
    public RuntimeAnimatorController cutterKirby;

    public Cutter_Kirby_Attack1_State attack1State;


    public override void OnAbilityCopied(Player owner) //변신을 적용합니다.
    {
        base.OnAbilityCopied(owner);
        owner.KirbyFormNum = 2; //커비 변신 초기화
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

        owner.stateMachine.ChangeState(attack1State);

    }

    public void Update()
    {

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
        attack1State = new Cutter_Kirby_Attack1_State(owner, owner.stateMachine, "Attack_1");
    }

    public void RemoveState(Player owner)
    {
        attack1State = null;
    }
}
