using UnityEngine;

public class BallExp : MonoBehaviour
{
    [SerializeField] private float speed = 5f;          // Tốc độ di chuyển của quả cầu
    [SerializeField] private float lifetime = 5f;       // Thời gian sống của quả cầu

    private Transform target;   // Đối tượng mà quả cầu sẽ nhắm đến (Player)
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Tìm kiếm đối tượng có tag "Player"
        target = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (target == null)
        {
            Destroy(gameObject, lifetime); // Nếu không tìm thấy Player, quả cầu sẽ tự hủy
        }
        else
        {
            // Nếu có Player, bắt đầu di chuyển về hướng Player
            MoveTowardsTarget();
        }

        // Hủy quả cầu sau thời gian lifetime nếu không va chạm
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (target != null)
        {
            // Cập nhật hướng di chuyển của quả cầu về phía người chơi
            Vector3 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra va chạm với đối tượng có tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject); // Hủy quả cầu khi va chạm với Player
        }
    }
}
