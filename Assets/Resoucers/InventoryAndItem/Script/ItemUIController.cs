using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIController : MonoBehaviour
{
    public Item item;

    public void SetItem(Item item)
    {
        this.item = item;

    }
    public void RemoveItem()
    {
        InventoryManager.Instance.RemoveItem(item);
        Destroy(this.gameObject);
        Debug.Log("Đã xóa item");
    }
   
}
