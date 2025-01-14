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

    public GameObject EffectSlash;
    public GameObject EffectSlash2;

   
    void Start()
    {
        animator = GetComponent<Animator>();
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
        TriggerComboStep();
        isAttacking = true;
    }

    void TriggerComboStep()
    {
        switch (currentComboStep)
        {
            case 1:
                animator.SetTrigger("Attack1");             
                break;
            case 2:
                animator.SetTrigger("Attack2");           
                break;
            case 3:
                animator.SetTrigger("Attack3");      
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
    
    void DisableEffect()
    {
        EffectSlash.SetActive(false); // Tắt hiệu ứng
        EffectSlash2.SetActive(false); // Tắt hiệu ứng
       
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
}
