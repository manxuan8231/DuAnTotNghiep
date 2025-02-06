using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    [SerializeField] private LayerMask Item;
    public TextMeshProUGUI textItem;
    private int countItem = 0;
    public GameObject button;

    private void Update()
    {
        // Kiểm tra va chạm với các đối tượng thuộc lớp Item
        CheckItemRaycast();
    }

    private void CheckItemRaycast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hit, 3, Item))
        {
            // Vẽ ray chỉ khi có va chạm
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);

            // Kiểm tra xem button có còn tồn tại không
            if (button != null)
            {
                button.SetActive(true);
            }
        }
        else
        {
            // Nếu không có va chạm, ẩn button
            if (button != null)
            {
                button.SetActive(false);
            }
        }
    }
}
