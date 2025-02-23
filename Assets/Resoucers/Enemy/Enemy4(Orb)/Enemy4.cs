using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Enemy4 : MonoBehaviour
{
    public enum EnemyState { Idle, Run, Rage, Combo1, Combo2, Combo3, Death, Return }
    private EnemyState currentState;
    public Transform player;
    public float radius = 25f;
    public float attackRange = 4f;
    public float maxRadius = 35f;
    public float rageDistance = 5f;
    private NavMeshAgent agent;
    private Animator animator;
    public Vector3 firstPosition;
    private bool isAttacking = false;
    private bool isRage = false;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private float maxHealth = 1000f;
    private float currentHealth;
   
    public GameObject takeHealth;
    public SphereCollider sphereCollider;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        firstPosition = transform.position;
        currentState = EnemyState.Idle;


        takeHealth.SetActive(false);
        sphereCollider.gameObject.SetActive(true);
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (currentState == EnemyState.Death) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= radius)
        {
            if (distanceToPlayer <= rageDistance && !isRage)
            {
                StartCoroutine(RageAndAttack());
            }
            else if (!isAttacking)
            {
                ChangeState(EnemyState.Run);
                agent.SetDestination(player.position);
            }
        }
        else if (distanceToPlayer <= maxRadius)
        {
            ChangeState(EnemyState.Return);
        }

        HandleState();
    }

    void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

       

        currentState = newState;

        // Reset tất cả trigger
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Rage");
        animator.ResetTrigger("Combo1");
        animator.ResetTrigger("Combo2");
        animator.ResetTrigger("Combo3");
        animator.ResetTrigger("Death");

        animator.SetTrigger(newState.ToString());
    }

    void HandleState()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                agent.isStopped = true;
                break;

            case EnemyState.Run:
                agent.isStopped = false;
                agent.speed = 3.5f;
                break;

            case EnemyState.Rage:
                agent.isStopped = true;
                break;

            case EnemyState.Combo1:
            case EnemyState.Combo2:
            case EnemyState.Combo3:
                agent.isStopped = true;
                break;

            case EnemyState.Return:
                agent.isStopped = false;
                agent.speed = 3.5f;
                agent.SetDestination(firstPosition);
                if (Vector3.Distance(transform.position, firstPosition) < 1f)
                {
                    ChangeState(EnemyState.Idle);
                    agent.isStopped = true;
                }
                break;

            case EnemyState.Death:
                agent.isStopped = true;
                Destroy(gameObject, 2f);
                break;
        }
    }

    IEnumerator RageAndAttack()
    {
        isRage = true;
        ChangeState(EnemyState.Rage);
        agent.isStopped = true;
        yield return new WaitForSeconds(2f);
        isRage = false;
        isAttacking = true;
        int random = Random.Range(0, 3);
        if (random == 0) ChangeState(EnemyState.Combo1);
        else if (random == 1) ChangeState(EnemyState.Combo2);
        else ChangeState(EnemyState.Combo3);

        yield return new WaitForSeconds(6.5f); // Giả sử combo kéo dài 3s
        isAttacking = false;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        if (animator != null)
        {
            animator.SetTrigger("GetHit");

        }

        else if (currentHealth <= 0)
        {
            sphereCollider.gameObject.SetActive(false);
            ChangeState(EnemyState.Death);
            Destroy(gameObject, 1.5f);

        }
    }
    private void UpdateHealthUI()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";

    }
    public void beginDame()
    {
        takeHealth.SetActive(true);
    }
    public void endDame()
    {
        takeHealth.SetActive(false);
    }
}
