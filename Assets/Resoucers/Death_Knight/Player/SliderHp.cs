using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderHp : MonoBehaviour
{
    [SerializeField] private Slider currentHP;
    private float maxHp = 1000;

    [SerializeField] private Slider currentMana;
    private float maxMana = 1000;

    [SerializeField] private Slider currentUlti;
    private float maxUlti = 1;

    [SerializeField] private TextMeshProUGUI textHP;

    [SerializeField] private TextMeshProUGUI textMana;

    [SerializeField] private TextMeshProUGUI textQ; // Thêm TextMeshPro cho chữ Q

    private Color colorOrange = new Color(1f, 0.65f, 0f); // Màu cam
    private Color colorYellow = Color.yellow;             // Màu vàng
    private float colorChangeSpeed = 2f; // Tốc độ thay đổi màu

    private float scaleSpeed = 2f; // Tốc độ thay đổi kích thước
    private float minScale = 0.8f; // Kích thước nhỏ nhất
    private float maxScale = 1.2f; // Kích thước lớn nhất

    void Start()
    {
       
        currentHP.value = maxHp;
        textHP.text = $"{maxHp}/{maxHp}";

        currentMana.value = maxMana;
        textMana.text = $"{maxMana}/{maxMana}";

        currentUlti.value = maxUlti;

        textQ.text = "Q"; // Đặt giá trị ban đầu cho chữ Q

    }

    void Update()
    {
        if(currentUlti.value >= 1000)
        {
            // Tạo hiệu ứng đổi màu
            float t = Mathf.PingPong(Time.time * colorChangeSpeed, 1f); // Giá trị dao động từ 0 đến 1
            Color interpolatedColor = Color.Lerp(colorOrange, colorYellow, t); // Nội suy giữa cam và vàng
            textQ.color = interpolatedColor; // Áp dụng màu cho chữ Q

            // Tạo hiệu ứng phóng to thu nhỏ
            float scale = Mathf.Lerp(minScale, maxScale, t); // Tính toán kích thước dựa trên t
            textQ.transform.localScale = new Vector3(scale, scale, 1f); // Áp dụng kích thước
            textQ.enabled = true;
        }
        else
        {
            textQ.enabled = false;
        }   
    }
    public void rollMana(float amount)
    {
        currentMana.value -= amount; 
        textMana.text = $"{currentMana.value}/{maxMana}";
    }
    public void jumpMana(float amount)
    {
        currentMana.value -= amount;
        textMana.text = $"{currentMana.value}/{maxMana}";
    }

    public void attackMana(float amount)
    {
        currentMana.value -= amount;
        textMana.text = $"{currentMana.value}/{maxMana}";
    }

    public void SkillEMana(float amount)
    {
        currentMana.value -= amount;
        textMana.text = $"{currentMana.value}/{maxMana}";
    }
    //khi hết mana thì kko cho dùng nữa
    public float GetCurrentMana()
    {
        return currentMana.value;
    }
    public float GetCurrentUlti()
    {
        return currentUlti.value;
    }
    //trừ dần unlti
    public void DecreaseUlti(float amount)
    {
        if (currentUlti.value > 0)
        {
            currentUlti.value -= amount;
            textQ.text = $"Q"; // Cập nhật UI
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            currentUlti.value = maxUlti += 1000;
            Destroy(other.gameObject);
        }
    }
}
