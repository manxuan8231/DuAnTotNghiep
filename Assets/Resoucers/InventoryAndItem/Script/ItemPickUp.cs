using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public bool onRage;
    public Item item;
    void Start()
    {
        onRage = false;
    }

    // Update is called once per frame
    void Update()
    {
        PickUpitem();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onRage = true;
            Debug.Log("Đã trong vùng");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onRage = false;
            Debug.Log("đã rời khỏi vùng");
        }
    }
    private void PickUpitem()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && onRage == true)
        {
            Destroy(gameObject);
            Debug.Log("đã nhặt");
            InventoryManager.Instance.AddItem(item);
        }
    }


}
