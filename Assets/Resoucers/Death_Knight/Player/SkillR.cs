using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillR : MonoBehaviour
{
    public float cooldownTime = 5f; // Thời gian hồi chiêu (5 giây)
    private bool isOnCooldown = false; // Biến kiểm tra xem kỹ năng có đang hồi chiêu không

    public GameObject teleportIndicatorPrefab; // Prefab vòng tròn xuất hiện dưới chân Boss
    private GameObject activeIndicator; // Biến lưu vòng tròn hiện tại
    private Transform targetBoss; // Lưu vị trí của Boss gần nhất

    public Slider cooldownSlider; // Thanh hiển thị hồi chiêu (nếu có)

    Animator animator;
    void Start()
    {
        // Nếu có thanh slider hồi chiêu, đặt giá trị tối đa và giá trị ban đầu
        if (cooldownSlider != null)
        {
            cooldownSlider.maxValue = cooldownTime;
            cooldownSlider.value = cooldownTime;
        }
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Khi giữ phím Q và kỹ năng chưa hồi chiêu -> Hiển thị vòng tròn
        if (Input.GetKey(KeyCode.Q) && !isOnCooldown)
        {
            ShowTeleportIndicator();
            
        }

        // Khi thả phím Q và kỹ năng không trong thời gian hồi chiêu -> Dịch chuyển đến Boss
        if (Input.GetKeyUp(KeyCode.Q) && !isOnCooldown && targetBoss != null)
        {
            TeleportToBoss();
        }

        // Nếu đang trong thời gian hồi chiêu, giảm giá trị thanh hồi chiêu theo thời gian
        if (isOnCooldown && cooldownSlider != null)
        {
            cooldownSlider.value -= Time.deltaTime;
        }
    }

    // Hiển thị vòng tròn dưới Boss gần nhất
    void ShowTeleportIndicator()
    {
        // Tìm Boss gần nhất
        targetBoss = FindClosestBoss();
        animator.SetTrigger("skillR1");
        if (targetBoss != null)
        {
            // Nếu vòng tròn chưa được tạo, tạo một vòng tròn mới
            if (activeIndicator == null)
            {
                activeIndicator = Instantiate(teleportIndicatorPrefab, targetBoss.position, Quaternion.identity);
            }
            else
            {
                // Nếu đã có vòng tròn, cập nhật vị trí vòng tròn theo Boss gần nhất
                activeIndicator.transform.position = targetBoss.position;
            }
        }
    }

    // Dịch chuyển Player đến vị trí Boss khi thả Q
    void TeleportToBoss()
    {
        if (targetBoss != null)
        {
            Debug.Log("chạy animator skill2");
            animator.SetTrigger("skillR2");
            transform.position = targetBoss.position; // Dịch chuyển nhân vật đến Boss         
            // Xóa vòng tròn hiển thị dưới Boss sau khi dịch chuyển
            if (activeIndicator != null)
            {
                Destroy(activeIndicator);             
            }

            // Bắt đầu thời gian hồi chiêu
            StartCoroutine(CooldownRoutine());
        }
    }

    // Coroutine xử lý thời gian hồi chiêu
    IEnumerator CooldownRoutine()
    {
        isOnCooldown = true; // Đặt trạng thái hồi chiêu

        // Đặt thanh slider về giá trị tối đa khi bắt đầu hồi chiêu
        if (cooldownSlider != null)
        {
            cooldownSlider.value = cooldownTime;
        }

        yield return new WaitForSeconds(cooldownTime); // Chờ trong thời gian hồi chiêu

        isOnCooldown = false; // Kết thúc hồi chiêu
    }

    // Tìm Boss gần nhất so với Player
    Transform FindClosestBoss()
    {
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss1"); // Tìm tất cả GameObject có tag "Boss1"
        Transform closestBoss = null; // Biến lưu trữ Boss gần nhất
        float closestDistance = Mathf.Infinity; // Khoảng cách gần nhất ban đầu là vô hạn
        Vector3 playerPosition = transform.position; // Lấy vị trí hiện tại của Player

        foreach (GameObject boss in bosses) // Duyệt qua tất cả các Boss trong danh sách
        {
            float distance = Vector3.Distance(playerPosition, boss.transform.position); // Tính khoảng cách đến Boss

            if (distance < closestDistance) // Nếu Boss này gần hơn Boss trước đó
            {
                closestDistance = distance; // Cập nhật khoảng cách gần nhất
                closestBoss = boss.transform; // Cập nhật Boss gần nhất
            }
        }
        return closestBoss; // Trả về Boss gần nhất
    }
}
