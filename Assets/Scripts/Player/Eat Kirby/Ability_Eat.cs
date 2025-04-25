using Photon.Pun;
using UnityEngine;

public class Ability_Eat : PlayerAbility
{
    public RuntimeAnimatorController EatKirby;

    public override void OnAbilityCopied(Player owner) //������ �����մϴ�.
    {
        base.OnAbilityCopied(owner);
        EatKirby = Resources.Load<RuntimeAnimatorController>("Test/Eat_Kirby"); //�ٲ� �ִϸ����� ������ ã�� �����մϴ�.
        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = EatKirby; //�÷��̾�� �ִϸ����� ������ �ٲ�ֽ��ϴ�.
        for (int i = 0; i < EatKirby.animationClips.Length; i++)
        {
            owner.GetComponentInChildren<PhotonAnimatorView>().SetParameterSynchronized(EatKirby.animationClips[i].name, PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
        }

        Debug.Log("Animal ability copied");
    }

    public override void OnAbilityDestroyed(Player owner) //���� �ʱ�ȭ �Դϴ�.
    {
        base.OnAbilityDestroyed(owner);
        owner.KirbyFormNum = 0; //Ŀ�� ���� �ʱ�ȭ
        Debug.Log("Animal ability destroyed");
    }
    public override void AttackHandle()
    {
        Debug.Log("Animal ability attack");
    }
}
