using UnityEngine;

public enum M_WeaponType
{
    throwing,
    target,
    bounce,
    boom
}

public class MonsterWeapon : MonoBehaviour
{
    [Header("°ø°Ý ½ºÅÈ")]
    [field: SerializeField] public float power { get; private set; }
    [field: SerializeField] public float speed { get; private set; }
    [field: SerializeField] public M_WeaponType weaponType { get; private set; }


}
