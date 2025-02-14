using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BossMoveAndAnimation : MonoBehaviour
{
  
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform target;
    [SerializeField] private float radius = 100f;
    [SerializeField] private float distanceAttack = 2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Animator animator;
  


    void Start()
    {
     
        ChangState(CharacterState.Idle);
    }




    void Update()
    {
        if (currentState == CharacterState.Death)
        {
            return;
        }
        if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
            return;
        HandleStateTransition();
    }

    private void HandleStateTransition()
    {

        var distanceToTarget = Vector3.Distance(target.position, transform.position);
      

        switch (currentState)
        {
            case CharacterState.Idle:

                //khi khoảng cách tới target nhỏ hơn hoặc bằng radius thì sẽ dí 
                if (distanceToTarget <= radius)
                {
                    ChangState(CharacterState.Walk);
                }
                Debug.Log("Idle");
                break;
            case CharacterState.Walk:
                if(distanceToTarget <= radius)
                {
                    navMeshAgent.SetDestination(target.position);
                    animator.SetBool("isWalking", true);

                }

                break;
            case CharacterState.Attack1:
                break;

        } 

    }
    public enum CharacterState
    {
       Idle,
       Walk,
       Attack1,
       Attack2,
       Skill1,
       Skill2,
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
                animator.SetBool("IsWalking", true);
                animator.SetBool("isIdle", false);
                
                break;
            case CharacterState.Attack1:
                navMeshAgent.isStopped = true;
                animator.SetTrigger("Attack1");
                break;
            case CharacterState.Attack2:
                navMeshAgent.isStopped = true;
                animator.SetTrigger("Attack2");
                break;
            case CharacterState.Skill1:
                navMeshAgent.isStopped = true;
                animator.SetTrigger("Skill1");
                break;
            case CharacterState.Skill2:
                navMeshAgent.isStopped = true;
                animator.SetTrigger("Skill2");
                break;
            case CharacterState.GetHit:
                navMeshAgent.isStopped = true;
                animator.SetTrigger("GetHit");

                break;
            case CharacterState.Death:
                navMeshAgent.isStopped = true;
                Destroy(gameObject, 1f);
                break;
            
            
        }
        currentState = newstate;
    }
    

  
}
