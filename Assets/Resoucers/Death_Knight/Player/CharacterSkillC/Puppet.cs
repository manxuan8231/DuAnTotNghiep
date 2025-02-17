using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Puppet : MonoBehaviour
{
    public NavMeshAgent agent;
    public float detectionRangeTarget = 200f; // Phạm vi phát hiện kẻ địch
    public float detectionRangeAttack = 2f;  // Phạm vi tấn công
    public float attackCooldownTime = 5f;    // Thời gian chờ giữa các đợt tấn công

    public Animator animator;
    private Transform currentTarget;
    private bool isAttacking = false;

    public GameObject weaponHand;
    public GameObject weapon;

    public BoxCollider boxCollider;
    public GameObject effect1;
    public AudioSource audioSource;
    public AudioClip audioClipSlash;
    private void Start()
    {
        boxCollider.enabled = false;
        effect1.SetActive(false);
    }
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
            animator.SetTrigger("Attack");
            weaponHand.SetActive(true);
            weapon.SetActive(false);
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldownTime);
        isAttacking = false;
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

    public void beginDame()
    {
        boxCollider.enabled = true;
        effect1.SetActive(true);
        audioSource.PlayOneShot(audioClipSlash);
    }
    public void endDame()
    {
        boxCollider.enabled = false;
        effect1.SetActive(false);
        audioSource.PlayOneShot(audioClipSlash);
    }

}
