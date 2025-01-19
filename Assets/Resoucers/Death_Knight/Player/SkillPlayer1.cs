using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillPlayer1 : MonoBehaviour
{
    public Animator animator; // Gán animator của nhân vật
    public float cooldownTime = 10f; // Thời gian hồi chiêu
    private bool isOnCooldown = false; // Trạng thái hồi chiêu

    public GameObject fireballPrefab; // Prefab của FireBall
    public Transform firePoint; // Vị trí bắn FireBall
    public float fireballSpeed = 10f; // Tốc độ di chuyển của FireBall
    public SliderHp sliderHp;

    // Thông số phóng to
    private bool isScaling = false; // Trạng thái phóng to
    private Vector3 targetScale; // Kích thước mục tiêu
    private Vector3 originalScale; // Kích thước ban đầu
    public float scaleSpeed = 1f; // Tốc độ phóng to

    // Aura hiệu ứng
    public GameObject auraPrefab; // Prefab cho aura
    private GameObject activeAura; // Tham chiếu tới aura đang được kích hoạt
    public Vector3 auraOffset = new Vector3(0f, 0f, 0f); // Vị trí offset của aura

    private float scaleDownSpeed = 2f; // Tốc độ thu nhỏ khi currentUlti = 0

    // Các vị trí để bắn ba quả cầu lửa
    public Vector3 leftOffset = new Vector3(-1f, 0f, 0f); // Vị trí bên trái
    public Vector3 rightOffset = new Vector3(1f, 0f, 0f); // Vị trí bên phải
    public Vector3 centerOffset = new Vector3(0f, 0f, 0f); // Vị trí chính giữa

    public Slider sliderCooldown; // Slider để theo dõi cooldown của skill E

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Lưu kích thước ban đầu của player
        originalScale = transform.localScale;

        // Đặt giá trị ban đầu cho sliderCooldown
        if (sliderCooldown != null)
        {
            sliderCooldown.maxValue = 100f;
           
        }
    }

    void Update()
    {
        // Kiểm tra nếu người chơi nhấn phím E và kỹ năng chưa trong thời gian hồi chiêu
        if (Input.GetKeyDown(KeyCode.E) && !isOnCooldown && sliderHp.GetCurrentMana() >= 20)
        {
            UseFireBall();
        }

        // Kiểm tra nếu người chơi nhấn phím Q và ulti đủ 1000
        if (Input.GetKeyDown(KeyCode.Q) && sliderHp.GetCurrentUlti() >= 1000)
        {
            StartScalingWithAura();
        }

        // Nếu đang phóng to, thực hiện quá trình thay đổi kích thước
        if (isScaling)
        {
            PerformScaling();
        }

        // Cập nhật vị trí của aura (di chuyển cùng player)
        if (activeAura != null)
        {
            activeAura.transform.position = transform.position + auraOffset;
        }

        // Giảm currentUlti mỗi giây khi đang phóng to
        if (isScaling)
        {
            sliderHp.DecreaseUlti(70f * Time.deltaTime); // Trừ 3 mỗi giây
        }

        // Nếu currentUlti = 0, thu nhỏ lại và tắt aura
        if (sliderHp.GetCurrentUlti() <= 0 && isScaling)
        {
            StopScaling(); // Dừng phóng to và bắt đầu thu nhỏ
        }
        else if (sliderHp.GetCurrentUlti() <= 0 && !isScaling && transform.localScale != originalScale)
        {
            // Nếu đang thu nhỏ, thực hiện thu nhỏ dần
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * scaleDownSpeed);

            // Tắt aura khi thu nhỏ
            if (activeAura != null)
            {
                Destroy(activeAura); // Hủy aura
                activeAura = null; // Xóa tham chiếu
            }
        }

        // Giảm dần giá trị của sliderCooldown mỗi frame khi cooldown đang áp dụng
        if (isOnCooldown && sliderCooldown != null && sliderCooldown.value > 0)
        {
            sliderCooldown.value -= (100f / cooldownTime) * Time.deltaTime; // Giảm dần theo thời gian hồi chiêu
        }
    }

    void UseFireBall()
    {
        // Kích hoạt animation FireBall
        animator.SetTrigger("FireBall");

        // Nếu đang ở trạng thái phóng to, bắn 3 quả cầu lửa ở 3 vị trí khác nhau
        if (isScaling)
        {
            ShootFireBallAtMultiplePositions();
        }
        else
        {
            // Bắn 1 quả cầu lửa bình thường
            ShootFireBall();
        }

        // Bắt đầu thời gian hồi chiêu
        StartCoroutine(CooldownRoutine());

        // Trừ mana
        sliderHp.SkillEMana(20);

        // Đặt lại giá trị sliderCooldown về 100 khi sử dụng skill
        if (sliderCooldown != null)
        {
            sliderCooldown.value = 100f;
        }
    }

    // Bắn 3 quả cầu lửa tại các vị trí chỉ định (trái, chính giữa và phải)
    void ShootFireBallAtMultiplePositions()
    {
        // Các vị trí mà quả cầu lửa sẽ được bắn ra từ
        Vector3[] positions = new Vector3[]
        {
            firePoint.position + leftOffset,   // Vị trí bên trái
            firePoint.position + centerOffset, // Vị trí chính giữa
            firePoint.position + rightOffset   // Vị trí bên phải
        };

        foreach (Vector3 position in positions)
        {
            // Tạo FireBall tại các vị trí khác nhau
            GameObject fireball = Instantiate(fireballPrefab, position, firePoint.rotation);

            // Lấy Rigidbody của FireBall và thêm vận tốc để bắn về phía trước
            Rigidbody rb = fireball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * fireballSpeed;
            }

            // Hủy FireBall sau 5 giây để tránh lãng phí tài nguyên
            Destroy(fireball, 4f);
        }
    }

    void ShootFireBall()
    {
        if (fireballPrefab != null && firePoint != null)
        {
            // Tạo FireBall tại vị trí firePoint
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

            // Lấy Rigidbody của FireBall và thêm vận tốc để bắn về phía trước
            Rigidbody rb = fireball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * fireballSpeed;
            }

            // Hủy FireBall sau 5 giây để tránh lãng phí tài nguyên
            Destroy(fireball, 2f);
        }
    }

    IEnumerator CooldownRoutine()
    {
        isOnCooldown = true; // Đặt trạng thái hồi chiêu
        yield return new WaitForSeconds(cooldownTime); // Chờ thời gian hồi chiêu
        isOnCooldown = false; // Kết thúc hồi chiêu
    }

    void StartScalingWithAura()
    {
        // Đặt trạng thái phóng to
        isScaling = true;
        targetScale = originalScale * 2f; // Tăng kích thước lên gấp đôi

        // Kích hoạt aura
        if (auraPrefab != null && activeAura == null)
        {
            activeAura = Instantiate(auraPrefab, transform.position + auraOffset, Quaternion.identity);
            activeAura.transform.parent = transform; // Gắn aura vào player
        }
    }

    void PerformScaling()
    {
        // Tăng dần kích thước bằng cách nội suy từ kích thước hiện tại đến kích thước mục tiêu
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);

        // Khi đạt gần kích thước mục tiêu, dừng phóng to
        if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
        {
            transform.localScale = targetScale;
        }
    }

    void StopScaling()
    {
        // Khi currentUlti = 0, dừng phóng to và thu nhỏ lại
        isScaling = false;
        targetScale = originalScale; // Quay lại kích thước ban đầu
    }
}
