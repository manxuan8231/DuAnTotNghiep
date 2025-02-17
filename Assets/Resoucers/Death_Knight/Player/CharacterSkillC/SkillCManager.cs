using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCManager : MonoBehaviour
{
    public GameObject character1;
    public GameObject character2;
    public GameObject character3;
    public GameObject character4;

    public float cooldownTime = 5f; // Thời gian hồi chiêu
    private bool isOnCooldown = false;
    public Slider cooldownSlider; // Thanh trượt hiển thị thời gian hồi chiêu

    public Transform playerTransform1;
    public Transform playerTransform2;
    public Transform playerTransform3;
    public Transform playerTransform4;

    public CinemachineVirtualCamera virtualCamera;
    public CinemachineVirtualCamera virtualCameraPlayer;

    Animator animator;
    public SliderHp sliderHp;
    public CharacterController characterController;
    void Start()
    {
        cooldownSlider.maxValue = cooldownTime;
        cooldownSlider.value = cooldownTime;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isOnCooldown && sliderHp.GetCurrentLevel() >= 40 && sliderHp.GetCurrentMana() >= 40)
        {
            animator.SetTrigger("skillC");
            StartCoroutine(CooldownWalk());
            sliderHp.SkillEMana(40);
            StartCoroutine(CooldownCamera());
            ActivateCharacters();
            StartCoroutine(CooldownRoutine()); // Bắt đầu hồi chiêu
            
        }

        // Cập nhật thanh trượt hồi chiêu
        if (isOnCooldown)
        {
            cooldownSlider.value -= Time.deltaTime;
        }
        
    }

    void ActivateCharacters()
    {
        // Triệu hồi nhân vật với cùng hướng quay của người chơi
        GameObject KhoiTao = Instantiate(character1, playerTransform1.position, playerTransform1.rotation);
        Destroy(KhoiTao, 20f);
        // Triệu hồi nhân vật với cùng hướng quay của người chơi
        GameObject KhoiTao2 = Instantiate(character2, playerTransform2.position, playerTransform2.rotation);
        Destroy(KhoiTao2, 20f);
        // Triệu hồi nhân vật với cùng hướng quay của người chơi
        GameObject KhoiTao3 = Instantiate(character3, playerTransform3.position, playerTransform3.rotation);
        Destroy(KhoiTao3, 20f);
        // Triệu hồi nhân vật với cùng hướng quay của người chơi
        GameObject KhoiTao4 = Instantiate(character4, playerTransform4.position, playerTransform4.rotation);
        Destroy(KhoiTao4, 20f);
    }

    IEnumerator CooldownRoutine()
    {
        isOnCooldown = true; // Bắt đầu hồi chiêu
        cooldownSlider.value = cooldownTime; // Reset thanh hồi chiêu
        yield return new WaitForSeconds(cooldownTime); // Chờ hết thời gian hồi chiêu
        isOnCooldown = false; // Kết thúc hồi chiêu
    }
    IEnumerator CooldownCamera()
    {
        virtualCamera.Priority = 20;
        virtualCameraPlayer.Priority = 0;
        yield return new WaitForSeconds(2);
        virtualCamera.Priority = 0;
        virtualCameraPlayer.Priority = 20;
    }
    IEnumerator CooldownWalk()
    {
        characterController.isDameLocked = true;
        characterController.isMovementLocked = true;
        yield return new WaitForSeconds(2);
        characterController.isMovementLocked = false;
        characterController.isDameLocked = false;
    }
}
