using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillR : MonoBehaviour
{
    public float cooldownTime = 5f; // Thời gian hồi chiêu của kỹ năng
    private bool isOnCooldown = false; // Kiểm tra trạng thái hồi chiêu
    public float skillRange = 50f; // Phạm vi tối đa để sử dụng kỹ năng

    public GameObject teleportIndicatorPrefab; // Prefab cho chỉ thị dịch chuyển
    private GameObject activeIndicator; // Thực thể chỉ thị dịch chuyển hiện tại
    private Transform target; // Mục tiêu hiện tại

    public Slider cooldownSlider; // Thanh trượt để hiển thị thời gian hồi chiêu
    public CharacterController characterController; // Điều khiển nhân vật
    Animator animator; // Animator của nhân vật

    public GameObject effect1; // Hiệu ứng 1
    public GameObject effect2; // Hiệu ứng 2

    public SliderHp sliderHp; // Điều khiển HP và mana
    public Camera playerCamera; // Camera của người chơi

    
    void Start()
    {
        if (cooldownSlider != null)
        {
            cooldownSlider.maxValue = cooldownTime; // Đặt giá trị tối đa cho thanh trượt hồi chiêu
            cooldownSlider.value = cooldownTime; // Đặt giá trị hiện tại cho thanh trượt hồi chiêu
        }
        animator = GetComponent<Animator>(); // Lấy thành phần Animator từ nhân vật
        effect1.SetActive(false); // Tắt hiệu ứng 1 ban đầu
        effect2.SetActive(false); // Tắt hiệu ứng 2 ban đầu
    }

    void Update()
    {
        target = FindTargetInCameraDirection(); // Tìm mục tiêu trong hướng camera
        float distanceToTarget = target ? Vector3.Distance(transform.position, target.position) : Mathf.Infinity; // Tính khoảng cách tới mục tiêu

        if (Input.GetKey(KeyCode.R) && !isOnCooldown && sliderHp.GetCurrentMana() > 20 && distanceToTarget <= skillRange)
        {
            ShowTeleportIndicator(); // Hiển thị chỉ thị dịch chuyển
        }

        if (Input.GetKeyUp(KeyCode.R) && !isOnCooldown && target != null && sliderHp.GetCurrentMana() > 20 && distanceToTarget <= skillRange)
        {
            TeleportToTarget(); // Dịch chuyển đến mục tiêu
        }

        if (isOnCooldown && cooldownSlider != null)
        {
            cooldownSlider.value -= Time.deltaTime; // Giảm giá trị của thanh trượt hồi chiêu theo thời gian
        }
    }

    void ShowTeleportIndicator()
    {
        effect1.SetActive(true); // Bật hiệu ứng 1
        animator.SetTrigger("skillR1"); // Kích hoạt animation "skillR1"
        characterController.isMovementLocked = true;
        if (target != null)
        {
            if (activeIndicator == null)
            {
                activeIndicator = Instantiate(teleportIndicatorPrefab, target.position, Quaternion.identity); // Tạo chỉ thị dịch chuyển tại vị trí mục tiêu
            }
            else
            {
                activeIndicator.transform.position = target.position; // Cập nhật vị trí của chỉ thị dịch chuyển
            }
        }
    }

    void TeleportToTarget()
    {
        if (target != null)
        {
            StartCoroutine(NoTakeHealth());
            sliderHp.SkillEMana(20); // Trừ mana khi sử dụng kỹ năng
            StartCoroutine(Effect2()); // Bắt đầu hiệu ứng 2
            animator.SetTrigger("skillR2"); // Kích hoạt animation "skillR2"
            characterController.weaponHand.SetActive(true); // Kích hoạt weaponHand

            if (target.CompareTag("Boss1")) // Nếu target là Boss
            {
                StartCoroutine(TeleportAroundBoss(target)); // Dịch chuyển tuần tự quanh Boss
            }
            else
            {
                transform.position = target.position; // Dịch chuyển đến mục tiêu bình thường
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
    IEnumerator TeleportAroundBoss(Transform boss)
    {
        Vector3[] positions =
        {
        boss.position + boss.forward,  // Trước mặt
        boss.position - boss.forward,  // Phía sau
        boss.position + boss.right,    // Bên phải
        boss.position - boss.right     // Bên trái
    };

        foreach (Vector3 pos in positions)
        {
            effect2.SetActive(true); // Bật hiệu ứng 2
            animator.SetTrigger("skillR2"); // Kích hoạt animation "skillR2"
            transform.position = pos; // Dịch chuyển đến vị trí mới

            yield return new WaitForSeconds(0.3f); // Chờ 0.3 giây

            effect2.SetActive(false); // Tắt hiệu ứng 2 trước khi chuyển tiếp
        }

        EndTeleport(); // Kết thúc dịch chuyển
    }

    void EndTeleport()
    {
        effect2.SetActive(false); // Đảm bảo tắt hiệu ứng 2 hoàn toàn sau lần dịch chuyển cuối
        characterController.isMovementLocked = false;
        if (activeIndicator != null)
        {
            Destroy(activeIndicator); // Hủy chỉ thị dịch chuyển
        }
        StartCoroutine(CooldownRoutine()); // Bắt đầu thời gian hồi chiêu
    }
    IEnumerator Effect2()
    {
        effect2.SetActive(true); // Bật hiệu ứng 2
        effect1.SetActive(false); // Tắt hiệu ứng 1
        yield return new WaitForSeconds(3f); // Chờ 3 giây
        effect2.SetActive(false); // Tắt hiệu ứng 2
    }

    IEnumerator CooldownRoutine()
    {
        isOnCooldown = true; // Đặt trạng thái hồi chiêu
        if (cooldownSlider != null)
        {
            cooldownSlider.value = cooldownTime; // Đặt lại giá trị của thanh trượt hồi chiêu về giá trị tối đa
        }
        yield return new WaitForSeconds(cooldownTime); // Chờ thời gian hồi chiêu
        isOnCooldown = false; // Kết thúc hồi chiêu
    }

    Transform FindTargetInCameraDirection()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Tìm tất cả các đối tượng có tag "Enemy"
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss1"); // Tìm tất cả các đối tượng có tag "Boss1"
        Transform bestTarget = null; // Mục tiêu tốt nhất
        float bestDot = -1f; // Giá trị dot tốt nhất
        Vector3 cameraForward = playerCamera.transform.forward; // Hướng của camera

        foreach (GameObject enemy in enemies)
        {
            Vector3 directionToTarget = (enemy.transform.position - transform.position).normalized; // Hướng tới mục tiêu
            float dot = Vector3.Dot(cameraForward, directionToTarget); // Tính giá trị dot
            float distance = Vector3.Distance(transform.position, enemy.transform.position); // Tính khoảng cách tới mục tiêu

            if (dot > bestDot && distance <= skillRange)
            {
                bestDot = dot; // Cập nhật giá trị dot tốt nhất
                bestTarget = enemy.transform; // Cập nhật mục tiêu tốt nhất
            }
        }

        foreach (GameObject boss in bosses)
        {
            Vector3 directionToTarget = (boss.transform.position - transform.position).normalized; // Hướng tới mục tiêu
            float dot = Vector3.Dot(cameraForward, directionToTarget); // Tính giá trị dot
            float distance = Vector3.Distance(transform.position, boss.transform.position); // Tính khoảng cách tới mục tiêu

            if (dot > bestDot && distance <= skillRange)
            {
                bestDot = dot; // Cập nhật giá trị dot tốt nhất
                bestTarget = boss.transform; // Cập nhật mục tiêu tốt nhất
            }
        }

        return bestTarget; // Trả về mục tiêu tốt nhất
    }
}
