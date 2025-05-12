using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptable", menuName = "Item/ItemScriptable", order = 1)]
public class ItemScriptable : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
    public float effectValue;

    // 나중에 분류 용도 (예: 회복, 공격 등)
    public ItemType type;
    public bool isAutoUse; // 즉시 사용 여부


}
public enum ItemType
{
    Heal,
    Ability,
    LifeUp,
}