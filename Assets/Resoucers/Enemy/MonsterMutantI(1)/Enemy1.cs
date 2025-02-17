using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private NavMeshAgent NavMeshAgent;
    [SerializeField] Transform target;
    [SerializeField] float radius = 25f;
    [SerializeField] private float rageDistance = 4f;
    [SerializeField] private float distanceAttack = 2f;
    [SerializeField] Animator animator;


    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip attackSound1;
    [SerializeField] private AudioClip attackSound2;
    [SerializeField] private AudioClip injuredSound;
    [SerializeField] private AudioClip deathSound;

    private float maxDistance = 40f;
    [SerializeField] Vector3 fisrtPosition;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private float maxHealth = 1000f;
    private float currentHealth;
    private bool isPlayingIdleSound = false;
    public GameObject takeHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        fisrtPosition = transform.position;
        ChangState(CharacterState.Idle);
        StartCoroutine(PlayIdleSoundRandomly());
    }

    void Update()
    {
        if (currentState == CharacterState.Die) return;
        if (NavMeshAgent == null || !NavMeshAgent.isOnNavMesh) return;
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
                if (distanceToTarget <= radius) ChangState(CharacterState.Run);
                break;

            case CharacterState.Run:
                if (distanceToTarget > radius) ChangState(CharacterState.Return);
                else if (distanceToTarget <= rageDistance) ChangState(CharacterState.Rage);
                else NavMeshAgent.SetDestination(target.position);
                break;

            case CharacterState.Combo1:
            case CharacterState.Combo2:
                if (distanceToTarget > distanceAttack && distanceToTarget <= radius)
                    ChangState(CharacterState.Run);
                else if (distanceToTarget > radius)
                    ChangState(CharacterState.Return);
                break;

            case CharacterState.Rage:
                if (distanceToTarget <= distanceAttack)
                {
                    ChangState(Random.Range(0, 2) == 0 ? CharacterState.Combo1 : CharacterState.Combo2);
                }
                else if (distanceToTarget > distanceAttack && distanceToTarget <= radius)
                {
                    ChangState(CharacterState.Run);
                }
                else if (distanceToTarget > radius)
                {
                    ChangState(CharacterState.Return);
                }
                break;

            case CharacterState.Return:
                if (distanceToOrigin <= 1f) ChangState(CharacterState.Idle);
                else
                {
                    NavMeshAgent.SetDestination(fisrtPosition);
                    ChangState(CharacterState.Run);
                }
                break;
        }
    }

    private void ChangState(CharacterState newstate)
    {
        if (currentState == newstate) return;

        switch (newstate)
        {
            case CharacterState.Idle:
                NavMeshAgent.isStopped = true;
                animator.SetBool("Idle", true);
                break;
            case CharacterState.Run:
                NavMeshAgent.isStopped = false;
                animator.SetBool("isRun", true);
                break;
            case CharacterState.Rage:
                NavMeshAgent.isStopped = true;
                animator.SetBool("Rage", true);
                break;
            case CharacterState.Combo1:
                PlaySound(attackSound1);
                animator.SetTrigger("Combo1");
                break;
            case CharacterState.Combo2:
                PlaySound(attackSound2);
                animator.SetTrigger("Combo2");
                break;
            case CharacterState.GetHit:
                PlaySound(injuredSound);
                animator.SetTrigger("GetHit");
                break;
            case CharacterState.Die:
                PlaySound(deathSound);
                animator.SetTrigger("Die");
                Destroy(gameObject, 1.5f);
                break;
        }
        currentState = newstate;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        if (animator != null) animator.SetTrigger("GetHit");

        if (currentHealth <= 0)
        {
            ChangState(CharacterState.Die);
            Destroy(gameObject, 3f);
            FindObjectOfType<SliderHp>().AddExp(5500);
        }
    }

    private void UpdateHealthUI()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";
    }

    private IEnumerator PlayIdleSoundRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 15));
            if (currentState == CharacterState.Idle && !isPlayingIdleSound)
            {
                PlaySound(idleSound);
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
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
