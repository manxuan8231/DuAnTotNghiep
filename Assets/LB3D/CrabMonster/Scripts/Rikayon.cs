using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{

    private bool isAttack;

    // Tính khoản cách dí theo
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Transform target;
    [SerializeField] float radius = 20f;  // Điều chỉnh radius để kẻ thù dí theo người chơi ở khoảng cách xa hơn
    [SerializeField] private Animator animator;

    // Vị trí ban đầu của enemy
    [SerializeField] Vector3 viTriBanDau;


    private bool isDead = false; // Trạng thái Enemy

    // Tấn công
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float distanceAttack = 20f; // Điều chỉnh khoảng cách tấn công
    private bool canAttack = true;

  
    public enum CharacterState
    {
        Normal, Attack, Die
    }

    // Trạng thái hiện tại của kẻ thù
    public CharacterState currentState;

    private void Start()
    {
      
        viTriBanDau = transform.position;

    }

    void Update()
    {
        if (currentState == CharacterState.Die || navMeshAgent == null || !navMeshAgent.isOnNavMesh)
        {
            return;
        }

        // Khoảng cách đến player
        var distance = Vector3.Distance(target.position, transform.position);
        if (distance <= radius)
        {
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.SetDestination(target.position);
            }

            animator.SetFloat("isRun", navMeshAgent.velocity.magnitude);

            if (distance <= distanceAttack && canAttack)
            {
                ChangeState(CharacterState.Attack);
            }
        }
        else if (distance > radius)
        {
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.SetDestination(viTriBanDau);
            }

            animator.SetFloat("isRun", navMeshAgent.velocity.magnitude);

            if (distance < 1f)
            {
                animator.SetFloat("isRun", 0);
            }
            ChangeState(CharacterState.Normal);
        }
    }

    private void ChangeState(CharacterState newState)
    {
        isAttack = false;

        switch (currentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attack:
                break;
            case CharacterState.Die:
                break;
        }

        switch (newState)
        {
            case CharacterState.Normal:
                break;

            case CharacterState.Attack:
                if (canAttack)
                {
                    int randomAttack = Random.Range(0, 2); 
                    if (randomAttack == 0)
                    {
                        animator.SetTrigger("Attack"); 

                       
                    }
                    if (randomAttack == 1)
                    {
                        animator.SetTrigger("Attack"); 
                       

                    }
                }
                break;

            case CharacterState.Die:
                animator.SetBool("isDie", true);
                Destroy(gameObject, 5f);
                StopAllCoroutines();
                break;
        }
        currentState = newState;
    }

   
}
