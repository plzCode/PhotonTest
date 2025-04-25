using UnityEngine;

public class Ability_Eat : PlayerAbility
{
    public RuntimeAnimatorController EatKirby;

    public override void OnAbilityCopied(Player owner) //변신을 적용합니다.
    {
        base.OnAbilityCopied(owner);
        EatKirby = Resources.Load<RuntimeAnimatorController>("Test/Eat_Kirby"); //바꿀 애니메이터 파일을 찾아 저장합니다.
        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = EatKirby; //플레이어에게 애니메이터 파일을 바꿔넣습니다.
        Debug.Log("Animal ability copied");
    }

    public override void OnAbilityDestroyed(Player owner) //변신 초기화 입니다.
    {
        base.OnAbilityDestroyed(owner);
        owner.KirbyFormNum = 0; //커비 변신 초기화
        Debug.Log("Animal ability destroyed");
    }
    public override void AttackHandle()
    {
        Debug.Log("Animal ability attack");
    }
}
