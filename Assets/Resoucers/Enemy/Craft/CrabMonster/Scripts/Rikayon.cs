using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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


    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHp;
    [SerializeField] private Slider healSlider;   
    [SerializeField] private TextMeshProUGUI healText;

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

        //if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
        //    return;
        if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
            return;
        var distanceToTarget = Vector3.Distance(target.position, transform.position);
        var distanceToOrigin = Vector3.Distance(transform.position, viTriBanDau);

        switch (currentState)
        {


            case CharacterState.Sleep:
                Debug.Log("Sleep");
                hasWokenUp = false;
                if (distanceToTarget <= wakeUpRadius && !hasWokenUp)
                {
                    hasWokenUp = true;
                    ChangeState(CharacterState.WakeUp);

                }
                break;

            case CharacterState.WakeUp:
                Debug.Log("wakeup");
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp")) // Chỉ gọi khi hoạt ảnh không lặp lại
                {
                    navMeshAgent.isStopped = true;
                    animator.SetTrigger("isWakeUp");
                    ChangeState(CharacterState.Idle);
                    //StartCoroutine(TransitionToIdleAfterDelay(1f));
                }
                break;


            case CharacterState.Idle:
                Debug.Log("idle");
                // chạy animation idle

                // true => chạy
                if (distanceToTarget <= radius)
                {
                    ChangeState(CharacterState.Run);
                }
                // nếu không tìm đc người chơi thì quay lại ngủ
                break;

            case CharacterState.Run:
                // 
                Debug.Log("Run");

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
                Debug.Log("attack");
                if (distanceToTarget > distanceAttack)
                {
                    ChangeState(CharacterState.Run);
                }
                else if (canAttack)
                {
                    StartCoroutine(PerformAttack());
                }
                break;


            case CharacterState.Return:
                Debug.Log("return");

                if (distanceToOrigin <= 1f) //Kiểm tra cả khoảng cách và trạng thái dừng
                {
                    animator.SetBool("isRun", false);
                    hasWokenUp = false;
                    ChangeState(CharacterState.Sleep);

                }
                else
                {
                    navMeshAgent.SetDestination(viTriBanDau);
                    animator.SetBool("isRun", true);
                    Debug.Log("RunVeVitribandau");
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
                animator.SetBool("isRun", true);
                break;
        }

        currentState = newState;
    }
    //private IEnumerator TransitionToIdleAfterDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay); // Đợi 1 giây
    //    ChangeState(CharacterState.Idle);      // Chuyển sang trạng thái Idle
    //}

}
