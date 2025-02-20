using TMPro;
using UnityEngine;

public class ItemUIController : MonoBehaviour
{
    public Item item;
    public TextMeshProUGUI quantityText;

    public void SetItem(Item newItem)
    {
        item = newItem;
        UpdateUI();
    }

    public void RemoveItem()
    {
        InventoryManager.Instance.RemoveItem(item); 
            Destroy(gameObject);
            UpdateUI();
     
    }

    public void UpdateUI()
    {
        quantityText.text = "x" + item.quantity.ToString();
    }
    public void UseItem()
    {
        RemoveItem();
        switch (item.itemType)
        {
            case Itemtype.Hp:
                FindObjectOfType<ItemUsing>().UsingItemHeal(); 
                Debug.Log("đã bú hp");
                break;
            case Itemtype.Mana:
                FindObjectOfType<ItemUsing>().UsingItemMana();
                Debug.Log("đã bú mana");
                break;
        }
    }
}
