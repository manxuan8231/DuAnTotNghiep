using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillR : MonoBehaviour
{
    public float cooldownTime = 5f;
    private bool isOnCooldown = false;
    public float skillRange = 50f;

    public GameObject teleportIndicatorPrefab;
    private GameObject activeIndicator;
    private Transform target;

    public Slider cooldownSlider;
    public CharacterController characterController;
    Animator animator;

    public GameObject effect1;
    public GameObject effect2;

    public SliderHp sliderHp;
    public Camera playerCamera;

    void Start()
    {
        if (cooldownSlider != null)
        {
            cooldownSlider.maxValue = cooldownTime;
            cooldownSlider.value = cooldownTime;
        }
        animator = GetComponent<Animator>();
        effect1.SetActive(false);
        effect2.SetActive(false);
    }

    void Update()
    {
        target = FindTargetInCameraDirection();
        float distanceToTarget = target ? Vector3.Distance(transform.position, target.position) : Mathf.Infinity;

        if (Input.GetKey(KeyCode.R) && !isOnCooldown && sliderHp.GetCurrentMana() > 20 && distanceToTarget <= skillRange && sliderHp.GetCurrentLevel() >= 10)
        {
            ShowTeleportIndicator();
        }

        if (Input.GetKeyUp(KeyCode.R) && !isOnCooldown && target != null && sliderHp.GetCurrentMana() > 20 && distanceToTarget <= skillRange && sliderHp.GetCurrentLevel() >= 10)
        {
            TeleportToTarget();
        }

        if (isOnCooldown && cooldownSlider != null)
        {
            cooldownSlider.value -= Time.deltaTime;
        }
    }

    void ShowTeleportIndicator()
    {
        effect1.SetActive(true);
        animator.SetTrigger("skillR1");
        characterController.isMovementLocked = true;
        if (target != null)
        {
            if (activeIndicator == null)
            {
                activeIndicator = Instantiate(teleportIndicatorPrefab, target.position, Quaternion.identity);
            }
            else
            {
                activeIndicator.transform.position = target.position;
            }
        }
    }

    void TeleportToTarget()
    {
        if (target != null)
        {
            StartCoroutine(NoTakeHealth());
            sliderHp.SkillEMana(20);
            StartCoroutine(Effect2());
            animator.SetTrigger("skillR2");
            characterController.weaponHand.SetActive(true);

            if (target.CompareTag("Boss1") || target.CompareTag("Enemy"))
            {
                StartCoroutine(TeleportAroundTarget(target));
            }
            else
            {
                transform.position = target.position;
                EndTeleport();
            }
        }
    }

    public IEnumerator NoTakeHealth()
    {
        characterController.isDameLocked = true;
        yield return new WaitForSeconds(4);
        characterController.isDameLocked = false;
    }

    IEnumerator TeleportAroundTarget(Transform target)
    {
        Vector3[] positions =
        {
            target.position + target.forward,
            target.position - target.forward,
            target.position + target.right,
            target.position - target.right
        };

        foreach (Vector3 pos in positions)
        {
            effect2.SetActive(true);
            animator.SetTrigger("skillR2");
            transform.position = pos;
            yield return new WaitForSeconds(0.3f);
            effect2.SetActive(false);
        }

        EndTeleport();
    }

    void EndTeleport()
    {
        effect2.SetActive(false);
        characterController.isMovementLocked = false;
        if (activeIndicator != null)
        {
            Destroy(activeIndicator);
        }
        StartCoroutine(CooldownRoutine());
    }

    IEnumerator Effect2()
    {
        effect2.SetActive(true);
        effect1.SetActive(false);
        yield return new WaitForSeconds(3f);
        effect2.SetActive(false);
    }

    IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        if (cooldownSlider != null)
        {
            cooldownSlider.value = cooldownTime;
        }
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }

    Transform FindTargetInCameraDirection()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss1");
        Transform bestTarget = null;
        float bestDot = -1f;
        Vector3 cameraForward = playerCamera.transform.forward;

        foreach (GameObject enemy in enemies)
        {
            Vector3 directionToTarget = (enemy.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(cameraForward, directionToTarget);
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (dot > bestDot && distance <= skillRange)
            {
                bestDot = dot;
                bestTarget = enemy.transform;
            }
        }

        foreach (GameObject boss in bosses)
        {
            Vector3 directionToTarget = (boss.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(cameraForward, directionToTarget);
            float distance = Vector3.Distance(transform.position, boss.transform.position);

            if (dot > bestDot && distance <= skillRange)
            {
                bestDot = dot;
                bestTarget = boss.transform;
            }
        }

        return bestTarget;
    }
}
