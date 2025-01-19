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

    [SerializeField] private Slider currentExp;
    private float maxExp = 10000;

    [SerializeField] private TextMeshProUGUI textHP;

    [SerializeField] private TextMeshProUGUI textMana;

    [SerializeField] private TextMeshProUGUI textQ; // Thêm TextMeshPro cho chữ Q
    [SerializeField] private TextMeshProUGUI textExp; // Thêm TextMeshPro cho thanh Exp

    [SerializeField] private TextMeshProUGUI textLevel; // Thêm TextMeshPro cho Level

    private Color colorOrange = new Color(1f, 0.65f, 0f); // Màu cam
    private Color colorYellow = Color.yellow;             // Màu vàng
    private float colorChangeSpeed = 2f; // Tốc độ thay đổi màu

    private float scaleSpeed = 2f; // Tốc độ thay đổi kích thước
    private float minScale = 0.8f; // Kích thước nhỏ nhất
    private float maxScale = 1.2f; // Kích thước lớn nhất

   
    private int level = 5; // Cấp độ người chơi

    // Thêm tham chiếu ParticleSystem
    [SerializeField] private ParticleSystem levelEffect;

    void Start()
    {
       
        currentHP.value = maxHp;
        textHP.text = $"{maxHp}/{maxHp}";

        currentMana.value = maxMana;
        textMana.text = $"{maxMana}/{maxMana}";

        currentUlti.value = maxUlti;

        textQ.text = "Q"; // Đặt giá trị ban đầu cho chữ Q

        currentExp.value = 0; // Khởi tạo thanh Exp bằng 0
        textExp.text = $"XP: 0/{maxExp}"; // Hiển thị XP ban đầu

        UpdateLevelText(); // Cập nhật level ban đầu

        levelEffect.gameObject.SetActive(false);
    }

    void Update()
    {
        if (currentUlti.value >= 1000)
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

        // Kiểm tra nếu XP đã đầy và cộng điểm
        if (currentExp.value >= maxExp)
        {
            currentExp.value = 0; // Đặt lại thanh XP về 0
          
            level++; // Tăng cấp độ
            Debug.Log($"Level: {level}"); // In điểm và cấp độ ra Console

            // Cập nhật level trong UI
            UpdateLevelText();

            // Kích hoạt particle effect khi cộng điểm
            if (levelEffect != null)
            {
                levelEffect.gameObject.SetActive(true);
                levelEffect.Play(); // Bắt đầu hiệu ứng particle
            }
        }

        textExp.text = $"XP: {currentExp.value}/{maxExp}"; // Cập nhật UI cho XP
    }

    void UpdateLevelText()
    {
        textLevel.text = $"Level: {level}"; // Hiển thị cấp độ
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

    // khi hết mana thì kko cho dùng nữa
    public float GetCurrentMana()
    {
        return currentMana.value;
    }

    public float GetCurrentUlti()
    {
        return currentUlti.value;
    }
    public float GetCurrentLevel()
    {
        return level;
    }
    // trừ dần ulti
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
        // Kiểm tra nếu ăn đồng xu (với tag "exp")
        if (other.gameObject.CompareTag("Exp"))
        {
            AddExp(1000); // Thêm 1200 XP khi ăn đồng xu
            Destroy(other.gameObject); // Hủy đồng xu
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra nếu ăn đồng xu (với tag "exp")
        if (collision.gameObject.CompareTag("Exp"))
        {
            AddExp(1000); // Thêm 1200 XP khi ăn đồng xu
            Destroy(collision.gameObject); // Hủy đồng xu
            //cong score ulti
            currentUlti.value = maxUlti += 30;
            Destroy(collision.gameObject);
        }
    }
    // Hàm để thêm XP
    public void AddExp(float amount)
    {
        currentExp.value += amount;
    }
}
