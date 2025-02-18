using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public int itemID;
    public string itemName;
    public int itemValue;
    public Sprite itemImage;
    public int quantity;
}
