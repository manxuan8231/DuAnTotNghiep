using System.Collections;
using System.Diagnostics.Tracing;
using System.IO.IsolatedStorage;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


public class Enemy4 : MonoBehaviour
{
    public enum EnemyState { Idle, Run, Rage, Combo1, Combo2, Combo3, Death, Return }
    private EnemyState currentState;
    private Transform player;
    public float radius = 25f;

    public float maxRadius = 35f;
    public float rageDistance = 5f;
    private NavMeshAgent agent;
    private Animator animator;
    public Vector3 firstPosition;
    public float canAttack = 4;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private float maxHealth = 1000f;
    private float currentHealth;
   
    public GameObject takeHealth;
    public SphereCollider sphereCollider;
    public float skill1CoolDown;
    [SerializeField] private float lastTimeSkill1 = 0;
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
        //khoảng cách ban đầu của enemy
        var distanceOrigin = Vector3.Distance(transform.position, firstPosition);
        //từ enemy tới target
        var distanceToTarget = Vector3.Distance(player.position, transform.position);

        // đuổi player
        if (distanceToTarget <= radius && distanceOrigin <= maxRadius)
        {


            ChangeState(EnemyState.Run);

            
             if(distanceToTarget <= rageDistance && distanceToTarget > canAttack)
            {
                StartCoroutine(RageChangeState());
            }
            else if (distanceToTarget < canAttack && Time.time >= lastTimeSkill1 + skill1CoolDown)
            {
                StartCoroutine(AttackChangeState());
            }

        }

        //chạy về
        if (distanceOrigin > maxRadius || distanceToTarget > radius)
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
        animator.ResetTrigger("isDeath");
        animator.ResetTrigger("isGetHit");
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
                agent.SetDestination(player.position);
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
                Destroy(gameObject, 3f);
                break;
        }
    }
    IEnumerator RageChangeState()
    {
        

        ChangeState(EnemyState.Rage);

        // Dừng enemy lại để hiển thị hiệu ứng Rage
        agent.isStopped = true;

        yield return new WaitForSeconds(1f); // Chờ 0.5s để thể hiện Rage

        // Dịch chuyển enemy đến gần Player (có thể tùy chỉnh khoảng cách)
        Vector3 teleportPosition = player.position + (transform.position - player.position).normalized * 2f;
        transform.position = teleportPosition;

      
      
    }
  IEnumerator AttackChangeState()
    {
       
        int random = Random.Range(0, 3);
        if (random == 0) ChangeState(EnemyState.Combo1);
        else if (random == 1) ChangeState(EnemyState.Combo2);
        else ChangeState(EnemyState.Combo3);
        lastTimeSkill1 = Time.time;
        yield return new WaitForSeconds(skill1CoolDown);
       
        
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        if (animator != null)
        {
            animator.SetTrigger("isGetHit");

        }

        if (currentHealth <= 0)
        {
            sphereCollider.gameObject.SetActive(false);
            ChangeState(EnemyState.Death);
          

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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float damage = 50f;
            TakeDamage(damage);
        }
    }
}
