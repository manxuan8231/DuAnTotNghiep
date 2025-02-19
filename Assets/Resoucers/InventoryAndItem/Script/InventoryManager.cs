using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<Item> items = new List<Item>();
    public Transform itemHolder;
    public GameObject itemPrefabs;
    public Toggle EnableRemoveItem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }

    public void AddItem(Item newItem)
    {
        // Kiểm tra xem item đã tồn tại chưa
        Item existingItem = items.Find(item => item.itemID == newItem.itemID);

        if (existingItem != null)
        {
            existingItem.quantity++;
        }
        else
        {
            // tạo bản sao để tránh thay đổi giá trị của ScriptableObject 
            Item itemCopy = Instantiate(newItem);
            itemCopy.quantity = 1;
            items.Add(itemCopy);
            
        }

        DisplayInventory();
    }

    public void RemoveItem(Item itemToRemove)
    {
        Item existingItem = items.Find(item => item.itemID == itemToRemove.itemID);

        if (existingItem != null)
        {
            
            items.Remove(existingItem);
            DisplayInventory();
        }
    }
    public void DisplayInventory()
    {
        foreach (Transform item in itemHolder)
        {
            Destroy(item.gameObject);
        }

        foreach (Item item in items)
        {
            GameObject obj = Instantiate(itemPrefabs, itemHolder);
            var itemName = obj.transform.Find("itemName")?.GetComponent<TextMeshProUGUI>();
            var itemImage = obj.transform.Find("itemImage")?.GetComponent<Image>();
            var itemQuantity = obj.transform.Find("itemQuantity")?.GetComponent<TextMeshProUGUI>(); // Thêm số lượng

            itemName.text = item.itemName;
            itemImage.sprite = item.itemImage;
            itemQuantity.text = "x" + item.quantity.ToString(); 
            obj.GetComponent<ItemUIController>().SetItem(item);
        }

        EnableRemoveButton();
    }

    public void EnableRemoveButton()
    {
        foreach (Transform item in itemHolder)
        {
            item.transform.Find("RemovedButton").gameObject.SetActive(EnableRemoveItem.isOn);
        }
    }
}
