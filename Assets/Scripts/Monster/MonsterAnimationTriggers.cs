using UnityEngine;

public class MonsterAnimationTriggers : MonoBehaviour
{
    [SerializeField] private GameObject spearPrefab;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void ThrowSpear()
    {
        Instantiate(spearPrefab,transform.position,Quaternion.identity);
    }
}
