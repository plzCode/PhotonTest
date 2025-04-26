using Photon.Pun;
using UnityEngine;

public class Ability_Eat : PlayerAbility
{
    public RuntimeAnimatorController EatKirby;

    public override void OnAbilityCopied(Player owner) //������ �����մϴ�.
    {
        base.OnAbilityCopied(owner);

        PhotonView pView = owner.GetComponent<PhotonView>();
        EatKirby = Resources.Load<RuntimeAnimatorController>("Test/Eat_Kirby"); //�ٲ� �ִϸ����� ������ ã�� �����մϴ�.
        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = EatKirby; //�÷��̾�� �ִϸ����� ������ �ٲ�ֽ��ϴ�.

        pView.RPC("Change_Animator_Controller", RpcTarget.AllBuffered, pView.ViewID);
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
