using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tele : MonoBehaviour
{
    public GameObject buttonF;
    public GameObject tele;
    public GameObject player;

    public float rangerTagetPlayer = 300f;
    public CinemachineVirtualCamera VirtualCamera;
    public CinemachineVirtualCamera VirtualCameraPlayer;
    private bool isCamera = true;
    public AudioSource audioSource;
    public GameObject even;
    void Start()
    {
        buttonF.SetActive(false);
        even.SetActive(false);
    }

   
    void Update()
    {
        TagetPlayer();
        if (buttonF.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
              player.transform.position = tele.transform.position;
              audioSource.enabled = false;
              buttonF.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            buttonF.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            buttonF.SetActive(false);
        }
    }
    private void TagetPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance <= rangerTagetPlayer)
        {
            if(isCamera == true)
            {
                StartCoroutine(CoonDownCamera());
            }
            
        }
    }
    private IEnumerator CoonDownCamera()
    {
        VirtualCamera.Priority = 20;
        VirtualCameraPlayer.Priority = 0;
        yield return new WaitForSeconds(3f);
        VirtualCamera.Priority = 0;
        VirtualCameraPlayer.Priority = 20;
        isCamera = false;
    }
}
