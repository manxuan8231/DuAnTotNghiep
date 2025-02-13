using System;
using UnityEngine;

public class AttackCombo : MonoBehaviour
{
    private Animator animator; // Animator của nhân vật
    private int currentComboStep = 0; // Bước hiện tại của combo
    public float comboResetTime = 1.5f; // Thời gian để reset combo nếu không tiếp tục
    public float comboCooldown = 1f; // Thời gian cooldown sau khi hoàn tất combo

    private float lastAttackTime = 0f; // Thời gian của lần tấn công cuối cùng
    private bool isAttacking = false; // Trạng thái có đang trong combo hay không
    private bool isCooldown = false; // Trạng thái cooldown sau khi hoàn tất combo

    public GameObject EffectSlash; //hiệu ứng chém
    public GameObject EffectSlash2;
    public GameObject EffectSlashFly;

    //box gây damage
    public GameObject damageZone;
    //box gây damage
    public GameObject damageZone2;

    private bool isGround = true;
    
    private Rigidbody Rigidbody;
    public SliderHp sliderHp;

    //sound
    public AudioSource audioSource;
    public AudioClip AudioClipAttack1;
    public AudioClip AudioClipAttack2;
    public AudioClip AudioClipAttack3;
    //sound attackQ
    public AudioClip AudioClipAttackQ;
    void Start()
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        damageZone.SetActive(false);
        damageZone2.SetActive(false);
    }

    void Update()
    {
        // Lắng nghe input và kích hoạt combo
        if (Input.GetMouseButtonDown(0) && !isCooldown && Time.time > lastAttackTime + 0.2f)
        {
            OnAttack();
        }

        // Reset combo nếu vượt quá thời gian chờ
        if (Time.time > lastAttackTime + comboResetTime && isAttacking)
        {
            ResetCombo();
        }
    }

    void OnAttack()
    {
        lastAttackTime = Time.time;
        currentComboStep++;

        // Nếu combo đã kết thúc, bắt đầu cooldown
        if (currentComboStep > 3)
        {
            StartCooldown();
            ResetCombo();
            return;
        }

        // Đặt trigger cho từng bước combo
        if(isGround)
        {
             TriggerComboStep();
             isAttacking = true;
        }
        if (!isGround)
        {
            TriggerAirComboStep();
            isAttacking = true;
        }
    }
    public void SoundAttack1()
    {
        
    }
    public void SoundAttack2()
    {
        audioSource.PlayOneShot(AudioClipAttack2);
    }
    public void SoundAttack3()
    {
        audioSource.PlayOneShot(AudioClipAttack3);
    }
    void TriggerComboStep()
    {
        switch (currentComboStep)
        {
            case 1:
                animator.SetTrigger("Attack1");
                //audioSource.PlayOneShot(AudioClipAttack1);
                sliderHp.attackMana(5);
                break;
            case 2:
                animator.SetTrigger("Attack2");
                sliderHp.attackMana(5);
                break;
            case 3:
                animator.SetTrigger("Attack3");
                sliderHp.attackMana(5);
                break;
        }
    }
   void TriggerAirComboStep()
   {
        switch (currentComboStep)
        {
            case 1:
                animator.SetTrigger("AirAttack1");
                sliderHp.attackMana(5);
                break;            
        }
        
    }
    
    void StartEffect() 
    {
        EffectSlash.SetActive(true);
        Invoke(nameof(DisableEffect), 1f); // Tắt hiệu ứng sau 0.5 giây
    }
    void StartEffect2()
    {
        EffectSlash2.SetActive(true);
        Invoke(nameof(DisableEffect), 1f); // Tắt hiệu ứng sau 0.5 giây     
    }
    void StartEffectFly()
    {
        EffectSlashFly.SetActive(true);
        Invoke(nameof(DisableEffect), 1f); // Tắt hiệu ứng sau 0.5 giây     
    }
    void DisableEffect()
    {
        EffectSlash.SetActive(false); // Tắt hiệu ứng
        EffectSlash2.SetActive(false); // Tắt hiệu ứng
        EffectSlashFly.SetActive(false);
    }
    void ResetCombo()
    {
        currentComboStep = 0;
        isAttacking = false;
    }

    void StartCooldown()
    {
        isCooldown = true; // Bắt đầu cooldown
        Invoke(nameof(EndCooldown), comboCooldown); // Kết thúc cooldown sau comboCooldown giây
    }

    void EndCooldown()
    {
        isCooldown = false; // Cho phép thực hiện combo mới
    }
    public void beginDame()
    {
        damageZone.SetActive(true);
    }

    public void endDame()
    {
        damageZone.SetActive(false);
    }
    public void beginDame2()
    {
        damageZone2.SetActive(true);
    }

    public void endDame2()
    {
        damageZone2.SetActive(false);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }

    public void StartAttackQ()
    {
       audioSource.PlayOneShot(AudioClipAttackQ);
        
    }
    public void StopAttackQ()
    {
       audioSource.Stop();
    }
}
