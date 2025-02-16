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
        if(Instance != null || Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }

    public void DisplayInventory()
    {
        foreach(Item item in items)
        {
            GameObject obj = Instantiate(itemPrefabs, itemHolder);
            var itemName = obj.transform.Find("itemName").GetComponent<TextMeshProUGUI>();
            var itemImage = obj.transform.Find("itemImage").GetComponent<Image>();
            itemName.text = item.itemName;
            Debug.Log("Đã thêm tên item");
            itemImage.sprite = item.itemImage;
            Debug.Log("Đã thêm ảnh item");


        }
    }
}
