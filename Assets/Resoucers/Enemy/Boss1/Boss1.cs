using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
using System.Collections;

public class Boss1 : MonoBehaviour
{
    public Transform player;
    public CinemachineVirtualCamera bossCam;
    public CinemachineVirtualCamera playerCam;
    public float detectRange = 10f;
    public float focusDuration = 3f;
    private bool isFocusing = false;

    public Transform[] movePoints; // 4 vị trí cố định để di chuyển
    private int currentPointIndex = -1;
    private bool isWaiting = true;
    private float idleStartTime = 0f;
    public float startIdleDuration = 8f;
    public float idleDuration = 5f;

    public float skillRange = 50f; // Khoảng cách để kích hoạt skill1
    public float skillDelay = 3f; // Thời gian chờ trước khi dùng skill
    public float skillCooldown = 5f; // Thời gian hồi chiêu

    public AudioSource audioSource;
    public AudioClip audioClipLaughVFX;
    public NavMeshAgent navMeshAgent;
    public Animator animator;

    private bool isUsingSkill = false; // Boss có đang dùng skill không?
    private bool isSkillOnCooldown = false; // Biến kiểm tra cooldown
    private float lastSkillTime = -Mathf.Infinity; // Thời gian boss dùng skill gần nhất

    private void Start()
    {
        playerCam.Priority = 20;
        bossCam.Priority = 0;
        idleStartTime = Time.time;
    }

    private void Update()
    {
        CameraFocus();
        CheckForSkillActivation();

        // Nếu boss không dùng skill, cho phép di chuyển
        if (!isUsingSkill)
        {
            HandleMovement();
        }
    }

    // Kiểm tra khoảng cách để kích hoạt skill1
    private void CheckForSkillActivation()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= skillRange && !isSkillOnCooldown && !isUsingSkill)
        {
            StartCoroutine(UseSkillWithDelay());
        }
    }

    private IEnumerator UseSkillWithDelay()
    {
        isUsingSkill = true; // Đánh dấu boss chuẩn bị dùng skill
        navMeshAgent.isStopped = true; // Dừng di chuyển
        navMeshAgent.ResetPath(); // ❗ Bỏ target vị trí khi đang dùng skill

        // ✅ Xoay mặt về phía player trước khi dùng skill
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;

        yield return new WaitForSeconds(skillDelay); // Chờ 3 giây trước khi kích hoạt skill

        animator.SetBool("isSkill1", true);
        lastSkillTime = Time.time; // Lưu thời gian dùng skill
        isSkillOnCooldown = true; // Bắt đầu cooldown

        yield return new WaitForSeconds(2f); // Giả sử skill kéo dài 2 giây
        animator.SetBool("isSkill1", false);

        isUsingSkill = false; // Kết thúc dùng skill
        navMeshAgent.isStopped = false; // Tiếp tục di chuyển

        yield return new WaitForSeconds(skillCooldown); // Chờ 5 giây cooldown
        isSkillOnCooldown = false; // Có thể dùng skill lại
    }

    // Quản lý di chuyển random giữa 4 điểm
    private void HandleMovement()
    {
        if (isWaiting)
        {
            if (Time.time - idleStartTime >= startIdleDuration)
            {
                isWaiting = false;
                MoveToRandomPoint();
            }
        }
        else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            StartIdle();
        }
    }

    // Chọn một vị trí random trong 4 điểm
    private void MoveToRandomPoint()
    {
        if (movePoints.Length == 0) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, movePoints.Length);
        } while (newIndex == currentPointIndex);

        currentPointIndex = newIndex;
        navMeshAgent.SetDestination(movePoints[currentPointIndex].position);
        animator.SetBool("isMoving", true);
    }

    // Khi đến nơi, boss idle 5 giây rồi di chuyển tiếp
    private void StartIdle()
    {
        isWaiting = true;
        idleStartTime = Time.time;
        startIdleDuration = idleDuration;
        animator.SetBool("isMoving", false);
    }

    // Xử lý camera focus
    private void CameraFocus()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectRange && !isFocusing)
        {
            isFocusing = true;
            StartCoroutine(FocusOnBoss());
        }
    }

    private IEnumerator FocusOnBoss()
    {
        bossCam.Priority = 20;
        playerCam.Priority = 10;
        PlayLaughSound();

        yield return new WaitForSeconds(focusDuration);

        bossCam.Priority = 10;
        playerCam.Priority = 20;
        StopLaughSound();
    }

    // Phát âm thanh cười
    private void PlayLaughSound()
    {
        if (audioSource != null && audioClipLaughVFX != null && !audioSource.isPlaying)
        {
            audioSource.clip = audioClipLaughVFX;
            audioSource.Play();
        }
    }

    // Dừng âm thanh
    private void StopLaughSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
