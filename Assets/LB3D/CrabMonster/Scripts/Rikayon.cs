using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{
    private bool isAttack;
    private bool canAttack = true;
    private bool hasWokenUp = false; // Thêm biến cờ kiểm tra để ngăn lặp trạng thái WakeUp

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform target;
    [SerializeField] private float radius = 20f;
    [SerializeField] private float wakeUpRadius = 30f;
    [SerializeField] private float distanceAttack = 2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 viTriBanDau;

    public enum CharacterState
    {
        Sleep,
        WakeUp,
        Idle,
        Run,
        Attack,
        Return
    }

    public CharacterState currentState;

    void Start()
    {
        viTriBanDau = transform.position;
        ChangeState(CharacterState.Sleep);
    }

    void Update()
    {
        if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
            return;

        var distanceToTarget = Vector3.Distance(target.position, transform.position);
        var distanceToOrigin = Vector3.Distance(transform.position, viTriBanDau);

        switch (currentState)
        {
            case CharacterState.Sleep:
                if (distanceToTarget <= wakeUpRadius && !hasWokenUp )
                {
                    hasWokenUp = true; // Đánh dấu đã thức dậy
                    ChangeState(CharacterState.WakeUp);
                }
                break;

            case CharacterState.WakeUp:
                navMeshAgent.isStopped = true;  // Dừng di chuyển
                animator.SetTrigger("isWakeUp"); // Kích hoạt animation WakeUp
                StartCoroutine(TransitionToIdleAfterDelay(1f)); // Thêm delay 1 giây trước khi chuyển sang Idle
                break;


            case CharacterState.Idle:
                if (distanceToTarget <= radius)
                {
                    ChangeState(CharacterState.Run);
                }
                break;

            case CharacterState.Run:
                if (distanceToTarget > radius)
                {
                    ChangeState(CharacterState.Return);
                }
                else if (distanceToTarget <= distanceAttack)
                {
                    ChangeState(CharacterState.Attack);
                }
                else
                {
                    navMeshAgent.SetDestination(target.position);
                    animator.SetBool("isRun", true);
                }
                break;

            case CharacterState.Attack:
                if (distanceToTarget <= distanceAttack)
                {
                    if (canAttack)
                    {
                        StartCoroutine(PerformAttack());
                    }
                }
                else
                {
                    ChangeState(CharacterState.Run);
                }
                break;

            case CharacterState.Return:
                if (distanceToOrigin <= 1f)
                {
                    hasWokenUp = false; // Reset cờ khi trở lại trạng thái Sleep
                    ChangeState(CharacterState.Sleep);
                }
                else
                {
                    navMeshAgent.SetDestination(viTriBanDau);
                    animator.SetBool("isRun", true);
                }
                break;
        }
    }

    private IEnumerator PerformAttack()
    {
        canAttack = false;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void ChangeState(CharacterState newState)
    {
        if (currentState == newState) return;

        animator.SetBool("isRun", false);
        animator.SetBool("isSleep", false);

        switch (newState)
        {
            case CharacterState.Sleep:
                navMeshAgent.isStopped = true;
                animator.SetBool("isSleep", true);
                break;

            case CharacterState.WakeUp:
                navMeshAgent.isStopped = true;
                animator.SetTrigger("isWakeUp");
                break;

            case CharacterState.Idle:
                navMeshAgent.isStopped = true;
                animator.SetBool("Idle", true);
                break;

            case CharacterState.Run:
                navMeshAgent.isStopped = false;
                animator.SetBool("isRun", true);
                break;

            case CharacterState.Attack:
                navMeshAgent.isStopped = true;
                break;

            case CharacterState.Return:
                navMeshAgent.isStopped = false;
                animator.SetBool("isRun", false);
                break;
        }

        currentState = newState;
    }
    private IEnumerator TransitionToIdleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Đợi 1 giây
        ChangeState(CharacterState.Idle);      // Chuyển sang trạng thái Idle
    }

}
