
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy1 : MonoBehaviour
{

    [SerializeField] private NavMeshAgent NavMeshAgent;
    Transform target;
    [SerializeField] float radius = 25f;
    [SerializeField] private float rageDistance = 4f;
    [SerializeField] private float distanceAttack = 2f;
    [SerializeField] Animator animator;

    [SerializeField] Vector3 fisrtPosition;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private float maxHealth = 1000f;
    private float currentHealth;
    public SliderHp sliderhp;
    public GameObject takeHealth;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip attackSound1;
    [SerializeField] private AudioClip attackSound2;
    [SerializeField] private AudioClip injuredSound;
    [SerializeField] private AudioClip rageSound;
    [SerializeField] private AudioClip deathSound;
    public CapsuleCollider capsuleCollider;
    private bool isPlayingIdleSound = false;
    [SerializeField] private string targetTag = "Player";
    void Start()
    {
        capsuleCollider.gameObject.SetActive(true);
        currentHealth = maxHealth;
        UpdateHealthUI();
        fisrtPosition = transform.position;
        ChangState(CharacterState.Idle);
        StartCoroutine(PlayIdleSoundRandomly());
        takeHealth.SetActive(false);

        // Tìm đối tượng theo tag
        GameObject playerObject = GameObject.FindGameObjectWithTag(targetTag);
        if (playerObject != null)
        {
            target = playerObject.transform; // Gán Transform của đối tượng tìm được vào target
        }
        else
        {
            Debug.LogError($"Không tìm thấy đối tượng nào có tag: {targetTag}");
        }
    }




    void Update()
    {

        if (currentState == CharacterState.Die)
        {
            return;
        }
        if (NavMeshAgent == null || !NavMeshAgent.isOnNavMesh)
        {
            return;
        }
        HandleStateTransition();
    }

    public enum CharacterState
    {
        Idle,
        Run,
        Rage,
        Combo1,
        Combo2,
        GetHit,
        Die,
        Return
    }



    public CharacterState currentState;

    private void HandleStateTransition()
    {
        var distanceToTarget = Vector3.Distance(target.position, transform.position);
        var distanceToOrigin = Vector3.Distance(transform.position, fisrtPosition);
        switch (currentState)
        {
            case CharacterState.Idle:
                if (distanceToTarget <= radius)
                {
                    ChangState(CharacterState.Run);

                }
                Debug.Log("Idle");

                break;

            case CharacterState.Run:

                if (distanceToTarget > radius)
                {

                    ChangState(CharacterState.Return);
                }
                else if (distanceToTarget <= rageDistance)
                {
                    ChangState(CharacterState.Rage);
                }
                else
                {
                    NavMeshAgent.SetDestination(target.position);
                    ChangState(CharacterState.Run);
                }

                break;
            case CharacterState.Combo1:
                if (distanceToTarget <= distanceAttack)
                {
                    ChangState(CharacterState.Rage);
                }
                else if (distanceToTarget > distanceAttack && distanceToTarget <= radius)
                {
                    ChangState(CharacterState.Run);
                }
                else if (distanceToTarget > radius)
                {
                    ChangState(CharacterState.Return);
                }
                Debug.Log("Attack1");
                break;

            case CharacterState.Combo2:
                if (distanceToTarget <= distanceAttack)
                {
                    ChangState(CharacterState.Rage);
                }
                else if (distanceToTarget > distanceAttack && distanceToTarget <= radius)
                {
                    ChangState(CharacterState.Run);
                }
                else if (distanceToTarget > radius)
                {
                    ChangState(CharacterState.Return);
                }
                Debug.Log("Attack2");
                break;

            case CharacterState.Rage:
                if (distanceToTarget <= distanceAttack)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        ChangState(CharacterState.Combo1);
                    }
                    else
                    {
                        ChangState(CharacterState.Combo2);
                    }
                }
                else if (distanceToTarget > distanceAttack && distanceToTarget <= radius)
                {
                    ChangState(CharacterState.Run);
                }
                else if (distanceToTarget > radius)
                {
                    ChangState(CharacterState.Return);
                }
                Debug.Log("rage");
                break;



            case CharacterState.GetHit:
                Debug.Log("GetHit");
                break;
            case CharacterState.Die:
                
                break;
            case CharacterState.Return:
                if (distanceToOrigin <= 0.1f)
                {
                    ChangState(CharacterState.Idle);

                }
                else
                {

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
                animator.SetBool("Rage", false);
                break;
            case CharacterState.Run:
                NavMeshAgent.isStopped = false;
                animator.SetBool("isRun", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Rage", false);
                break;
            case CharacterState.Rage:
                NavMeshAgent.isStopped = true;
                animator.SetBool("Rage", true); 
                animator.SetBool("isRun", false);
                animator.SetBool("Idle", false);
                break;
            case CharacterState.Combo1:
                NavMeshAgent.isStopped = true;
                animator.SetBool("Combo1", true);

                break;
            case CharacterState.Combo2:
                NavMeshAgent.isStopped = true;
                animator.SetBool("Combo2", true);
                break;
            case CharacterState.GetHit:
                NavMeshAgent.isStopped = false;
                animator.SetTrigger("GetHit");

                break;
            case CharacterState.Die:
                animator.SetTrigger("Die");
                audioSource.PlayOneShot(deathSound);
                Destroy(gameObject, 3f);
                animator.ResetTrigger("GetHit");
               
                break;
            case CharacterState.Return:
                break;

        }
        currentState = newstate;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        audioSource.PlayOneShot(injuredSound);
        if (animator != null)
        {
            animator.SetTrigger("GetHit");

        }

        if (currentHealth <= 0)
        {
            capsuleCollider.gameObject.SetActive(false);
            sliderhp.AddExp(5500);
            ChangState(CharacterState.Die);
           
            Destroy(gameObject, 2f);
           
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
    private IEnumerator PlayIdleSoundRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 15));
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
   
}

