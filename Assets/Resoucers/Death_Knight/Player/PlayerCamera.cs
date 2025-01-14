using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player; // Nhân vật cần theo dõi
    
    public float followDistance = 7f; // Khoảng cách theo dõi ban đầu
    public float followHeight = 3.5f; // Chiều cao theo dõi
    public float rotationSpeed = 3f; // Tốc độ xoay camera
    public float zoomSpeed = 1.5f; // Tốc độ phóng to/thu nhỏ
    public float minDistance = 2f; // Khoảng cách tối thiểu
    public float maxDistance = 15f; // Khoảng cách tối đa
    public float pitchLimit = 35f; // Giới hạn góc ngẩng
    public LayerMask collisionMask; // Lớp va chạm cho camera
    public float collisionOffset = 0.2f; // Khoảng cách bù khi gặp va chạm

    private float currentRotation = 0f; // Góc xoay hiện tại
    private float currentPitch = 0f; // Góc ngẩng hiện tại

    void Start()
    {
        currentRotation = transform.eulerAngles.y;
        currentPitch = transform.eulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Lấy input từ chuột
        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");

        // Cập nhật góc xoay và góc ngẩng
        currentRotation += horizontalInput * rotationSpeed;
        currentPitch -= verticalInput * rotationSpeed;
        currentPitch = Mathf.Clamp(currentPitch, -pitchLimit, pitchLimit);

        // Xử lý cuộn chuột để zoom camera
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        followDistance -= scrollInput * zoomSpeed;
        followDistance = Mathf.Clamp(followDistance, minDistance, maxDistance);

        // Tính toán vị trí mong muốn của camera
        Vector3 targetPosition = player.position + Vector3.up * followHeight;
        Vector3 direction = Quaternion.Euler(currentPitch, currentRotation, 0) * Vector3.back;
        Vector3 desiredPosition = targetPosition + direction * followDistance;

        // Kiểm tra va chạm để tránh xuyên qua đất hoặc vật thể
        if (Physics.Raycast(targetPosition, direction, out RaycastHit hit, followDistance, collisionMask))
        {
            // Nếu va chạm, giảm khoảng cách camera
            desiredPosition = hit.point - direction * collisionOffset;
            followDistance = Mathf.Clamp(Vector3.Distance(targetPosition, hit.point) - collisionOffset, minDistance, followDistance);
        }

        // Cập nhật vị trí camera
        transform.position = desiredPosition;

        // Camera luôn nhìn vào nhân vật (vị trí háng hoặc tâm)
        transform.LookAt(player.position + Vector3.up * 1.0f);

        // Chuyển đổi trạng thái con trỏ chuột
        if (Input.GetKeyDown(KeyCode.L))
        {
            Cursor.lockState = Cursor.visible ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !Cursor.visible;
        }
    }
}
