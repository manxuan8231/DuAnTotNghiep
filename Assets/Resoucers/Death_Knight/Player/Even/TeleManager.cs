using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleManager : MonoBehaviour
{
    public GameObject teleManager;
    private AudioSource audioSource;
    public AudioClip audioClip;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        teleManager.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            audioSource.PlayOneShot(audioClip);
            // Đảo trạng thái bật/tắt
            teleManager.SetActive(!teleManager.activeSelf);
        }
    }
}
