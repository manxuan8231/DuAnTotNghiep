using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Statue : MonoBehaviour
{
    [SerializeField] private Slider healthSlider; // Thanh trượt hiển thị máu
    [SerializeField] private TextMeshProUGUI healthText; // Text hiển thị giá trị máu
    [SerializeField] private float maxHealth = 5000f; // Máu tối đa của tượng
    private float currentHealth; // Máu hiện tại
    public AudioClip screamVFX;
    public AudioSource AudioSource;
    public GameObject effectElectron;
    void Start()
    {
        currentHealth = maxHealth; // Gán máu hiện tại bằng máu tối đa khi bắt đầu
        healthSlider.maxValue = maxHealth; // Đặt giá trị tối đa cho thanh máu
        healthSlider.value = currentHealth; // Cập nhật giá trị hiện tại của thanh máu
        healthText.text = $"{currentHealth}/{maxHealth}"; // Cập nhật text hiển thị
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Giảm máu hiện tại
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Giới hạn giá trị máu trong khoảng 0 đến maxHealth

        healthSlider.value = currentHealth; // Cập nhật giá trị của thanh máu
        healthText.text = $"{currentHealth}/{maxHealth}"; // Cập nhật text hiển thị

        Debug.Log("Đã trừ máu");

        if (currentHealth <= 0)
        {
            Debug.Log("Tượng đã bị phá hủy");
            Destroy(gameObject);
            // Gọi các phương thức khác khi tượng bị phá hủy
            SliderHp sliderHp = FindObjectOfType<SliderHp>();
            if (sliderHp != null)
            {
                sliderHp.AddExp(10000);
                sliderHp.AddUlti(500);
            }           
        }if(currentHealth == 2500)
        {
            AudioSource.PlayOneShot(screamVFX);
            effectElectron.SetActive(true);
        }
    }
    public float CurrentHealth()
    {
        return currentHealth;
    }
}
