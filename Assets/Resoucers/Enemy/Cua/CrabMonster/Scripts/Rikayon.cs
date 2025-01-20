using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAnimationController : MonoBehaviour
{
    private bool isAttack;
    private bool canAttack = true;
    private bool hasWokenUp = false;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform target;
    [SerializeField] private float radius = 20f;
    [SerializeField] private float wakeUpRadius = 30f;
    [SerializeField] private float distanceAttack = 2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 viTriBanDau;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private float maxHealth = 1000f;
    private float currentHealth;

    
    public enum CharacterState
    {
        Sleep,
        WakeUp,
        Idle,
        Run,
        Attack,
        Die,
        TakeDame,
        Return
    }

    public CharacterState currentState;

    void Start()
    {
        viTriBanDau = transform.position;
        currentHealth = maxHealth;
        UpdateHealthUI();
        ChangeState(CharacterState.Sleep);
       
    }

    void Update()
    {
        if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
            return;

        HandleStateTransition();

    }
    private void HandleStateTransition()
    {
        var distanceToTarget = Vector3.Distance(target.position, transform.position);
        var distanceToOrigin = Vector3.Distance(transform.position, viTriBanDau);

        switch (currentState)
        {
            case CharacterState.Sleep:
                if (distanceToTarget <= wakeUpRadius && !hasWokenUp)
                {
                    hasWokenUp = true;
                    ChangeState(CharacterState.WakeUp);
                }
                break;

            case CharacterState.WakeUp:
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp"))
                {
                    animator.SetTrigger("isWakeUp");
                    ChangeState(CharacterState.Idle);
                }
                break;

            case CharacterState.Idle:
                if (distanceToTarget <= radius)
                {
                    ChangeState(CharacterState.Run);
                }
                break;

            case CharacterState.Run:
                if (distanceToTarget > radius)
                {
                    ChangeState(CharacterState.Return);
                }
                else if (distanceToTarget <= distanceAttack)
                {
                    ChangeState(CharacterState.Attack);
                }
                else
                {
                    navMeshAgent.SetDestination(target.position);
                }
                break;

            case CharacterState.Attack:
                if (distanceToTarget > distanceAttack && distanceToTarget <= radius)
                {
                    ChangeState(CharacterState.Run);
                }
                else if (distanceToTarget > radius)
                {
                    ChangeState(CharacterState.Return);
                }
                else if (canAttack)
                {
                    StartCoroutine(PerformAttack());
                }
                break;

            case CharacterState.Return:
                if (distanceToOrigin <= 1f)
                {
                    ChangeState(CharacterState.Sleep);
                }
                else
                {
                    navMeshAgent.SetDestination(viTriBanDau);
                }
                break;
            case CharacterState.TakeDame:            
                break;

            case CharacterState.Die:
                // Không xử lý gì trong trạng thái Die
                break;
        }
    }
    private IEnumerator PerformAttack()
    {
        
        canAttack = false;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    private void ChangeState(CharacterState newState)
    {
        if (currentState == newState)
            return;

        // Reset tất cả các trigger và trạng thái animation
        animator.ResetTrigger("isWakeUp");
        animator.ResetTrigger("Attack");
        animator.SetBool("isRun", false);
        animator.SetBool("isSleep", false);

        switch (newState)
        {
            case CharacterState.Sleep:
                navMeshAgent.isStopped = true;
                animator.SetBool("isSleep", true);
                hasWokenUp = false;
                break;

            case CharacterState.WakeUp:
                navMeshAgent.isStopped = true;
                animator.SetTrigger("isWakeUp");
                break;

            case CharacterState.Idle:
                navMeshAgent.isStopped = true;
                animator.SetBool("Idle", true);
                break;

            case CharacterState.Run:
                navMeshAgent.isStopped = false;
                animator.SetBool("isRun", true);
                break;

            case CharacterState.Attack:
                navMeshAgent.isStopped = true;
                break;

            case CharacterState.Return:
                navMeshAgent.isStopped = false;
                animator.SetBool("isRun", true);
                break;
            case CharacterState.TakeDame:
                navMeshAgent.isStopped = true;
                animator.SetTrigger("TakeDame");
                break;
            case CharacterState.Die:
                navMeshAgent.isStopped = true;
                animator.SetTrigger("Die");
                Destroy(gameObject);
                break;
        }

        currentState = newState;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DamageZone"))
        {
            TakeDamage(100);
        }
        if (collision.gameObject.CompareTag("FireBall"))
        {
            TakeDamage(200);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DamageZone"))
        {
            TakeDamage(250);
        }
        if (other.gameObject.CompareTag("FireBall"))
        {
            TakeDamage(200);
        }
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        if(animator != null ){
            animator.SetTrigger("TakeDame");
        }
        
        if (currentHealth <= 0)
        {
            ChangeState(CharacterState.Die);
            
        }
    }

    private void UpdateHealthUI()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";
    }
   

}
