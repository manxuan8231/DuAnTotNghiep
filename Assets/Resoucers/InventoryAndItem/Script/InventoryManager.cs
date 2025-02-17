using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; set; }
    public Dictionary<Item, int> items = new Dictionary<Item, int>();
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
        if (items.ContainsKey(item))
        {
            items[item]++; // Tăng số lượng nếu item đã tồn tại
        }
        else
        {
            items[item] = 1; // Thêm item mới với số lượng là 1
        }
        DisplayInventory();

    }

    public void RemoveItem(Item item) 
    {
        items.Remove(item);
        
    }
    public void DisplayInventory()
    {
        foreach(Transform item in itemHolder)
        {
            Destroy(item.gameObject);


        }
        foreach (var kvp in items)
        {
            Item item = kvp.Key;
            int quantity = kvp.Value;

            if (item == null)
            {
                Debug.LogWarning("không tìm thấy item trong inventory!");
                continue;
            }
            GameObject obj = Instantiate(itemPrefabs, itemHolder);
            if (obj == null)
            {
                Debug.LogError("không tìm thấy itemprebab");
                continue;
            }
            var itemName = obj.transform.Find("itemName")?.GetComponent<TextMeshProUGUI>();
            var itemImage = obj.transform.Find("itemImage")?.GetComponent<Image>();
            var itemQuantity = obj.transform.Find("itemQuantity")?.GetComponent<TextMeshProUGUI>();
            itemName.text = item.itemName;
            Debug.Log("Đã thêm tên item");
            itemImage.sprite = item.itemImage;
            Debug.Log("Đã thêm ảnh item");
            itemQuantity.text = "x" + quantity;
            Debug.Log($"Đã thêm {item.itemName} x {quantity}");

            
        }
        
    }

   
}