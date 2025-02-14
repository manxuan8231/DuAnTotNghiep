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
    [SerializeField] private float distanceAttack;
    [SerializeField] private float attackCooldown;
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
      

        switch (currentState)//trạng thái hiện tại
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
                        }
                        else
                        {
                            ChangState(CharacterState.Attack2);
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
                Destroy(gameObject, 1f);
                break;
            
            
        }
        currentState = newstate;
    }
    

  
}
