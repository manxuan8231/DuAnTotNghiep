using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ThanLan : MonoBehaviour
{

    [SerializeField] private NavMeshAgent NavMeshAgent;
    [SerializeField] Transform target;
    [SerializeField] float radius = 25f;
    [SerializeField] Animator animator;
    [SerializeField] private float maxHealth = 1000;
    private float currentHealth;
    private float maxDistance = 40f;
    [SerializeField] Vector3 fisrtPosition;
    [SerializeField] private float battleRange = 15f; // Khoảng cách cho BattleAttack

    [SerializeField] private float distanceAttack = 2f;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip injuredSound;
    [SerializeField] private AudioClip deathSound;
    //lay hp player
    public GameObject getHealthPlayer;
    public SphereCollider sphereCollider;
    public enum CharacterState
    {
        Idle,
        Run,
        Attack,
        BattleAttack,
        Die,
        TakeDame,
        Return

    }

    public CharacterState currentState;

    void Start()
    {
        sphereCollider.gameObject.SetActive(true);
        currentHealth = maxHealth;
        fisrtPosition = transform.position;
        ChangState(CharacterState.Idle);
        StartCoroutine(PlayIdleSound());
        getHealthPlayer.SetActive(false);
    }

    void Update()
    {
        if (currentState == CharacterState.Die) return;
        if (NavMeshAgent == null || !NavMeshAgent.isOnNavMesh) return;
        HandleStateTransition();
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

    private void HandleStateTransition()
    {
        var distanceToTarget = Vector3.Distance(target.position, transform.position);
        var distanceToOrigin = Vector3.Distance(transform.position, fisrtPosition);

        switch (currentState)
        {
            case CharacterState.Idle:
                // Nếu target vào phạm vi
                if (distanceToTarget <= radius)
                {
                    ChangState(CharacterState.Run);
                }
                Debug.Log("Idle");
                break;

            case CharacterState.Run:
                // Nếu target ra khỏi phạm vi rượt đuổi
                if (distanceToTarget > radius)
                {
                    ChangState(CharacterState.Return);
                }
                // Nếu target trong vùng BattleAttack
                else if (distanceToTarget <= battleRange)
                {
                    ChangState(CharacterState.BattleAttack);
                }
                // Nếu target trong vùng tấn công
                else if (distanceToTarget <= distanceAttack)
                {
                    ChangState(CharacterState.Attack);
                }
                else
                {
                    NavMeshAgent.SetDestination(target.position); // Tiếp tục rượt đuổi target
                }
                Debug.Log("Run");
                break;

            case CharacterState.BattleAttack:
                // Khi trong vùng BattleAttack, kiểm tra nếu vào vùng tấn công
                if (distanceToTarget <= distanceAttack)
                {
                    
                    ChangState(CharacterState.Attack);
                }
                else if (distanceToTarget > battleRange) // Nếu target ra khỏi vùng BattleAttack
                {
                    ChangState(CharacterState.Run);
                }
                Debug.Log("Battle");
                break;

            case CharacterState.Attack:
                // Nếu target ra khỏi vùng tấn công nhưng vẫn trong phạm vi rượt đuổi
                if (distanceToTarget > distanceAttack && distanceToTarget <= radius)
                {
                    ChangState(CharacterState.Run);
                }
                // Nếu target ra khỏi phạm vi rượt đuổi
                else if (distanceToTarget > radius)
                {
                    ChangState(CharacterState.Return);
                }
                Debug.Log("Attack");
                break;

            case CharacterState.TakeDame:
                Debug.Log("takedame");
                break;

            case CharacterState.Die:
                Debug.Log("Die");
                break;

            case CharacterState.Return:
                // Kiểm tra nếu đã quay về vị trí ban đầu
                if (Vector3.Distance(transform.position, fisrtPosition) <= 1f)
                {
                    // Quay lại trạng thái Idle khi về đến vị trí ban đầu
                    ChangState(CharacterState.Idle);

                }
                else
                {
                    // Tiếp tục di chuyển về vị trí ban đầu
                    NavMeshAgent.SetDestination(fisrtPosition);
                    ChangState(CharacterState.Run);
                }
                Debug.Log("Return");
                break;
        }
    }

    private void ChangState(CharacterState newstate)
    {
        if (currentState == newstate)
            return;

       

        switch (newstate)
        {
            case CharacterState.Idle:
                NavMeshAgent.isStopped = true;
                animator.SetBool("Idle", true);
                animator.SetBool("isRun", false);
                break;

            case CharacterState.Run:
                NavMeshAgent.isStopped = false;
                animator.SetBool("Idle", false);
                animator.SetBool("isRun", true);
                break;

            case CharacterState.BattleAttack:
                NavMeshAgent.isStopped = true;
                animator.SetTrigger("BattleIdle");
                StartCoroutine(DelayBattleAttack());
                break;

            case CharacterState.Attack:
                NavMeshAgent.isStopped = true;

                audioSource.PlayOneShot(attackSound);
                animator.SetTrigger("Attack");

                break;

            case CharacterState.TakeDame:
                NavMeshAgent.isStopped = true;

                animator.SetTrigger("TakeDame");
                audioSource.PlayOneShot(injuredSound);

                Debug.Log("takeDame");
                break;

            case CharacterState.Return:
                NavMeshAgent.isStopped = false;

                animator.SetBool("isRun", true);
                NavMeshAgent.SetDestination(fisrtPosition); // Quay về vị trí ban đầu
                break;

            case CharacterState.Die:
               
                animator.SetTrigger("Die");
                audioSource.PlayOneShot(deathSound);
                Destroy(gameObject, 1f); // Hủy đối tượng sau 3 giây
                animator.ResetTrigger("TakeDame");
                break;
        }

        currentState = newstate;
    }
    // nếu như đang từ trạng thái battle thì phải đợi chạy hết trạng thái battle khoảng 3 giây xong thì sẽ chuyển sang trạng thái Run tới gần target rồi mới attack


    //private IEnumerator SwitchToAttackAfterDelay()
    //{
    //    yield return new WaitForSeconds(1f);
    //    if (currentState == CharacterState.BattleAttack)
    //    {
    //        ChangState(CharacterState.Attack);
    //    }
    //}

    private IEnumerator DelayBattleAttack()
    {
        yield return new WaitForSeconds(3f);
        if (Vector3.Distance(target.position, transform.position) <= distanceAttack)
        {
            ChangState(CharacterState.Attack);
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        if (animator != null)
        {
            animator.SetTrigger("TakeDame");
            audioSource.PlayOneShot(injuredSound);

        }

        if (currentHealth <= 0)
        {
            ChangState(CharacterState.Die);
            sphereCollider.gameObject.SetActive(false);
            Destroy(gameObject, 3f); // 3 giây sau khi chết
            FindObjectOfType<SliderHp>().AddExp(5500);

          
        }
    }
    private void UpdateHealthUI()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";
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
        getHealthPlayer.SetActive(true);
    }
    public void endDame()
    {
        getHealthPlayer.SetActive(false);
    }
}