using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
using System.Collections;
using TMPro;

public class DanDuong : MonoBehaviour
{
    public GameObject chest1;
    public GameObject chest2;
    public GameObject chest3;
    
    public float moveSpeed = 5f; // Tốc độ di chuyển đến rương
    private Transform targetChest;
    private bool isMoving = false;
    private bool playerNearby = false;

    private float distancePlayer = 10000f; // khoảng cách phát hiện player để thay đổi camera
    public Transform player;
    private bool offTaget = true;
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineVirtualCamera virtualCameraPlayer;

    private NavMeshAgent navAgent; // NavMeshAgent component

    // hiển thị thoại
    public GameObject PanelSoul;
    public TextMeshProUGUI ContentSoul;

    void Start()
    {
        // Get the NavMeshAgent component
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = moveSpeed;
        offTaget = true;
        PanelSoul.SetActive(false);
    }

    void Update()
    {
        // Nếu đang di chuyển và đối tượng không bị mất chest và player ở gần
        if (isMoving && targetChest != null && playerNearby)
        {
            // Kiểm tra nếu targetChest đã bị hủy hoặc không còn kích hoạt
            if (targetChest == null || !targetChest.gameObject.activeInHierarchy)
            {
                // Tìm chest gần nhất mới
                targetChest = GetNearestChest();
                if (targetChest != null)
                {
                    isMoving = true; // Tiếp tục di chuyển tới chest mới
                    navAgent.SetDestination(targetChest.position); 
                }
                else
                {
                    isMoving = false; // Nếu không còn chest nào, dừng di chuyển
                    return;
                }
            }

            // Kiểm tra nếu đã đến nơi
            if (Vector3.Distance(transform.position, targetChest.position) < 0.1f)
            {
                isMoving = false;
                // Điều chỉnh tọa độ Y khi đến rương
                transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            }
        }
        else if (!playerNearby)
        {
            isMoving = false;
        }

        // Kiểm tra khoảng cách camera và player
        float targetCameraDistance = Vector3.Distance(transform.position, player.position);
        if (targetCameraDistance <= distancePlayer && offTaget == true) // Nếu camera mục tiêu có khoảng cách nhỏ hơn hoặc bằng player, chạy Coroutine
        {
            StartCoroutine(TargetCamera());
        }
    }

    private IEnumerator TargetCamera()
    {
        if (PanelSoul != null)
        {
            PanelSoul.SetActive(true);
            Debug.Log("PanelSoul đã được bật!");
        }
        if (ContentSoul != null)
        {
            ContentSoul.text = "Ta sẽ dẫn đường cho ngươi";
            Debug.Log("Text hiển thị");
        }

        Debug.Log("Camera chuyển sang linh hồn");
        virtualCamera.Priority = 20;
        virtualCameraPlayer.Priority = 0;
       
    yield return new WaitForSeconds(4f);
       
        PanelSoul.SetActive(false);
        
        virtualCamera.Priority = 0;
        virtualCameraPlayer.Priority = 20;        
        Debug.Log("PanelSoul đã ẩn!");
        offTaget = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerNearby = true;
            targetChest = GetNearestChest(); // Lấy rương gần nhất khi player vào trigger
            isMoving = true;
            navAgent.SetDestination(targetChest.position); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerNearby = false;
            isMoving = false;
            navAgent.ResetPath(); // Stop the agent from moving
        }
    }

    private Transform GetNearestChest()
    {
        GameObject[] chests = { chest1, chest2, chest3 };
        Transform nearest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject chest in chests)
        {
            if (chest != null && chest.activeInHierarchy) // Chỉ xét các rương đang hoạt động
            {
                float distance = Vector3.Distance(transform.position, chest.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = chest.transform;
                }
            }
        }

        return nearest; // Trả về rương gần nhất
    }
}
