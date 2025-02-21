using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossMoveAndAnimation : MonoBehaviour
{

    public NavMeshAgent navMeshAgent;
    [SerializeField] private Transform target;
    [SerializeField] private float radius = 100f;
    [SerializeField] private float distanceAttack;  
    [SerializeField] private float attackCooldown;
    [SerializeField] private Animator animator;
    [SerializeField] private Slider currentHealth;
    [SerializeField] private float maxHealth = 30000f;
    [SerializeField] private TextMeshProUGUI txtHealth;
    [SerializeField] GameObject isOnHealth ;//biến hiện thanh máu khi thấy player
    [SerializeField] bool isCantDamage = false;//biến khi quái death không thể nhận damage
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] public GameObject slashEffect;
    public GameObject statue;

    void Start()
    {
      
        slashEffect.SetActive(false);
        capsuleCollider.enabled = true;
        navMeshAgent.enabled = true;
        isOnHealth.SetActive(false);
        isCantDamage = true;
        currentHealth.maxValue = maxHealth;
        txtHealth.text = $"{currentHealth.value}/{maxHealth}";
        ChangState(CharacterState.Idle);
        statue.SetActive(false);
    }
    void Update()
    {
        if (currentState == CharacterState.Death)
        {
            navMeshAgent.enabled=false;
            capsuleCollider.enabled = false;

            return;
        }
        if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
            return;
        HandleStateTransition();
    }
    public void TakeDame(float amount)
    {
        if(isCantDamage == true)
        {
          
            currentHealth.value -= amount;
            txtHealth.text = $"{currentHealth.value}/{maxHealth}";
            currentHealth.value = Mathf.Clamp(currentHealth.value, 0, maxHealth);   
          
            
            if (currentHealth.value <= 0)
            {
                ChangState(CharacterState.Death);
            }            
            if (currentHealth.value <= 20000f) { statue.SetActive(true);  }
          
            
        }
       
    }
   
    public float CurrentHealth()
    {
        return currentHealth.value;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TakeDame(300);
            
            Debug.Log("matmau");
        }
    }

    private void HandleStateTransition()
    {

        var distanceToTarget = Vector3.Distance(target.position, transform.position);
      

        switch (currentState)//trạng thái hiện tại
        {
            case CharacterState.Idle:

                //khi khoảng cách tới target nhỏ hơn hoặc bằng radius thì sẽ dí 
                if (distanceToTarget <= radius)
                {
                    isOnHealth.SetActive(true);
                    ChangState(CharacterState.Walk);

                }
                Debug.Log("Idle");
                break;
            case CharacterState.Walk:
                //khoảng cách tới tager nếu như nhỏ hơn radius thì sẽ di chuyển tơi target đồng thời bật trạng thái Walk
                if (distanceToTarget <= radius)
                {
                    navMeshAgent.SetDestination(target.position);
                    animator.SetBool("isWalking", true);
                    if (distanceToTarget <= distanceAttack)
                    {
                        int randoom = Random.Range(0, 2);
                        if (randoom == 0)
                        {

                            ChangState(CharacterState.Attack1);
                            Debug.Log("Attack1");
                            if (animator == null)
                            {
                                Debug.LogError("Animator is NULL in ChangState!");
                            }
                        }
                        else
                        {
                            ChangState(CharacterState.Attack2);
                            Debug.Log("Attack2");
                            if (animator == null)
                            {
                                Debug.LogError("Animator is NULL in ChangState!");
                            }
                        }
                    }
                   

                }

                break;
            case CharacterState.Attack1:
                if(distanceToTarget > distanceAttack)
                {
                    ChangState(CharacterState.Walk);
                }
                else
                {

                    ChangState(CharacterState.Attack1);
                    

                }
                break;
            case CharacterState.Attack2:
                if (distanceToTarget > distanceAttack)
                {
                    ChangState(CharacterState.Walk);
                }
                else
                {
                    ChangState(CharacterState.Attack2);
                    
                }
                break;
            case CharacterState.Death:
                Debug.Log("Death");
                break;
        }

    }
    public enum CharacterState
    {
       Idle,
       Walk,
       Attack1,
       Attack2,
       GetHit,
       Death

    }
    
    public CharacterState currentState;
    private void ChangState(CharacterState newstate)
    {
        if (currentState == newstate)
            return;
        switch (newstate)
        {
            case CharacterState.Idle:
                navMeshAgent.isStopped = true;
                animator.SetBool("isIdle", true);
                animator.SetBool("IsWalking", false);
                break;
            case CharacterState.Walk:
                navMeshAgent.isStopped = false;
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdle", false);
                animator.ResetTrigger("Attack1");
                animator.ResetTrigger("Attack2");

                break;
            case CharacterState.Attack1:
                navMeshAgent.isStopped = true;
                animator.SetBool("isWalking", false);
                animator.SetTrigger("Attack1");
             
                break;
            case CharacterState.Attack2:
                navMeshAgent.isStopped = true;
                animator.SetBool("isWalking", false);
                animator.SetTrigger("Attack2");
                
                break;
            case CharacterState.GetHit:
                navMeshAgent.isStopped = true;
                animator.SetTrigger("GetHit");
            
                break;
            case CharacterState.Death:
                navMeshAgent.isStopped = true;
                animator.SetTrigger("Death");
                isCantDamage = false;
                Destroy(gameObject,5f);
                
                break;
            
            
        }
        currentState = newstate;
    }

    public void StartDame()
    {
        boxCollider.enabled = true;

    }
    public void EndDame()
    {
        boxCollider.enabled = false;
    }

    public void EffectAttack()
    {
        slashEffect.SetActive(true);
    }

    public void EndEffectAttack()
    {
        slashEffect.SetActive(false);
        
    }

}
