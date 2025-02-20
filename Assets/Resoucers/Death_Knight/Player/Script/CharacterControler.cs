using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Animator animator;

    public float moveSpeed = 5f; // Vận tốc di chuyển
    public float jumpForce = 10f; // Lực nhảy
    public float sprintSpeed = 8f; // Tốc độ chạy nhanh khi giữ Shift
 
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

    private Coroutine rollCoroutine;
    private Coroutine jumpCoroutine;

    public bool isMovementLocked = false; // Kiểm soát trạng thái "không di chuyển"
    public bool isDameLocked = false;
    private bool isECooldown = false; // Kiểm tra trạng thái hồi chiêu của phím E
   
    //khởi tạo script
    public SliderHp sliderHp;
    //offrain
    public GameObject offRain;
    //sounds

    public AudioSource audioSound;
    public AudioClip audioClipJump;//gan file
    public AudioClip audioClipWalk;
    public AudioClip audioClipRun;
    public AudioClip audioClipE;
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

        // Chỉ xử lý di chuyển khi không bị khóa
        if (!isMovementLocked)
        {
            // Nhảy khi nhấn Space và đang ở trên mặt đất
            if (Input.GetKeyDown(KeyCode.Space) && currentState == CharacterState.Normal && jumpCoroutine == null)
            {
                Jump();
                jumpCoroutine = StartCoroutine(JumpCoolDown());
            }

            // Tấn công khi nhấn chuột trái
            if (Input.GetMouseButtonDown(0) && currentState == CharacterState.Normal)
            {
                StartCoroutine(Attack());
                HandleWeaponSwitch(true); // Hiện weaponHand khi tấn công
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && rollCoroutine == null && sliderHp.GetCurrentMana() >= 50)
            {
                rollCoroutine = StartCoroutine(RollCoolDown());
                sliderHp.rollMana(50); // trừ 10 mana
            }
        }

        // Bấm E để khóa di chuyển trong 0.5 giây và có hồi chiêu 5 giây
        if (Input.GetKeyDown(KeyCode.E) && !isMovementLocked && !isECooldown)
        {
            audioSound.PlayOneShot(audioClipE);
            StartCoroutine(LockMovement(0.5f));
            StartCoroutine(ECooldown(5f)); // Thời gian hồi chiêu 5 giây
            weaponHand.SetActive(false);
            weaponDefault.SetActive(true);
        }
        
        // Kiểm tra trạng thái rơi tự do
        if (rb.velocity.y < 0)
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
        if (currentState == CharacterState.Normal && !isMovementLocked)
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
        sliderHp.jumpMana(5);

        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // Áp dụng lực nhảy
        animator.SetTrigger("startJump"); // Kích hoạt animation nhảy
       
    }

    private IEnumerator Attack()
    {
        currentState = CharacterState.Attack; // Chuyển trạng thái thành Attack
       
        yield return new WaitForSeconds(0.1f); // Thời gian tấn công (tùy chỉnh theo animation)

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

            resetWeaponCoroutine = StartCoroutine(ResetWeaponAfterDelay(5f)); // Bắt đầu reset sau 1 giây
        }
    }

    private IEnumerator ResetWeaponAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (weaponDefault != null) weaponDefault.SetActive(true);
        if (weaponHand != null) weaponHand.SetActive(false);
    }

    private IEnumerator JumpCoolDown()
    {
        yield return new WaitForSeconds(2f);
        jumpCoroutine = null;
    }

    private IEnumerator RollCoolDown()
    {
        animator.SetTrigger("roll");
     
        yield return new WaitForSeconds(2f);
        rollCoroutine = null;
    }

    private IEnumerator LockMovement(float lockDuration)
    {
        isMovementLocked = true;
        yield return new WaitForSeconds(lockDuration);
        isMovementLocked = false;
    }

    private IEnumerator ECooldown(float cooldownDuration)
    {
        isECooldown = true; // Kích hoạt hồi chiêu
        yield return new WaitForSeconds(cooldownDuration);
        isECooldown = false; // Hết hồi chiêu
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra nếu nhân vật tiếp đất
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("isLandJump", true); // Kích hoạt animation tiếp đất
         
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Kiểm tra nếu nhân vật rời khỏi mặt đất
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("isLandJump", false); // Tắt animation tiếp đất
          
        }
    }
    public void AudioJump()
    {
        audioSound.PlayOneShot(audioClipJump);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rain"))
        {
            offRain.SetActive(false);
        }
        if (other.gameObject.CompareTag("Explosion"))
        {
            isDameLocked = true;
            animator.SetTrigger("TakeHitBack");
            Debug.Log("đã té");
            StartCoroutine(LockMovement());
        }
    }
    public IEnumerator LockMovement()//không cho di chuyển khi bị stun
    {
        isMovementLocked = true;
        isDameLocked = true;
        yield return new WaitForSeconds(5f); // Thời gian tấn công (tùy chỉnh theo animation)

        isMovementLocked = false;
        isDameLocked = false;
    }
    //audio
    public void Walk()
    {
        audioSound.PlayOneShot(audioClipWalk);
    }
    public void Run()
    {
        audioSound.PlayOneShot(audioClipRun);
    }
}
