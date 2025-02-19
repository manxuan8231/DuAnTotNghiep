using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public bool onRange;
    public Item item;

    void Start()
    {
        onRange = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && onRange)
        {
            InventoryManager.Instance.AddItem(item);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onRange = true;
            Debug.Log("Đã vào vùng nhặt đồ");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onRange = false;
            Debug.Log("Đã rời vùng nhặt đồ");
        }
    }
}
