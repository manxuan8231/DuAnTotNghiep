using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Puppet : MonoBehaviour
{
    public NavMeshAgent agent;
    public float detectionRangeTarget = 200f; // Phạm vi phát hiện kẻ địch
    public float detectionRangeAttack = 2f; // Phạm vi tấn công
    public float attackCooldownTime = 5f; // Thời gian chờ giữa các đợt tấn công

    public Animator animator; // Thêm Animator
    private Transform currentTarget; // Lưu mục tiêu hiện tại
    private bool isAttacking = false; // Kiểm soát trạng thái tấn công

    private bool onAttack = true;
    private bool onMovement = true;

    public GameObject weaponHand;
    public GameObject weapon;
    void Update()
    {
        currentTarget = FindClosestTarget();

        if (currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.position);

            if (distance > detectionRangeAttack)
            {
                // Đuổi theo mục tiêu
                agent.SetDestination(currentTarget.position);
                animator.SetBool("Run", true);
            }
            else
            {
                // Tấn công nếu trong phạm vi
                AttackTarget();
            }
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    void AttackTarget()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            agent.SetDestination(transform.position); // Dừng di chuyển
            animator.SetBool("Run", false);
            animator.SetTrigger("Attack"); // Kích hoạt animation Attack
            weaponHand.SetActive(true);
            weapon.SetActive(false);
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldownTime); // Đợi 5 giây trước khi có thể đánh tiếp
        isAttacking = false; // Cho phép đánh tiếp
    }

    // Tìm mục tiêu gần nhất
    Transform FindClosestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss1");

        List<GameObject> allTargets = new List<GameObject>();
        allTargets.AddRange(enemies);
        allTargets.AddRange(bosses);

        Transform closestTarget = null;
        float closestDistance = detectionRangeTarget;

        foreach (GameObject target in allTargets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance <= closestDistance)
            {
                closestTarget = target.transform;
                closestDistance = distance;
            }
        }

        return closestTarget;
    }
}
