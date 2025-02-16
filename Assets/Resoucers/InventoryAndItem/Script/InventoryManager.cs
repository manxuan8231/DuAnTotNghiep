using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<Item> items = new List<Item>();

    public Transform itemHolder;
    public GameObject itemPrefab;
    private void Awake()
    {
        if (Instance != null || Instance != this)
        {
            Destroy(Instance);
        }



        Instance = this;
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }


    public void DisPlayInventory()
    {
        foreach (Item item in items)
        {

        }

    }
}
