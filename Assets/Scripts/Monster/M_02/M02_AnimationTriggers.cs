using UnityEngine;

public class M02_AnimationTriggers : MonoBehaviour
{
    [SerializeField] private GameObject spearPrefab;
    private Monster_02 enemy => GetComponentInParent<Monster_02>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void ThrowSpear()
    {
        Instantiate(spearPrefab, transform.position, Quaternion.identity);
    }
}
