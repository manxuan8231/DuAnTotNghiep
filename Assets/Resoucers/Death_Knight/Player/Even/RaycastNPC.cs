using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaycatNPC : MonoBehaviour
{
    [SerializeField] private LayerMask TruongThon;
    public GameObject buttonTT;
    
   
    private void Update()
    {
        // Kiểm tra va chạm với các đối tượng thuộc lớp TruongThon
        CheckTruongThonRaycast();
    }


    private void CheckTruongThonRaycast()
    {
        if (buttonTT == null) return;
        if (Physics.Raycast(transform.position, transform.forward, out var other, 10, TruongThon))
        {
            // Vẽ ray chỉ khi có va chạm
            Debug.DrawRay(transform.position, transform.forward * other.distance, Color.red);
            buttonTT.SetActive(true);
        }
        else
        {
            
            // Nếu không có va chạm, ẩn button
            buttonTT.SetActive(false);
        }
    }
}
