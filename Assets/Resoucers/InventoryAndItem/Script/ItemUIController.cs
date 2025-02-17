using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIController : MonoBehaviour
{
    public Item item;
    public void Remove()
    {
        //InventoryManager.Instance.RemoveItem(item);
        Destroy(this.gameObject);
    }
   
}
