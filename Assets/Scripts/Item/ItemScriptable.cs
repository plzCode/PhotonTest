using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptable", menuName = "Item/ItemScriptable", order = 1)]
public class ItemScriptable : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
    public float effectValue;

    // ���߿� �з� �뵵 (��: ȸ��, ���� ��)
    public ItemType type;
    public bool isAutoUse; // ��� ��� ����


}
public enum ItemType
{
    Heal,
    Ability,
    LifeUp,
}