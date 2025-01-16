using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Animator animator;

    public float moveSpeed = 5f; // Vận tốc di chuyển
    public float jumpForce = 10f; // Lực nhảy
    [SerializeField] private float sprintSpeed = 8f; // Tốc độ chạy nhanh khi giữ Shift

    private float horizontal, vertical; // Hướng di chuyển
    private Rigidbody rb;
    private Coroutine weaponScaleCoroutine; // Để quản lý Coroutine thay đổi kích thước
    public enum CharacterState { Normal, Attack } // Trạng thái của nhân vật
    public CharacterState currentState; // Trạng thái hiện tại của player

    private Vector3 movement; // Tọa độ của player

    public Transform cameraTransform;

    private bool isGrounded = true; // Trạng thái tiếp đất

    public GameObject weaponDefault; // Weapon ban đầu
    public GameObject weaponHand; // Weapon xuất hiện khi tấn công

    private bool isRunning = false; // Trạng thái chạy
    private Coroutine resetWeaponCoroutine; // Để quản lý Coroutine reset weapon

    private bool isWeaponHandScaledUp = false; // Trạng thái kích thước của weaponHand

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Lấy Rigidbody

        // Đảm bảo trạng thái ban đầu
        if (weaponDefault != null) weaponDefault.SetActive(true);
        if (weaponHand != null) weaponHand.SetActive(false);
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // Nhảy khi nhấn Space và đang ở trên mặt đất
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && currentState == CharacterState.Normal)
        {
            Jump();
        }

        // Tấn công khi nhấn chuột trái
        if (Input.GetMouseButtonDown(0) && currentState == CharacterState.Normal)
        {
            StartCoroutine(Attack());
            HandleWeaponSwitch(true); // Hiện weaponHand khi tấn công
        }

        // Thay đổi kích thước weaponHand khi nhấn phím E
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleWeaponHandScale();
        }

        // Kiểm tra trạng thái rơi tự do
        if (!isGrounded && rb.velocity.y < 0)
        {
            animator.SetBool("isFalling", true); // Kích hoạt animation rơi
        }
        else
        {
            animator.SetBool("isFalling", false); // Tắt animation rơi
        }
    }

    private void FixedUpdate()
    {
        if (currentState == CharacterState.Normal)
        {
            CalculateMovement();
        }
    }

    void CalculateMovement()
    {
        movement = new Vector3(horizontal, 0, vertical).normalized;

        if (movement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            float speed = moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Mouse1))
            {
                speed = sprintSpeed;
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }

            rb.MovePosition(rb.position + moveDirection.normalized * speed * Time.fixedDeltaTime);

            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }

    void Jump()
    {
        isGrounded = false; // Chuyển trạng thái thành đang nhảy
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // Áp dụng lực nhảy
        animator.SetTrigger("startJump"); // Kích hoạt animation nhảy
    }

    private IEnumerator Attack()
    {
        currentState = CharacterState.Attack; // Chuyển trạng thái thành Attack

        yield return new WaitForSeconds(0.5f); // Thời gian tấn công (tùy chỉnh theo animation)

        currentState = CharacterState.Normal; // Quay lại trạng thái Normal
    }

    private void HandleWeaponSwitch(bool isAttacking)
    {
        if (isAttacking)
        {
            if (resetWeaponCoroutine != null)
                StopCoroutine(resetWeaponCoroutine); // Ngừng reset nếu đang chạy

            if (weaponDefault != null) weaponDefault.SetActive(false);
            if (weaponHand != null) weaponHand.SetActive(true);

            resetWeaponCoroutine = StartCoroutine(ResetWeaponAfterDelay(5f)); // Bắt đầu reset sau 5 giây
        }
    }

    private IEnumerator ResetWeaponAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (weaponDefault != null) weaponDefault.SetActive(true);
        if (weaponHand != null) weaponHand.SetActive(false);
    }

    private void ToggleWeaponHandScale()
    {
        if (weaponHand != null)
        {
            if (weaponScaleCoroutine != null)
                StopCoroutine(weaponScaleCoroutine); // Dừng Coroutine cũ nếu đang chạy

            if (isWeaponHandScaledUp)
            {
                // Từ từ thu nhỏ
                weaponScaleCoroutine = StartCoroutine(ChangeWeaponScale(Vector3.one, 0.5f)); // Trở về kích thước ban đầu
            }
            else
            {
                // Từ từ phóng to
                weaponScaleCoroutine = StartCoroutine(ChangeWeaponScale(Vector3.one * 10f, 0.5f)); // Phóng to gấp đôi
            }

            isWeaponHandScaledUp = !isWeaponHandScaledUp; // Đảo trạng thái
        }
    }

    private IEnumerator ChangeWeaponScale(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = weaponHand.transform.localScale; // Kích thước ban đầu
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            weaponHand.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration); // Nội suy kích thước
            yield return null; // Chờ đến frame tiếp theo
        }

        weaponHand.transform.localScale = targetScale; // Đảm bảo đạt đến kích thước mục tiêu
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra nếu nhân vật tiếp đất
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isLandJump", true); // Kích hoạt animation tiếp đất
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        // Kiểm tra nếu nhân vật tiếp đất
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isLandJump", false); // Tắt animation tiếp đất
        }
    }
}
