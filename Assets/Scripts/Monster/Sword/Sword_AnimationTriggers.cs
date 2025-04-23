using UnityEngine;

public class Sword_AnimationTriggers : MonoBehaviour
{

    private Monster_Sword enemy => GetComponentInParent<Monster_Sword>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackForwardStart()
    {
        enemy.Sword_AttackForward();
    }
    private void AttackForwardStop()
    {
        enemy.SetZeroVelocity();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                Debug.Log("�÷��̾��" + enemy.attackPower + "��ŭ �������� ��");
                hit.GetComponent<Player>().TakeDamage(enemy.transform.position,enemy.attackPower);
            }
        }
    }
}
