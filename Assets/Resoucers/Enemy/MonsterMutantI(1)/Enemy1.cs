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

    private float maxDistance = 40f;
    [SerializeField] Vector3 fisrtPosition;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;
  


    void Start()
    {
        fisrtPosition = transform.position;
        ChangState(CharacterState.Idle);
    }




    void Update()
    {

        if (currentState == CharacterState.Die)
        {
            return;
        }
        if (NavMeshAgent == null || !NavMeshAgent.isOnNavMesh)
            return;
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
        switch(currentState)
        {
            case CharacterState.Idle:
             if(distanceToTarget <= radius)
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
                if (distanceToOrigin <= 1f)
                {
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


        switch(newstate)
        {
            case CharacterState.Idle:

                NavMeshAgent.isStopped= true;
                animator.SetBool("Idle", true);
                animator.SetBool("isRun", false);
                animator.SetBool("Rage", false);
                break;
            case CharacterState.Run:
                NavMeshAgent.isStopped= false;
                animator.SetBool("isRun", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Rage", false);

                break;
            case CharacterState.Rage:
                NavMeshAgent.isStopped= true;
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
                NavMeshAgent.isStopped = true;
                animator.SetTrigger("Combo1");
                break;
            case CharacterState.Die:
                animator.SetTrigger("Die");
                Destroy(gameObject, 1.5f);
                break;
            case CharacterState.Return:
                break;

        }
        currentState = newstate;
    }

}
