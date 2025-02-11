using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Boss1 : MonoBehaviour
{
    public Transform player;

    // Xử lý chức năng launching
    public CinemachineVirtualCamera bossCam;
    public CinemachineVirtualCamera playerCam;
    public float detectRange = 10f;
    public float focusDuration = 3f;
    private bool isFocusing = false;

    // Audio sounds
    public AudioSource audioSource;
    public AudioClip audioClipLaughVFX;

    // AI
    public NavMeshAgent navMeshAgent;

    // Xử lý tấn công
    public float distancePlayer = 10f; // Khoảng cách nhìn thấy player để tấn công
    public float attackCooldown = 10f; // Thời gian hồi chiêu
    private float lastAttackTime = 0f;
    public GameObject Weappon;
    public Animator animator;

    //xử lý skill
    public float distanceSkill = 50; //khoản cách nhìn player để dùng skill
    private bool onSkill = false;

    private void Start()
    {
        playerCam.Priority = 20;
        bossCam.Priority = 0;
        Weappon.SetActive(false);
    }

    private void Update()
    {
        CameraFocus(); // Xử lý camera
        Attack(); // Xử lý tấn công
    }

    // Xử lý camera focus
    private void CameraFocus()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectRange && !isFocusing)
        {
            isFocusing = true;
            animator.SetBool("isLaughing", true);
            StartCoroutine(FocusOnBoss());
        }
    }

    private System.Collections.IEnumerator FocusOnBoss()
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

    // Xử lý tấn công
    private void Attack()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= distancePlayer && Time.time >= lastAttackTime + attackCooldown)
        {
            int random = Random.Range(0, 3);
            if(random == 0)
            {
                Debug.Log("Thực hiện attack 1");
                animator.SetTrigger("Attack");
                Weappon.SetActive(true);
            }
                if (random == 1)
            {
                Debug.Log("Thực hiện attack 2");
                animator.SetTrigger("Attack2");
                Weappon.SetActive(true);
            }
                if(random == 2)
            {
                Debug.Log("Thực hiện dịch chuyển ra");

            }
            lastAttackTime = Time.time; // Cập nhật thời gian tấn công cuối cùng
        }
    }
}
