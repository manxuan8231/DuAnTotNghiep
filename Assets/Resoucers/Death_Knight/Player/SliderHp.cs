using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderHp : MonoBehaviour
{
    [SerializeField] private Slider currentHP;
    private int maxHp = 1000;

    [SerializeField] private Slider currentMana;
    private int maxMana = 1000;

    [SerializeField] private Slider currentUlti;
    private int maxUlti = 1;

    [SerializeField] private Slider currentExp;
    private float maxExp = 10000;

    [SerializeField] private TextMeshProUGUI textHP;

    [SerializeField] private TextMeshProUGUI textMana;

    [SerializeField] private TextMeshProUGUI textQ; // Thêm TextMeshPro cho chữ Q
    [SerializeField] private TextMeshProUGUI textExp; // Thêm TextMeshPro cho thanh Exp

    [SerializeField] private TextMeshProUGUI textLevel; // Thêm TextMeshPro cho Level

    [SerializeField] private TextMeshProUGUI textScore; // Thêm TextMeshPro choscore

    private Color colorOrange = new Color(1f, 0.65f, 0f); // Màu cam
    private Color colorYellow = Color.yellow;             // Màu vàng
    private float colorChangeSpeed = 2f; // Tốc độ thay đổi màu

    private float scaleSpeed = 2f; // Tốc độ thay đổi kích thước
    private float minScale = 0.8f; // Kích thước nhỏ nhất
    private float maxScale = 1.2f; // Kích thước lớn nhất

    public int score = 0;   
    public int level = 5; // Cấp độ người chơi

    // Thêm tham chiếu ParticleSystem
    [SerializeField] private ParticleSystem levelEffect;

    private bool isManaIncreasing = false;
    private bool isInManaZone = false; // Biến kiểm tra xem người chơi có trong vùng Mana không
    private float manaIncreaseSpeed = 50f; // Tốc độ tăng Mana mỗi giây

    EnemyAnimationController rikayon;
    void Start()
    {
       
        currentHP.value = maxHp;
        textHP.text = $"{maxHp}/{maxHp}";

        currentMana.value = maxMana;
        textMana.text = $"{maxMana.ToString("0")}/{maxMana}";

        currentUlti.value = maxUlti;

        textQ.text = "Q"; // Đặt giá trị ban đầu cho chữ Q

        currentExp.value = 0; // Khởi tạo thanh Exp bằng 0
        textExp.text = $"XP: 0/{maxExp}"; // Hiển thị XP ban đầu

        textLevel.text = $"Level: {level}"; // Hiển thị cấp độ

        textScore.text = $"Score: {score}";

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
            textLevel.text = $"Level: {level}"; // Hiển thị cấp độ

            score++;
            textScore.text = $"Score: {score}";
            // Kích hoạt particle effect khi cộng điểm
            if (levelEffect != null)
            {
                levelEffect.gameObject.SetActive(true);
                levelEffect.Play(); // Bắt đầu hiệu ứng particle
            }
        }

        textExp.text = $"XP: {currentExp.value}/{maxExp}"; // Cập nhật UI cho XP

       
    }

    // Coroutine để tăng Mana
    IEnumerator IncreaseManaOverTime()
    {
        // Kiểm tra nếu Mana chưa đầy
        while (currentMana.value < maxMana && isInManaZone)
        {
            // Tăng Mana từ từ mỗi lần gọi coroutine
            currentMana.value += manaIncreaseSpeed * Time.deltaTime;

            // Cập nhật UI
            textMana.text = $"{currentMana.value}/{maxMana}";

            // Đảm bảo Mana không vượt quá maxMana
            if (currentMana.value > maxMana)
            {
                currentMana.value = maxMana;
            }

            yield return null; // Đợi đến frame tiếp theo
        }

        // Dừng coroutine khi Mana đầy hoặc người chơi ra ngoài vòng tròn
        isManaIncreasing = false;
    }

    // Hàm được gọi khi nhân vật vào vòng tròn Mana (sự kiện Trigger)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mana")) // Kiểm tra xem đối tượng có phải là vùng Mana không
        {
            isInManaZone = true; // Đánh dấu người chơi vào vùng Mana
            if (!isManaIncreasing) // Nếu không có coroutine nào đang chạy, bắt đầu tăng Mana
            {
                StartCoroutine(IncreaseManaOverTime());
                isManaIncreasing = true;
            }
        }
    }

    // Hàm được gọi khi nhân vật ra khỏi vòng tròn Mana (sự kiện Trigger)
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mana")) // Kiểm tra xem đối tượng có phải là vùng Mana không
        {
            isInManaZone = false; // Đánh dấu người chơi ra khỏi vùng Mana
                                  // Không cần dừng coroutine, vì coroutine sẽ tự dừng khi Mana đầy hoặc người chơi ra khỏi vùng
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

    // khi hết mana thì kko cho dùng nữa
    public float GetCurrentMana()
    {
        return currentMana.value;
    }
    public void GetMana(float amount)
    {
        currentMana.value += amount;
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

    // Hàm để thêm XP
    public void AddExp(float amount)
    {
        currentExp.value += amount;
    }

    public void AddUlti(float amount)
    {
        currentUlti.value += amount;    
    }

    
}
