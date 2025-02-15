using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillZController : MonoBehaviour
{
    public float chargeSpeed = 20f; // Tốc độ tích năng lượng
    private float currentCharge = 1f; // Bắt đầu từ 1
    public GameObject energyBallPrefab; // Quả cầu năng lượng
    public Transform firePoint; // Vị trí bắn ra quả cầu năng lượng

    public TMP_Text chargeText; // TextMeshPro để hiển thị năng lượng
    public Slider cooldownSlider; // Slider để hiển thị thời gian hồi chiêu

    public Transform cameraTransform; // Tham chiếu đến camera

    private bool isCharging = false;
    private bool isOnCooldown = false;
    public float cooldownTime = 5f; // Thời gian hồi chiêu của kỹ năng
    private Animator animator;

    //effect
    public GameObject effect;
    //mui ten
    public GameObject muiTen;

    public CharacterController characterController;

    public SliderHp sliderHp;
    void Start()
    {
        cooldownSlider.maxValue = cooldownTime;
        cooldownSlider.value = cooldownTime;

        // Ban đầu ẩn text % năng lượng
        chargeText.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
        effect.SetActive(false);
        muiTen.SetActive(false);
    }

    void Update()
    {
        // Kiểm tra nếu đang giữ phím Z và không trong thời gian hồi chiêu
        if (Input.GetKey(KeyCode.Z) && !isOnCooldown && sliderHp.GetCurrentMana() >= 30)
        {        
            isCharging = true;
            ChargeEnergy();
            FlipPlayerToCamera();

            // Hiện text % năng lượng khi đang đè phím Z
            chargeText.gameObject.SetActive(true);

            // Kích hoạt animation khi đè phím Z
            animator.SetTrigger("skillZ");
            effect.SetActive(true);
            muiTen.SetActive(true);
            
        }

        // Kiểm tra nếu thả phím Z
        if (Input.GetKeyUp(KeyCode.Z) && isCharging && )
        {
            isCharging = false;
            if (currentCharge >= 100)
            {
                FireEnergyBall();
                StartCoroutine(CooldownRoutine()); // Bắt đầu thời gian hồi chiêu
            }
            currentCharge = 1f; // Đặt lại giá trị ban đầu của năng lượng tích lũy
            UpdateUI();

            // Ẩn text % năng lượng khi thả phím Z
            chargeText.gameObject.SetActive(false);

            // Tắt animation khi thả phím Z
           
            effect.SetActive(false);
            muiTen.SetActive(false);
           
        }

        // Cập nhật thanh trượt hồi chiêu
        if (isOnCooldown)
        {
            cooldownSlider.value -= Time.deltaTime;
        }
    }

    void ChargeEnergy()
    {
        
        if (currentCharge < 100)
        {
            currentCharge += chargeSpeed * Time.deltaTime;
            if (currentCharge > 100)
            {
                currentCharge = 100;
            }
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        chargeText.text = $"{currentCharge:F0}%"; // Cập nhật TextMeshPro năng lượng
    }

    void FireEnergyBall()
    {
        GameObject energyBall = Instantiate(energyBallPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = energyBall.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * 20f; // Điều chỉnh tốc độ của quả cầu năng lượng
        Destroy(energyBall, 5f);
    }

    IEnumerator CooldownRoutine()
    {
        isOnCooldown = true; // Đặt trạng thái hồi chiêu
        cooldownSlider.value = cooldownTime; // Đặt lại giá trị của Slider hồi chiêu về giá trị tối đa
        yield return new WaitForSeconds(cooldownTime); // Chờ thời gian hồi chiêu
        isOnCooldown = false; // Kết thúc hồi chiêu
    }

    void FlipPlayerToCamera()
    {
        // Xoay nhân vật theo hướng của camera
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0; // Đảm bảo hướng chỉ trên mặt phẳng ngang

        if (cameraForward.sqrMagnitude > 0.01f)
        {
            Quaternion newRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5f);
        }
       
    }
}
