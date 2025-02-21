using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatueBoss2 : MonoBehaviour
{
    
    [SerializeField] private Slider currentHealth; // Thanh trượt hiển thị máu
    [SerializeField] private TextMeshProUGUI healthText; // Text hiển thị giá trị máu
    private float maxHealth = 3000f; // Máu tối đa của tượng
    public GameObject boss;
    


    void Start()
    {
        currentHealth.maxValue = maxHealth;
        healthText.text = $"{currentHealth.value}/{maxHealth}"; // Cập nhật text hiển thị
        boss.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
      
    }

  
    //xử lý máu tượng
    public void TakeDamage(float damage)
    {
        currentHealth.value -= damage; // Giảm máu hiện tại
       
        healthText.text = $"{currentHealth.value}/{maxHealth}"; // Cập nhật text hiển thị
        currentHealth.value = Mathf.Clamp(currentHealth.value, 0, maxHealth); // Giới hạn giá trị máu trong khoảng 0 đến maxHealt
        Debug.Log("Đã trừ máu");

        if (currentHealth.value <= 0)
        {
            //boss.SetActive(true);
            Debug.Log("Tượng đã bị phá hủy");   
            Destroy(gameObject, 0.5f);
        }
    }

    

   
}
