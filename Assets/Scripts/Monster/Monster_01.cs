using UnityEngine;

public class Monster_01 : MonoBehaviour
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
        Instantiate(spearPrefab);
    }
}
