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


    public AudioSource audioSource;
    public AudioClip audioClipLaughVFX;
    public NavMeshAgent navMeshAgent;
    public Animator animator;

   

    private void Start()
    {
        playerCam.Priority = 20;
        bossCam.Priority = 0;
       
    }

    private void Update()
    {
        CameraFocus();    
    }

  
    // Xử lý camera focus
    private void CameraFocus()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectRange && !isFocusing)
        {
            isFocusing = true;
            animator.SetBool("isLaughing",true);
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
