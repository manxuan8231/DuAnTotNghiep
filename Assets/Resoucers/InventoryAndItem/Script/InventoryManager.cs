using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; set; }

    public List<Item> items = new List<Item>();
    public Transform itemHolder;
    public GameObject itemPrefabs;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddItem(Item item)
    {
        if (item != null)
        {
            items.Add(item);
            DisplayInventory();
        }
        else
        {
            Debug.LogWarning("Attempted to add a null item to the inventory!");
        }
    }

    public void DisplayInventory()
    {



        if (itemPrefabs == null)
        {
            Debug.LogError("itemPrefabs is not assigned!");
            return;
        }
        if (itemHolder == null)
        {
            Debug.LogError("itemHolder is not assigned!");
            return;
        }

        foreach(Transform item in itemHolder)
        {
            Destroy(item.gameObject);
        }
        foreach (Item item in items)
        {
            if (item == null)
            {
                Debug.LogWarning("Found a null item in the inventory!");
                continue;
            }

            GameObject obj = Instantiate(itemPrefabs, itemHolder);
            if (obj == null)
            {
                Debug.LogError("Failed to instantiate itemPrefabs!");
                continue;
            }

            var itemName = obj.transform.Find("itemName")?.GetComponent<TextMeshProUGUI>();
            var itemImage = obj.transform.Find("itemImage")?.GetComponent<Image>();

            if (itemName == null)
            {
                Debug.LogError("itemName is missing in the prefab!");
                continue;
            }
            if (itemImage == null)
            {
                Debug.LogError("itemImage is missing in the prefab!");
                continue;
            }

            if (item.itemName == null)
            {
                Debug.LogWarning("item.itemName is null!");
                continue;
            }
            if (item.itemImage == null)
            {
                Debug.LogWarning("item.itemImage is null!");
                continue;
            }

            itemName.text = item.itemName;
            Debug.Log("Đã thêm tên item");
            itemImage.sprite = item.itemImage;
            Debug.Log("Đã thêm ảnh item");
        }
    }
}