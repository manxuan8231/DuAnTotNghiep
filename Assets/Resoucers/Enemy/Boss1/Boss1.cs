using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Boss1 : MonoBehaviour
{
    public Transform player; // Tham chiếu đến player
    public CinemachineVirtualCamera bossCam; // Camera focus vào boss
    public CinemachineVirtualCamera playerCam; // Camera của player
    public float detectRange = 10f; // Khoảng cách để boss phát hiện player
    public float focusDuration = 3f; // Thời gian camera giữ ở boss
    private bool isFocusing = false; // Trạng thái camera đang focus

   
    private void Start()
    {
       
        // Đặt lại priority mặc định
        playerCam.Priority = 20;
        bossCam.Priority = 0;
     
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectRange && !isFocusing) // Nếu player trong phạm vi & camera chưa focus
        {
            StartCoroutine(FocusOnBoss());
        }
    }

    //quản lí thời gian camera
    private IEnumerator FocusOnBoss()
    {
        bossCam.Priority = 20;//ưu tiên camera
        playerCam.Priority = 10;
        yield return new WaitForSeconds(focusDuration);
        bossCam.Priority = 10;
        playerCam.Priority = 20;

    }
}
