using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatueBoss2 : MonoBehaviour
{
    
    [SerializeField] private Slider healthSlider; // Thanh trượt hiển thị máu
    [SerializeField] private TextMeshProUGUI healthText; // Text hiển thị giá trị máu
    [SerializeField] private float maxHealth = 3000f; // Máu tối đa của tượng
    private float currentHealth; // Máu hiện tại
    public GameObject boss;
    


    void Start()
    {
        currentHealth = maxHealth; // Gán máu hiện tại bằng máu tối đa khi bắt đầu
        healthSlider.maxValue = maxHealth; // Đặt giá trị tối đa cho thanh máu
        healthSlider.value = currentHealth; // Cập nhật giá trị hiện tại của thanh máu
        healthText.text = $"{currentHealth}/{maxHealth}"; // Cập nhật text hiển thị
        boss.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
      
    }

  
    //xử lý máu tượng
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Giảm máu hiện tại
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Giới hạn giá trị máu trong khoảng 0 đến maxHealth

        healthSlider.value = currentHealth; // Cập nhật giá trị của thanh máu
        healthText.text = $"{currentHealth}/{maxHealth}"; // Cập nhật text hiển thị

        Debug.Log("Đã trừ máu");

        if (currentHealth <= 0)
        {
            boss.SetActive(true);
            Debug.Log("Tượng đã bị phá hủy");   
            Destroy(gameObject, 0.5f);
        }
    }

    

   
}
