using UnityEngine;
using UnityEngine.UIElements;

public class M_Boomerang_AnimationTriggers : MonoBehaviour
{
    private Monster_Sword enemy => GetComponentInParent<Monster_Sword>();
    [SerializeField] private GameObject boomerangPrefab;
    [SerializeField] private Transform startPos;
    private GameObject currentBoomerang;
    private Boomerang_Obj boomerang_Script;
    

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void PickupBoomerang()
    {
        currentBoomerang = Instantiate(boomerangPrefab,startPos.position,Quaternion.identity);
        boomerang_Script = currentBoomerang.GetComponent<Boomerang_Obj>();
        boomerang_Script.SetVelocity(0, 0);
        if (enemy.facingDir==-1) 
        {
            currentBoomerang.transform.Rotate(0, 180, 0);
            boomerang_Script.facingDir = -1;
        }
    }

    private void ThrowBoomerang()
    {
        boomerang_Script.ThrowBoomerang();
    }
    
    
}
