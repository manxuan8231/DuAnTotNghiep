using UnityEngine;

public class FollowCameraRotation : MonoBehaviour
{
    [SerializeField] Transform target; // Camera mà thanh máu sẽ hướng đến
    [SerializeField] Transform enemy;  // Enemy mà thanh máu cần xoay theo

    void Start()
    {
        if (target == null)
        {
            target = Camera.main.transform;
        }

        if (enemy == null)
        {
            enemy = transform.parent; // Giả sử HealthBarCanvas là con của Enemy
        }
    }

    void LateUpdate()
    {
        if (enemy != null)
        {
            // Đảm bảo thanh máu xoay theo quái vật
            transform.position = enemy.position + Vector3.up * 2f; // Điều chỉnh vị trí cao hơn quái vật
            transform.rotation = Quaternion.LookRotation(target.forward); // Luôn hướng về camera
        }
    }
}
