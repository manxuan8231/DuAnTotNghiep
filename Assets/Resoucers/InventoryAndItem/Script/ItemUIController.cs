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
        UpdateUI();
    }

    public void UpdateUI()
    {
        quantityText.text = "x" + item.quantity.ToString();

        if (item.quantity <= 0)
        {
            Destroy(gameObject);
        }
    }
}
