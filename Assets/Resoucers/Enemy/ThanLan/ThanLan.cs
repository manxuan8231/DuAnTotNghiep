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
    public int maxHp, currentHP;
    private float maxDistance = 40f;
    [SerializeField] Vector3 fisrtPosition;
    [SerializeField] private float battleRange = 15f; // Khoảng cách cho BattleAttack

    [SerializeField] private float distanceAttack = 2f;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;
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
        currentHP = maxHp;
        fisrtPosition = transform.position;
        ChangState(CharacterState.Idle);

    }

    // Update is called once per frame
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
                    //ChangState(CharacterState.Run);
                }
                Debug.Log("Return");
                break;
        }
    }

    private void ChangState(CharacterState newstate)
    {
        if (currentState == newstate)
            return;

        //// Tắt trạng thái trước đó nếu cần
        //animator.SetBool("Idle", false);
        //animator.SetBool("isRun", false);

        switch (newstate)
        {
            case CharacterState.Idle:
                NavMeshAgent.isStopped = true;
                animator.SetBool("Idle", true);
                animator.SetBool("isRun", false);


                break;

            case CharacterState.Run:
                NavMeshAgent.isStopped = false;
                animator.SetBool("isRun", true);
                animator.SetBool("Idle", false);

                break;

            case CharacterState.BattleAttack:
                NavMeshAgent.isStopped = false;
                animator.SetTrigger("BattleIdle");
                if (Vector3.Distance(target.position, transform.position) <= distanceAttack)
                {
                    ChangState(CharacterState.Attack);
                }
                //StartCoroutine(DelayBattleAttack());
                
                break;

            case CharacterState.Attack:
                NavMeshAgent.isStopped = true;
                animator.SetTrigger("Attack");
                animator.SetBool("isRun", false);
                break;

            case CharacterState.TakeDame:
                NavMeshAgent.isStopped = true;
                animator.SetTrigger("TakeDame");
                UpdateHealthUI();
                if (currentHP < 0)
                {
                    ChangState(CharacterState.Die);
                }
                Debug.Log("takeDame");
                break;

            case CharacterState.Return:
                NavMeshAgent.isStopped = false;
                animator.SetBool("isRun", true);
                NavMeshAgent.SetDestination(fisrtPosition); // Quay về vị trí ban đầu
                break;

            case CharacterState.Die:
                NavMeshAgent.isStopped = true;
                animator.SetTrigger("Die");
                Destroy(gameObject, 1f);
                break;
        }

        currentState = newstate;
    }

    

    //private IEnumerator SwitchToAttackAfterDelay()
    //{
    //    yield return new WaitForSeconds(1f);
    //    if (currentState == CharacterState.BattleAttack)
    //    {
    //        ChangState(CharacterState.Attack);
    //    }
    //}

    //private IEnumerator DelayBattleAttack()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    if (Vector3.Distance(target.position, transform.position) <= distanceAttack)
    //    {
    //        ChangState(CharacterState.Attack);
    //    }
    //}
    private void UpdateHealthUI()
    {
        healthBarFill.fillAmount = currentHP / maxHp;
        healthText.text = $"{currentHP}/{maxHp}";
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentHP -= 250; 
            UpdateHealthUI(); // Cập nhật giao diện thanh máu

            if (currentHP > 0)
            {
                ChangState(CharacterState.TakeDame);
            }
            else
            {
                ChangState(CharacterState.Die);
            }
        }
    }
}