using UnityEngine;
using UnityEngine.UI;

public class SkillR : MonoBehaviour
{
    public float cooldownTime = 5f;
    private bool isOnCooldown = false;
    public float skillRange = 50f; // Phạm vi tối đa để sử dụng kỹ năng

    public GameObject teleportIndicatorPrefab;
    private GameObject activeIndicator;
    private Transform targetBoss;

    public Slider cooldownSlider;
    public CharacterController characterController;
    Animator animator;

    public GameObject effect1;
    public GameObject effect2;

    public SliderHp sliderHp;

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
        targetBoss = FindClosestBoss(); // Luôn cập nhật Boss gần nhất
        float distanceToBoss = targetBoss ? Vector3.Distance(transform.position, targetBoss.position) : Mathf.Infinity;

        if (Input.GetKey(KeyCode.R) && !isOnCooldown && sliderHp.GetCurrentMana() > 20 && distanceToBoss <= skillRange)
        {
            ShowTeleportIndicator();
        }

        if (Input.GetKeyUp(KeyCode.R) && !isOnCooldown && targetBoss != null && sliderHp.GetCurrentMana() > 20 && distanceToBoss <= skillRange)
        {
            TeleportToBoss();
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

        if (targetBoss != null)
        {
            if (activeIndicator == null)
            {
                activeIndicator = Instantiate(teleportIndicatorPrefab, targetBoss.position, Quaternion.identity);
            }
            else
            {
                activeIndicator.transform.position = targetBoss.position;
            }
        }
    }

    void TeleportToBoss()
    {
        if (targetBoss != null)
        {
            sliderHp.SkillEMana(20);
            StartCoroutine(Effect2());
            animator.SetTrigger("skillR2");
            characterController.weaponHand.SetActive(true);
            transform.position = targetBoss.position;

            if (activeIndicator != null)
            {
                Destroy(activeIndicator);
            }

            StartCoroutine(CooldownRoutine());
        }
    }

    System.Collections.IEnumerator Effect2()
    {
        effect2.SetActive(true);
        effect1.SetActive(false);
        yield return new WaitForSeconds(3f);
        effect2.SetActive(false);
    }

    System.Collections.IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        if (cooldownSlider != null)
        {
            cooldownSlider.value = cooldownTime;
        }
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }

    Transform FindClosestBoss()
    {
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss1");
        Transform closestBoss = null;
        float closestDistance = Mathf.Infinity;
        Vector3 playerPosition = transform.position;

        foreach (GameObject boss in bosses)
        {
            float distance = Vector3.Distance(playerPosition, boss.transform.position);
            if (distance < closestDistance && distance <= skillRange)
            {
                closestDistance = distance;
                closestBoss = boss.transform;
            }
        }
        return closestBoss;
    }
}
