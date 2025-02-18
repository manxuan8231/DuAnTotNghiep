using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; set; }
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

    public void AddItem(Item item)
    {
        items.Add(item);
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
        foreach (Item item in items )
        {

          
            GameObject obj = Instantiate(itemPrefabs, itemHolder);
            var itemName = obj.transform.Find("itemName")?.GetComponent<TextMeshProUGUI>();
            var itemImage = obj.transform.Find("itemImage")?.GetComponent<Image>();
            itemName.text = item.itemName;
            Debug.Log("Đã thêm tên item");
            itemImage.sprite = item.itemImage;
            Debug.Log("Đã thêm ảnh item");
            obj.GetComponent<ItemUIController>().SetItem(item);
            
        }
        EnableRemoveButton();
    }

   public void EnableRemoveButton()
    {
        if (EnableRemoveItem.isOn)
        {
            foreach (Transform item in itemHolder)
            {
                item.transform.Find("RemovedButton").gameObject.SetActive(true);
                Debug.Log("đã hiện button");
            }

        }
        else
        {
            foreach (Transform item in itemHolder)
            {
                item.transform.Find("RemovedButton").gameObject.SetActive(false);
                Debug.Log("đã ẩn button");
            }
        }

    }
}