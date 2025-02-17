using Cinemachine;
using UnityEngine;

public class MoveToY : MonoBehaviour
{
    public float targetY = 5f; // Vị trí Y cần di chuyển tới
    public float moveSpeed = 2f; // Tốc độ di chuyển
    public Transform target; // Đối tượng mục tiêu
    public CapsuleCollider capsuleCollider;
    private Rigidbody rb; // Rigidbody của nhân vật

    private bool isMoving = false; // Kiểm soát trạng thái di chuyển

    public GameObject weaponHand;
    public GameObject weapon;
    public bool isCamera;

    public Puppet puppet;
    private void Start()
    {
       
        isMoving = true; // Bắt đầu di chuyển ngay khi game chạy
        rb = GetComponent<Rigidbody>();     
        // Tắt Rigidbody và Collider để tránh va chạm khi di chuyển
        rb.isKinematic = true;
      capsuleCollider.enabled = false;
        weaponHand.SetActive(false);
        weapon.SetActive(true);
        isCamera = true;
    }

    void Update()
    {
        if (isMoving)
        {
            MoveToTargetY();
        }
    }

    // Phương thức di chuyển đến vị trí Y của target
    void MoveToTargetY()
    {
        if (target == null) return; // Kiểm tra target có tồn tại không

        Vector3 currentPosition = transform.position; // Vị trí hiện tại
        Vector3 targetPosition = new Vector3(target.position.x, targetY, target.position.z); // Vị trí đích

        // Di chuyển mượt bằng Lerp
        transform.position = Vector3.Lerp(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

        // Dừng lại khi gần đạt vị trí mong muốn
        if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
        {
            transform.position = targetPosition;
            isMoving = false;
            isCamera = false;
            // Bật lại Rigidbody & Collider để nhân vật hoạt động bình thường
          
            rb.isKinematic = false;
            capsuleCollider.enabled = true;
            weaponHand.SetActive(true);
            weapon.SetActive(false);
            puppet.agent.enabled = true;
            this.enabled = false;

        }
    }
}
