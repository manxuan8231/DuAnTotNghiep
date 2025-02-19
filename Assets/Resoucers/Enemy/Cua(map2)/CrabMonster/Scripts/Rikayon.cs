using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAnimationController : MonoBehaviour
{
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
    public BoxCollider boxCollider;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip injuredSound;
    [SerializeField] private AudioClip deathSound;

    public GameObject attackDame;
    public enum CharacterState { Sleep, WakeUp, Idle, Run, Attack, Die, TakeDame, Return }
    public CharacterState currentState;

    void Start()
    {
        viTriBanDau = transform.position;
        currentHealth = maxHealth;
        UpdateHealthUI();
        ChangeState(CharacterState.Sleep);
        StartCoroutine(PlayIdleSound());
        attackDame.SetActive(false);
    }

    void Update()
    {
        if (currentState == CharacterState.Die || navMeshAgent == null || !navMeshAgent.isOnNavMesh)
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
        }
    }

    private IEnumerator PerformAttack()
    {
        canAttack = false;
        animator.SetTrigger("Attack");
        audioSource.PlayOneShot(attackSound);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void ChangeState(CharacterState newState)
    {
        if (currentState == newState) return;

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
                audioSource.PlayOneShot(injuredSound);
                break;

            case CharacterState.Die:
                navMeshAgent.isStopped = true;
                audioSource.PlayOneShot(deathSound);
                animator.SetTrigger("Die");
                Destroy(gameObject, 5f);
                break;
        }
        currentState = newState;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        animator.SetTrigger("TakeDame");
        audioSource.PlayOneShot(injuredSound);

        if (currentHealth <= 0)
        {
            ChangeState(CharacterState.Die);
            Destroy(gameObject, 3f);
            boxCollider.enabled = false;
        }
    }

    private void UpdateHealthUI()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";
    }

    private IEnumerator PlayIdleSound()
    {
        while (currentState != CharacterState.Die)
        {
            yield return new WaitForSeconds(Random.Range(5f, 15f));
            if (currentState == CharacterState.Idle)
            {
                audioSource.PlayOneShot(idleSound);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SkillR"))
        {
            TakeDamage(100);
        }
        if (other.gameObject.CompareTag("SkillZ"))
        {
            TakeDamage(999);
        }
    }

    public void beginDame()
    {
        attackDame.SetActive(true);
    }
    public void endDame()
    {
        attackDame.SetActive(false);
    }
}
