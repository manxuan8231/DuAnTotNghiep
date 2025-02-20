using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Itemtype{
    Hp,
    Mana
}
[CreateAssetMenu(fileName = "item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public int itemID;
    public string itemName;
    public int itemValue;
    public Sprite itemImage;
    public int quantity;
    public Itemtype itemType;
}
